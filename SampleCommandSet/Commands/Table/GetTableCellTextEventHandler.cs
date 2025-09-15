using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using System;
using System.Threading;

namespace SampleCommandSet.Commands.Table
{
    public class GetTableCellTextEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public JObject Parameters { get; set; }
        public string ResultInfo { get; private set; }
        public bool TaskCompleted { get; private set; }
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);

        public bool WaitForCompletion(int timeoutMilliseconds = 10000)
        {
            return _resetEvent.WaitOne(timeoutMilliseconds);
        }

        public void Execute(UIApplication app)
        {
            try
            {
                var uiDoc = app.ActiveUIDocument;
                var doc = uiDoc.Document;

                var tableViewId = new ElementId(Parameters["tableViewId"].Value<long>());
                var tableView = doc.GetElement(tableViewId) as TableView;
                var row = Parameters["row"].Value<int>();
                var col = Parameters["col"].Value<int>();

                if (tableView != null)
                {
                    var tableData = tableView.GetTableData();
                    var sectionData = tableData.GetSectionData(SectionType.Body);
                    ResultInfo = sectionData.GetCellText(row, col);
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to get table cell text: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Get Table Cell Text";
        }
    }
}
