using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The <see cref="AndOrTree" /> class represents an and or tree.
    /// </summary>
    public class AndOrTree
    {
        // See CSDN link: https://blog.csdn.net/dala_da/article/details/78586845 for a little hint.

        /// <summary>
        /// String representation of or node type.
        /// </summary>
        private const string Or = "or";

        /// <summary>
        /// String representation of and node type.
        /// </summary>
        private const string And = "and";

        /// <summary>
        /// String value indicating whether a specified node is solvable is unknown.
        /// </summary>
        private const string Unknown = "unknown";

        /// <summary>
        /// String value indicating a specified node is solvable.
        /// </summary>
        private const string True = "true";

        /// <summary>
        /// String value indicating a specified node is unsolvable.
        /// </summary>
        private const string False = "false";

        /// <summary>
        /// Root node of this <see cref="AndOrTree" />.
        /// </summary>
        private AndOrTreeNode root;

        /// <summary>
        /// An array that stores all the nodes of this <see cref="AndOrTree" />.
        /// </summary>
        private AndOrTreeNode[] treeNodes;

        /// <summary>
        /// The solution tree of this <see cref="AndOrTree" />.
        /// </summary>
        private AndOrTree solutionTree;

        /// <summary>
        /// Initializes a new instance of <see cref="AndOrTree" /> from a given root node (through which can traverse through all the nodes in the tree).
        /// </summary>
        /// <param name="root">The root node of the <see cref="AndOrTree" /> (through which can traverse through all the nodes in the tree).</param>
        public AndOrTree(AndOrTreeNode root)
        {
            LinkedList<AndOrTreeNode> nodes = new LinkedList<AndOrTreeNode>();
            Queue<AndOrTreeNode> bfsQueue = new Queue<AndOrTreeNode>();
            bfsQueue.Enqueue(root);
            while (bfsQueue.Count != 0)
            {
                AndOrTreeNode nextNode = bfsQueue.Dequeue();
                nodes.AddLast(nextNode);
                foreach (AndOrTreeNode child in nextNode.GetChildren())
                    bfsQueue.Enqueue(child);
            }

            treeNodes = nodes.ToArray();
            this.root = root;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AndOrTree" /> whose node information is stored in the given file.
        /// </summary>
        /// <param name="filePath">The path of the file that stores the node information of the <see cref="AndOrTree" />.</param>
        /// <remarks>
        /// The structure of the file is described as follows:
        /// 1. Each line contains the information of a single node.
        /// 2. Each line follows the structure: "[ID] [NodeType] [Solvable] [Children ...]", separated by space.
        ///     [ID] field accepts an integer.
        ///     [NodeType] field accepts "or" and "and" (case insensitive).
        ///     [Solvable] field accepts "true", "false" and "unknown" (case insensitive), where "unknown" indicates that the node is not a leaf node.
        ///     [Children] field accepts a list of integers or just empty.
        ///     eg.
        ///     1 or true
        ///     4 and unknown 1 2
        /// 3. For convenience of development of this constructor, every node must be above of its parent node, otherwise, an exception will be thrown.
        /// 4. Do not place an empty line at the beginning or end of the file, empty line will also raise an exception.
        /// 5. Invalid string (such as "OrNode" for [NodeType] field) will raise an exception. Please do follow the structure described above.
        /// </remarks>
        public AndOrTree(string filePath)
        {
            // Get contents of the file.
            string[] treeNodeText = System.IO.File.ReadAllLines(filePath);

            // Get the number of nodes.
            int nodeCount = treeNodeText.Length;
            treeNodes = new AndOrTreeNode[nodeCount];

            // Iterate through all node information.
            for (int i = 0; i < nodeCount; i++)
            {
                // Call `ToLower()` before calling `Split()` to make this processing case insensitive.
                string[] line = treeNodeText[i].ToLower().Split(' ');

                // Get the type of this node.
                AndOrTreeNodeType nodeType;
                if (line[1] == Or)
                    nodeType = AndOrTreeNodeType.OrNode;
                else if (line[1] == And)
                    nodeType = AndOrTreeNodeType.AndNode;
                else
                    throw new FormatException("Invalid data format of `AndOrTreeNodeType` in file " + filePath + ", line " + i);

                // Get the value indicating whether this node is solvable.
                bool? solvable;
                if (line[2] == True)
                    solvable = true;
                else if (line[2] == False)
                    solvable = false;
                else if (line[2] == Unknown)
                    solvable = null;
                else
                    throw new FormatException("Invalid data format of `value` in file " + filePath + ", line " + i);

                // Create this node.
                AndOrTreeNode node = new AndOrTreeNode(int.Parse(line[0]), nodeType, solvable);
                treeNodes[i] = node;

                // Add child nodes for this node.
                for (int j = 3; j < line.Length; j++)
                {
                    int child = int.Parse(line[j]);
                    if (child >= i)
                        throw new FormatException("Invalid data format of `parent` in file " + filePath + ", line " + i);

                    // Link this node and its parent.
                    treeNodes[child].Parent = node;
                    node.AddNode(treeNodes[child]);
                }
            }

            // Get the root node of this tree (which is the node whose parent node is null).
            // Actually, since each node will shown before its parent, the root node is the node at the last line in the file.
            foreach (AndOrTreeNode node in treeNodes)
            {
                if (node.Parent == null)
                    root = node;
            }
        }

        /// <summary>
        /// Runs depth first search to find the solution tree of this <see cref="AndOrTree" />.
        /// </summary>
        /// <returns>The solution tree of this <see cref="AndOrTree" />, null if no such solution tree.</returns>
        public AndOrTree DepthFirstSearch()
        {
            // Eliminate the result found before.
            solutionTree = null;

            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            // According to DFS algorithm from Graph Theory, queue is unnecessary here.
            //Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            //nodes.Enqueue(root);

            // Start DFS search.
            bool solved = false;
            DepthFirstSearch(root, marked, ref solved);

            // Return the solution tree found by DFS search.
            return solutionTree;
        }

        /// <summary>
        /// Runs depth first search on the given node.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="marked">A boolean array whose entries indicates whether a node is processed by DFS before.</param>
        /// <param name="solved">A boolean value indicating whether a solution tree is found (true) or not (false).</param>
        private void DepthFirstSearch(AndOrTreeNode node, bool[] marked, ref bool solved)
        {
            // Stop searching if DFS has found a solution.
            if (solved)
                return;

            // Mark this node.
            marked[node.Id] = true;

            // If this is not a leaf node, keep searching.
            // Otherwise, if this is a leaf node and solvable, than try to trace back.
            // Otherwise, this is a unsolvable leaf node, which means no need to trace back and return to the caller immediately.
            if (!node.IsLeafNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                    DepthFirstSearch(child, marked, ref solved);
            }
            else if (node.Solvable == true)
            {
                // Trace back.

                LinkedList<AndOrTreeNode> solutionNodes = new LinkedList<AndOrTreeNode>();
                solutionTree = TraceUp(node, solutionNodes, marked);

                // Set `solved` = true to stop further searching procedure.
                if (solutionNodes.Contains(root))
                    solved = true;
            }
        }

        /// <summary>
        /// Runs depth first search to find the solution tree of this <see cref="AndOrTree" /> within limited depth.
        /// </summary>
        /// <param name="maxDepth">The limited depth.</param>
        /// <returns>The solution tree of this <see cref="AndOrTree" />, null if no such solution tree.</returns>
        public AndOrTree DepthFirstSearch(long maxDepth)
        {
            // Eliminate the result found before.
            solutionTree = null;

            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            // According to DFS algorithm from Graph Theory, queue is unnecessary here.
            //Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            //nodes.Enqueue(root);

            // Start DFS search.
            bool solved = false;
            DepthFirstSearch(maxDepth, 0, root, marked, ref solved);

            // Return the solution tree found by DFS search.
            return solutionTree;
        }

        /// <summary>
        /// Runs depth first search on the given node.
        /// </summary>
        /// <param name="maxDepth">The limited depth.</param>
        /// <param name="currentDepth">Current depth.</param>
        /// <param name="node">The node to process.</param>
        /// <param name="marked">A boolean array whose entries indicates whether a node is processed by DFS before.</param>
        /// <param name="solved">A boolean value indicating whether a solution tree is found (true) or not (false).</param>
        public void DepthFirstSearch(long maxDepth, int currentDepth, AndOrTreeNode node, bool[] marked, ref bool solved)
        {
            // Stop searching if DFS has found a solution.
            if (solved)
                return;

            // Mark this node.
            marked[node.Id] = true;

            // Stop searching if the depth is larger than the given limit.
            if (currentDepth >= maxDepth)
                return;

            // If this is not a leaf node, keep searching.
            // Otherwise, if this is a leaf node and solvable, than try to trace back.
            // Otherwise, this is a unsolvable leaf node, which means no need to trace back and return to the caller immediately.
            if (!node.IsLeafNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                    DepthFirstSearch(maxDepth, currentDepth + 1, child, marked, ref solved);
            }
            else if (node.Solvable == true)
            {
                // Trace back.

                LinkedList<AndOrTreeNode> solutionNodes = new LinkedList<AndOrTreeNode>();
                solutionTree = TraceUp(node, solutionNodes, marked);

                // Set `solved` = true to stop further searching procedure.
                if (solutionNodes.Contains(root))
                    solved = true;
            }
        }

        /// <summary>
        /// Runs breadth first search on the given node.
        /// </summary>
        /// <returns>The solution tree of this <see cref="AndOrTree" />, null if no such solution tree.</returns>
        public AndOrTree BreadthFirstSearch()
        {
            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            nodes.Enqueue(root);
            
            bool[] marked = new bool[treeNodes.Length];
            while (nodes.Count != 0)
            {
                // Get the next node to process.
                AndOrTreeNode node = nodes.Dequeue();
                marked[node.Id] = true;

                // Add all child nodes of this node if it is not a leaf node.
                if (!node.IsLeafNode)
                {
                    foreach (AndOrTreeNode child in node.GetChildren())
                        nodes.Enqueue(child);
                }
                else if (node.Solvable == true)
                {
                    LinkedList<AndOrTreeNode> solutionNodes = new LinkedList<AndOrTreeNode>();
                    solutionTree = TraceUp(node, solutionNodes, marked);

                    // Break the search procedure if a solution is find.
                    if (solutionNodes.Contains(root))
                        break;
                }
            }

            return solutionTree;
        }

        /// <summary>
        /// Traces up to find the solution tree.
        /// </summary>
        /// <param name="node">The potential solution node to process.</param>
        /// <param name="solutionNodes">A linked list that contains all the potential solution nodes.</param>
        /// <param name="marked">A boolean array whose entries indicates whether a node is processed by DFS/BFS before.</param>
        /// <returns>The solution tree of this <see cref="AndOrTree" />, null if no such solution tree.</returns>
        private AndOrTree TraceUp(AndOrTreeNode node, LinkedList<AndOrTreeNode> solutionNodes, bool[] marked)
        {
            // Build solution tree if trace back to root successfully.
            if (node == root)
            {
                solutionNodes.AddLast(node);
                return BuildSolutionTree(solutionNodes);
            }

            // This `if` statement might be unnecessary since the trace back procedure will be called only when DFS/BFS reaches a solvable leaf node.
            if (node.IsLeafNode)
            {
                if (node.Solvable == true)
                {
                    solutionNodes.AddLast(node);
                    return TraceUp(node.Parent, solutionNodes, marked);
                }
                else
                    return null;
            }

            // Trace back to the parent node of this node if this is an or node since there is child node of this node is solvable.
            if (node.NodeType == AndOrTreeNodeType.OrNode)
            {
                solutionNodes.AddLast(node);
                return TraceUp(node.Parent, solutionNodes, marked);
            }

            // Trace down each child node of this and node.
            // Trace up only when each child of this and node is solvable.
            // Return null, which represents that this and node is not solvable, if any child node of this and node is not solvable.
            if (node.NodeType == AndOrTreeNodeType.AndNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (!TraceDown(child, solutionNodes, marked))
                        return null;
                }

                solutionNodes.AddLast(node);
                return TraceUp(node.Parent, solutionNodes, marked);
            }

            return null;
        }

        /// <summary>
        /// Traces down from a potential solution node to examine whether the sub-tree rooted at the given node is a part of the solution tree (i.e. is solvable),
        /// and store all the potential solution nodes.
        /// </summary>
        /// <param name="node">The potential solution node to process.</param>
        /// <param name="solutionNodes">A linked list that contains all the potential solution nodes.</param>
        /// <param name="marked">A boolean array whose entries indicates whether a node is processed by DFS/BFS before.</param>
        /// <returns>True if this node is solvable, otherwise, false.</returns>
        private bool TraceDown(AndOrTreeNode node, LinkedList<AndOrTreeNode> solutionNodes, bool[] marked)
        {
            // If this node is not marked, then do nothing.
            // If process this node here, then the BFS and DFS sequence will be broken.
            if (!marked[node.Id])
                return false;

            // Return true if this node is accessed before.
            // This `if` statement might be unnecessary.
            if (solutionNodes.Contains(node))
                return true;

            // Leaf nodes should be processed specially.
            if (node.IsLeafNode)
            {
                // If this node is a leaf node and it is a solvable node, than add this node to the solution nodes and return true.
                // Otherwise, this node is not solvable, return false.
                if (node.Solvable == true)
                {
                    solutionNodes.AddLast(node);
                    return true;
                }
                else
                    return false;
            }

            // If any child node of an or node is solvable, add this or node to solution nodes.
            // Otherwise, this or node is unsolvable, no need to keep searching, return false.
            if (node.NodeType == AndOrTreeNodeType.OrNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (TraceDown(child, solutionNodes, marked))
                    {
                        solutionNodes.AddLast(node);
                        return true;
                    }
                }


                return false;
            }

            // If all child nodes of an and node are solvable, add this and node to solution nodes.
            // Otherwise, this and node is unsolvable, no need to keep searching, return false.
            if (node.NodeType == AndOrTreeNodeType.AndNode)
            {
                foreach (AndOrTreeNode child in node.GetChildren())
                {
                    if (!TraceDown(child, solutionNodes, marked))
                        return false;
                }

                solutionNodes.AddLast(node);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Builds a solution tree of this <see cref="AndOrTree" /> by solution nodes in the given collection.
        /// </summary>
        /// <param name="solutionNodes">The collection that contains all the nodes in the solution tree.</param>
        /// <returns>A solution tree of this <see cref="AndOrTree" />.</returns>
        private AndOrTree BuildSolutionTree(LinkedList<AndOrTreeNode> solutionNodes)
        {
            // Create a new tree with the same root.
            solutionTree = new AndOrTree(this.root.DeepCopy());

            // Make deep copies of nodes in the solution tree.
            // Other nodes will be `null`.
            solutionTree.treeNodes = new AndOrTreeNode[this.treeNodes.Length];
            foreach (AndOrTreeNode node in solutionNodes)
                solutionTree.treeNodes[node.Id] = node.DeepCopy();

            // Fix links of solution tree.
            foreach (AndOrTreeNode node in solutionNodes)
            {
                if (node.Parent != null)
                {
                    solutionTree.treeNodes[node.Id].Parent = solutionTree.treeNodes[node.Parent.Id];
                    solutionTree.treeNodes[node.Parent.Id].AddNode(solutionTree.treeNodes[node.Id]);
                }
            }

            return solutionTree;
        }

        /// <summary>
        /// Returns the string representation of this <see cref="AndOrTree" />, which contains all the node information in each line, followed by the ID of the root node at the last line.
        /// </summary>
        /// <returns>The string representation of this <see cref="AndOrTree" />, which contains all the node information in each line, followed by the ID of the root node at the last line.</returns>
        public override string ToString()
        {
            StringBuilder treeStructure = new StringBuilder();

            foreach (AndOrTreeNode node in treeNodes)
            {
                if (node != null)
                {
                    treeStructure.Append(node.ToString());
                    treeStructure.Append(Environment.NewLine);
                }
            }

            treeStructure.Append("Root: " + root.Id);
            treeStructure.Append(Environment.NewLine);

            return treeStructure.ToString();
        }
    }
}
