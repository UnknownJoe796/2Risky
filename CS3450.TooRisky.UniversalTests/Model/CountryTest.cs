using CS3450.TooRisky.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3450.TooRisky.Tests.Model
{
    [TestClass]
    public class CountryTest
    {

        [TestMethod]
        public void OwnedBy()
        {
            Game game = new GameTest().MakeExampleGame();
            Assert.AreEqual(
                game.Countries["Testistan"].OwnedBy,
                PlayerNumber.P1
                , "Ownership test 1 failed");
            Assert.AreEqual(
                game.Countries["Testanbul"].OwnedBy,
                PlayerNumber.P2
                , "Ownership test 2 failed");
        }

        [TestMethod]
        public void AdjacentCountries()
        {
            Game game = new GameTest().MakeExampleGame();
            var owned = game.Countries["Testistan"].AdjacentCountries();
            Assert.IsTrue(owned.Contains(game.Countries["Testlyvania"]));
            Assert.IsTrue(owned.Contains(game.Countries["Testanbul"]));
            Assert.IsFalse(owned.Contains(game.Countries["Test States of America"]));
        }

        [TestMethod]
        public void Attacks()
        {
            Game game = new GameTest().MakeExampleGame();
            var actions = game.Countries["Testistan"].Attacks();
            Assert.IsTrue(actions.Contains(new Attack()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsFalse(actions.Contains(new Attack()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }

        [TestMethod]
        public void Moves()
        {
            Game game = new GameTest().MakeExampleGame();

            game.Players[PlayerNumber.P1].UnitsToMove = 1;
            var actions = game.Countries["Testistan"].Moves();
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsTrue(actions.Contains(new Move()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }

        [TestMethod]
        public void Moves_Out()
        {
            Game game = new GameTest().MakeExampleGame();

            game.Players[PlayerNumber.P1].UnitsToMove = 0;
            var actions = game.Countries["Testistan"].Moves();
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testanbul"
            }));
            Assert.IsFalse(actions.Contains(new Move()
            {
                PlayerNumber = PlayerNumber.P1,
                FromName = "Testistan",
                ToName = "Testlyvania"
            }));
        }
    }
}
