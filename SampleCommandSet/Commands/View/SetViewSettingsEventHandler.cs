using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.View
{
    public class SetViewSettingsEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
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
                var activeView = doc.ActiveView;

                using (var t = new Transaction(doc, "Set View Settings"))
                {
                    t.Start();

                    if (Parameters.TryGetValue("detailLevel", out var detailLevelToken))
                    {
                        var detailLevel = (ViewDetailLevel)Enum.Parse(typeof(ViewDetailLevel), detailLevelToken.Value<string>());
                        activeView.DetailLevel = detailLevel;
                    }

                    if (Parameters.TryGetValue("scale", out var scaleToken))
                    {
                        activeView.Scale = scaleToken.Value<int>();
                    }

                    if (Parameters.TryGetValue("displayStyle", out var displayStyleToken))
                    {
                        var displayStyle = (DisplayStyle)Enum.Parse(typeof(DisplayStyle), displayStyleToken.Value<string>());
                        activeView.DisplayStyle = displayStyle;
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to set view settings: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Set View Settings";
        }
    }
}
