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

        public bool Value { get; set; }

        public AndOrTreeNodeType NodeType { get; }

        public AndOrTreeNode Parent { get; set; }

        private LinkedList<AndOrTreeNode> children;

        public bool IsLeafNode => children.Count == 0;

        public AndOrTreeNode(int id, AndOrTreeNodeType nodeType, bool value = false)
        {
            this.Id = id;
            this.Value = value;
            this.NodeType = nodeType;
            this.Parent = null;
            children = new LinkedList<AndOrTreeNode>();
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
    }
}
