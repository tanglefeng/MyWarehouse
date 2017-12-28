using System;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.SystemTracing;
using Kengic.Was.Systems.Message;

namespace Kengic.Was.DistributedServices.Common
{
    public class OperationTracingHelper
    {
        public static OperationTracing GetOperationTracing(bool status, string user, string operation, string source,
            string obj, string objectValue, string context, string description)
        {
            var operationTracing = new OperationTracing
            {
                Id = Guid.NewGuid().ToString("N"),
                User = user,
                Source = source,
                Operation = MessageRepository.GetMessage(operation),
                Object = obj,
                ObjectValue = objectValue,
                CreateTime = DateTime.Now,
                Result =
                    status
                        ? MessageRepository.GetMessage(StaticParameterForMessage.Success)
                        : MessageRepository.GetMessage(StaticParameterForMessage.Failure),
                Context = context,
                Description = description
            };


            return operationTracing;
        }
    }
}