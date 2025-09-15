using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Sheet
{
    public class GetSheetInfoCommand : ExternalEventCommandBase
    {
        private GetSheetInfoEventHandler _handler => (GetSheetInfoEventHandler)Handler;

        public override string CommandName => "get_sheet_info";

        public GetSheetInfoCommand(UIApplication uiApp)
            : base(new GetSheetInfoEventHandler(), uiApp)
        {
        }

        public override object Execute(JObject parameters, string requestId)
        {
            _handler.Parameters = parameters;
            if (RaiseAndWaitForCompletion(10000)) // 10 second timeout
            {
                return _handler.ResultInfo;
            }
            else
            {
                throw new TimeoutException("Getting sheet info timed out.");
            }
        }
    }
}
