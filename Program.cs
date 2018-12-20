using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
namespace ConfigSplitter
{
    class Program
    {
        //decalre here sections you want to split
        readonly static List<string> _elementPaths = new List<string> {
                    "/configuration/appSettings",
                    "/configuration/connectionStrings",
                    "/configuration/system.web/identity",
                    "/configuration/system.web/sessionState",
                    "/configuration/system.web/globalization",
                    "/configuration/system.web/authentication",
                    "/configuration/system.web/authorization",
                    "/configuration/system.web/customErrors",
                    };
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            try
            {
                ByFunctional();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }
            Console.ReadLine();
        }

        //Split config file sections using functional programming
        private static void ByFunctional()
        {
            new OpenFileDialog { Filter = "Web.config files (*.config)|*.config" }
            .Pipeline()
            .FindAll(c => c.ShowDialog() == DialogResult.OK)
            .ConvertAll(c => new {File = new FileInfo(c.FileName),Document = XDocument.Load(c.FileName)})
            .Exec(data =>
                _elementPaths
                .ConvertAll(el => data.Document.XPathSelectElement(el))
                .FindAll(el => el != null && string.IsNullOrWhiteSpace(el.Attribute("configSource")?.Value))
                .Exec(el => new XElement(el).Save(Path.Combine(data.File.DirectoryName, $"{el.Name}{data.File.Extension}")))
                .Exec(el => el.Attributes().Exec(x => x.Remove()).ToList())
                .Exec(el => el.Elements().Exec(x => x.Remove()))
                .Exec(el => el.SetAttributeValue("configSource", $"{el.Name}{data.File.Extension}"))
                .Exec(el => Console.WriteLine(el.Name))
                .When(li => li.Any())
                .Then(() => data.Document.Save(data.File.FullName))
                .Then(() => Console.WriteLine("Done"))
                .Else(() => Console.WriteLine("Nothing happened!..."))
            );
        }
        //Split config file sections using Imperative programming
        private static void ByImperative()
        {
            var dialod = new System.Windows.Forms.OpenFileDialog { Filter = "Web.config files (*.config)|*.config" };
            if (dialod.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fi = new FileInfo(dialod.FileName);
                XDocument doc = XDocument.Load(fi.FullName);
                bool needToSave = false;
                foreach (var path in _elementPaths)
                {
                    Console.WriteLine(path);
                    var el = doc.Root.XPathSelectElement(path);
                    if (el != null)
                    {
                        var attrName = "configSource";
                        var currentAttrVal = el.Attribute(attrName)?.Value;
                        if (string.IsNullOrWhiteSpace(currentAttrVal))
                        {
                            var newElement = XElement.Parse(el.ToString());
                            var newElFileName = $"{el.Name}{fi.Extension}";
                            foreach (var item in el.Attributes().ToList())
                                item.Remove();
                            el.Elements().ToList().ForEach(c => c.Remove());
                            el.SetAttributeValue(attrName, newElFileName);
                            needToSave = true;
                            newElement.Save(Path.Combine(fi.DirectoryName, newElFileName));
                            Console.WriteLine(el.Name);
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
    }
}
