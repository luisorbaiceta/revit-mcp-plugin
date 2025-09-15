using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Table
{
    public class GetTableViewInfoCommand : ExternalEventCommandBase
    {
        private GetTableViewInfoEventHandler _handler => (GetTableViewInfoEventHandler)Handler;

        public override string CommandName => "get_table_view_info";

        public GetTableViewInfoCommand(UIApplication uiApp)
            : base(new GetTableViewInfoEventHandler(), uiApp)
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
                throw new TimeoutException("Getting table view info timed out.");
            }
        }
    }
}
