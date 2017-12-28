using Kengic.Was.CrossCutting.Common;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Domain.Entity.WorkTask.WorkTasks
{
    public class SubWorkTask<TKey> : WorkTask<TKey>
    {
        public int SerialNumber { get; set; }
        public int ParallelNumber { get; set; }
        public string ExecuteWorkTaskId { get; set; }

        public override void FormateWorkTask(JObject jObject)
        {
            SerialNumber = JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.SerialNumber);
            ParallelNumber = JSonHelper.GetValue<int>(jObject, StaticParameterForWorkTask.ParallelNumber);
            ExecuteWorkTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ExecuteWorkTaskId);
            base.FormateWorkTask(jObject);
        }
    }
}