using System;

namespace Kengic.Was.Connector.Prodave
{
    public class ProdaveHelper
    {
        public static byte[] ConvertIpToAddress(string ip)
        {
            var ips = ip.Split('.');
            var addr = new byte[] {0, 0, 0, 0, 0, 0};
            for (var i = 0; i < ips.Length; i++)
            {
                addr[i] = Convert.ToByte(ips[i]);
            }
            return addr;
        }

        /// <summary>
        ///     根据错误代码返回错误信息 例如int errCode=ActiveConn(1); sring errInfo =
        ///     GetErrInfo(err);
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns>
        ///     错误信息
        /// </returns>
        public static string GetErrorCode(int errCode)
        {
            switch (errCode)
            {
                case -1:
                    return "User-Defined  Error!"; //自定义错误,主要是参数传递错误!
                case 0x0000:
                    return "Success";
                case 0x0001:
                    return "Load dll failed";
                case 0x00E1:
                    return "User max";
                case 0x00E2:
                    return "SCP entry";
                case 0x00E7:
                    return "SCP board open";
                case 0x00E9:
                    return "No Windows server";
                case 0x00EA:
                    return "Protect";
                case 0x00CA:
                    return "SCP no resources";
                case 0x00CB:
                    return "SCP configuration";
                case 0x00CD:
                    return "SCP illegal";
                case 0x00CE:
                    return "SCP incorrect parameter";
                case 0x00CF:
                    return "SCP open device";
                case 0x00D0:
                    return "SCP board";
                case 0x00D1:
                    return "SCP software";
                case 0x00D2:
                    return "SCP memory";
                case 0x00D7:
                    return "SCP no meas";
                case 0x00D8:
                    return "SCP user mem";
                case 0x00DB:
                    return "SCP timeout";
                case 0x00F0:
                    return "SCP db file does not exist";
                case 0x00F1:
                    return "SCP no global dos memory";
                case 0x00F2:
                    return "SCP send not successful";
                case 0x00F3:
                    return "SCP receive not successful";
                case 0x00F4:
                    return "SCP no device available";
                case 0x00F5:
                    return "SCP illegal subsystem";
                case 0x00F6:
                    return "SCP illegal opcode";
                case 0x00F7:
                    return "SCP buffer too short";
                case 0x00F8:
                    return "SCP buffer1 too short";
                case 0x00F9:
                    return "SCP illegal protocol sequence";
                case 0x00FA:
                    return "SCP illegal PDU arrived";
                case 0x00FB:
                    return "SCP request error";
                case 0x00FC:
                    return "SCP no license";
                case 0x0101:
                    return "Connection is not established / parameterized";
                case 0x010a:
                    return "Negative Acknowledgment received / timeout errors";
                case 0x010c:
                    return "Data not available or locked";
                case 0x012A:
                    return "No system memory left";
                case 0x012E:
                    return "Incorrect parameter";
                case 0x0132:
                    return "No storage space in the DPRAM";
                case 0x0200:
                    return "xx";
                case 0x0201:
                    return "Falsche Schnittstelle angegeben";
                case 0x0202:
                    return "Incorrect interface indicated";
                case 0x0203:
                    return "Toolbox already installed";
                case 0x0204:
                    return "Toolbox with other compounds already installed";
                case 0x0205:
                    return "Toolbox is not installed";
                case 0x0206:
                    return "Handle can not be set";
                case 0x0207:
                    return "Data segment can not be blocked";
                case 0x0209:
                    return "Erroneous data field";
                case 0x0300:
                    return "Timer init error";
                case 0x0301:
                    return "Com init error";
                case 0x0302:
                    return "Module is too small, DW does not exist";
                case 0x0303:
                    return "Block boundary erschritten, number correct";
                case 0x0310:
                    return "Could not find any hardware";
                case 0x0311:
                    return "Hardware defective";
                case 0x0312:
                    return "Incorrect configuration parameters";
                case 0x0313:
                    return "Incorrect baud rate/interrupt vector";
                case 0x0314:
                    return "HSA incorrectly parameterized";
                case 0x0315:
                    return "Address already assigned";
                case 0x0316:
                    return "Device already assigned";
                case 0x0317:
                    return "Interrupt not available";
                case 0x0318:
                    return "Interrupt occupied";
                case 0x0319:
                    return "SAP not occupied";
                case 0x031A:
                    return "Could not find any remote station";
                case 0x031B:
                    return "syni error";
                case 0x031C:
                    return "System error";
                case 0x031D:
                    return "Error in buffer size";
                case 0x0320:
                    return "DLL/VxD not found";
                case 0x0321:
                    return "DLL function error";
                case 0x0330:
                    return "Version conflict";
                case 0x0331:
                    return "Com config error";
                case 0x0332:
                    return "smc timeout";
                case 0x0333:
                    return "Com not configured";
                case 0x0334:
                    return "Com not available";
                case 0x0335:
                    return "Serial drive in use";
                case 0x0336:
                    return "No connection";
                case 0x0337:
                    return "Job rejected";
                case 0x0380:
                    return "Internal error";
                case 0x0381:
                    return "Device not in Registry";
                case 0x0382:
                    return "L2 driver not in Registry";
                case 0x0384:
                    return "L4 driver not in Registry";
                case 0x03FF:
                    return "System error";
                case 0x4001:
                    return "Connection is not known";
                case 0x4002:
                    return "Connection is not established";
                case 0x4003:
                    return "Connection is being established";
                case 0x4004:
                    return "Connection is collapsed";
                case 0x0800:
                    return "Toolbox occupied";
                case 0x8001:
                    return "in this mode is not allowed";
                case 0x8101:
                    return "Hardware error";
                case 0x8103:
                    return "Object Access not allowed";
                case 0x8104:
                    return "Context is not supported";
                case 0x8105:
                    return "ungtige Address";
                case 0x8106:
                    return "Type (data) is not supported";
                case 0x8107:
                    return "Type (data) is not consistent";
                case 0x810A:
                    return "Object does not exist";
                case 0x8301:
                    return "Memory on CPU is not sufficient";
                case 0x8404:
                    return "grave error";
                case 0x8500:
                    return "Incorrect PDU Size";
                case 0x8702:
                    return "Invalid address";
                case 0xA0CE:
                    return "User occupied";
                case 0xA0CF:
                    return "User does not pick up";
                case 0xA0D4:
                    return
                        "Connection not available because modem prevents immediate redial (waiting time before repeat dial not kept to) ";
                case 0xA0D5:
                    return "No dial tone";
                case 0xD201:
                    return "Syntax error module name";
                case 0xD202:
                    return "Syntax error function parameter";
                case 0xD203:
                    return "Syntax error Bausteshortyp";
                case 0xD204:
                    return "no memory module in eingeketteter";
                case 0xD205:
                    return "Object already exists";
                case 0xD206:
                    return "Object already exists";
                case 0xD207:
                    return "Module available in the EPROM";
                case 0xD209:
                    return "Module does not exist";
                case 0xD20E:
                    return "no module present";
                case 0xD210:
                    return "Block number is too big";
                case 0xD241:
                    return "Protection level of function is not sufficient";
                case 0xD406:
                    return "Information not available";
                case 0xEF01:
                    return "Wrong ID2";
                case 0xFFFE:
                    return "unknown error FFFE hex";
                case 0xFFFF:
                    return "Timeout error. Interface KVD";
                default:
                    return "Unkonw error";
            }
        }
    }
}