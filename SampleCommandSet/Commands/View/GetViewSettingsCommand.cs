using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.View
{
    public class GetViewSettingsCommand : ExternalEventCommandBase
    {
        private GetViewSettingsEventHandler _handler => (GetViewSettingsEventHandler)Handler;

        public override string CommandName => "get_view_settings";

        public GetViewSettingsCommand(UIApplication uiApp)
            : base(new GetViewSettingsEventHandler(), uiApp)
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
                throw new TimeoutException("Getting view settings timed out.");
            }
        }
    }
}
