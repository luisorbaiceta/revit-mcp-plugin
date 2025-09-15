using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.Sheet
{
    public class AddViewToSheetEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
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

                using (var t = new Transaction(doc, "Add View To Sheet"))
                {
                    t.Start();

                    var sheetId = new ElementId(Parameters["sheetId"].Value<long>());
                    var viewId = new ElementId(Parameters["viewId"].Value<long>());
                    var sheet = doc.GetElement(sheetId) as ViewSheet;
                    var view = doc.GetElement(viewId) as View;

                    if (sheet != null && view != null)
                    {
                        Viewport.Create(doc, sheet.Id, view.Id, new XYZ(0, 0, 0));
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to add view to sheet: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Add View To Sheet";
        }
    }
}
