using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using SampleCommandSet.Models;
using System;
using System.Linq;
using System.Threading;
using SampleCommandSet.Extensions;

namespace SampleCommandSet.Commands.Sheet
{
    public class CreateSheetEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public JObject Parameters { get; set; }
        public SheetInfo ResultInfo { get; private set; }
        public bool TaskCompleted { get; private set; }
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);

        public bool WaitForCompletion(int timeoutMilliseconds = 10000)
        {
            return _resetEvent.WaitOne(timeoutMilliseconds);
        }

        public void Execute(UIApplication app)
        {
            try
            {
                var uiDoc = app.ActiveUIDocument;
                var doc = uiDoc.Document;

                using (var t = new Transaction(doc, "Create Sheet"))
                {
                    t.Start();

                    var titleBlockId = new ElementId(Parameters["titleBlockId"].Value<long>());

                    var newSheet = ViewSheet.Create(doc, titleBlockId);

                    if (Parameters.TryGetValue("sheetName", out var sheetNameToken))
                    {
                        newSheet.Name = sheetNameToken.Value<string>();
                    }

                    if (Parameters.TryGetValue("sheetNumber", out var sheetNumberToken))
                    {
                        newSheet.SheetNumber = sheetNumberToken.Value<string>();
                    }

                    ResultInfo = new SheetInfo
                    {
                        Id = newSheet.Id.GetIdValue(),
                        UniqueId = newSheet.UniqueId,
                        Name = newSheet.Name,
                        SheetNumber = newSheet.SheetNumber,
                        ViewIds = newSheet.GetAllPlacedViews().Select(v => v.GetIdValue()).ToList()
                    };

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to create sheet: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Create Sheet";
        }
    }
}
