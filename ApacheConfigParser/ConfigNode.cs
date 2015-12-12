using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApacheConfigParser
{
    public class ConfigNode
    {
        private string _name;
        private string _content;
        private List<ConfigNode> _children = new List<ConfigNode>();
        private ConfigNode _parent;

        /// <summary>
        /// Private constructor. <see cref="ConfigNode"/> instances should be created via the creation factory methods.
        /// </summary>
        /// <param name="name">The node name</param>
        /// <param name="content">The node content</param>
        /// <param name="parent">The parent of the node</param>
        private ConfigNode(string name, string content, ConfigNode parent)
        {
            _name = name;
            _content = content;
            _parent = parent;
        }

        /// <summary>
        /// Creates a root node.
        /// </summary>
        /// <returns>A new root configuration node.</returns>
        public static ConfigNode CreateRootNode()
        {
            return new ConfigNode(null, null, null);
        }

        /// <summary>
        /// Creates a child node which contains a configuration name and configuration content as well
        /// as a parent node in the tree. If the child node is an apache configuration section it may
        /// also have child nodes of its own.
        /// </summary>
        /// <param name="name">The configuration name (cannot be null)</param>
        /// <param name="content">The configuration content (cannot be null)</param>
        /// <param name="parent">The child nodes parent (cannot be null)</param>
        /// <returns>A new child configuration node</returns>
        /// <exception cref="NullReferenceException">If name, content, or parent is null</exception>
        public static ConfigNode CreateChildNode(string name, string content, ConfigNode parent)
        {
            if (name == null)
            {
                throw new NullReferenceException("Name: null");
            }
            if (content == null)
            {
                throw new NullReferenceException("Content: null");
            }
            if (parent == null)
            {
                throw new NullReferenceException("Parent: null");
            }

            ConfigNode child = new ConfigNode(name, content, parent);
            parent.AddChild(child);

            return child;
        }

        /// <returns>The configuration name; null if this is a root node</returns>
        public string GetName()
        {
            return _name;
        }

        /// <returns>The configuration content; null if this is a root node</returns>
        public string GetContent()
        {
            return _content;
        }

        /// <returns>The nodes parent; null if this is a root node</returns>
        public ConfigNode GetParent()
        {
            return _parent;
        }

        /// <returns>A list of child configuration nodes</returns>
        public List<ConfigNode> GetChildren()
        {
            return _children;
        }

        /// <returns>True if this is a root node; false otherwise</returns>
        public bool IsRootNode()
        {
            return _parent == null;
        }

        public override string ToString()
        {
            var _nameNullString = "null";
            var _contentNullString = "null";

            return "ConfigNode {name=" + (_name ?? _nameNullString) + ", content=" + (_content ?? _contentNullString) + ", childNodeCount=" + _children.Count() + "}";
        }

        private void AddChild(ConfigNode child)
        {
            _children.Add(child);
        }
    }
}
