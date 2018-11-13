using System;
using System.Runtime.InteropServices;

namespace I2cDev
{
    // This is a port of the linux i2c-dev.h inline functions for the purpose of using
    // GrovePi+  sensors with .NET Core applications running on a Raspberry PI.
    // The constants and structures used by the I2C_RDWR ioctl call are not included
    // since these are not used by GrovePi.
    // Note/warning: not all i2c-dev functions have been tested!
    public class I2cDev
    {
        /* To determine what functionality is present */
        public static class I2cFunc
        {
            public const int I2C_FUNC_I2C	= 0x00000001;
            public const int I2C_FUNC_10BIT_ADDR = 0x00000002;
            public const int I2C_FUNC_PROTOCOL_MANGLING = 0x00000004; /* I2C_M_{REV_DIR_ADDR,NOSTART,..} */
            public const int I2C_FUNC_SMBUS_PEC = 0x00000008;
            public const int I2C_FUNC_SMBUS_BLOCK_PROC_CALL = 0x00008000; /* SMBus 2.0 */
            public const int I2C_FUNC_SMBUS_QUICK = 0x00010000;
            public const int I2C_FUNC_SMBUS_READ_BYTE = 0x00020000;
            public const int I2C_FUNC_SMBUS_WRITE_BYTE	= 0x00040000;
            public const int I2C_FUNC_SMBUS_READ_BYTE_DATA = 0x00080000;
            public const int I2C_FUNC_SMBUS_WRITE_BYTE_DATA = 0x00100000;
            public const int I2C_FUNC_SMBUS_READ_WORD_DATA = 0x00200000;
            public const int I2C_FUNC_SMBUS_WRITE_WORD_DATA = 0x00400000;
            public const int I2C_FUNC_SMBUS_PROC_CALL = 0x00800000;
            public const int I2C_FUNC_SMBUS_READ_BLOCK_DATA = 0x01000000;
            public const int I2C_FUNC_SMBUS_WRITE_BLOCK_DATA = 0x02000000;
            public const int I2C_FUNC_SMBUS_READ_I2C_BLOCK = 0x04000000; /* I2C-like block xfer  */
            public const int I2C_FUNC_SMBUS_WRITE_I2C_BLOCK = 0x08000000; /* w/ 1-byte reg. addr. */
            public const int I2C_FUNC_SMBUS_BYTE = (I2C_FUNC_SMBUS_READ_BYTE | I2C_FUNC_SMBUS_WRITE_BYTE);
            public const int I2C_FUNC_SMBUS_BYTE_DATA = (I2C_FUNC_SMBUS_READ_BYTE_DATA | I2C_FUNC_SMBUS_WRITE_BYTE_DATA);
            public const int I2C_FUNC_SMBUS_WORD_DATA = (I2C_FUNC_SMBUS_READ_WORD_DATA | I2C_FUNC_SMBUS_WRITE_WORD_DATA);
            public const int I2C_FUNC_SMBUS_BLOCK_DATA = (I2C_FUNC_SMBUS_READ_BLOCK_DATA | I2C_FUNC_SMBUS_WRITE_BLOCK_DATA);
            public const int I2C_FUNC_SMBUS_I2C_BLOCK = (I2C_FUNC_SMBUS_READ_I2C_BLOCK | I2C_FUNC_SMBUS_WRITE_I2C_BLOCK);
            /* Old name, for compatibility */
            public const int I2C_FUNC_SMBUS_HWPEC_CALC = I2C_FUNC_SMBUS_PEC;
        }

        /*
        * Data for SMBus Messages
        */
        public const int I2C_SMBUS_BLOCK_MAX = 32;	/* As specified in SMBus standard */
        public const int I2C_SMBUS_I2C_BLOCK_MAX = 32;	/* Not specified but we use same structure */

        // i2c_smbus_data_block, i2c_smbus_data_byte_word are used
        // to emulate the following union for P/Invoke marshaling
        //
        // union i2c_smbus_data {
        //     __u8 byte;
        //     __u16 word;
        //     __u8 block[I2C_SMBUS_BLOCK_MAX + 2]; /* block[0] is used for length */
        //                                                 /* and one more for PEC */
        // };
        //
        // Per https://docs.microsoft.com/en-us/dotnet/framework/interop/marshaling-classes-structures-and-unions
        // value types and reference types are not permitted to overlap in managed code, so 2 structures are used

        [StructLayout(LayoutKind.Sequential)]
        public struct i2c_smbus_data_block
        {
            /* block[0] is used for length */
            /* and one more for PEC */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = I2C_SMBUS_BLOCK_MAX + 2)]
            public byte[] block;// Constants.I2C_SMBUS_BLOCK_MAX + 2
        }

        [StructLayout(LayoutKind.Explicit, Size= I2C_SMBUS_BLOCK_MAX + 2)]
        public struct i2c_smbus_data_byte_word
        {
            [FieldOffset(0)]
            public byte byteValue;

            [FieldOffset(0)]
            public UInt16 word;
        }
    
     
        /* smbus_access read or write markers */
        public class I2cSmbusAccess
        {
            public const byte I2C_SMBUS_READ = 1;
            public const byte I2C_SMBUS_WRITE = 0;
        }

        /* SMBus transaction types (size parameter in the above functions)
        Note: these no longer correspond to the (arbitrary) PIIX4 internal codes! */
        public class I2cSmbusTransactionType
        { 
            public const byte I2C_SMBUS_QUICK = 0;
            public const byte I2C_SMBUS_BYTE = 1;
            public const byte I2C_SMBUS_BYTE_DATA = 2;
            public const byte I2C_SMBUS_WORD_DATA = 3;
            public const byte I2C_SMBUS_PROC_CALL = 4;
            public const byte I2C_SMBUS_BLOCK_DATA = 5;
            public const byte I2C_SMBUS_I2C_BLOCK_BROKEN =  6;
            public const byte I2C_SMBUS_BLOCK_PROC_CALL = 7;		/* SMBus 2.0 */
            public const byte I2C_SMBUS_I2C_BLOCK_DATA = 8;
        }


        /* /dev/i2c-X ioctl commands.  The ioctl's parameter is always an
        * unsigned long, except for:
        *	- I2C_FUNCS, takes pointer to an unsigned long
        *	- I2C_RDWR, takes pointer to struct i2c_rdwr_ioctl_data
        *	- I2C_SMBUS, takes pointer to struct i2c_smbus_ioctl_data
        */
        public static class I2cIoctlCommands
        {
            /* number of times a device address should
               be polled when not acknowledging */
            public const int I2C_RETRIES = 0x0701;	
            
            /* set timeout in units of 10 ms */
            public const int I2C_TIMEOUT = 0x0702;

            /* NOTE: Slave address is 7 or 10 bits, but 10-bit addresses
             * are NOT supported! (due to code brokenness)
             */

            /* Use this slave address */
            public const int I2C_SLAVE = 0x0703;

            /* Use this slave address, even if it
               is already in use by a driver! */ 
            public const int I2C_SLAVE_FORCE = 0x0706;	
            
            /* 0 for 7 bit addrs, != 0 for 10 bit */
            public const int I2C_TENBIT = 0x0704;	

            /* Get the adapter functionality mask */
            public const int I2C_FUNCS = 0x0705;	

            /* Combined R/W transfer (one STOP only) */
            public const int I2C_RDWR =	0x0707;	

            /* != 0 to use PEC with SMBus */
            public const int I2C_PEC = 0x0708;	

            /* SMBus transfer */
            public const int I2C_SMBUS = 0x0720;	

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct i2c_smbus_ioctl_data
        {
            public byte read_write;
            public byte command;
            public UInt32 size;
            public IntPtr i2c_smbus_data;
        };


       
        public static int errno { get; private set;}
      
        [DllImport("libc.so.6", SetLastError = true)]
        public static extern int open(string fileName, int mode);

        [DllImport("libc.so.6", SetLastError = true)]
        public extern static int ioctl(int fd, int request, int data);

        [DllImport("libc.so.6", SetLastError = true)]
        public extern static int ioctl(int fd, int request, ref i2c_smbus_ioctl_data data);

        [DllImport("lib-i2c-helper.so", SetLastError = true)]
        private extern static int helper_ioctl(int fd, int request, ref i2c_smbus_ioctl_data data);

        public static int i2c_smbus_access(int file,
                                            char read_write,
                                            byte command,
                                            int size)
        
        {
            i2c_smbus_ioctl_data args = new i2c_smbus_ioctl_data();
            args.read_write = (byte)read_write;
            args.command = command;
            args.size = (uint)size;
            args.i2c_smbus_data = IntPtr.Zero;

            var result = ioctl(file, I2cIoctlCommands.I2C_SMBUS, ref args);
            errno = Marshal.GetLastWin32Error();

            return result;
        }

        public static int i2c_smbus_access(int file,
                                            char read_write,
                                            byte command,
                                            int size,
                                            ref i2c_smbus_data_byte_word data)
        {
           
            IntPtr buffer = IntPtr.Zero;
            
            try
            {
                i2c_smbus_ioctl_data args = new i2c_smbus_ioctl_data();
                args.read_write = (byte)read_write;
                args.command = command;
                args.size = (uint)size;
                buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(data));
                Marshal.StructureToPtr(data, buffer, false);
                args.i2c_smbus_data = buffer;
            
                var result = ioctl(file, I2cIoctlCommands.I2C_SMBUS, ref args);
                errno = Marshal.GetLastWin32Error();

                    
                data = (i2c_smbus_data_byte_word)Marshal.PtrToStructure(args.i2c_smbus_data,
                                                                typeof(i2c_smbus_data_byte_word));        
                return result;                                                                
            }
            finally
            {   
                if (buffer != IntPtr.Zero)     
                    Marshal.FreeCoTaskMem(buffer);
            }
        }

      

        public static int i2c_smbus_access(int file,
                                            char read_write,
                                            byte command,
                                            int size,
                                            ref i2c_smbus_data_block data)
        {
      
          
                
            i2c_smbus_ioctl_data args = new i2c_smbus_ioctl_data();
            args.read_write = (byte)read_write;
            args.command = command;
            args.size = (uint)size;
          
            var buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(data));
            Marshal.StructureToPtr(data, buffer, false);
            args.i2c_smbus_data = buffer;
          
            var result = ioctl(file, I2cIoctlCommands.I2C_SMBUS, ref args);
            errno = Marshal.GetLastWin32Error();

                
            data = (i2c_smbus_data_block)Marshal.PtrToStructure(args.i2c_smbus_data,
                                                            typeof(i2c_smbus_data_block));
            Marshal.FreeCoTaskMem(buffer);

            return result;
        }

        public static int i2c_smbus_write_quick(int file, byte value)
        {
            return i2c_smbus_access(file,(char)value,0,I2cSmbusTransactionType.I2C_SMBUS_QUICK);
        }

        public static int i2c_smbus_read_byte(int file)
        {
            i2c_smbus_data_block data = new i2c_smbus_data_block();
            data.block = new byte[I2C_SMBUS_BLOCK_MAX + 2];
            if (i2c_smbus_access(file, (char)I2cSmbusAccess.I2C_SMBUS_READ, 0, I2cSmbusTransactionType.I2C_SMBUS_BYTE, ref data) != 0)
                return -1;
            else
                return (0x0FF & data.block[0]);
        }

        public static int i2c_smbus_write_byte(int file, byte value)
        {
            return i2c_smbus_access(file,(char)I2cSmbusAccess.I2C_SMBUS_WRITE,value,
                                    I2cSmbusTransactionType.I2C_SMBUS_BYTE);         
        }

        public static int i2c_smbus_read_byte_data(int file, byte command)
        {
            i2c_smbus_data_byte_word data = new i2c_smbus_data_byte_word();
            
            if (i2c_smbus_access(file,(char) I2cSmbusAccess.I2C_SMBUS_READ,command,
                                I2cSmbusTransactionType.I2C_SMBUS_BYTE_DATA, ref data) != 0)
                return -1;
            else
                return 0x0FF & data.byteValue;
        }

        public static int i2c_smbus_write_byte_data(int file, byte command,
                                                    byte value)
        {
            i2c_smbus_data_byte_word data = new i2c_smbus_data_byte_word();

            data.byteValue = value;
            return i2c_smbus_access(file, (char)I2cSmbusAccess.I2C_SMBUS_WRITE, command,
                                    I2cSmbusTransactionType.I2C_SMBUS_BYTE_DATA, ref data);
        }

        public static int i2c_smbus_read_word_data(int file, byte command)
        {
            i2c_smbus_data_byte_word data = new i2c_smbus_data_byte_word();
            if (i2c_smbus_access(file,(char)I2cSmbusAccess.I2C_SMBUS_READ, command,
                                I2cSmbusTransactionType.I2C_SMBUS_WORD_DATA, ref data) != 0)
                return -1;
            else
                return 0x0FFFF & data.word;
        }

        public static int i2c_smbus_write_word_data(int file,byte command,
                                                    UInt16 value)
        {
            i2c_smbus_data_byte_word data = new i2c_smbus_data_byte_word();
            data.word = value;
            return i2c_smbus_access(file,(char)I2cSmbusAccess.I2C_SMBUS_WRITE,command,
                                    I2cSmbusTransactionType.I2C_SMBUS_WORD_DATA, ref data);
        }

        public static int i2c_smbus_process_call(int file, byte command, UInt16 value)
        {
            i2c_smbus_data_byte_word data = new i2c_smbus_data_byte_word();
            data.word = value;
            if (i2c_smbus_access(file,(char)I2cSmbusAccess.I2C_SMBUS_WRITE,command,
                                I2cSmbusTransactionType.I2C_SMBUS_PROC_CALL,ref data) != 0)
                return -1;
            else
                return 0x0FFFF & data.word;
        }



         /* Returns the number of read bytes */
        public static int i2c_smbus_read_block_data(int file, byte command,
                                                      byte[] values)
        {
            i2c_smbus_data_block data = new i2c_smbus_data_block();
            data.block = new byte[I2C_SMBUS_BLOCK_MAX + 2];
            int i;
            if (i2c_smbus_access(file, (char)I2cSmbusAccess.I2C_SMBUS_READ, command,
                                 I2cSmbusTransactionType.I2C_SMBUS_BLOCK_DATA, ref data) != 0)
                return -1;
            else
            {
                for (i = 1; i <= data.block[0]; i++)
                    values[i - 1] = data.block[i];
                return data.block[0];
            }
        }

        public static int i2c_smbus_write_block_data(int file, byte command,
                                                       byte length, byte[] values)
        {
            i2c_smbus_data_block data = new i2c_smbus_data_block();
            data.block = new byte[I2C_SMBUS_BLOCK_MAX + 2];
            int i;
            if (length > 32)
                length = 32;
            for (i = 1; i <= length; i++)
                data.block[i] = values[i - 1];
            data.block[0] = length;
            return i2c_smbus_access(file, (char)I2cSmbusAccess.I2C_SMBUS_WRITE, command,
                                    I2cSmbusTransactionType.I2C_SMBUS_BLOCK_DATA, ref data);
        }

        /* Returns the number of read bytes */
        /* Until kernel 2.6.22, the length is hardcoded to 32 bytes. If you
        ask for less than 32 bytes, your code will only work with kernels
        2.6.23 and later. */
        public static int i2c_smbus_read_i2c_block_data(int file, byte command,
                                                        byte length, byte[] values)
        {
            i2c_smbus_data_block data = new i2c_smbus_data_block();
            data.block = new byte[I2C_SMBUS_BLOCK_MAX + 2];
            int i;

            if (length > 32)
                length = 32;
            data.block[0] = length;
            if (i2c_smbus_access(file,(char)I2cSmbusAccess.I2C_SMBUS_READ,command,
                                length == 32 ? I2cSmbusTransactionType.I2C_SMBUS_I2C_BLOCK_BROKEN :
                                I2cSmbusTransactionType.I2C_SMBUS_I2C_BLOCK_DATA, ref data) != 0)
                return -1;
            else {
                for (i = 1; i <= data.block[0]; i++)
                    values[i-1] = data.block[i];
                return data.block[0];
            }
        }

        public static int i2c_smbus_write_i2c_block_data(int file, byte command,
                                                              byte length,
                                                              byte[] values)
        {
            i2c_smbus_data_block data = new i2c_smbus_data_block();
            data.block = new byte[I2C_SMBUS_BLOCK_MAX + 2];
            int i;
            if (length > 32)
                length = 32;

            for (i = 1; i <= length; i++)
                data.block[i] = values[i - 1];
            data.block[0] = length;
            return i2c_smbus_access(file, (char)I2cSmbusAccess.I2C_SMBUS_WRITE, command,
                                    I2cSmbusTransactionType.I2C_SMBUS_I2C_BLOCK_BROKEN,  ref data);
        }

    }
}