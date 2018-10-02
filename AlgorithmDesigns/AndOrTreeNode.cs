using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDesigns
{
    /// <summary>
    /// The <see cref="AndOrTreeNode" /> class represents a node in an and or tree.
    /// </summary>
    public class AndOrTreeNode
    {
        /// <summary>
        /// The ID of this node.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets a value indicating
        /// </summary>
        public bool? Solvable { get; }

        /// <summary>
        /// Gets the type of this <see cref="AndOrTreeNode" />.
        /// </summary>
        public AndOrTreeNodeType NodeType { get; }

        /// <summary>
        /// Gets or sets the Parent node of this <see cref="AndOrTreeNode" />.
        /// </summary>
        public AndOrTreeNode Parent { get; set; }

        /// <summary>
        /// The collection of child nodes of this <see cref="AndOrTreeNode" />.
        /// </summary>
        private readonly LinkedList<AndOrTreeNode> children;

        /// <summary>
        /// Gets a value indicating whether this <see cref="AndOrTreeNode" /> is a leaf node.
        /// </summary>
        public bool IsLeafNode => children.Count == 0;

        /// <summary>
        /// Initializes a new instance of <see cref="AndOrTreeNode" /> with given value.
        /// </summary>
        /// <param name="id">The ID of this node.</param>
        /// <param name="nodeType">The type of this node.</param>
        /// <param name="solvable">A value indicating whether this node is solvable when initializing it.</param>
        public AndOrTreeNode(int id, AndOrTreeNodeType nodeType, bool? solvable = null)
        {
            this.Id = id;
            this.Solvable = solvable;
            this.NodeType = nodeType;
            this.Parent = null;
            children = new LinkedList<AndOrTreeNode>();
        }

        /// <summary>
        /// Returns a deep copy of this node (containing `ID`, `NodeType` and `Solvable` information, but not containing any child nodes).
        /// </summary>
        /// <returns>A deep copy of this node (containing `ID`, `NodeType` and `Solvable` information, but not containing any child nodes).</returns>
        public AndOrTreeNode DeepCopy()
        {
            AndOrTreeNode copy = new AndOrTreeNode(this.Id, this.NodeType, this.Solvable);
            return copy;
        }

        /// <summary>
        /// Adds a child node to this <see cref="AndOrTreeNode" />.
        /// </summary>
        /// <param name="node">The child node to add.</param>
        public void AddNode(AndOrTreeNode node)
        {
            children.AddLast(node);
        }

        /// <summary>
        /// Removes a child node from this <see cref="AndOrTreeNode" />.
        /// </summary>
        /// <param name="node">The child node to remove.</param>
        public void RemoveNode(AndOrTreeNode node)
        {
            children.Remove(node);
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}" /> that supports iterate through all child nodes of this <see cref="AndOrTreeNode" />.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}" /> that supports iterate through all child nodes of this <see cref="AndOrTreeNode" />.</returns>
        public IEnumerable<AndOrTreeNode> GetChildren()
        {
            return children;
        }

        /// <summary>
        /// Returns the string representation of this <see cref="AndOrTreeNode" />.
        /// </summary>
        /// <returns>The string representation of this <see cref="AndOrTreeNode" />.</returns>
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
