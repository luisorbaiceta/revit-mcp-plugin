using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.View
{
    public class SetViewCropInfoCommand : ExternalEventCommandBase
    {
        private SetViewCropInfoEventHandler _handler => (SetViewCropInfoEventHandler)Handler;

        public override string CommandName => "set_view_crop_info";

        public SetViewCropInfoCommand(UIApplication uiApp)
            : base(new SetViewCropInfoEventHandler(), uiApp)
        {
        }

        public override object Execute(JObject parameters, string requestId)
        {
            _handler.Parameters = parameters;
            if (RaiseAndWaitForCompletion(10000)) // 10 second timeout
            {
                return null;
            }
            else
            {
                throw new TimeoutException("Setting crop info timed out.");
            }
        }
    }
}
