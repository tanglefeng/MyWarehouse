namespace Kengic.Was.Connector.Tim
{
    public class TimReturnCode
    {
        //task properly executed
        public const string ReturnOk = "OK";
        public const string Return00 = "00";
        //weight errors,measurement errors
        public const string ReturnMe = "ME";
        //coordinate error
        public const string ReturnCe = "CE";
        //no read
        public const string ReturnNr = "NR";
        //full bay on storage
        public const string ReturnFb = "FB";
        //storage location,inbound conveyor,subsystem notifies a free location
        public const string ReturnFf = "FF";
        //Tu id unknow
        public const string ReturnTu = "TU";
        //destination is block
        public const string ReturnBl = "BL";
        //destination unreachable
        public const string ReturnDu = "DU";
        //source storage location no accessible because a TU is present at location depth x and
        //a retrieval action is to be carried out at storage lacation depth x+1
        public const string ReturnSn = "SN";
        //destination storage location not accessible
        public const string ReturnDn = "DN";
        //syntax failure,incomplete,wrong structure
        public const string ReturnSf = "SF";
        //task was Remove ata the controls panel of the subordinate subsystem
        public const string ReturnTd = "TD";
        //no available
        public const string ReturnNa = "NA";
        //TU active on srm of TRV
        public const string ReturnAc = "AC";
        //load too high for destination
        public const string ReturnLh = "LH";
        //load too width for destination
        public const string ReturnLw = "LW";
        //sorter type message of sortDirector
        public const string ReturnSo = "SO";
        //merge type message of sortDirector
        public const string ReturnMm = "MM";
        //set barcode in AP
        public const string ReturnTi = "TI";
    }
}