﻿using Windows.System;
using Windows.System.Collections.Generic;
using Windows.System.Linq;
using Windows.System.Text;
using Windows.System.Threading.Tasks;
using Windows.System.Xml;

namespace CS3450.TooRisky
{
    static class XML
    {
        public void read()
        {
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
