namespace Kengic.Was.CrossCutting.Sockets
{
    public class SequenceNumberCreator
    {
        private int _squenceNo;

        public int GetSequenceNo()
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

        public void SetSequenceZero() => _squenceNo = 0;
    }
}