using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.Sheet
{
    public class RemoveViewFromSheetEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public JObject Parameters { get; set; }
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

                using (var t = new Transaction(doc, "Remove View From Sheet"))
                {
                    t.Start();

                    var sheetId = new ElementId(Parameters["sheetId"].Value<long>());
                    var viewportId = new ElementId(Parameters["viewportId"].Value<long>());
                    var sheet = doc.GetElement(sheetId) as ViewSheet;

                    if (sheet != null)
                    {
                        sheet.DeleteViewport(doc.GetElement(viewportId) as Viewport);
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to remove view from sheet: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Remove View From Sheet";
        }
    }
}
