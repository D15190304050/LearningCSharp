using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Concurrent
{
    public class Words
    {
        /// <summary>
        /// Object to store the current state, for passing to the caller.
        /// </summary>
        public class CurrentState
        {
            public int LinesCounted { get; set; }
            public int WordsMatched { get; set; }
        }

        public string FilePath { get; set; }
        public string WordToCount { get; set; }

        private int wordCount;
        private int linesCounted;

        public void CountWords(BackgroundWorker worker, DoWorkEventArgs e)
        {
            // Initialize the variables.
            CurrentState state = new CurrentState();
            string line = "";
            int elapsedTime = 20;
            DateTime lastReportTime = DateTime.Now;

            if ((WordToCount == "") || (WordToCount == string.Empty))
                throw new ArgumentException("Word to count not specified.");

            string escapedWordToCount = System.Text.RegularExpressions.Regex.Escape(WordToCount);

            // Open a stream.
            using (StreamReader reader = new StreamReader(FilePath))
            {
                // Process lines while there are lines remaining in the file.
                while (!reader.EndOfStream)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        line = reader.ReadLine();
                        wordCount += CountString(line, escapedWordToCount);
                        linesCounted++;

                        // Raise an event so the window can monitor progress.
                        int compare = DateTime.Compare(DateTime.Now, lastReportTime.AddMilliseconds(elapsedTime));
                        if (compare > 0)
                        {
                            state.WordsMatched = wordCount;
                            state.LinesCounted = linesCounted;
                            worker.ReportProgress(0, state);
                            lastReportTime = DateTime.Now;
                        }
                    }

                    // Uncomment for testing.
                    //System.Threading.Thread.Sleep(5);
                }

                // Report the final count values.
                state.LinesCounted = linesCounted;
                state.WordsMatched = wordCount;
                worker.ReportProgress(0, state);
            }
        }

        /// <summary>
        /// This function counts the number of thimes a word is found in a line.
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="compareString"></param>
        /// <returns></returns>
        private int CountString(string sourceString, string wordToCount)
        {
            if (sourceString == null)
                return 0;

            // To count all occurance of the string, event within words, remove both instance of @"\b" from the following line.
            Regex regex = new Regex(
                @"\b" + wordToCount + @"\b",
                RegexOptions.IgnoreCase
                );

            MatchCollection matches = regex.Matches(sourceString);

            return matches.Count;
        }
    }
}
