using System.Runtime.InteropServices;
using System.Text;

namespace Kengic.Was.CrossCutting.Prodave
{
    public class Prodave6
    {
        public enum DatType : byte //PLC数据类型
        {
            Byte = 0x02,
            Word = 0x04,
            Dword = 0x06
        }

        public enum FieldType : byte //PLC区域类型
        {
            //Value types as ASCII characters区域类型对应的ASCII字符
            //data byte (d/D)
            //d = 100,
            D = 68,
            //input byte (e/E)
            //e = 101,
            E = 69,
            //output byte (a/A)
            //a = 97,
            A = 65,
            //memory byte (m/M)
            //m = 109,
            M = 77,
            //timer word (t/T),
            //t = 116,
            T = 84
        }

        public const int MaxConnections = 64; // 64 is default in PRODAVE
        public const int MaxDeviceName = 128; // e.g. "S7ONLINE"
        public const int MaxBuffers = 64; // 64 for blk_read() and blk_write() 
        public const int MaxBuffer = 65536; // Transfer buffer for error text) 

        /// <summary>
        ///     连接PLC操作
        /// </summary>
        /// <param name="connectionNum">连接号0-63</param>
        /// <param name="accessPoint">常值"S7ONLINE"</param>
        /// <param name="connectionAddressLength">
        ///     待连接plc地址属性表长度,字节为单位,常值9
        /// </param>
        /// <param name="connectionProperty">待连接plc地址属性表</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int LoadConnection_ex6(int connectionNum, string accessPoint, int connectionAddressLength,
            ref ConnectionProperty connectionProperty);


        /// <summary>
        ///     断开PLC操作
        /// </summary>
        /// <param name="connectionNum">连接号0-63</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int UnloadConnection_ex6(ushort connectionNum);


        /// <summary>
        ///     激活PLC连接操作
        /// </summary>
        /// <param name="connectionNum">连接号0-63</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int SetActiveConnection_ex6(ushort connectionNum);


        /// <summary>
        ///     PLC db区读取操作
        /// </summary>
        /// <param name="blockNum">data block号</param>
        /// <param name="dataType">要读取的数据类型</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要读取类型的数量</param>
        /// <param name="dataBufferLength">需要读取类型的缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <param name="bufferLength">缓冲区数据交互的长度</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int db_read_ex6(ushort blockNum, DatType dataType, ushort startAddress, ref uint dataAmount,
            uint dataBufferLength,
            ushort[] buffer, ref uint bufferLength);

        /// <summary>
        ///     PLC db区写入操作
        /// </summary>
        /// <param name="blockNum">data block号</param>
        /// <param name="dataType">要写入的数据类型</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要写入类型的数量</param>
        /// <param name="dataBufferLength">需要写入类型的缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int db_write_ex6(ushort blockNum, DatType dataType, ushort startAddress,
            ref uint dataAmount, uint dataBufferLength,
            ushort[] buffer);

        /// <summary>
        ///     PLC 任意区读取操作
        /// </summary>
        /// <param name="fieldType">要读取的区类型</param>
        /// <param name="blockNum">data block号(DB区特有,默认为0)</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要读取类型的数量</param>
        /// <param name="dataBufferLength">需要写入类型的缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <param name="bufferLength">缓冲区数据交互的长度</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int field_read_ex6(FieldType fieldType, ushort blockNum, ushort startAddress,
            uint dataAmount, uint dataBufferLength,
            byte[] buffer, ref uint bufferLength);

        /// <summary>
        ///     PLC 任意区写入操作
        /// </summary>
        /// <param name="fieldType">要写入的区类型</param>
        /// <param name="blockNum">data block号(DB区特有,默认为0)</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要读取类型的数量</param>
        /// <param name="dataBufferLength">需要写入类型的缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int field_write_ex6(FieldType fieldType, ushort blockNum, ushort startAddress,
            uint dataAmount, uint dataBufferLength,
            byte[] buffer);


        /// <summary>
        ///     PLC M区某字节的某位读取操作
        /// </summary>
        /// <param name="memoryByteNum">M区字节号</param>
        /// <param name="memoryBitNum">位号</param>
        /// <param name="value">当前的值(0/1)</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int mb_bittest_ex6(ushort memoryByteNum, ushort memoryBitNum, ref int value);

        /// <summary>
        ///     PLC M区某字节的某位写入操作
        /// </summary>
        /// <param name="memoryByteNum">M区字节号</param>
        /// <param name="memoryBitNum">位号</param>
        /// <param name="value">要写入的值(0/1)</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int mb_setbit_ex6(ushort memoryByteNum, ushort memoryBitNum, byte value);

        /// <summary>
        ///     200系列PLC 任意区读取操作
        /// </summary>
        /// <param name="fieldType">要读取的区类型</param>
        /// <param name="blockNum">data block号(DB区特有,默认为0)</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要读取类型的数量</param>
        /// <param name="dataBufferLength">缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <param name="bufferLength">缓冲区数据交互的长度</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int as200_field_read_ex6(FieldType fieldType, ushort blockNum, ushort startAddress,
            uint dataAmount, uint dataBufferLength,
            byte[] buffer, ref uint bufferLength);

        /// <summary>
        ///     200系列PLC 任意区写入操作
        /// </summary>
        /// <param name="fieldType">要写入的区类型</param>
        /// <param name="blockNum">data block号(DB区特有,默认为0)</param>
        /// <param name="startAddress">起始地址号</param>
        /// <param name="dataAmount">需要写入类型的数量</param>
        /// <param name="dataBufferLength">缓冲区长度,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int as200_field_write_ex6(FieldType fieldType, ushort blockNum, ushort startAddress,
            uint dataAmount, uint dataBufferLength,
            byte[] buffer);

        /// <summary>
        ///     200系列PLC M区某字节的某位读取操作
        /// </summary>
        /// <param name="memoryByteNum">M区字节号</param>
        /// <param name="memoryBitNum">位号</param>
        /// <param name="value">当前的值(0/1)</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int as200_mb_bittest_ex6(ushort memoryByteNum, ushort memoryBitNum, ref int value);

        /// <summary>
        ///     200系列PLC M区某字节的某位写入操作
        /// </summary>
        /// <param name="memoryByteNum">M区字节号</param>
        /// <param name="memoryBitNum">位号</param>
        /// <param name="value">要写入的值(0/1)</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int as200_mb_setbit_ex6(ushort memoryByteNum, ushort memoryBitNum, byte value);

        /// <summary>
        ///     诊断错误信息操作
        /// </summary>
        /// <param name="errorCode">错误代号</param>
        /// <param name="dataBufferLength">缓冲区大小,字节为单位</param>
        /// <param name="buffer">缓冲区</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int GetErrorMessage_ex6(int errorCode, uint dataBufferLength,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder buffer);

        /// <summary>
        ///     S7浮点数转换成PC浮点数
        /// </summary>
        /// <param name="gp">S7浮点数</param>
        /// <param name="pieee">PC浮点数</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int gp_2_float_ex6(uint gp, ref float pieee);

        /// <summary>
        ///     PC浮点数转换成S7浮点数
        /// </summary>
        /// <param name="ieee">PC浮点数</param>
        /// <param name="pgp">S7浮点数</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int float_2_gp_ex6(float ieee, ref uint pgp);

        /// <summary>
        ///     检测某字节的某位的值是0或1
        /// </summary>
        /// <param name="value">字节值</param>
        /// <param name="memoryBitNum">位号</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern int testbit_ex6(byte value, int memoryBitNum);

        /// <summary>
        ///     检测某字节的byte值转换成int数组
        /// </summary>
        /// <param name="value">byte值</param>
        /// <param name="buffer">int数组(长度为8)</param>
        [DllImport("Prodave6.dll")]
        public static extern void byte_2_bool_ex6(byte value, int[] buffer);

        /// <summary>
        ///     检测某字节的int数组转换成byte值
        /// </summary>
        /// <param name="buffer">int数组(长度为8)</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern byte bool_2_byte_ex6(int[] buffer);

        /// <summary>
        ///     交换数据的高低字节——16位数据
        /// </summary>
        /// <param name="value">待交换的数据</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern ushort kf_2_integer_ex6(ushort value); //16位数据——WORD

        /// <summary>
        ///     交换数据的高低字节——32位数据
        /// </summary>
        /// <param name="value">待交换的数据</param>
        /// <returns>
        /// </returns>
        [DllImport("Prodave6.dll")]
        public static extern uint kf_2_long_ex6(uint value); //32位数据——DWORD

        /// <summary>
        ///     交换数据缓冲区的的高低字节区，例如buffer[0]与buffer[1]，buffer[2]与buffer[3]交换
        /// </summary>
        /// <param name="buffer">待交换的数据缓冲区</param>
        /// <param name="amount">要交换的字节数,如Amount=buffer.Length，则交换全部缓冲</param>
        [DllImport("Prodave6.dll")]
        public static extern void swab_buffer_ex6(byte[] buffer, uint amount);

        /// <summary>
        ///     复制数据缓冲区
        /// </summary>
        /// <param name="targetBuffer">目的数据缓冲区</param>
        /// <param name="sourceBuffer">源数据缓冲区</param>
        /// <param name="amount">要复制的数量,字节为单位</param>
        [DllImport("Prodave6.dll")]
        public static extern void copy_buffer_ex6(byte[] targetBuffer, byte[] sourceBuffer, uint amount);


        /// <summary>
        ///     把二进制数组传换成BCD码的数组——16位数据 转换前是否先交换高低字节,转换后是否要交换高低字节
        ///     inBytechange为1则转换BCD码之前,先交换高低字节 outBytechange为1则转换BCD码之后,再交换高低字节
        ///     如果inBytechange和outBytechange都没有置1,则不发生高低位的交换 16位数据BCD码值的许可范围是：+999
        ///     —— -999
        /// </summary>
        /// <param name="values">要处理的数组</param>
        /// <param name="amount">要处理的字节数</param>
        /// <param name="inBytechange"></param>
        /// <param name="outBytechange"></param>
        [DllImport("Prodave6.dll")]
        public static extern void ushort_2_bcd_ex6(ushort[] values, uint amount, int inBytechange, int outBytechange);


        /// <summary>
        ///     把二进制数组传换成BCD码的数组——32位数据 转换前是否先交换高低字节,转换后是否要交换高低字节
        ///     inBytechange为1则转换BCD码之前,先交换高低字节 outBytechange为1则转换BCD码之后,再交换高低字节
        ///     如果inBytechange和outBytechange都没有置1,则不发生高低位的交换 32位数据BCD码值的许可范围是：+9 999
        ///     999 —— -9 999 999
        /// </summary>
        /// <param name="values">要处理的数组</param>
        /// <param name="amount">要处理的字节数</param>
        /// <param name="inBytechange"></param>
        /// <param name="outBytechange"></param>
        [DllImport("Prodave6.dll")] //     
        public static extern void ulong_2_bcd_ex6(uint[] values, uint amount, int inBytechange, int outBytechange);

        /// <summary>
        ///     把BCD码的数组传换成二进制数组——16位数据 转换前是否先交换高低字节,转换后是否要交换高低字节
        ///     inBytechange为1则转换BCD码之前,先交换高低字节 outBytechange为1则转换BCD码之后,再交换高低字节
        ///     如果inBytechange和outBytechange都没有置1,则不发生高低位的交换 16位数据BCD码值的许可范围是：+999
        ///     —— -999
        /// </summary>
        /// <param name="values">要处理的数组</param>
        /// <param name="amount">要处理的字节数</param>
        /// <param name="inBytechange"></param>
        /// <param name="outBytechange"></param>
        [DllImport("Prodave6.dll")]
        public static extern void bcd_2_ushort_ex6(ushort[] values, uint amount, int inBytechange, int outBytechange);


        /// <summary>
        ///     把BCD码的数组传换成二进制数组——32位数据 转换前是否先交换高低字节,，转换后是否要交换高低字节
        ///     inBytechange为1则转换BCD码之前,先交换高低字节 outBytechange为1则转换BCD码之后,再交换高低字节
        ///     如果inBytechange和outBytechange都没有置1,则不发生高低位的交换 32位数据BCD码值的许可范围是,+9 999
        ///     999 —— -9 999 999
        /// </summary>
        /// <param name="values">要处理的数组</param>
        /// <param name="amount">要处理的字节数</param>
        /// <param name="inBytechange"></param>
        /// <param name="outBytechange"></param>
        [DllImport("Prodave6.dll")]
        public static extern void bcd_2_ulong_ex6(uint[] values, uint amount, int inBytechange, int outBytechange);


        /// <summary>
        ///     查看64个连接中哪些被占用,哪些已经建立
        /// </summary>
        /// <param name="dataBufferLength">传输缓冲的字节长度</param>
        /// <param name="buffer">64位长度的数组(0或1)</param>
        [DllImport("Prodave6.dll")]
        public static extern void GetLoadedConnections_ex6(uint dataBufferLength, int[] buffer);

        /// <summary>
        ///     将高低2个byte转换成1个word
        /// </summary>
        /// <param name="dbb0"></param>
        /// <param name="dbb1"></param>
        /// <returns>
        /// </returns>
        public static ushort bytes_2_word(byte dbb0, byte dbb1) //
            => (ushort) ((dbb0 << 8) | dbb1);

        /// <summary>
        ///     将高低4个byte转换成1个dword
        /// </summary>
        /// <param name="dbb0"></param>
        /// <param name="dbb1"></param>
        /// <param name="dbb2"></param>
        /// <param name="dbb3"></param>
        /// <returns>
        /// </returns>
        public static uint bytes_2_dword(byte dbb0, byte dbb1, byte dbb2, byte dbb3) //
        {
            var dbd0 = (uint) (dbb0*16777216 + dbb1*65536 + dbb2*256 + dbb3);
            return dbd0;
        }

        /// <summary>
        ///     将高低2个word转换成1个dword
        /// </summary>
        /// <param name="dbw0"></param>
        /// <param name="dbw2"></param>
        /// <returns>
        /// </returns>
        public static uint words_2_dword(ushort dbw0, ushort dbw2)
        {
            var dbd0 = (uint) (dbw0*65536 + dbw2);
            return dbd0;
        }

        /// <summary>
        ///     将word拆分为2个byte
        /// </summary>
        /// <param name="dbw0"></param>
        /// <returns>
        /// </returns>
        public static byte[] word_2_bytes(ushort dbw0)
        {
            var bytes = new byte[2];
            bytes[0] = (byte) (dbw0 & 0xFF);
            bytes[1] = (byte) ((dbw0 & 0xFF00) >> 8);

            return bytes;
        }

        /// <summary>
        ///     将dword拆分为4个byte
        /// </summary>
        /// <param name="dbd0"></param>
        /// <returns>
        /// </returns>
        public static byte[] dword_2_bytes(uint dbd0)
        {
            var bytes = new byte[4];
            bytes[0] = (byte) (dbd0/16777216);
            dbd0 = dbd0%16777216;
            bytes[1] = (byte) (dbd0/65536);
            dbd0 = dbd0%65536;
            bytes[2] = (byte) (dbd0/256);
            bytes[3] = (byte) (dbd0%256);
            return bytes;
        }

        /// <summary>
        ///     将dword拆分为2个word
        /// </summary>
        /// <param name="dbd0"></param>
        /// <returns>
        /// </returns>
        public static ushort[] dword_2_words(uint dbd0)
        {
            var words = new ushort[2];
            words[0] = (ushort) (dbd0/65536);
            words[1] = (ushort) (dbd0%65536);
            return words;
        }

        public struct ConnectionProperty //待连接plc地址属性表
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            //public CON_ADR_TYPE Adr; // connection address
            public byte[] Address; // connection address

            // MPI/PB station address (2)
            // IP address (192.168.0.1)
            // MAC address (08-00-06-01-AA-BB)
            public byte AddressType; // Type of address: MPI/PB (1), IP (2), MAC (3)

            public byte SlotNum; // Slot Num
            public byte RackNum; // Rack Num
        }
    }
}