using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    using System.IO;

    /// <summary>
    /// The BinaryStandardInput class provides methods for reading in bits from standard input, either one bit a time
    /// (as a boolean), 8 bits at a time (as a byte or char), 16 bits a time (as an int16), 32 bits a time (as an int),
    /// or 64 bits a time (as a double or int64).
    /// </summary>
    public static class BinaryStandardInput
    {
        /// <summary>
        /// End of file.
        /// </summary>
        private const int EOF = -1;

        /// <summary>
        /// One character buffer.
        /// </summary>
        private static int buffer;

        /// <summary>
        /// Number of bits left in buffer.
        /// </summary>
        private static int left;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static BinaryStandardInput()
        {
            FillBuffer();
        }

        /// <summary>
        /// Read in a char.
        /// </summary>
        private static void FillBuffer()
        {
            try
            {
                buffer = Console.Read();
                left = 8;
            }
            catch (IOException e)
            {
                Console.WriteLine("EOF");
                buffer = EOF;
                left = -1;
            }
        }

        /// <summary>
        /// Check whether the buffer is empty, read in another char if the buffer is empty.
        /// </summary>
        private static void CheckNextChar()
        {
            if (left == 0)
                FillBuffer();
        }

        /// <summary>
        /// Returns true if nad only if standard input is empty, false otherwise.
        /// </summary>
        /// <returns>True if nad only if standard input is empty, false otherwise.</returns>
        public static bool IsEmpty()
        {
            return buffer == EOF;
        }

        /// <summary>
        /// Check whether the input stream is empty, throw an IOException if is empty.
        /// </summary>
        private static void CheckIsEmpty()
        {
            if (IsEmpty())
                throw new IOException("Reading from empty input stream.");
        }

        /// <summary>
        /// Reads the next bit of data from standard input and return as a bool variable.
        /// </summary>
        /// <returns>
        /// The next bit of data from standard input and return as a bool variable.
        /// Meaning: true for 1 and false for 0.
        /// </returns>
        public static bool ReadBoolean()
        {
            CheckIsEmpty();
            left--;
            bool bit = (((buffer >> left) & 1) == 1);
            CheckNextChar();
            return bit;
        }

        /// <summary>
        /// Shifts an integer value (in 0) to the right by a specified number of bits which means offset.
        /// </summary>
        /// <param name="original">The integer value which need to be shifted right.</param>
        /// <param name="offset">Number of bits need to be shifted right.</param>
        /// <returns></returns>
        private static int ShiftRight(int original, int offset)
        {
            offset &= 0x1F;
            while (offset != 0)
            {
                original >>= 1;
                original &= 0x7FFFFFFF;
                offset--;
            }
            return original;
        }

        /// <summary>
        /// Reads the next 8 bits from standard input and returns as a 8-bits char variable.
        /// </summary>
        /// <returns>
        /// The next 8 bits from standard input and returns as a 8-bits char variable.
        /// Note that a char is a 16-bit type.
        /// </returns>
        public static char ReadChar()
        {
            CheckIsEmpty();

            // Sepcial case when aligned byte.
            if (left == 8)
            {
                int x = buffer;
                FillBuffer();
                return (char)x;
            }

            // Combine last left bites of current buffer with first (8 - left) bits of new buffer.
            int c = buffer;
            c <<= (8 - left);
            int oldLeft = left;
            FillBuffer();
            CheckIsEmpty();
            left = oldLeft;
            c |= ShiftRight(buffer, left);
            return (char)(c & 0xFF);
            
            // The above code doesn't quite work for the last character if left = 8.
            // Because buffer will be -1, so there is a special case for aligned byte.
        }

        /// <summary>
        /// Reads the next r bits from standard input and returns as a r-bit character.
        /// </summary>
        /// <param name="bitLength">Number of bits to read.</param>
        /// <returns>The next r bits from standard input as a char variable.</returns>
        public static char ReadChar(int bitLength)
        {
            if ((bitLength < 1) || (bitLength > 16))
                throw new ArgumentException("Illegal value of bitLength = " + bitLength);

            // Optimize bitLength = 8 case.
            if (bitLength == 8)
                return ReadChar();

            int c = 0;
            for (int i = 0; i < bitLength; i++)
            {
                c <<= 1;
                bool nextBit = ReadBoolean();
                if (nextBit)
                    c |= 1;
            }

            return (char)c;
        }

        /// <summary>
        /// Reads the remaining bytes of data from standard input and returns as a string.
        /// </summary>
        /// <returns>The remaining bytes of data from standard input and returns as a string.</returns>
        public static string ReadString()
        {
            CheckIsEmpty();

            StringBuilder sb = new StringBuilder();
            while (!IsEmpty())
            {
                char c = ReadChar();
                if (c == 13)
                    break;
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads the next 16 bits from standard input and returns as a 16-bit short.
        /// </summary>
        /// <returns>The next 16 bits from standard input and returns as a 16-bit short.</returns>
        public static short ReadInt16()
        {
            short x = 0;
            for (int i = 0; i < 2; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= ((short)c);
            }
            return x;
        }

        /// <summary>
        /// Reads the next 32 bits from standard input and returns as a 32-bit int.
        /// </summary>
        /// <returns>The next 32 bits from standard input and returns as a 32-bit int.</returns>
        public static int ReadInt()
        {
            int x = 0;
            for (int i = 0; i < 4; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= c;
            }
            return x;
        }

        /// <summary>
        /// Reads the next 64 bits from standard input and returns as a 64-bit long.
        /// </summary>
        /// <returns>The next 64 bits from standard input and returns as a 64-bit long.</returns>
        public static long ReadLong()
        {
            long x = 0;
            for (int i = 0; i < 8; i++)
            {
                char c = ReadChar();
                x <<= 8;
                x |= c;
            }
            return x;
        }

        /// <summary>
        /// Reads the next 64 bits from standard input and returns as a 64-bit double.
        /// </summary>
        /// <returns>The next 64 bits from standard input and returns as a 64-bit double.</returns>
        public static double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
        }

        /// <summary>
        /// Reads the next 8 bits from standard input and returns as a 8-bit byte.
        /// </summary>
        /// <returns>The next 8 bits from standard input and returns as a 8-bit byte.</returns>
        public static byte ReadByte()
        {
            char c = ReadChar();
            byte x = (byte)(c & 0xFF);
            return x;
        }
    }
}