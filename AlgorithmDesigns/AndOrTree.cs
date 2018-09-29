using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class AndOrTree
    {
        private const string Or = "or";
        private const string And = "and";
        private const string True = "true";
        private const string False = "false";

        private AndOrTreeNode root;
        private AndOrTreeNode[] treeNodes;

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

                bool value;
                if (line[2] == True)
                    value = true;
                else if (line[2] == False)
                    value = false;
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


            return null;
        }

        private void DepthFirstSearch(AndOrTreeNode node)
        {

        }

        public AndOrTree DepthFirstSearch(long maxDepth)
        {
            return null;
        }

        public AndOrTree BreadthFirstSearch()
        {
            return null;
        }

        public override string ToString()
        {
            StringBuilder treeStructure = new StringBuilder();

            foreach (AndOrTreeNode node in treeNodes)
            {
                treeStructure.Append(node.Id + " (" + node.NodeType + "), " + node.Value + ": ");

                foreach (AndOrTreeNode child in node.GetChildren())
                    treeStructure.Append(child.Id + " ");

                treeStructure.Append(Environment.NewLine);
            }

            treeStructure.Append("Root: " + root.Id);
            treeStructure.Append(Environment.NewLine);

            return treeStructure.ToString();
        }
    }
}
