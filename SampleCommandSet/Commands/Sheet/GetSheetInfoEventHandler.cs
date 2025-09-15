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
    public class GetSheetInfoEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
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

                var sheetId = new ElementId(Parameters["sheetId"].Value<long>());
                var sheet = doc.GetElement(sheetId) as ViewSheet;

                if (sheet != null)
                {
                    ResultInfo = new SheetInfo
                    {
                        Id = sheet.Id.GetIdValue(),
                        UniqueId = sheet.UniqueId,
                        Name = sheet.Name,
                        SheetNumber = sheet.SheetNumber,
                        ViewIds = sheet.GetAllPlacedViews().Select(v => v.GetIdValue()).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to get sheet info: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Get Sheet Info";
        }
    }
}
