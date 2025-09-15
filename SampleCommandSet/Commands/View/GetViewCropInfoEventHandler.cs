using Autodesk.Revit.UI;
using revit_mcp_sdk.API.Interfaces;
using System;
using System.Threading;
using SampleCommandSet.Extensions;
using SampleCommandSet.Models;

namespace SampleCommandSet.Commands.View
{
    public class GetViewCropInfoEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public ViewCropInfo ResultInfo { get; private set; }
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

                ResultInfo = new ViewCropInfo
                {
                    CropBox = new BoundingBoxInfo
                    {
                        Min = new Point { X = activeView.CropBox.Min.X, Y = activeView.CropBox.Min.Y, Z = activeView.CropBox.Min.Z },
                        Max = new Point { X = activeView.CropBox.Max.X, Y = activeView.CropBox.Max.Y, Z = activeView.CropBox.Max.Z }
                    },
                    CropBoxActive = activeView.CropBoxActive,
                    CropBoxVisible = activeView.CropBoxVisible
                };
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to get crop info: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Get View Crop Info";
        }
    }
}
