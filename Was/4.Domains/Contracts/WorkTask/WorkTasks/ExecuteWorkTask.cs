using Kengic.Was.CrossCutting.Common;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Domain.Entity.WorkTask.WorkTasks
{
    public class ExecuteWorkTask<TKey> : WorkTask<TKey>
    {
        public int CurrentSerialNumber { get; set; }
        public int SumSerialNumber { get; set; }

        public override void FormateWorkTask(JObject jObject)
        {
            CurrentSerialNumber = JSonHelper.GetValue<int>(jObject,
                StaticParameterForWorkTask.CurrentSerialNumber);
            SumSerialNumber = JSonHelper.GetValue<int>(jObject,
                StaticParameterForWorkTask.SumSerialNumber);
            base.FormateWorkTask(jObject);
        }
    }
}