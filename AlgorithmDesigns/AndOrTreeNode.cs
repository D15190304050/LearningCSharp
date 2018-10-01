using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    public class AndOrTreeNode
    {
        public int Id { get; }

        public bool? Solvable { get; set; }

        public AndOrTreeNodeType NodeType { get; }

        public AndOrTreeNode Parent { get; set; }

        private LinkedList<AndOrTreeNode> children;

        public bool IsLeafNode => children.Count == 0;

        public AndOrTreeNode(int id, AndOrTreeNodeType nodeType, bool? solvable = null)
        {
            this.Id = id;
            this.Solvable = solvable;
            this.NodeType = nodeType;
            this.Parent = null;
            children = new LinkedList<AndOrTreeNode>();
        }

        public AndOrTreeNode DeepCopy()
        {
            AndOrTreeNode copy = new AndOrTreeNode(this.Id, this.NodeType, this.Solvable);
            return copy;
        }

        public void AddNode(AndOrTreeNode node)
        {
            children.AddLast(node);
        }

        public void RemoveNode(AndOrTreeNode node)
        {
            children.Remove(node);
        }

        public IEnumerable<AndOrTreeNode> GetChildren()
        {
            return children;
        }

        public override string ToString()
        {
            StringBuilder nodeInfo = new StringBuilder();
            string solvable;
            if (this.Solvable == true)
                solvable = "True";
            else if (this.Solvable == false)
                solvable = "False";
            else
                solvable = "Unknown";
            nodeInfo.Append(this.Id + " (" + this.NodeType + "), " + solvable + ": ");
            foreach (AndOrTreeNode child in this.GetChildren())
                nodeInfo.Append(child.Id + " ");

            return nodeInfo.ToString();
        }
    }
}
