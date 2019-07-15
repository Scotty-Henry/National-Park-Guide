using Capstone.Web.DAO;
using Capstone.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace IntergrationTests
{
    [TestClass]
    public class NationalParkIntegrationTests
    {
        private TransactionScope _tran = null;
        private NationalParkSQLDAO _db = null;
        private ParkItem _park = null;

        [TestInitialize]
        public void Initialize()
        {
            _db = new NationalParkSQLDAO("Data Source=localhost\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True");
            _tran = new TransactionScope();
            ParkItem park = new ParkItem
            {
                Acreage = 100,
                AnnualVisitorCount = 5,
                Climate = "Woodland",
                ElevationInFeet = 20,
                EntryFee = 500,
                InspirationalQuote = "Money rules the world.",
                InspirationalQuoteSource = "Bill Gates",
                MilesOfTrail = 2,
                NumberOfAnimalSpecies = 200,
                NumberOfCampsites = 100,
                ParkCode = "CASH",
                ParkDescription = "Park for people with money.",
                ParkName = "The Banks",
                State = "OH",
                YearFounded = 1800
            };
            _park = park;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _park = null;
            _tran.Dispose();
        }

        [TestMethod]
        public void TestPark()
        {
            //Tests get park by code
            var returnedPark = _db.GetParkByCode(_park.ParkCode);

            //Tests adds park
            bool successful = _db.AddNewParkItem(_park);

            Assert.IsTrue(successful, "Park not successfully added");
            Assert.AreEqual(_park.Acreage, returnedPark.Acreage, "Acreage did not match");
            Assert.AreEqual(_park.AnnualVisitorCount, returnedPark.AnnualVisitorCount, "Annual Visitor Count did not match");
            Assert.AreEqual(_park.ElevationInFeet, returnedPark.ElevationInFeet, "Elevation In Feet did not match");
            Assert.AreEqual(_park.EntryFee, returnedPark.EntryFee, "Entry Fee did not match");
            Assert.AreEqual(_park.InspirationalQuote, returnedPark.InspirationalQuote, "Inspirational Quote did not match");
            Assert.AreEqual(_park.InspirationalQuoteSource, returnedPark.InspirationalQuoteSource, "Inspirational Quote Source did not match");
            Assert.AreEqual(_park.MilesOfTrail, returnedPark.MilesOfTrail, "Miles Of Trail did not match");
            Assert.AreEqual(_park.NumberOfAnimalSpecies, returnedPark.NumberOfAnimalSpecies, "Number Of Animal Species did not match");
            Assert.AreEqual(_park.NumberOfCampsites, returnedPark.NumberOfCampsites, "Number Of Campsites did not match");
            Assert.AreEqual(_park.ParkCode, returnedPark.ParkCode, "Park Code did not match");
            Assert.AreEqual(_park.ParkDescription, returnedPark.ParkDescription, "Park Description did not match");
            Assert.AreEqual(_park.ParkName, returnedPark.ParkName, "Park Name did not match");
            Assert.AreEqual(_park.State, returnedPark.State, "State did not match");
            Assert.AreEqual(_park.YearFounded, returnedPark.YearFounded, "Year Founded did not match");

            //Tests get parks
            int count = _db.GetParks().Count;
            Assert.IsTrue(count >= 1, "Did not return parks");
        }

        [TestMethod]
        public void TestWeather()
        {
            //add park so we have a park code to test with
            _db.AddNewParkItem(_park);

            WeatherItem weather = new WeatherItem
            {
                FiveDayForecast = 5,
                Forecast = "rain",
                High = 100,
                Low = 30,
                ParkCode = "CASH"
            };

            // Tests add weather
            bool successful = _db.AddWeatherItem(weather);
            Assert.IsTrue(successful, "Weather not successfully added");

            //Tests get weather by code
            int count = _db.GetWeatherByCode(_park.ParkCode).Count;
            Assert.IsTrue(count >= 1, "Did not return weather");

            foreach(var item in _db.GetWeatherByCode(_park.ParkCode))
            {
                Assert.AreEqual(weather.FiveDayForecast, item.FiveDayForecast, "Five day forecast did not match");
                Assert.AreEqual(weather.Forecast, item.Forecast, "Forecast did not match");
                Assert.AreEqual(weather.High, item.High, "High did not match");
                Assert.AreEqual(weather.Low, item.Low, "Low did not match");
                Assert.AreEqual(weather.ParkCode, item.ParkCode, "Park Code did not match");
            }

        }
    }
}
