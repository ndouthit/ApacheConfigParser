using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApacheConfigParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ApacheConfigParser.Tests
{
    [TestClass()]
    public class ConfigParserTests
    {
        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ParseInputNullOrEmptyTest()
        {
            ConfigParser p = new ConfigParser();
            p.Parse(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ParseInputFileNotFoundTest()
        {
            var httpdConfPath = @"Resources\httpd-does-not-exist.conf";

            ConfigParser p = new ConfigParser();
            p.Parse(httpdConfPath);
        }

        [TestMethod()]
        public void ParseExampleConfTest()
        {
            var httpdConfPath = @"Resources\example.conf";

            ConfigParser p = new ConfigParser();
            ConfigNode root = p.Parse(httpdConfPath);

            VerifyNode(null, null, 2, root);

            List<ConfigNode> vhosts = root.GetChildren();

            Assert.AreEqual(vhosts.Count, 2);

            // Virtual Host 1
            {
                ConfigNode vhost = vhosts[0];
                VerifyNode("VirtualHost", "*", 2, vhost);

                List<ConfigNode> vHostDirectives = vhost.GetChildren();

                ConfigNode serverName = vHostDirectives[0];
                VerifyNode("ServerName", "example.com", 0, serverName);

                ConfigNode redirect = vHostDirectives[1];
                VerifyNode("Redirect", "permanent / http://www.example.com/", 0, redirect);
            }

            // Virtual Host 2
            {
                ConfigNode vhost = vhosts[1];
                VerifyNode("VirtualHost", "*", 7, vhost);

                List<ConfigNode> vHostDirectives = vhost.GetChildren();

                ConfigNode documentRoot = vHostDirectives[0];
                VerifyNode("DocumentRoot", @"""C:\xampp\apache\htdocs\www.example.com""", 0, documentRoot);

                ConfigNode serverName = vHostDirectives[1];
                VerifyNode("ServerName", "www.example.com", 0, serverName);

                ConfigNode rewriteEngine = vHostDirectives[2];
                VerifyNode("RewriteEngine", "On", 0, rewriteEngine);

                ConfigNode rewriteRule = vHostDirectives[3];
                VerifyNode("RewriteRule", @"^/$ /w/extract2.php?title=Www.example.com_portal&template=Www.example.com_template [L]",
                            0, rewriteRule);

                ConfigNode phpAdminFlag = vHostDirectives[4];
                VerifyNode("php_admin_flag", "engine on", 0, phpAdminFlag);

                // Directory 1
                {
                    ConfigNode directory = vHostDirectives[5];
                    VerifyNode("Directory", @"""C:\xampp\apache\htdocs\www""", 3, directory);

                    List<ConfigNode> directoryDirectives = directory.GetChildren();

                    ConfigNode order = directoryDirectives[0];
                    VerifyNode("Order", "Deny,Allow", 0, order);

                    ConfigNode allow = directoryDirectives[1];
                    VerifyNode("Allow", "from env=tarpitted_bots", 0, allow);

                    ConfigNode deny = directoryDirectives[2];
                    VerifyNode("Deny", "from env=bad_bots", 0, deny);
                }

                // Directory 2
                {
                    ConfigNode directory = vHostDirectives[6];
                    VerifyNode("Directory", @"""C:\xampp\apache\htdocs\www\stats""", 8, directory);

                    List<ConfigNode> directoryDirectives = directory.GetChildren();

                    ConfigNode allowOverride = directoryDirectives[0];
                    VerifyNode("AllowOverride", "All", 0, allowOverride);

                    ConfigNode expiresByTypeGif = directoryDirectives[1];
                    VerifyNode("ExpiresByType", "image/gif A0", 0, expiresByTypeGif);

                    ConfigNode expiresByTypePng = directoryDirectives[2];
                    VerifyNode("ExpiresByType", "image/png A0", 0, expiresByTypePng);

                    ConfigNode expiresByTypeJpeg = directoryDirectives[3];
                    VerifyNode("ExpiresByType", "image/jpeg A0", 0, expiresByTypeJpeg);

                    ConfigNode expiresByTypeCss = directoryDirectives[4];
                    VerifyNode("ExpiresByType", "text/css A2592000", 0, expiresByTypeCss);

                    ConfigNode expiresByTypeTextJs = directoryDirectives[5];
                    VerifyNode("ExpiresByType", "text/javascript A2592000", 0, expiresByTypeTextJs);

                    ConfigNode expiresByTypeAppJs = directoryDirectives[6];
                    VerifyNode("ExpiresByType", "application/x-javascript A2592000", 0, expiresByTypeAppJs);

                    ConfigNode expiresByTypeHtml = directoryDirectives[7];
                    VerifyNode("ExpiresByType", "text/html A0", 0, expiresByTypeHtml);

                }
            }
        }

        public static void VerifyNode(string expectedName, string expectedContent, int expectedChildCount, ConfigNode node)
        {
            Assert.AreEqual(expectedName, node.GetName());
            Assert.AreEqual(expectedContent, node.GetContent());
            Assert.AreEqual(expectedChildCount, node.GetChildren().Count);
        }
    }
}