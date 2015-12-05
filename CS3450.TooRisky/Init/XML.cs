using CS3450.TooRisky.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel;

namespace CS3450.TooRisky
{
    public class XML
    {
        private Game game = Game.DisposeAndAcreateNewGame();
        List<Continent> continents = new List<Continent>();
        List<Country> countries = new List<Country>();
        public string map { set; private get; }
        public string image { get; private set;}
        public Game Read()
        {
            if (map == null)
                map = "classic.xml";
            using (XmlReader reader = XmlReader.Create(Path.Combine(Package.Current.InstalledLocation.Path, "Assets/" + map)))
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
                                image = imageName;
                                break;
                            case "Zone":
                                Continent tempContinent = new Continent();
                                tempContinent.Name = reader["name"];
                                tempContinent.Worth = Int32.Parse(reader["bonus"]);
                                continents.Add(tempContinent);
                                break;
                            case "Country":
                                string attribute = reader["name"];
                                if (attribute != null)
                                {
                                    Country tempCountry = new Country();
                                    tempCountry.Name = attribute;
                                    countries.Add(tempCountry);
                                    continents.Last().CountryNames.Add(attribute);
                                }
                                break;
                            case "AdjCountry":
                                if (reader.Read())
                                {
                                    countries.Last().AdjacentCountryNames.Add(reader.Value.Trim());
                                }
                                break;
                            case "X":
                                if (reader.Read())
                                {
                                    if (reader.Value.Trim().Equals("x"))
                                        countries.Last().X = 0;
                                    else
                                       countries.Last().X = Int32.Parse(reader.Value.Trim());
                                }
                                break;
                            case "Y":
                                if (reader.Read())
                                {
                                    if (reader.Value.Trim().Equals("y"))
                                        countries.Last().Y = 0;
                                    else
                                        countries.Last().Y = Int32.Parse(reader.Value.Trim());
                                }
                                break;
                        }
                    }
                }
                foreach (Country temp in countries)
                {
                    game.Countries.Add(temp.Name, temp);
                }
                foreach (Continent temp in continents)
                {
                    game.Continents.Add(temp.Name, temp);
                }
                return game;
            }
        }

    }
}
