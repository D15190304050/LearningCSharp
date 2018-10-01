using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class AndOrTree
    {
        // CSDN link: https://blog.csdn.net/dala_da/article/details/78586845.

        private const string Or = "or";
        private const string And = "and";
        private const string Unknown = "unknown";
        private const string True = "true";
        private const string False = "false";

        private AndOrTreeNode root;
        private AndOrTreeNode[] treeNodes;
        private AndOrTree solutionTree;

        public AndOrTree()
        {
        }

        public AndOrTree(string filePath)
        {
            string[] treeNodeText = System.IO.File.ReadAllLines(filePath);
            int nodeCount = treeNodeText.Length;
            treeNodes = new AndOrTreeNode[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                string[] line = treeNodeText[i].Split(' ');
                AndOrTreeNodeType nodeType;
                if (line[1] == Or)
                    nodeType = AndOrTreeNodeType.OrNode;
                else if (line[1] == And)
                    nodeType = AndOrTreeNodeType.AndNode;
                else
                    throw new FormatException("Invalid data format of `AndOrTreeNodeType` in file " + filePath + ", line " + i);

                bool? value;
                if (line[2] == True)
                    value = true;
                else if (line[2] == False)
                    value = false;
                else if (line[2] == Unknown)
                    value = null;
                else
                    throw new FormatException("Invalid data format of `value` in file " + filePath + ", line " + i);

                AndOrTreeNode node = new AndOrTreeNode(int.Parse(line[0]), nodeType, value);
                treeNodes[i] = node;

                for (int j = 3; j < line.Length; j++)
                {
                    int child = int.Parse(line[j]);
                    if (child >= i)
                        throw new FormatException("Invalid data format of `parent` in file " + filePath + ", line " + i);

                    treeNodes[child].Parent = node;
                    node.AddNode(treeNodes[child]);
                }
            }

            foreach (AndOrTreeNode node in treeNodes)
            {
                if (node.Parent == null)
                    root = node;
            }
        }

        public AndOrTree DepthFirstSearch()
        {
            solutionTree = null;

            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
            {
                solutionTree = new AndOrTree();

                if (root.Solvable == true)
                {
                    solutionTree.root = this.root.DeepCopy();
                    return solutionTree;
                }
                else
                    return null;
            }

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            // According to DFS algorithm from Graph Theory, queue is unnecessary here.
            //Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            //nodes.Enqueue(root);

            bool solved = false;
            DepthFirstSearch(root, marked, ref solved);

            return solutionTree;
        }

        private void DepthFirstSearch(AndOrTreeNode node, bool[] marked, ref bool solved)
        {
            // Stop searching if DFS has found a solution.
            if (solved)
                return;

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
                LinkedList<AndOrTreeNode> solutionNodes = new LinkedList<AndOrTreeNode>();
                solutionTree = TraceUp(node, solutionNodes, marked);

                if (solutionNodes.Contains(root))
                    solved = true;
            }
        }

        public AndOrTree DepthFirstSearch(long maxDepth)
        {
            solutionTree = null;

            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
            {
                solutionTree = new AndOrTree();

                if (root.Solvable == true)
                {
                    solutionTree.root = this.root.DeepCopy();
                    return solutionTree;
                }
                else
                    return null;
            }

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            // According to DFS algorithm from Graph Theory, queue is unnecessary here.
            //Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            //nodes.Enqueue(root);

            bool solved = false;
            DepthFirstSearch(maxDepth, 0, root, marked, ref solved);

            return solutionTree;
        }

        public void DepthFirstSearch(long maxDepth, int currentDepth, AndOrTreeNode node, bool[] marked, ref bool solved)
        {
            // Stop searching if DFS has found a solution.
            if (solved)
                return;
            marked[node.Id] = true;

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
                LinkedList<AndOrTreeNode> solutionNodes = new LinkedList<AndOrTreeNode>();
                solutionTree = TraceUp(node, solutionNodes, marked);

                if (solutionNodes.Contains(root))
                    solved = true;
            }
        }

        public AndOrTree BreadthFirstSearch()
        {
            // If this tree has only 1 node (the root).
            // If the root is true, return the tree itself.
            // Otherwise return null, meaning: no proper solution.
            if (root.IsLeafNode)
                return root.Solvable == true ? this : null;

            Queue<AndOrTreeNode> nodes = new Queue<AndOrTreeNode>();
            nodes.Enqueue(root);

            // A boolean array that records whether a node is marked before.
            bool[] marked = new bool[treeNodes.Length];

            while (nodes.Count != 0)
            {
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

                    if (solutionNodes.Contains(root))
                        break;
                }
            }

            return solutionTree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="solutionNodes"></param>
        /// <param name="marked"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="solutionNodes"></param>
        /// <param name="marked"></param>
        /// <returns></returns>
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

        private AndOrTree BuildSolutionTree(LinkedList<AndOrTreeNode> solutionNodes)
        {
            AndOrTree solutionTree = new AndOrTree();
            solutionTree.root = this.root.DeepCopy();
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
