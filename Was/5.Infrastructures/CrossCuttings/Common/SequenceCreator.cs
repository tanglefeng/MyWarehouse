using System;

namespace Kengic.Was.CrossCutting.Common
{
    public class SequenceCreator
    {
        private static int _squenceNo;
        public static string GetIdBySeqNoAndKey(string key) => key + Guid.NewGuid().ToString("N");

        public static int GetSequenceNo()
        {
            if (_squenceNo >= 99999)
            {
                _squenceNo = 1;
            }
            else
            {
                _squenceNo = _squenceNo + 1;
            }

            return _squenceNo;
        }

        public static string GetIdBySequenceNo()
        {
            var sequenceNo = GetSequenceNo();
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + sequenceNo.ToString("d5");
        }

      
    }
}