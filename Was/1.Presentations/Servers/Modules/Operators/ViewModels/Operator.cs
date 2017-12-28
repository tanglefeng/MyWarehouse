using System;
using Kengic.Was.Operator.Common;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;

namespace Kengic.Was.Presentation.Server.Module.Operators.ViewModels
{
    public class Operator : SeviceBase
    {
        private readonly IOperator _operator;

        public Operator(IOperator tOperator)
        {
            _operator = tOperator;
        }

        public void Active()
        {
            try
            {
                Status = StatusType.ActivePending;
                OperatorRepository.ActiveOperator(_operator);
                Status = StatusType.Actived;
            }
            catch (Exception)
            {
                Status = StatusType.Faulted;
                throw;
            }
        }

        public void Deactive()
        {
            try
            {
                Status = StatusType.DeactivePending;
                OperatorRepository.DeactiveOperator(_operator);
                Status = StatusType.Deactived;
            }
            catch (Exception)
            {
                Status = StatusType.Faulted;
                throw;
            }
        }

        public override void Start()
        {
            try
            {
                Status = StatusType.StartPending;
                OperatorRepository.InitializeOperator(_operator);
                OperatorRepository.StartOperator(_operator);
                Status = StatusType.Running;
            }
            catch (Exception)
            {
                Status = StatusType.Faulted;
                throw;
            }
        }

        public override void Stop()
        {
            Status = StatusType.StopPending;
            OperatorRepository.CloseOperator(_operator);
            Status = StatusType.Stopped;
        }
    }
}