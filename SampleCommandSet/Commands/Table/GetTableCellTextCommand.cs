using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Table
{
    public class GetTableCellTextCommand : ExternalEventCommandBase
    {
        private GetTableCellTextEventHandler _handler => (GetTableCellTextEventHandler)Handler;

        public override string CommandName => "get_table_cell_text";

        public GetTableCellTextCommand(UIApplication uiApp)
            : base(new GetTableCellTextEventHandler(), uiApp)
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
                throw new TimeoutException("Getting table cell text timed out.");
            }
        }
    }
}
