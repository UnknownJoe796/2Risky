using CS3450.TooRisky.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Tests.Model
{
    [TestClass]
    public class ContinentTest
    {

        [TestMethod]
        public void Countries()
        {
            Game game = new GameTest().MakeExampleGame();
            var owned = game.Continents["Testartica"].Countries(game);
            Assert.IsTrue(owned.Contains(game.Countries["Testistan"]));
            Assert.IsTrue(owned.Contains(game.Countries["Testlyvania"]));
            Assert.IsFalse(owned.Contains(game.Countries["Testanbul"]));
        }

        [TestMethod]
        public void OwnedByName()
        {
            Game game = new GameTest().MakeExampleGame();
            var continent = game.Continents["Testartica"];
            Assert.IsTrue(continent.OwnedByName(game, PlayerNumber.P1));
            Assert.IsFalse(continent.OwnedByName(game, PlayerNumber.P2));
        }
    }
}
