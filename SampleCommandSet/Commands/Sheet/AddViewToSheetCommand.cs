using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Sheet
{
    public class AddViewToSheetCommand : ExternalEventCommandBase
    {
        private AddViewToSheetEventHandler _handler => (AddViewToSheetEventHandler)Handler;

        public override string CommandName => "add_view_to_sheet";

        public AddViewToSheetCommand(UIApplication uiApp)
            : base(new AddViewToSheetEventHandler(), uiApp)
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
                throw new TimeoutException("Adding view to sheet timed out.");
            }
        }
    }
}
