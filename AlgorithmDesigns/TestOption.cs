using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The TestOption enum represents the test options for testing different algorithms.
    /// </summary>
    internal enum TestOption
    {
        /// <summary>
        /// Test the algorithm using the default test array.
        /// </summary>
        DefaultIntArray,

        /// <summary>
        /// Test the algorithm using the array specified by the caller.
        /// </summary>
        SepcifiedArray
    }
}
