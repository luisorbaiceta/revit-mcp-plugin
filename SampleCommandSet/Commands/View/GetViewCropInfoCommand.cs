using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.View
{
    public class GetViewCropInfoCommand : ExternalEventCommandBase
    {
        private GetViewCropInfoEventHandler _handler => (GetViewCropInfoEventHandler)Handler;

        public override string CommandName => "get_view_crop_info";

        public GetViewCropInfoCommand(UIApplication uiApp)
            : base(new GetViewCropInfoEventHandler(), uiApp)
        {
        }

        public override object Execute(JObject parameters, string requestId)
        {
            if (RaiseAndWaitForCompletion(10000)) // 10 second timeout
            {
                return _handler.ResultInfo;
            }
            else
            {
                throw new TimeoutException("Getting crop info timed out.");
            }
        }
    }
}
