using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Base;
using System;

namespace SampleCommandSet.Commands.Sheet
{
    public class RemoveViewFromSheetCommand : ExternalEventCommandBase
    {
        private RemoveViewFromSheetEventHandler _handler => (RemoveViewFromSheetEventHandler)Handler;

        public override string CommandName => "remove_view_from_sheet";

        public RemoveViewFromSheetCommand(UIApplication uiApp)
            : base(new RemoveViewFromSheetEventHandler(), uiApp)
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
                throw new TimeoutException("Removing view from sheet timed out.");
            }
        }
    }
}
