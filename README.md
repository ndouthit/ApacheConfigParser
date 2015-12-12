# ApacheConfigParser

A simple parser for Apache HTTP server config files.

[![Build status](https://ci.appveyor.com/api/projects/status/0pva9cf6w7q4v2tx?svg=true)](https://ci.appveyor.com/project/ndouthit/apacheconfigparser)

---

This project was ported from Stackify's Java-based [apache-config](https://github.com/stackify/apache-config/) package.

---

## Overview

The parser iterates through the config file and builds out a tree of the various settings.

## Usage

The code below parses the httpd.conf file and outputs the contents of the ServerName directive in the main body of the file in addition to the ServerName directives within the VirtualHost tags.

**httpd.conf:**
```
Listen 80

ServerName localhost:80

DocumentRoot "C:/xampp/htdocs"

<VirtualHost *:443>
        ServerAdmin webmaster@foo.com
        ServerName www.foo.com
		
        DocumentRoot /home/www/www.foo.com/htdocs/

        # Logfiles
        ErrorLog  /home/www/www.foo.com/logs/error.log
        CustomLog /home/www/www.foo.com/logs/access.log combined
</VirtualHost>

<VirtualHost *>
        ServerAdmin webmaster@bar.com
        ServerName www.bar.com

        DocumentRoot /home/www/www.bar.com/htdocs/
		
        # Logfiles
        ErrorLog  /home/www/www.bar.com/logs/error.log
        CustomLog /home/www/www.bar.com/logs/access.log combined
</VirtualHost>
```

**Code:**
```cs
var httpdConfPath = @"C:\xampp\apache\conf\httpd.conf";

ConfigParser p = new ConfigParser();
ConfigNode root = p.Parse(httpdConfPath);

foreach (var child1 in root.GetChildren())
{
    if (child1.GetName().Equals("ServerName"))
        Console.WriteLine(child1.GetContent());
    if (child1.GetName().Equals("VirtualHost"))
    {
        foreach (var child2 in child1.GetChildren())
        {
            if (child2.GetName().Equals("ServerName"))
            {
                Console.WriteLine(child2.GetContent());
            }
        }
    }
}
```

**Output:**
```
localhost:80
www.foo.com
www.bar.com
```