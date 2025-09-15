using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using revit_mcp_sdk.API.Interfaces;
using SampleCommandSet.Models;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.View
{
    public class GetViewSettingsEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public ViewSettingsInfo ResultInfo { get; private set; }
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
                var activeView = doc.ActiveView;

                ResultInfo = new ViewSettingsInfo
                {
                    DetailLevel = activeView.DetailLevel.ToString(),
                    Scale = activeView.Scale,
                    DisplayStyle = activeView.DisplayStyle.ToString()
                };
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to get view settings: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Get View Settings";
        }
    }
}
