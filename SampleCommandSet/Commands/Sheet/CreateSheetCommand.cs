using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Sheet
{
    public class CreateSheetCommand : ExternalEventCommandBase
    {
        private CreateSheetEventHandler _handler => (CreateSheetEventHandler)Handler;

        public override string CommandName => "create_sheet";

        public CreateSheetCommand(UIApplication uiApp)
            : base(new CreateSheetEventHandler(), uiApp)
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
                throw new TimeoutException("Creating sheet timed out.");
            }
        }
    }
}
