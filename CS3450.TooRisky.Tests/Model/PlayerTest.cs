﻿using CS3450.TooRisky.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Tests.Model
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void CountriesOwned()
        {
            Game game = new GameTest().MakeExampleGame();
            var owned = game.Players[PlayerNumber.P1].CountriesOwned;
            Assert.IsTrue(owned.Contains(game.Countries["Testistan"]));
            Assert.IsTrue(owned.Contains(game.Countries["Testlyvania"]));
            Assert.IsFalse(owned.Contains(game.Countries["Testanbul"]));
        }

        [TestMethod]
        public void ContinentsOwned()
        {
            Game game = new GameTest().MakeExampleGame();
            var owned = game.Players[PlayerNumber.P1].ContinentsOwned;
            Assert.IsTrue(owned.Contains(game.Continents["Testartica"]));
            Assert.IsFalse(owned.Contains(game.Continents["Testarica"]));
        }
    }
}
