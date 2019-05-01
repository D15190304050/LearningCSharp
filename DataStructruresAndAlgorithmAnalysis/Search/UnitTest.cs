using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Search
{
    using BasicDataStructures;

    /// <summary>
    /// This class provide a UnitTest for the data structures and algorithm about search.
    /// </summary>
    internal static class UnitTest
    {
        private const string tinyTale = @"Test data\tinyTale.txt";

        /// <summary>
        /// Unit test method for hash table, both linear probing hash table and separate chaining hash table.
        /// </summary>
        /// <param name="ht"></param>
        public static void HashTableUnitTest(ISymbolTable<string, int> ht)
        {
            // Run the FrequencyCounter.MaxFrequency() test.
            Console.WriteLine("Run the FrequencyCounter.MaxFrequency() test.");
            string fileName = "tinyTale.txt";
            string path = @".\Test Data";
            string fullFileName = System.IO.Path.Combine(path, fileName);
            FrequencyCounter.MaxFrequency(fullFileName, 1, ht);

            // Try to traverse all the key-value pairs.
            Console.WriteLine("\nList the content in the st.");
            foreach (var kvp in ht.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}", kvp.Key, kvp.Value);

            // Report the size.
            Console.WriteLine("\nCurrent size of st is {0}", ht.Size());

            // Try to remove element from st.

            // Remove an KVP by key and list the rest content.
            Console.WriteLine("\nList the content in the st after removing the element associated with key \"it\".");
            ht.Remove("it");
            foreach (var kvp in ht.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}", kvp.Key, kvp.Value);

            // Remove the KVP doesn't exist by the key "qwe".
            Console.WriteLine("\nList the content in the st after removing the element doesn't exist in the st with key \"qwe\".");
            ht.Remove("qwe");
            foreach (var kvp in ht.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}", kvp.Key, kvp.Value);

            // Remove the element by key-value pair.
            // Remove the KVP ("winter", 1).
            Console.WriteLine("\nList the content in the st after removing the element by key-value pair (\"winter\", 1).");
            ht.Remove(new KeyValuePair<string, int>("winter", 1));
            foreach (var kvp in ht.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}", kvp.Key, kvp.Value);

            // Try to clear the st.
            ht.Clear();
            Console.WriteLine("\nAfter clearing the st, the size of st is {0}.", ht.Size());

            // Try to re-add some elements.
            // An easy way here is just re-run the FrequencyCounter.MaxFrequency() method.
            Console.WriteLine("\nRe-add some elements to st.");
            FrequencyCounter.MaxFrequency(fullFileName, 1, ht);
            Console.WriteLine("Current size of st is {0}.", ht.Size());
        }
        /* Output for linear probing hash table:
            Run the FrequencyCounter.MaxFrequency() test.
            The max word is of, and its frequency is 10

            List the content in the st.
            The show time count of word "of" is 10
            The show time count of word "season" is 2
            The show time count of word "it" is 10
            The show time count of word "times" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "age" is 2
            The show time count of word "despair" is 1
            The show time count of word "belief" is 1
            The show time count of word "light" is 1
            The show time count of word "the" is 10
            The show time count of word "hope" is 1
            The show time count of word "best" is 1
            The show time count of word "spring" is 1
            The show time count of word "darkness" is 1
            The show time count of word "winter" is 1
            The show time count of word "was" is 10
            The show time count of word "epoch" is 2
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "foolishness" is 1
            The show time count of word "" is 0

            Current size of st is 21

            List the content in the st after removing the element associted with key "it".
            The show time count of word "of" is 10
            The show time count of word "season" is 2
            The show time count of word "times" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "age" is 2
            The show time count of word "despair" is 1
            The show time count of word "belief" is 1
            The show time count of word "light" is 1
            The show time count of word "the" is 10
            The show time count of word "hope" is 1
            The show time count of word "best" is 1
            The show time count of word "spring" is 1
            The show time count of word "darkness" is 1
            The show time count of word "winter" is 1
            The show time count of word "was" is 10
            The show time count of word "epoch" is 2
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "foolishness" is 1
            The show time count of word "" is 0

            List the content in the st after removing the element doesn't exist in the st with key "qwe".
            The show time count of word "of" is 10
            The show time count of word "season" is 2
            The show time count of word "times" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "age" is 2
            The show time count of word "despair" is 1
            The show time count of word "belief" is 1
            The show time count of word "light" is 1
            The show time count of word "the" is 10
            The show time count of word "hope" is 1
            The show time count of word "best" is 1
            The show time count of word "spring" is 1
            The show time count of word "darkness" is 1
            The show time count of word "winter" is 1
            The show time count of word "was" is 10
            The show time count of word "epoch" is 2
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "foolishness" is 1
            The show time count of word "" is 0

            List the content in the st after removing the element by key-value pair ("winter", 1).
            The show time count of word "of" is 10
            The show time count of word "season" is 2
            The show time count of word "times" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "age" is 2
            The show time count of word "despair" is 1
            The show time count of word "belief" is 1
            The show time count of word "light" is 1
            The show time count of word "the" is 10
            The show time count of word "hope" is 1
            The show time count of word "best" is 1
            The show time count of word "spring" is 1
            The show time count of word "darkness" is 1
            The show time count of word "was" is 10
            The show time count of word "epoch" is 2
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "foolishness" is 1
            The show time count of word "" is 0

            After clearing the st, the size of st is 0.

            Re-add some elements to st.
            The max word is of, and its frequency is 10
            Current size of st is 21.
            Press any key to continue...
            */
        /* Output for separate chaining hash table:
            Run the FrequencyCounter.MaxFrequency() test.
            The max word is of, and its frequency is 10

            List the content in the st.
            The show time count of word "best" is 1
            The show time count of word "of" is 10
            The show time count of word "age" is 2
            The show time count of word "the" is 10
            The show time count of word "spring" is 1
            The show time count of word "was" is 10
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "it" is 10
            The show time count of word "hope" is 1
            The show time count of word "despair" is 1
            The show time count of word "times" is 2
            The show time count of word "light" is 1
            The show time count of word "season" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "belief" is 1
            The show time count of word "darkness" is 1
            The show time count of word "" is 0
            The show time count of word "foolishness" is 1
            The show time count of word "winter" is 1
            The show time count of word "epoch" is 2

            Current size of st is 21

            List the content in the st after removing the element associted with key "it".
            The show time count of word "best" is 1
            The show time count of word "of" is 10
            The show time count of word "age" is 2
            The show time count of word "the" is 10
            The show time count of word "spring" is 1
            The show time count of word "was" is 10
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "hope" is 1
            The show time count of word "despair" is 1
            The show time count of word "times" is 2
            The show time count of word "light" is 1
            The show time count of word "season" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "belief" is 1
            The show time count of word "darkness" is 1
            The show time count of word "" is 0
            The show time count of word "foolishness" is 1
            The show time count of word "winter" is 1
            The show time count of word "epoch" is 2

            List the content in the st after removing the element doesn't exist in the st with key "qwe".
            The show time count of word "best" is 1
            The show time count of word "of" is 10
            The show time count of word "age" is 2
            The show time count of word "the" is 10
            The show time count of word "spring" is 1
            The show time count of word "was" is 10
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "hope" is 1
            The show time count of word "despair" is 1
            The show time count of word "times" is 2
            The show time count of word "light" is 1
            The show time count of word "season" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "belief" is 1
            The show time count of word "darkness" is 1
            The show time count of word "" is 0
            The show time count of word "foolishness" is 1
            The show time count of word "winter" is 1
            The show time count of word "epoch" is 2

            List the content in the st after removing the element by key-value pair ("winter", 1).
            The show time count of word "best" is 1
            The show time count of word "of" is 10
            The show time count of word "age" is 2
            The show time count of word "the" is 10
            The show time count of word "spring" is 1
            The show time count of word "was" is 10
            The show time count of word "worst" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "hope" is 1
            The show time count of word "despair" is 1
            The show time count of word "times" is 2
            The show time count of word "light" is 1
            The show time count of word "season" is 2
            The show time count of word "wisdom" is 1
            The show time count of word "belief" is 1
            The show time count of word "darkness" is 1
            The show time count of word "" is 0
            The show time count of word "foolishness" is 1
            The show time count of word "epoch" is 2

            After clearing the st, the size of st is 0.

            Re-add some elements to st.
            The max word is of, and its frequency is 10
            Current size of st is 21.
            Press any key to continue...
            */

        /// <summary>
        /// Unit test method for SequentialSearch. Run the FrequencyCounter.MaxFrequency() method. Other method will be tested in separate chaining hash table.
        /// </summary>
        public static void SequentialSearchUnitTest()
        {
            SequentialSearch<string, int> st = new SequentialSearch<string, int>();
            FrequencyCounter.MaxFrequency(tinyTale, 1, st);
        }
        /* Output: ...of...10 */

        /// <summary>
        /// Unit test method for BinarySearchTreeBase.
        /// </summary>
        public static void BinarySearchTreeBaseUnitTest(BinarySearchTreeBase<string, int> bst)
        {
            // Basic fucntion test by FrequencyCounter.
            Console.WriteLine("Run the FrequencyCounter.MaxFrequency() test.");
            FrequencyCounter.MaxFrequency(tinyTale, 1, bst);

            // Test the IEnmerator.
            Console.WriteLine("\nList the content in the bst.");
            foreach (var kvp in bst.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}.", kvp.Key, kvp.Value);

            // And then report the size.
            Console.WriteLine("\nThe current size of the binary search tree is {0}.", bst.Size());

            // Try to remove elements from the bst.

            // Remove an element by key and report result.
            bst.Remove("it");
            Console.WriteLine("\nList the content in the bst after removing the element associated with key \"it\".");
            foreach (var kvp in bst.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}.", kvp.Key.ToString(), kvp.Value.ToString());

            // Remove the node with the max key.
            bst.RemoveMax();
            Console.WriteLine("\nList the content in the bst after rmeoving the element with the max key.");
            foreach (var kvp in bst.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}.", kvp.Key.ToString(), kvp.Value.ToString());

            // Remove the node doesn't exist by key "qwe".
            bst.Remove("qwe");
            Console.WriteLine("\nList the content in the bst after removing the element doesn't exist in the bst with key \"qwe\".");
            foreach (var kvp in bst.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}", kvp.Key.ToString(), kvp.Value.ToString());

            // Remove the node by key-value pair.
            // Remove the node ("winter", 1).
            bst.Remove(new KeyValuePair<string, int>("winter", 1));
            Console.WriteLine("\nList the content in the bst after removing the element by key-value pair (\"winter\", 1).");
            foreach (var kvp in bst.GetKeyValuePairs())
                Console.WriteLine("The show time count of word \"{0}\" is {1}.", kvp.Key.ToString(), kvp.Value.ToString());

            // Select the key with rank 7.
            Console.WriteLine("\nHere is the key with rank 7: {0}.", bst.Select(7));

            // Get the rank of key "best".
            Console.WriteLine("\nThe rank of key \"best\' is {0}.\n", bst.Rank("best"));

            // Try to Compute the floor key and ceiling key of key "cast"
            Console.WriteLine("The floor key of \"cast\" is {0}.", bst.FloorKey("cast"));
            Console.WriteLine("The ceiling key of \"cast\" is {0}.", bst.CeilingKey("cast"));

            // Get the size by a specific range ["best", "hope"]
            Console.WriteLine("\nThe size between [\"best\", \"hope\"] is {0}.", bst.Size("best", "hope"));

            // Try to clear the bst.
            bst.Clear();
            Console.WriteLine("\nAfter clearing the bst, the size of bst is {0}.\n", bst.Size());

            // Try to re-add some nodes.
            // An easy way here is just re-run the FrequencyCounter.MaxFrequency() method.
            FrequencyCounter.MaxFrequency(tinyTale, 1, bst);
        }
        /* Output:
            Run the FrequencyCounter.MaxFrequency() test.
            The max word is it, and its frequency is 10

            List the content in the bst.
            The show time count of word "" is 0.
            The show time count of word "age" is 2.
            The show time count of word "belief" is 1.
            The show time count of word "best" is 1.
            The show time count of word "darkness" is 1.
            The show time count of word "despair" is 1.
            The show time count of word "epoch" is 2.
            The show time count of word "foolishness" is 1.
            The show time count of word "hope" is 1.
            The show time count of word "incredulity" is 1.
            The show time count of word "it" is 10.
            The show time count of word "light" is 1.
            The show time count of word "of" is 10.
            The show time count of word "season" is 2.
            The show time count of word "spring" is 1.
            The show time count of word "the" is 10.
            The show time count of word "times" is 2.
            The show time count of word "was" is 10.
            The show time count of word "winter" is 1.
            The show time count of word "wisdom" is 1.
            The show time count of word "worst" is 1.

            The current size of the binary search tree is 21.

            List the content in the bst after removing the element associated with key "it".
            The show time count of word "" is 0.
            The show time count of word "age" is 2.
            The show time count of word "belief" is 1.
            The show time count of word "best" is 1.
            The show time count of word "darkness" is 1.
            The show time count of word "despair" is 1.
            The show time count of word "epoch" is 2.
            The show time count of word "foolishness" is 1.
            The show time count of word "hope" is 1.
            The show time count of word "incredulity" is 1.
            The show time count of word "light" is 1.
            The show time count of word "of" is 10.
            The show time count of word "season" is 2.
            The show time count of word "spring" is 1.
            The show time count of word "the" is 10.
            The show time count of word "times" is 2.
            The show time count of word "was" is 10.
            The show time count of word "winter" is 1.
            The show time count of word "wisdom" is 1.
            The show time count of word "worst" is 1.

            List the content in the bst after rmeoving the element with the max key.
            The show time count of word "" is 0.
            The show time count of word "age" is 2.
            The show time count of word "belief" is 1.
            The show time count of word "best" is 1.
            The show time count of word "darkness" is 1.
            The show time count of word "despair" is 1.
            The show time count of word "epoch" is 2.
            The show time count of word "foolishness" is 1.
            The show time count of word "hope" is 1.
            The show time count of word "incredulity" is 1.
            The show time count of word "light" is 1.
            The show time count of word "of" is 10.
            The show time count of word "season" is 2.
            The show time count of word "spring" is 1.
            The show time count of word "the" is 10.
            The show time count of word "times" is 2.
            The show time count of word "was" is 10.
            The show time count of word "winter" is 1.
            The show time count of word "wisdom" is 1.

            List the content in the bst after removing the element doesn't exist in the bst with key "qwe".
            The show time count of word "" is 0
            The show time count of word "age" is 2
            The show time count of word "belief" is 1
            The show time count of word "best" is 1
            The show time count of word "darkness" is 1
            The show time count of word "despair" is 1
            The show time count of word "epoch" is 2
            The show time count of word "foolishness" is 1
            The show time count of word "hope" is 1
            The show time count of word "incredulity" is 1
            The show time count of word "light" is 1
            The show time count of word "of" is 10
            The show time count of word "season" is 2
            The show time count of word "spring" is 1
            The show time count of word "the" is 10
            The show time count of word "times" is 2
            The show time count of word "was" is 10
            The show time count of word "winter" is 1
            The show time count of word "wisdom" is 1

            List the content in the bst after removing the element by key-value pair ("winter", 1).
            The show time count of word "" is 0.
            The show time count of word "age" is 2.
            The show time count of word "belief" is 1.
            The show time count of word "best" is 1.
            The show time count of word "darkness" is 1.
            The show time count of word "despair" is 1.
            The show time count of word "epoch" is 2.
            The show time count of word "foolishness" is 1.
            The show time count of word "hope" is 1.
            The show time count of word "incredulity" is 1.
            The show time count of word "light" is 1.
            The show time count of word "of" is 10.
            The show time count of word "season" is 2.
            The show time count of word "spring" is 1.
            The show time count of word "the" is 10.
            The show time count of word "times" is 2.
            The show time count of word "was" is 10.
            The show time count of word "wisdom" is 1.

            Here is the key with rank 7: foolishness.

            The rank of key "best' is 3.

            The floor key of "cast" is best.
            The ceiling key of "cast" is darkness.

            The size between ["best", "hope"] is 6.

            After clearing the bst, the size of bst is 0.

            The max word is it, and its frequency is 10
            Press any key to continue...
            */

        /// <summary>
        /// Unit test method for TreeSet.
        /// </summary>
        public static void TreeSetUnitTest()
        {
            // Open a file and read all words in it.
            string text = System.IO.File.ReadAllText(tinyTale);
            string[] words = System.Text.RegularExpressions.Regex.Split(text, "\\s+");

            // Create a set from the words[].
            TreeSet<string> set = new TreeSet<string>(words);

            // Print all the content.
            Console.WriteLine("Content in the current set is following:");
            PrintEnumerator(set);
            Console.WriteLine();

            // Print the total number of the elements in the current set.
            Console.WriteLine("Print the total number of the elements in the current set: {0}\n", set.Count);

            // Contains() test.
            Console.WriteLine("The current set contains word \"it\": {0}", set.Contains("it"));
            Console.WriteLine("The current set contains word \"Graph\": {0}", set.Contains("Graph"));
            Console.WriteLine();

            // CopyTo() test. Copy the set to an array with same capacity and print the array.
            words = new string[set.Count];
            set.CopyTo(words);
            Console.WriteLine("Copy content in the set into an array named \"words\".");
            Console.WriteLine("Content in the array words is following:");
            PrintEnumerator(words);
            Console.WriteLine();

            // Create a deep copy of the current set for the following test for some set operation.
            var copy = new TreeSet<string>(set);

            // Remove() test. Remove an item from the current set.
            set.Remove("winter");
            Console.WriteLine("Print the current set after removing the word \"winter\":");
            PrintEnumerator(set);
            Console.WriteLine("And now the number of elements in the current set is: {0}", set.Count);
            Console.WriteLine();

            // Extract content in the words with even index into a new set, and print it.
            Queue<string> evenContent = new Queue<string>();
            for (int i = 0; i < words.Length; i++)
            {
                if (i % 2 == 0)
                    evenContent.Enqueue(words[i]);
            }
            Console.WriteLine("Content in the words with even index is following:");
            PrintEnumerator(evenContent);
            Console.WriteLine();

            // Test the set relationship.
            Console.WriteLine("Test the set relationship.");
            Console.WriteLine("Test the relationship between set and evenContent:");
            Console.WriteLine("set is sub set of evenContent: {0}", set.IsSubsetOf(evenContent));
            Console.WriteLine("set is proper sub set of eventContent: {0}", set.IsProperSubsetOf(evenContent));
            Console.WriteLine("set is super set of evenContent: {0}", set.IsSupersetOf(evenContent));
            Console.WriteLine("set is proper super set of evenContent: {0}", set.IsProperSupersetOf(evenContent));
            Console.WriteLine("set is equal to evenContent: {0}\n", set.SetEquals(evenContent));
            Console.WriteLine("Test the relatioonship between set and copy:");
            Console.WriteLine("set is sub set of copy: {0}", set.IsSubsetOf(copy));
            Console.WriteLine("set is proper sub set of copy: {0}", set.IsProperSubsetOf(copy));
            Console.WriteLine("set is super set of copy: {0}", set.IsSupersetOf(copy));
            Console.WriteLine("set is proper super set of copy: {0}", set.IsProperSupersetOf(copy));
            Console.WriteLine("set is equal to copy: {0}", set.SetEquals(copy));
            set.Add("winter");
            Console.WriteLine("set is equal to copy, by Equals(): {0}\n", set.Equals(copy));

            // Test the set operations.
            // copy here is to recover set after each operation.
            evenContent.Enqueue("catch");
            Console.WriteLine("Add word \"catch\" into evenContent.");
            Console.WriteLine("Print the intersection of set and evenContent:");
            set.IntersectWith(evenContent);
            PrintEnumerator(set);
            Console.WriteLine();
            set = new TreeSet<string>(copy);
            Console.WriteLine("Print the union of set and evenContent:");
            set.UnionWith(evenContent);
            PrintEnumerator(set);
            Console.WriteLine();
            set = new TreeSet<string>(copy);
            Console.WriteLine("Print the SymmetricException:");
            set.SymmetricExceptWith(evenContent);
            PrintEnumerator(set);
        }

        /// <summary>
        /// Print the content in the enumerator line by line.
        /// </summary>
        /// <typeparam name="T">A generic data type.</typeparam>
        /// <param name="enumerator">The enumerator which stores content to be print.</param>
        private static void PrintEnumerator<T>(IEnumerable<T> enumerator)
        {
            foreach (T item in enumerator)
                Console.WriteLine(item);
        }
        /* Output:
            Content in the current set is following:
            age
            belief
            best
            darkness
            despair
            epoch
            foolishness
            hope
            incredulity
            it
            light
            of
            season
            spring
            the
            times
            was
            winter
            wisdom
            worst

            Print the total number of the elements in the current set: 20

            The current set contains word "it": True
            The current set contains word "Graph": False

            Copy content in the set into an array named "words".
            Content in the array words is following:
            age
            belief
            best
            darkness
            despair
            epoch
            foolishness
            hope
            incredulity
            it
            light
            of
            season
            spring
            the
            times
            was
            winter
            wisdom
            worst

            Print the current set after removing the word "winter":
            age
            belief
            best
            darkness
            despair
            epoch
            foolishness
            hope
            incredulity
            it
            light
            of
            season
            spring
            the
            times
            was
            wisdom
            worst
            And now the number of elements in the current set is: 19

            Content in the words with even index is following:
            age
            best
            despair
            foolishness
            incredulity
            light
            season
            the
            was
            wisdom

            Test the set relationship.
            Test the relationship between set and evenContent:
            set is sub set of evenContent: False
            set is proper sub set of eventContent: False
            set is super set of evenContent: True
            set is proper super set of evenContent: True
            set is equal to evenContent: False

            Test the relatioonship between set and copy:
            set is sub set of copy: True
            set is proper sub set of copy: True
            set is super set of copy: False
            set is proper super set of copy: False
            set is equal to copy: False
            set is equal to copy, by Equals(): True

            Add word "catch" into evenContent.
            Print the intersection of set and evenContent:
            age
            best
            despair
            foolishness
            incredulity
            light
            season
            the
            was
            wisdom

            Print the union of set and evenContent:
            age
            belief
            best
            catch
            darkness
            despair
            epoch
            foolishness
            hope
            incredulity
            it
            light
            of
            season
            spring
            the
            times
            was
            winter
            wisdom
            worst

            Print the SymmetricException:
            belief
            catch
            darkness
            epoch
            hope
            it
            of
            spring
            times
            winter
            worst
            Press any key to continue...
            */
    }
}