using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm.String
{
    using BasicDataStructures;

    /// <summary>
    /// The UnitTest class provides unit test methods for classes in PersonalDataStructuresAndAlgorithm.String name space.
    /// </summary>
    internal static class UnitTest
    {
        private const string lsd = @"Test data\lsd.txt";
        private const string msdTest = @"Test data\msdTest.txt";
        private const string trieTest = @"Test data\trieTestFile.txt";
        private const string tinyL = @"Test data\tinyL.txt";

        /// <summary>
        /// Print strings in the array in each line.
        /// </summary>
        /// <param name="array">The array to print.</param>
        private static void PrintWords(IEnumerable<string> array)
        {
            foreach (string s in array)
                Console.WriteLine(s);
        }

        /// <summary>
        /// Unit test method for LeastSignificantDigit.
        /// </summary>
        public static void LeastSignificantDigitUnitTest()
        {
            // Read content from file.
            string text = System.IO.File.ReadAllText(lsd);

            // Split the content into individual words.
            string[] words = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // The index of the content to be read in words[].
            // Increase by 1 whenever read content from words[].
            int currentIndex = 0;

            // Create a string array to store the strings to be sortesd.
            string[] array = new string[int.Parse(words[currentIndex++])];
            while (currentIndex < words.Length)
            {
                array[currentIndex - 1] = words[currentIndex];
                currentIndex++;
            }

            // Sort the strings.
            LeastSignificantDigit.Sort(array, array[0].Length);

            // Print results.
            PrintWords(array);
        }
        /* Output:
            1ICK750
            1ICK750
            1OHV845
            1OHV845
            1OHV845
            2IYE230
            2RLA629
            2RLA629
            3ATW723
            3CIO720
            3CIO720
            4JZY524
            4PGC938
            */

        /// <summary>
        /// Unit test method for MostSignificantDigit.
        /// </summary>
        public static void MostSignificantDigitUnitTest()
        {
            string[] words = System.IO.File.ReadAllLines(msdTest);
            MostSignificantDigit.Sort(words);
            PrintWords(words);
        }
        /* Output:
            are
            by
            catch
            caught
            done
            sea
            seashells
            seashells
            sells
            sells
            she
            she
            shells
            shore
            surely
            the
            the
            */

        /// <summary>
        /// Unit test method for Quick3String.
        /// </summary>
        public static void Quick3StringUnitTest()
        {
            string[] words = System.IO.File.ReadAllLines(msdTest);
            Quick3String.Sort(words);
            PrintWords(words);
        }
        /* Output:
            are
            by
            catch
            caught
            done
            sea
            seashells
            sells
            sells
            she
            she
            shells
            shore
            surely
            the
            the
            seashells
            Press any key to continue...
            */

        /// <summary>
        /// Unit test method for TrieSymbolTable.
        /// </summary>
        public static void TrieSymbolTableUnitTest()
        {
            // Build the symbol table from file.
            TrieSymbolTable<int?> st = new TrieSymbolTable<int?>();
            string[] words = System.IO.File.ReadAllLines(trieTest);
            for (int i = 0; i < words.Length; i++)
                st.Add(words[i], i);

            // Print results.
            Console.WriteLine("Keys(\"\"):");
            foreach (string key in st.Keys())
                Console.WriteLine(key + " " + st[key]);
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"shellsort\"):");
            Console.WriteLine(st.LongestPrefixOf("shellsort"));
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"quicksort\")");
            Console.WriteLine(st.LongestPrefixOf("quicksort"));
            Console.WriteLine();

            Console.WriteLine("KeysWithPrefix(\"shor\")");
            PrintWords(st.KeysWithPrefix("shor"));
            Console.WriteLine();

            Console.WriteLine("KeysThatMatch(\".he.l.\")");
            PrintWords(st.KeysThatMatch(".he.l."));
            Console.WriteLine();
        }
        /* Output:
            Keys(""):
            Lhellk 9
            hello 5
            hellp 8
            quick 2
            quickso 3
            quicksor 1
            shellsort 0
            shellsortcc 7
            shellsortss 6
            shor 4
            shore 11
            short 10

            LongestPrefixOf("shellsort"):
            shellsort

            LongestPrefixOf("quicksort")
            quicksor

            KeysWithPrefix("shor")
            shor
            shore
            short

            KeysThatMatch(".he.l.")
            Lhellk
            */

        public static void TrieSetUnitTest()
        {
            // Build the trie set from file.
            TrieSet set = new TrieSet();
            string[] words = System.IO.File.ReadAllLines(trieTest);
            foreach (string key in words)
                set.Add(key);

            // Print results.
            Console.WriteLine("Keys(\"\"):");
            foreach (string key in set)
                Console.WriteLine(key);
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"shellsort\"):");
            Console.WriteLine(set.LongestPrefixOf("shellsort"));
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"quicksort\")");
            Console.WriteLine(set.LongestPrefixOf("quicksort"));
            Console.WriteLine();

            Console.WriteLine("KeysWithPrefix(\"shor\")");
            PrintWords(set.KeysWithPrefix("shor"));
            Console.WriteLine();

            Console.WriteLine("KeysThatMatch(\".he.l.\")");
            PrintWords(set.KeysThatMatch(".he.l."));
            Console.WriteLine();
        }
        /* Output:
            Keys(""):
            Lhellk
            hello
            hellp
            quick
            quickso
            quicksor
            shellsort
            shellsortcc
            shellsortss
            shor
            shore
            short

            LongestPrefixOf("shellsort"):
            shellsort

            LongestPrefixOf("quicksort")
            quicksor

            KeysWithPrefix("shor")
            shor
            shore
            short

            KeysThatMatch(".he.l.")
            Lhellk
            */

        /// <summary>
        /// Unit test method for TernarySearchTrie.
        /// </summary>
        public static void TernarySearchTrieUnitTest()
        {
            // Build the symbol table from file.
            TernarySearchTrie<int?> st = new TernarySearchTrie<int?>();
            string[] words = System.IO.File.ReadAllLines(trieTest);
            for (int i = 0; i < words.Length; i++)
                st.Add(words[i], i);

            // Print results.
            Console.WriteLine("Keys(\"\"):");
            foreach (string key in st.Keys())
                Console.WriteLine(key + " " + st[key]);
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"shellsort\"):");
            Console.WriteLine(st.LongestPrefixOf("shellsort"));
            Console.WriteLine();

            Console.WriteLine("LongestPrefixOf(\"quicksort\")");
            Console.WriteLine(st.LongestPrefixOf("quicksort"));
            Console.WriteLine();

            Console.WriteLine("KeysWithPrefix(\"shor\")");
            PrintWords(st.KeysWithPrefix("shor"));
            Console.WriteLine();

            Console.WriteLine("KeysThatMatch(\".he.l.\")");
            PrintWords(st.KeysThatMatch(".he.l."));
            Console.WriteLine();
        }
        /* Output:
            Keys(""):
            Lhellk 9
            hello 5
            hellp 8
            quick 2
            quickso 3
            quicksor 1
            shellsort 0
            shellsortcc 7
            shellsortss 6
            shor 4
            shore 11
            short 10

            LongestPrefixOf("shellsort"):
            shellsort

            LongestPrefixOf("quicksort")
            quicksor

            KeysWithPrefix("shor")
            shor
            shore
            short

            KeysThatMatch(".he.l.")
            Lhellk
            */

        /// <summary>
        /// Unit test method for StringSearchBase.
        /// </summary>
        private static void StringSearchBaseUnitTest(StringSearchBase search)
        {
            string sText = "abacadabrabracabracadabrabrabracad";
            string sPattern = "rab";
            char[] cText = sText.ToCharArray();
            char[] cPattern = sPattern.ToCharArray();

            int offsetByString = search.Search(sText);
            int offsetByChar = search.Search(cText);

            // Print results.
            Console.WriteLine("Text:   " + sText);

            Console.Write("Pattern:");
            for (int i = 0; i < offsetByString; i++)
                Console.Write(" ");
            Console.WriteLine(sPattern);

            Console.Write("Pattern:");
            for (int i = 0; i < offsetByChar; i++)
                Console.Write(" ");
            Console.WriteLine(cPattern);
        }
        /* Output:
             Text:   abacadabrabracabracadabrabrabracad
            Pattern:        rab
            Pattern:        rab
            */

        /// <summary>
        /// Unit test method for KMP.
        /// </summary>
        public static void KMPUnitTest()
        {
            StringSearchBaseUnitTest(new KMP("rab"));
        }

        /// <summary>
        /// Unit test method for BoyerMoore.
        /// </summary>
        public static void BoyerMooreUnitTest()
        {
            StringSearchBaseUnitTest(new BoyerMoore("rab"));
        }

        /// <summary>
        /// Unit test method for NFA.
        /// </summary>
        public static void NFAUnitTest()
        {
            string[] patterns = { "(A*B|AC)D", "(A*B|AC)D", "(a|(bc)*d)*", "(a|(bc)*d)*" };
            string[] texts = { "AAAABD", "AAAAC", "abcbcd", "abcbcbcdaaaabcbcdaaaddd" };

            for (int i = 0; i < patterns.Length; i++)
            {
                NFA nfa = new NFA("(" + patterns[i] + ")");
                Console.WriteLine(nfa.Recognizes(texts[i]));
            }
        }
        /* Output:
            True
            False
            True
            True
            */

        /// <summary>
        /// Unit test method for GREP.
        /// </summary>
        public static void GREPUnitTest()
        {
            GREP.Grep(tinyL);
        }
        /* Output:
            ABD
            ABCCBD
            Press any key to continue...
            */

        /// <summary>
        /// Unit test method for BinaryStandardOutput.
        /// </summary>
        public static void BinaryStandardOutputUnitTest()
        {
            // Test for Write(bool bit).
            Console.WriteLine("Test for Write(bool bit).");
            Console.WriteLine("Generate 8 bits, and write the by WriteBit(). Here the value of the byte is 65.");
            Console.WriteLine("Here is the 8 bits:");
            int[] bits = { 0, 1, 0, 0, 0, 0, 0, 1 };
            foreach (int bit in bits)
                Console.Write(bit + " ");
            Console.WriteLine("\nHere is the result of calling Write(bool bit) 8 times:");
            for (int i = 0; i < 8; i++)
                BinaryStandardOutput.Write(bits[i] == 1 ? true : false);
            Console.WriteLine("\n");

            // Test for Write(byte bValue).
            Console.WriteLine("Test for Write(byte bValue)");
            byte bValue = 0;
            for (int i = 0; i < bits.Length; i++)
                bValue = (byte)((bValue << 1) | bits[i]);
            Console.WriteLine("Here is the byte: {0}", bValue);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(bValue);
            Console.WriteLine("\n");

            // Test for Write(int iValue).
            // Attention: Even they have the same bit sequence, they appears different, if the value is greater than 255.
            Console.WriteLine("Test for Write(int iValue).");
            int iValue = 29;
            Console.WriteLine("Here is the iValue: {0}", iValue);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(iValue);
            Console.WriteLine("\n");

            // Test for Write(int iValue, int length).
            Console.WriteLine("Test for Write(int iValue, int length).");
            Console.WriteLine("Here is the iValue: {0}", iValue);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(iValue, 8);
            Console.WriteLine("\n");

            // Test for Write(short sValue)
            Console.WriteLine("Test for Write(short sValue).");
            short sValue = 27;
            Console.WriteLine("Here is the sValue: {0}", sValue);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(sValue);
            Console.WriteLine("\n");

            // Test for Write(long lValue).
            Console.WriteLine("Test for Write(long lValue).");
            long lValue = 27;
            Console.WriteLine("Here is the lValue: {0}", lValue);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(lValue);
            Console.WriteLine("\n");

            // Test for Write(double dValue).
            Console.WriteLine("Test for Write(double dValue).");
            double dValue = 0.7;
            long doubleToLong = BitConverter.DoubleToInt64Bits(dValue);
            Console.WriteLine("Here is the dValue: {0}", dValue);
            Console.WriteLine("The long value with the same bit sequence is: {0}", doubleToLong);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(dValue);
            Console.WriteLine("\n");

            // Test for Write(char c).
            Console.WriteLine("Test for Write(char c).");
            char c = 'A';
            Console.WriteLine("Here is the char: {0}", c);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(c);
            Console.WriteLine("\n");

            // Test for Write(char c, int length).
            Console.WriteLine("Test for Write(char c, int length).");
            Console.WriteLine("Here is the char: {0}", c);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(c, 12);
            BinaryStandardOutput.Flush();
            Console.WriteLine("\n");

            // Test for Write(string s).
            string s = "ABC";
            Console.WriteLine("Here is the string: " + s);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(s);
            Console.WriteLine("\n");

            // Test for Write(string s, int length).
            Console.WriteLine("Here is the string: " + s);
            Console.WriteLine("Here is the result:");
            BinaryStandardOutput.Write(s, 8);
            Console.WriteLine("\n");
        }
        /* Output:
            Test for Write(bool bit).
            Generate 8 bits, and write the by WriteBit(). Here the value of the byte is 65.
            Here is the 8 bits:
            0 1 0 0 0 0 0 1
            Here is the result of calling Write(bool bit) 8 times:
            65

            Test for Write(byte bValue)
            Here is the byte: 65
            Here is the result:
            65

            Test for Write(int iValue).
            Here is the iValue: 29
            Here is the result:
            00029

            Test for Write(int iValue, int length).
            Here is the iValue: 29
            Here is the result:
            29

            Test for Write(short sValue).
            Here is the sValue: 27
            Here is the result:
            027

            Test for Write(long lValue).
            Here is the lValue: 27
            Here is the result:
            000000027

            Test for Write(double dValue).
            Here is the dValue: 0.7
            The long value with the same bit sequence is: 4604480259023595110
            Here is the result:
            63230102102102102102102

            Test for Write(char c).
            Here is the char: A
            Here is the result:
            65

            Test for Write(char c, int length).
            Here is the char: A
            Here is the result:
            416

            Here is the string: ABC
            Here is the result:
            656667

            Here is the string: ABC
            Here is the result:
            656667
            */  

        /// <summary>
        /// Unit test method for BinaryStandardInput.
        /// </summary>
        public static void BinaryStandardInputUnitTest()
        {
            // Prompt the user to enter a string for BinaryStandardInput to read.
            Console.WriteLine("Please enter a string for this test.");

            // The string for test is
            // aABCDEFGHIJKLMNOPQRSTUVWXYZab ABC
            // The first a is s sentinel to make sure this string will appear before the following sentences.
            Console.Read();
            Console.WriteLine();

            // Test for ReadBoolean().
            Console.WriteLine("Test for ReadBoolean().");
            bool bit;
            for (int i = 0; i < 8; i++)
            {
                bit = BinaryStandardInput.ReadBoolean();
                BinaryStandardOutput.Write(bit);
            }
            Console.WriteLine("\n");

            // Test for ReadChar().
            Console.WriteLine("Test for ReadChar().");
            char a = BinaryStandardInput.ReadChar();
            BinaryStandardOutput.Write(a);
            Console.WriteLine("\n");

            // Test for ReadChar(int length).
            // Read 2 char from console, and the given length is 12, to make sure memory alignment.
            Console.WriteLine("Test for ReadChar(length).");
            for (int i = 0; i < 2; i++)
            {
                a = BinaryStandardInput.ReadChar(12);
                BinaryStandardOutput.Write(a, 12);
            }
            Console.WriteLine("\n");

            // Test for ReadByte().
            Console.WriteLine("Test for ReadByte().");
            byte bValue = BinaryStandardInput.ReadByte();
            BinaryStandardOutput.Write(bValue);
            Console.WriteLine("\n");

            // Test for ReadInt16().
            Console.WriteLine("Test for ReadInt16().");
            short sValue = BinaryStandardInput.ReadInt16();
            BinaryStandardOutput.Write(sValue);
            Console.WriteLine("\n");

            // Test for ReadInt().
            Console.WriteLine("Test for ReadInt().");
            int iValue = BinaryStandardInput.ReadInt();
            BinaryStandardOutput.Write(iValue);
            Console.WriteLine("\n");

            // Test for ReadLong().
            Console.WriteLine("Test for ReadLong().");
            long lValue = BinaryStandardInput.ReadLong();
            BinaryStandardOutput.Write(lValue);
            Console.WriteLine("\n");

            // Test for ReadDouble().
            Console.WriteLine("Test for ReadDouble().");
            double dValue = BinaryStandardInput.ReadDouble();
            BinaryStandardOutput.Write(dValue);
            Console.WriteLine("\n");

            // Test for ReadString().
            Console.WriteLine("Test for ReadString().");
            string s = BinaryStandardInput.ReadString();
            BinaryStandardOutput.Write(s);
            Console.WriteLine();
        }
        /* Output:
            Please enter a string for this test.
            aABCDEFGHIJKLMNOPQRSTUVWXYZab ABC

            Test for ReadBoolean().
            65

            Test for ReadChar().
            66

            Test for ReadChar(length).
            676869

            Test for ReadByte().
            70

            Test for ReadInt16().
            7172

            Test for ReadInt().
            73747576

            Test for ReadLong().
            7778798081828384

            Test for ReadDouble().
            8586878889909798

            Test for ReadString().
            32656667
            */

        /// <summary>
        /// Unit test method for Huffman.
        /// </summary>
        public static void HuffmanUnitTest()
        {
            // Test for Compress().
            Console.WriteLine("Test for Compress().");
            Huffman.Compress();
            Console.WriteLine();

            // Test for Expand().
            Console.WriteLine("Test for Expand().");
            Huffman.Expand();
        }
    }
}