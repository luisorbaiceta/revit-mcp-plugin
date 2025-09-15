using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using SampleCommandSet.Models;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.View
{
    public class SetViewCropInfoEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
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

                using (var t = new Transaction(doc, "Set View Crop Info"))
                {
                    t.Start();

                    if (Parameters.TryGetValue("cropBox", out var cropBoxToken))
                    {
                        var cropBox = cropBoxToken.ToObject<BoundingBoxInfo>();
                        var bbox = new BoundingBoxXYZ
                        {
                            Min = new XYZ(cropBox.Min.X, cropBox.Min.Y, cropBox.Min.Z),
                            Max = new XYZ(cropBox.Max.X, cropBox.Max.Y, cropBox.Max.Z)
                        };
                        activeView.CropBox = bbox;
                    }

                    if (Parameters.TryGetValue("cropBoxActive", out var cropBoxActiveToken))
                    {
                        activeView.CropBoxActive = cropBoxActiveToken.Value<bool>();
                    }

                    if (Parameters.TryGetValue("cropBoxVisible", out var cropBoxVisibleToken))
                    {
                        activeView.CropBoxVisible = cropBoxVisibleToken.Value<bool>();
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to set crop info: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Set View Crop Info";
        }
    }
}
