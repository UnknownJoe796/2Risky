using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using CS3450.TooRisky.Model;

namespace CS3450.TooRisky.UniversalTests.Model
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public Game MakeExampleGame()
        {
            Game game = new Game();

            game.AddPlayer(new Player()
            {
                Name = "Player One",
                Color = 0xFFFF0000,
                IpAddress = "127.0.0.0"
            });

            game.AddPlayer(new Player()
            {
                Name = "Player Two",
                Color = 0xFF0000FF,
                IpAddress = "127.0.0.0"
            });



            Country country;

            country = new Country()
            {
                Name = "Testistan",
                Units = 1,
                X = 0,
                Y = 0,
                OwnedByName = "Player One"
            };
            country.AdjacentCountryNames.Add("Testlyvania");
            country.AdjacentCountryNames.Add("Testanbul");
            game.AddCountry(country);

            country = new Country()
            {
                Name = "Testlyvania",
                Units = 1,
                X = 10,
                Y = 0,
                OwnedByName = "Player One"
            };
            country.AdjacentCountryNames.Add("Testistan");
            country.AdjacentCountryNames.Add("Testanbul");
            game.AddCountry(country);

            country = new Country()
            {
                Name = "Testanbul",
                Units = 1,
                X = 0,
                Y = 10,
                OwnedByName = "Player Two"
            };
            country.AdjacentCountryNames.Add("Testistan");
            country.AdjacentCountryNames.Add("Testlyvania");
            country.AdjacentCountryNames.Add("Test States of America");
            game.AddCountry(country);

            country = new Country()
            {
                Name = "Test States of America",
                Units = 1,
                X = 0,
                Y = 20,
                OwnedByName = "Player Two"
            };
            country.AdjacentCountryNames.Add("Testanbul");
            game.AddCountry(country);



            Continent continent;
            continent = new Continent()
            {
                Name = "Testartica",
                Worth = 3,
            };
            continent.CountryNames.Add("Testistan");
            continent.CountryNames.Add("Testlyvania");
            game.AddContinent(continent);

            continent = new Continent()
            {
                Name = "Testarica",
                Worth = 3,
            };
            continent.CountryNames.Add("Testanbul");
            continent.CountryNames.Add("Test States of America");
            game.AddContinent(continent);

            return game;
        }
    }
}
