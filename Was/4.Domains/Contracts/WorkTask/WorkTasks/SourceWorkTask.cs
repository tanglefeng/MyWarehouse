using Kengic.Was.CrossCutting.Common;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Domain.Entity.WorkTask.WorkTasks
{
    public class SourceWorkTask<TKey> : WorkTask<TKey>
    {
        public string ExecuteWorkTaskId { get; set; }

        public override void FormateWorkTask(JObject jObject)
        {
            ExecuteWorkTaskId = JSonHelper.GetValue<string>(jObject,
                StaticParameterForWorkTask.ExecuteWorkTaskId);
            base.FormateWorkTask(jObject);
        }
    }
}