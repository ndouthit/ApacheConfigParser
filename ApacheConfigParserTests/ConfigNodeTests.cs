using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApacheConfigParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApacheConfigParser.Tests
{
    [TestClass()]
    public class ConfigNodeTests
    {
        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateChildNodeWithNullNameThrowsNullRefereceExceptionTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode.CreateChildNode(null, "content", root);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateChildNodeWithNullContentThrowsNullRefereceExceptionTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode.CreateChildNode("name", null, root);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateChildNodeWithNullParentThrowsNullRefereceExceptionTest()
        {
            ConfigNode.CreateChildNode("name", "content", null);
        }

        [TestMethod()]
        public void GetNameFromRootIsNullTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            Assert.IsNull(root.GetName());
        }

        [TestMethod()]
        public void GetContentFromRootIsNullTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            Assert.IsNull(root.GetContent());
        }

        [TestMethod()]
        public void GetParentFromRootIsNullTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            Assert.IsNull(root.GetParent());
        }

        [TestMethod()]
        public void GetNameFromChildTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child = ConfigNode.CreateChildNode("name", "content", root);

            Assert.AreEqual("name", child.GetName());
        }

        [TestMethod()]
        public void GetContentFromChildTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child = ConfigNode.CreateChildNode("name", "content", root);

            Assert.AreEqual("content", child.GetContent());
        }

        [TestMethod()]
        public void GetParentFromChildTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child = ConfigNode.CreateChildNode("name", "content", root);

            Assert.AreEqual(root, child.GetParent());
        }

        [TestMethod()]
        public void IsRootNodeTrueTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();

            Assert.IsTrue(root.IsRootNode());
        }

        [TestMethod()]
        public void IsRootNodeFalseTest()
        {
            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child = ConfigNode.CreateChildNode("name", "content", root);

            Assert.IsFalse(child.IsRootNode());
        }

        [TestMethod()]
        public void ToStringRootTest()
        {
            string expectedString = "ConfigNode {name=null, content=null, childNodeCount=3}";

            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child1 = ConfigNode.CreateChildNode("child1", "content1", root);
            ConfigNode child2 = ConfigNode.CreateChildNode("child2", "content2", root);
            ConfigNode child3 = ConfigNode.CreateChildNode("child3", "content3", root);

            Assert.AreEqual(expectedString, root.ToString());
        }

        [TestMethod()]
        public void ToStringChildTest()
        {
            string expectedString = "ConfigNode {name=child2, content=content2, childNodeCount=0}";

            ConfigNode root = ConfigNode.CreateRootNode();
            ConfigNode child1 = ConfigNode.CreateChildNode("child1", "content1", root);
            ConfigNode child2 = ConfigNode.CreateChildNode("child2", "content2", root);
            ConfigNode child3 = ConfigNode.CreateChildNode("child3", "content3", root);

            Assert.AreEqual(expectedString, child2.ToString());
        }
    }
}