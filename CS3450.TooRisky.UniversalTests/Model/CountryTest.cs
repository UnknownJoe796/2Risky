using CS3450.TooRisky.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.UniversalTests.Model
{
    [TestClass]
    public class CountryTest
    {

        [TestMethod]
        public void OwnedBy()
        {
            Game game = new GameTest().MakeExampleGame();
            Assert.AreEqual(
                game.Countries["Testistan"].OwnedBy(game),
                game.Players["Player One"]
                );
            Assert.AreEqual(
                game.Countries["Testanbul"].OwnedBy(game),
                game.Players["Player Two"]
                );
        }

        [TestMethod]
        public void AdjacentCountries()
        {
            Game game = new GameTest().MakeExampleGame();
            var owned = game.Countries["Testistan"].AdjacentCountries(game);
            Assert.IsTrue(owned.Contains(game.Countries["Testlyvania"]));
            Assert.IsTrue(owned.Contains(game.Countries["Testanbul"]));
            Assert.IsFalse(owned.Contains(game.Countries["Test States of America"]));
        }

        [TestMethod]
        public void Attacks()
        {
            Game game = new GameTest().MakeExampleGame();
            var actions = game.Countries["Testistan"].Attacks(game);
            Assert.IsTrue(actions.Contains(new Attack()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsFalse(actions.Contains(new Attack()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }

        [TestMethod]
        public void Moves()
        {
            Game game = new GameTest().MakeExampleGame();
            game.Players["Player One"].UnitsToMove = 1;
            var actions = game.Countries["Testistan"].Moves(game);
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsTrue(actions.Contains(new Move()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }

        [TestMethod]
        public void Moves_Out()
        {
            Game game = new GameTest().MakeExampleGame();
            game.Players["Player One"].UnitsToMove = 0;
            var actions = game.Countries["Testistan"].Moves(game);
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerName = "Player One",
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }
    }
}
