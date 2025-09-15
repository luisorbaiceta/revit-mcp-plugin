using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using revit_mcp_sdk.API.Interfaces;
using SampleCommandSet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SampleCommandSet.Extensions;

namespace SampleCommandSet.Commands.Table
{
    public class GetTableViewInfoEventHandler : IExternalEventHandler, IWaitableExternalEventHandler
    {
        public JObject Parameters { get; set; }
        public TableViewInfo ResultInfo { get; private set; }
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

                if (tableView != null)
                {
                    ResultInfo = new TableViewInfo
                    {
                        Id = tableView.Id.GetIdValue(),
                        UniqueId = tableView.UniqueId,
                        Name = tableView.Name
                    };

                    if (tableView is ViewSchedule schedule)
                    {
                        ResultInfo.Fields = new List<ScheduleFieldInfo>();
                        foreach (ScheduleField field in schedule.Definition.GetFields())
                        {
                            ResultInfo.Fields.Add(new ScheduleFieldInfo
                            {
                                Name = field.GetName(),
                                ParameterName = field.GetSchedulableField().GetName(doc)
                            });
                        }

                        ResultInfo.Filters = new List<ScheduleFilterInfo>();
                        foreach (ScheduleFilter filter in schedule.Definition.GetFilters())
                        {
                            ResultInfo.Filters.Add(new ScheduleFilterInfo
                            {
                                FieldName = schedule.Definition.GetField(filter.FieldId).GetName(),
                                FilterType = filter.FilterType.ToString(),
                                Value = filter.GetStringValue()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to get table view info: " + ex.Message);
            }
            finally
            {
                TaskCompleted = true;
                _resetEvent.Set();
            }
        }

        public string GetName()
        {
            return "Get Table View Info";
        }
    }
}
