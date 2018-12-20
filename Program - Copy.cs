using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
namespace ConfigConverter
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            try
            {
                var dialod = new System.Windows.Forms.OpenFileDialog();
                dialod.Filter = "Web.config files (*.config)|*.config";
                if (dialod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var fi = new FileInfo(dialod.FileName);
                    XDocument doc = XDocument.Load(fi.FullName);
                    string[] elementPaths = new string[] {
                    "/configuration/appSettings",
                    "/configuration/connectionStrings",
                    "/configuration/system.web/identity",
                    "/configuration/system.web/sessionState",
                    "/configuration/system.web/globalization",
                    "/configuration/system.web/authentication",
                    "/configuration/system.web/authorization",
                    "/configuration/system.web/customErrors",
                };
                    string[] fixedAppstrings = new string[] {
                    "ClientValidationEnabled",
                    "UnobtrusiveJavaScriptEnabled",
                    "ModelName",
                    "aspnet:MaxHttpCollectionKeys",
                    "TempFolder",
                    "DefaultImageUrl",
                    "PoundName",
                    "PiasterName",
                };
                    bool needToSave = false;
                    for (int i = 0; i < elementPaths.Length; i++)
                    {
                        var path = elementPaths[i];
                        Console.WriteLine(path);
                        var el = doc.Root.XPathSelectElement(path);
                        if (el != null)
                        {
                            var attrName = i == 0 ? "file" : "configSource";
                            var currentAttrVal = el.Attribute(attrName)?.Value;
                            if (string.IsNullOrWhiteSpace(currentAttrVal))
                            {
                                var newElement = XElement.Parse(el.ToString());
                                var newElFileName = $"web.{el.Name}{fi.Extension}";
                                el.Attributes().ToList().ForEach(c => c.Remove());
                                if (i == 0)
                                {
                                    el.Elements().Where(c => c.Name == "add" && !fixedAppstrings.Contains(c.Attribute("key").Value)).ToList().ForEach(c => c.Remove());
                                    newElement.Elements().Where(c => c.Name == "add" && fixedAppstrings.Contains(c.Attribute("key").Value)).ToList().ForEach(c => c.Remove());
                                }
                                else
                                    el.Elements().ToList().ForEach(c => c.Remove());
                                el.SetAttributeValue(attrName, newElFileName);
                                needToSave = true;
                                newElement.Save(Path.Combine(fi.DirectoryName, newElFileName));
                            }
                        }
                    }
                    if (needToSave)
                    {
                        doc.Save(fi.FullName);
                        Console.WriteLine("Done");
                    }
                    else
                    {
                        Console.WriteLine("Nothing happened!...");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }
            Console.ReadLine();

        }
    }
}
