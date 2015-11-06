using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        { // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create("../../../../classic.xml"))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "Map":
                                string imageName = reader["image"];
                                Console.WriteLine(imageName);
                                break;
                            case "Zone":
                                string zoneName = reader["name"];
                                Console.WriteLine(zoneName);
                                break;
                            case "Country":
                                string attribute = reader["name"];
                                if (attribute != null)
                                {
                                    Console.WriteLine("\t" + attribute);
                                }
                                break;
                            case "AdjCountry":
                                if (reader.Read())
                                {
                                    Console.WriteLine("\t\t Adjacent: " + reader.Value.Trim());
                                }
                                break;
                            case "X":
                                if (reader.Read())
                                {
                                    Console.WriteLine("\t X POS: " + reader.Value.Trim());
                                }
                                break;
                            case "Y":
                                if (reader.Read())
                                {
                                    Console.WriteLine("\t Y POS: " + reader.Value.Trim());
                                }
                                break;
                        }
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
