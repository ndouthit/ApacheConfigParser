using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApacheConfigParser
{
    /// <summary>
    /// A parser for Apache configuration files.
    /// This parser reads the configuration file line by line and builds a tree of the configuration.
    /// </summary>
    public class ConfigParser
    {
        private static readonly Regex commentRegex = new Regex("#.*", RegexOptions.IgnoreCase);
        private static readonly Regex directiveRegex = new Regex("([^\\s]+)\\s*(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex sectionOpenRegex = new Regex("<([^/\\s>]+)\\s*([^>]+)?>", RegexOptions.IgnoreCase);
        private static readonly Regex sectionCloseRegex = new Regex("</([^\\s>]+)\\s*>", RegexOptions.IgnoreCase);

        private static Match commentMatcher = commentRegex.Match(string.Empty);
        private static Match directiveMatcher = directiveRegex.Match(string.Empty);
        private static Match sectionOpenMatcher = sectionOpenRegex.Match(string.Empty);
        private static Match sectionCloseMatcher = sectionCloseRegex.Match(string.Empty);

        public ConfigParser()
        {
        }

        /// <summary>
        /// Parses the provided Apache configuration file.
        /// </summary>
        /// <param name="confPath">Path to Apache configuration file</param>
        /// <returns>A tree of ConfigNodes</returns>
        /// <example>
        /// var httpdConfPath = @"C:\xampp\apache\conf\httpd.conf";
        /// Parser p = new Parser();
        /// ConfigNode c = p.Parse(httpdConfPath);
        /// foreach (var child in c.GetChildren())
        /// {
        /// Console.WriteLine(child);
        /// }
        /// </example>
        public ConfigNode Parse(string confPath)
        {
            if (string.IsNullOrEmpty(confPath))
            {
                throw new NullReferenceException("confPath: null or empty");
            }

            if( !(File.Exists(confPath)) )
            {
                throw new FileNotFoundException("confPath: file not found");
            }

            ConfigNode currentNode = ConfigNode.CreateRootNode();

            using (var reader = new StreamReader(confPath))
            {
                string line;
                while ( (line = reader.ReadLine()) != null )
                {
                    commentMatcher = commentRegex.Match(line);
                    sectionOpenMatcher = sectionOpenRegex.Match(line);
                    sectionCloseMatcher = sectionCloseRegex.Match(line);
                    directiveMatcher = directiveRegex.Match(line);
                    if (commentMatcher.Success)
                    {
                        continue;
                    }
                    else if(sectionOpenMatcher.Success)
                    {
                        string name = sectionOpenMatcher.Groups[1].Value;
                        string content = sectionOpenMatcher.Groups[2].Value;
                        ConfigNode sectionNode = ConfigNode.CreateChildNode(name, content, currentNode);
                        currentNode = sectionNode;
                    }
                    else if(sectionCloseMatcher.Success)
                    {
                        currentNode = currentNode.GetParent();
                    }
                    else if(directiveMatcher.Success)
                    {
                        string name = directiveMatcher.Groups[1].Value;
                        string content = directiveMatcher.Groups[2].Value;
                        ConfigNode.CreateChildNode(name, content, currentNode);
                    }
                }
            }

            return currentNode;
        }
    }
}
