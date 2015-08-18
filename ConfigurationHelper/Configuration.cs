using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Configuration;

namespace ConfigurationHelper
{
    public class Configuration
    {
        public static string GetConnectionString(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
                //string connectionstring = string.Empty;
                //string path = Assembly.GetExecutingAssembly().Location;
                //path = Path.GetDirectoryName(path);
                //string file = Path.Combine(path, "datasystem.config");
                //XmlDocument xmldoc = new XmlDocument();
                //xmldoc.Load(file);

                //XmlNodeList nodelist =
                //    xmldoc.SelectNodes("//configuration/connectionStrings/add");
                //foreach (XmlNode node in nodelist)
                //{
                //    if (node.Attributes["name"].Value == name)
                //    {
                //        connectionstring = node.Attributes["connectionString"].Value;
                //        break;
                //    }
                //}
                //return connectionstring;
            }
            catch
            {
                throw;
            }
        }
    }
}
