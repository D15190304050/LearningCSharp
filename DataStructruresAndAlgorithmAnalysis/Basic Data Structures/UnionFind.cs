using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDataStructuresAndAlgorithm
{
    namespace BasicDataStructures
    {
        /// <summary>
        /// The UnionFinds class represents a union-find data type (also known as the disjoint set data type).
        /// </summary>
        public class UnionFind
        {
            /// <summary>
            /// 
            /// </summary>
            private int[] id;
            private int[] size;

            // Number of components.
            public int ComponentCount { get; private set; }

            // Initialize sites with integer names (0 to sites-1).
            public UnionFind(int sites)
            {
                ComponentCount = sites;
                id = new int[sites];
                for (int i = 0; i < sites; i++)
                    id[i] = i;
                size = new int[sites];
                for (int i = 0; i < sites; i++)
                    size[i] = 1;
            }

            // Add connection between p and q.
            public void Union(int p, int q)
            {
                int rootP = Find(p);
                int rootQ = Find(q);
                if (rootP == rootQ)
                    return;

                if (size[rootP] < rootQ)
                {
                    id[rootP] = rootQ;
                    size[q] += size[p];
                }
                else
                {
                    id[rootQ] = rootP;
                    size[p] = rootQ;
                }
                ComponentCount--;
            }

            // Component identifier for p (0 to sites-1)
            public int Find(int p)
            {
                while (id[p] != p)
                    p = id[p];
                return p;
            }

            /// <summary>
            /// Returns true if p and q are in the same component.
            /// </summary>
            /// <param name="p"></param>
            /// <param name="q"></param>
            /// <returns></returns>
            public bool Connected(int p, int q)
            {
                return Find(p) == Find(q);
            }
        }
    }
}
