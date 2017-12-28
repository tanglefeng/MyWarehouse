using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Operators;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Systems.ActivityContracts;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Operator.Common
{
    public class OperatorRepository
    {
        private const string OperatorManagerName = "OperatorManager";
        private const string OperatorSection = "operatorSection";

        private static readonly ConcurrentDictionary<string, IOperator>
            OperatorQueue =
                new ConcurrentDictionary<string, IOperator>();

        public static void LoadOperatorConfiguration(string fileName)
        {
            try
            {
                var configurations = ConfigurationOperation<OperatorSection>.GetCustomSection(fileName,
                    OperatorSection);
                if (configurations == null)
                {
                    LogRepository.WriteErrorLog(OperatorManagerName,
                        StaticParameterForMessage.NoSection, fileName);
                    return;
                }

                foreach (OperatorElement operatorElement in configurations.Operators)
                {
                    var opInstance = ServiceLocator.Current.GetInstance<IOperator>(operatorElement.Name);
                    opInstance.OperatorElement = operatorElement;
                    opInstance.IsReciveMessage = false;
                    opInstance.Id = operatorElement.Id;
                    opInstance.Name = operatorElement.Name;
                    opInstance.LogName = operatorElement.LogName;
                    OperatorQueue.TryAdd(operatorElement.Id, opInstance);

                    LogRepository.WriteInfomationLog(OperatorManagerName,
                        StaticParameterForMessage.LoadOperatorSuccess, operatorElement.Name);
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName,
                    StaticParameterForMessage.LoadOperatorException, ex.ToString());
            }
        }

        private static List<IOperator> GetOperatorsList() => OperatorQueue.Values.ToList();
        public static bool InitializeOperator() => ExecuteOperator(InitializeOperator);

        public static bool InitializeOperator(IOperator tOperator)
        {
            try
            {
                var operatorInstance = GetOperatorInstance(tOperator.Id);
                if (operatorInstance == null)
                {
                    return false;
                }

                if (operatorInstance.Status >= OperatorStatus.Initialize)
                {
                    return false;
                }

                if (operatorInstance.Initialize())
                {
                    operatorInstance.Status = OperatorStatus.Initialize;
                    LogRepository.WriteInfomationLog(OperatorManagerName,
                        StaticParameterForMessage.InitializeOperatorSuccess, tOperator.Id);
                    return true;
                }

                LogRepository.WriteErrorLog(OperatorManagerName,
                    StaticParameterForMessage.InitializeOperatorFailure, tOperator.Id);
                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName,
                    StaticParameterForMessage.InitializeOperatorException, ex.Message);
                return false;
            }
        }

        public static bool StartOperator() => ExecuteOperator(StartOperator);

        public static bool StartOperator(IOperator tOperator)
        {
            try
            {
                var operatorInstance = GetOperatorInstance(tOperator.Id);
                if (operatorInstance == null)
                {
                    return false;
                }

                if (operatorInstance.Status >= OperatorStatus.Strat)
                {
                    return false;
                }
                if (operatorInstance.Start())
                {
                    operatorInstance.Status = OperatorStatus.Strat;
                    LogRepository.WriteInfomationLog(OperatorManagerName,
                        StaticParameterForMessage.StartOperatorSuccess, tOperator.Id);
                    return true;
                }
                LogRepository.WriteErrorLog(OperatorManagerName, StaticParameterForMessage.StartOperatorFailure,
                    tOperator.Id);

                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName,
                    StaticParameterForMessage.StartOperatorException, ex.ToString());
                return false;
            }
        }

        public static bool ActiveOperator(IOperator tOperator)
        {
            try
            {
                var operatorInstance = GetOperatorInstance(tOperator.Id);
                if (operatorInstance == null)
                {
                    return false;
                }
                if (operatorInstance.Status >= OperatorStatus.Active)
                {
                    return false;
                }
                operatorInstance.IsReciveMessage = true;
                operatorInstance.Status = OperatorStatus.Active;
                LogRepository.WriteInfomationLog(OperatorManagerName,
                    StaticParameterForMessage.ActiveOperatorSuccess, tOperator.Id);

                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName, StaticParameterForMessage.ActiveOperatorException,
                    ex.Message);
                return false;
            }
        }

        public static bool ActiveOperator() => ExecuteOperator(ActiveOperator);

        public static bool DeactiveOperator(IOperator tOperator)
        {
            try
            {
                var operatorInstance = GetOperatorInstance(tOperator.Id);
                if (operatorInstance == null)
                {
                    return false;
                }
                operatorInstance.IsReciveMessage = false;
                operatorInstance.Status = OperatorStatus.Strat;
                LogRepository.WriteInfomationLog(OperatorManagerName,
                    StaticParameterForMessage.DeactiveOperatorSuccess, tOperator.Id);

                return true;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName,
                    StaticParameterForMessage.DeactiveOperatorException, ex.Message);
                return false;
            }
        }

        public static bool DeactiveOperator() => ExecuteOperator(DeactiveOperator);

        public static bool CloseOperator(IOperator tOperator)
        {
            try
            {
                var operatorInstance = GetOperatorInstance(tOperator.Id);
                if (operatorInstance == null)
                {
                    return false;
                }

                if (operatorInstance.Close())
                {
                    operatorInstance.Status = OperatorStatus.Initialize;
                    LogRepository.WriteInfomationLog(OperatorManagerName,
                        StaticParameterForMessage.CloseOperatorSuccess, tOperator.Id);
                    return true;
                }

                LogRepository.WriteErrorLog(OperatorManagerName,
                    StaticParameterForMessage.CloseOperatorFailure, tOperator.Id);

                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(OperatorManagerName, StaticParameterForMessage.CloseOperatorException,
                    ex.Message);
                return false;
            }
        }

        public static bool CloseOperator() => ExecuteOperator(CloseOperator);
        private static bool IsExistOperator(string operatorId) => OperatorQueue.ContainsKey(operatorId);

        public static IOperator GetOperatorInstance(string operatorId)
        {
            if (!IsExistOperator(operatorId))
            {
                LogRepository.WriteErrorLog(OperatorManagerName,
                    StaticParameterForMessage.OperatorIsNotExist, operatorId);
                return null;
            }

            IOperator tOperatorElement;

            if (OperatorQueue.TryGetValue(operatorId, out tOperatorElement))
            {
                return tOperatorElement;
            }
            LogRepository.WriteErrorLog(OperatorManagerName,
                StaticParameterForMessage.OperatorInstanceIsNotExist, operatorId);
            return null;
        }

        private static Tuple<bool, string> ExecuteActivity(string operatorId, string methodName, object[] parameter)
        {
            string messageCode;
            try
            {
                if (!IsExistOperator(operatorId))
                {
                    messageCode = StaticParameterForMessage.OperatorIsNotExist;
                    LogRepository.WriteErrorLog(OperatorManagerName, messageCode, operatorId);
                    return new Tuple<bool, string>(false, messageCode);
                }

                object operatorInstance = GetOperatorInstance(operatorId);
                if (operatorInstance == null)
                {
                    messageCode = StaticParameterForMessage.OperatorInstanceIsNotExist;
                    LogRepository.WriteErrorLog(OperatorManagerName, messageCode, operatorId);
                    return new Tuple<bool, string>(false, messageCode);
                }

                if (!((IOperator) operatorInstance).IsReciveMessage)
                {
                    messageCode = StaticParameterForMessage.NoReceiveMessage;
                    LogRepository.WriteErrorLog(OperatorManagerName, messageCode, operatorId);
                    return new Tuple<bool, string>(false, messageCode);
                }


                var t = operatorInstance.GetType();
                var miHandler = t.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if ((miHandler != null) && (miHandler.GetParameters().Length == parameter.Length))
                {
                    return (Tuple<bool, string>) miHandler.Invoke(operatorInstance, parameter);
                }

                messageCode = StaticParameterForMessage.InvokeFailure;
                LogRepository.WriteErrorLog(OperatorManagerName, messageCode, operatorId);
                return new Tuple<bool, string>(false, messageCode);
            }
            catch (Exception ex)
            {
                messageCode = StaticParameterForMessage.InvokeException;
                LogRepository.WriteErrorLog(OperatorManagerName, messageCode, ex.ToString());
                return new Tuple<bool, string>(false, messageCode);
            }
        }

        public static Tuple<bool, string> ExecuteActivity(ProcessMessage processMessage)
        {
            if (!ActivityContractRepository.IsExistActivityContract(processMessage.ActivityContrace))
            {
                const string messageCode = StaticParameterForMessage.ActivityContractIsNotExist;
                LogRepository.WriteErrorLog(OperatorManagerName,
                    StaticParameterForMessage.ActivityContractIsNotExist, processMessage.ActivityContrace);
                return new Tuple<bool, string>(false, messageCode);
            }

            var activityContract =
                ActivityContractRepository.GetActivityContract(processMessage.ActivityContrace);

            var objPara = new object[1];

            var receiver = activityContract.OperatorName;
            var methodName = activityContract.OperatorMethod;

            objPara[0] = processMessage;

            return ExecuteActivity(receiver, methodName, objPara);
        }

        private static bool ExecuteOperator(
            Func<IOperator, bool> executeMethod)
        {
            var operatorList = GetOperatorsList();
            if ((operatorList == null) || (operatorList.Count <= 0))
            {
                return false;
            }

            foreach (var operatorOjb in operatorList)
            {
                executeMethod(operatorOjb);
            }

            return true;
        }
    }
}