using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.View
{
    public class SetViewSettingsCommand : ExternalEventCommandBase
    {
        private SetViewSettingsEventHandler _handler => (SetViewSettingsEventHandler)Handler;

        public override string CommandName => "set_view_settings";

        public SetViewSettingsCommand(UIApplication uiApp)
            : base(new SetViewSettingsEventHandler(), uiApp)
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
                throw new TimeoutException("Setting view settings timed out.");
            }
        }
    }
}
