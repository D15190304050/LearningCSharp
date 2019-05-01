using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.String
{
    /// <summary>
    /// The BinaryStandardOutput class provides static methods for converting primitive type variables
    /// (bool, byte, char, int, long) to sequence of bits and writing them to standard output.
    /// Using big-endian (most significant first).
    /// </summary>
    public static class BinaryStandardOutput
    {
        /// <summary>
        /// 8-bit buffer of bits to write out.
        /// </summary>
        private static int buffer;

        /// <summary>
        /// Number of bits remaining in the buffer.
        /// </summary>
        private static int left;

        /// <summary>
        /// Initializes the binary standard output buffer.
        /// </summary>
        static BinaryStandardOutput()
        {
            buffer = 0;
            left = 0;
        }

        /// <summary>
        /// Simulate the unsigned shift right operation.
        /// </summary>
        /// <param name="iValue">The int to make shift right.</param>
        /// <param name="offset">The offset.</param>
        private static int UnsignedShiftRight(int iValue, int offset)
        {
            return ((iValue >> offset) & 0x7FFFFFFF);
        }

        /// <summary>
        /// Simulate the unsigned shift right operation.
        /// </summary>
        /// <param name="lValue">The long to make shift right.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static long UnsignedShiftRight(long lValue, int offset)
        {
            return ((lValue >> offset) & 0x7FFFFFFFFFFFFFFF);
        }

        /// <summary>
        /// Write the specified bit to standard output.
        /// </summary>
        /// <param name="bit">The bit to write.</param>
        private static void WriteBit(bool bit)
        {
            // Add bit to buffer.
            buffer <<= 1;

            if (bit)
                buffer |= 1;

            // If buffer is full (8 bits), write out as a single byte.
            left++;
            if (left == 8)
                ClearBuffer();
        }

        /// <summary>
        /// Write out any remaining bits in buffer to standard output, padding with 0s.
        /// </summary>
        private static void ClearBuffer()
        {
            if (left == 0)
                return;

            if (left > 0)
                buffer <<= (8 - left);

            Console.Write(buffer);
            left = 0;
            buffer = 0;
        }

        /// <summary>
        /// Write the 8-bit byte to standard output.
        /// </summary>
        /// <param name="iVal"></param>
        private static void WriteByte(int iVal)
        {
            // Check the range of the argument.
            if ((iVal < 0) || (iVal > 255))
                throw new ArgumentException("The argument for WriteByte must between [0, 255]");

            // Optimized if byte-aligned.
            if (left == 0)
            {
                Console.Write(iVal);
                return;
            }

            // Otherwise write one bit at a time.
            for (int i = 0; i < 8; i++)
            {
                bool bit = (UnsignedShiftRight(iVal, 8 - i - 1) & 1) == 1;
                WriteBit(bit);
            }
        }

        /// <summary>
        /// Flush the standard output, padding 0s if number of bits write so far is not a multiple of 8.
        /// </summary>
        public static void Flush()
        {
            ClearBuffer();
        }

        /// <summary>
        /// Write the specified bit to standard output.
        /// </summary>
        /// <param name="bit">The bool to write.</param>
        public static void Write(bool bit)
        {
            WriteBit(bit);
        }

        /// <summary>
        /// Write the 8-bit byte to standard output.
        /// </summary>
        /// <param name="bValue">The byte to write.</param>
        public static void Write(byte bValue)
        {
            WriteByte(bValue & 0xFF);
        }

        /// <summary>
        /// Write the 32-bit int to standard output.
        /// </summary>
        /// <param name="iValue">The int to write.</param>
        public static void Write(int iValue)
        {
            for (int offset = 24; offset >= 0; offset -= 8)
                WriteByte(UnsignedShiftRight(iValue, offset) & 0xFF);
        }

        /// <summary>
        /// Write the length-bit int to standard output.
        /// </summary>
        /// <param name="iValue">The int to write.</param>
        /// <param name="length">The number of relevant bits in the char.</param>
        public static void Write(int iValue, int length)
        {
            if (length == 32)
            {
                Write(iValue);
                return;
            }

            if ((length < 1) || (length > 32))
                throw new ArgumentException("Illegal value for length = " + length);
            if ((iValue < 0) || (iValue >= (1 << length)))
                throw new ArgumentException("Illegal " + length + "-bit char = " + iValue);

            for (int i = 0; i < length; i++)
            {
                bool bit = (UnsignedShiftRight(iValue, length - i - 1) & 1) == 1;
                WriteBit(bit);
            }
        }

        /// <summary>
        /// Write the 64-bit double to standard output.
        /// </summary>
        /// <param name="">The double to write.</param>
        public static void Write(double dValue)
        {
            Write(BitConverter.DoubleToInt64Bits(dValue));
        }

        /// <summary>
        /// Write the 64-bit long to standard output.
        /// </summary>
        /// <param name="lValue">The long to write.</param>
        public static void Write(long lValue)
        {
            for (int offset = 56; offset >= 0; offset -= 8)
                WriteByte((int)((UnsignedShiftRight(lValue, offset) & 0xFF)));
        }

        /// <summary>
        /// Write the 16-bit short to standard output.
        /// </summary>
        /// <param name="sValue">The short to write.</param>
        public static void Write(short sValue)
        {
            WriteByte(UnsignedShiftRight(sValue, 8) & 0xFF);
            WriteByte(sValue & 0xFF);
        }

        /// <summary>
        /// Write the 8-bit char to standard output.
        /// </summary>
        /// <param name="c">The char to write.</param>
        public static void Write(char c)
        {
            if ((c < 0) || (c > 255))
                throw new ArgumentOutOfRangeException("Illegal 8-bit char = " + c);
            WriteByte(c);
        }

        /// <summary>
        /// Write the length-bit char to standard output.
        /// </summary>
        /// <param name="c">The char to write.</param>
        /// <param name="length">The number of relevant bits in the char.</param>
        public static void Write(char c, int length)
        {
            if (length == 8)
            {
                Write(c);
                return;
            }

            if ((length < 1) || (length > 16))
                throw new ArgumentOutOfRangeException("Illegal value for length = " + length);
            if (c >= (1 << length))
                throw new ArgumentException("Illegal " + length + "-bit char = " + c);

            for (int i = 0; i < length; i++)
            {
                bool bit = (UnsignedShiftRight(c, length - i - 1) & 1) == 1;
                WriteBit(bit);
            }
        }

        /// <summary>
        /// Write the string of 8-bit characters to standard output.
        /// </summary>
        /// <param name="s">The string to write.</param>
        public static void Write(string s)
        {
            for (int i = 0; i < s.Length; i++)
                Write(s[i]);
        }

        /// <summary>
        /// Write the string of length-bit characters to standard output.
        /// </summary>
        /// <param name="s">The string to write.</param>
        /// <param name="length">The number of relevant bits in each character.</param>
        public static void Write(string s, int length)
        {
            for (int i = 0; i < s.Length; i++)
                Write(s[i], length);
        }
    }
}