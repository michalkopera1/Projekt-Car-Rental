//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using tekst.Controllers;
//using tekst.Data;
//using tekst.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Collections.Generic;

//namespace tekst.Tests
//{
//    [TestClass]
//    public class CarControllerTests 
//    {
//        private Mock<ApplicationDbContext> _mockContext;
//        private CarController _controller;

//        // Przygotowanie danych testowych
//        [TestInitialize]
//        public void TestInitialize()
//        {
//            // Mockowanie DbSet dla Cars i Rentals
//            var mockCars = new Mock<DbSet<Car>>();
//            var mockRentals = new Mock<DbSet<Rental>>();

//            var carsData = new List<Car>
//            {
//                new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, DailyPrice = 100 },
//                new Car { Id = 2, Make = "Ford", Model = "Focus", Year = 2021, DailyPrice = 120 },
//            }.AsQueryable();

//            var rentalsData = new List<Rental>
//            {
//                new Rental { CarId = 1, CustomerId = 1, RentDate = DateTime.Now }
//            }.AsQueryable();

//            mockCars.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(carsData.Provider);
//            mockCars.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(carsData.Expression);
//            mockCars.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(carsData.ElementType);
//            mockCars.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(carsData.GetEnumerator());

//            mockRentals.As<IQueryable<Rental>>().Setup(m => m.Provider).Returns(rentalsData.Provider);
//            mockRentals.As<IQueryable<Rental>>().Setup(m => m.Expression).Returns(rentalsData.Expression);
//            mockRentals.As<IQueryable<Rental>>().Setup(m => m.ElementType).Returns(rentalsData.ElementType);
//            mockRentals.As<IQueryable<Rental>>().Setup(m => m.GetEnumerator()).Returns(rentalsData.GetEnumerator());

//            // Mockowanie ApplicationDbContext
//            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
//            _mockContext.Setup(c => c.Cars).Returns(mockCars.Object);
//            _mockContext.Setup(c => c.Rentals).Returns(mockRentals.Object);

//            // Inicjalizacja kontrolera z mockowanym kontekstem
//            _controller = new CarController(_mockContext.Object);
//        }

//        // Testowanie metody Index
//        [TestMethod]
//        public void Index_ReturnsListOfCars()
//        {
//            // Act
//            var result = _controller.Index() as ViewResult;

//            // Assert
//            Assert.IsNotNull(result);
//            var model = result.Model as List<Car>;
//            Assert.IsNotNull(model);
//            Assert.AreEqual(2, model.Count);
//            Assert.AreEqual("Toyota", model[0].Make); // Weryfikacja, że pierwszy samochód to Toyota
//        }

//        // Testowanie metody Create (GET)
//        [TestMethod]
//        public void Create_ReturnsView()
//        {
//            // Act
//            var result = _controller.Create() as ViewResult;

//            // Assert
//            Assert.IsNotNull(result);
//        }

//        // Testowanie metody Create (POST) - przy poprawnych danych
//        [TestMethod]
//        public void Create_ValidCar_ReturnsRedirectToIndex()
//        {
//            // Arrange
//            var newCar = new Car { Make = "BMW", Model = "X5", Year = 2022, DailyPrice = 150 };

//            // Act
//            var result = _controller.Create(newCar) as RedirectToActionResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Index", result.ActionName);
//        }

//        // Testowanie metody Create (POST) - przy błędnych danych
//        [TestMethod]
//        public void Create_InvalidCar_ReturnsView()
//        {
//            // Arrange
//            var newCar = new Car { Make = "", Model = "", Year = 2022, DailyPrice = 150 }; // Błędne dane

//            _controller.ModelState.AddModelError("Make", "Marka jest wymagana");

//            // Act
//            var result = _controller.Create(newCar) as ViewResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(newCar, result.Model);
//        }

//        // Testowanie metody Delete
//        [TestMethod]
//        public void Delete_CarExists_ReturnsRedirectToIndex()
//        {
//            // Arrange
//            var carId = 1;

//            // Act
//            var result = _controller.Delete(carId) as RedirectToActionResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Index", result.ActionName);
//        }
//    }
//}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using tekst.Controllers;
using tekst.Data;
using tekst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace tekst.Tests
{
    [TestClass]
    public class CarControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<DbSet<Car>> _mockCars;
        private CarController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // Przygotowanie danych
            var carsData = new List<Car>
            {
                new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, DailyPrice = 100 },
                new Car { Id = 2, Make = "Ford", Model = "Focus", Year = 2021, DailyPrice = 120 },
            }.AsQueryable();

            // Mockowanie DbSet dla Cars
            _mockCars = new Mock<DbSet<Car>>();
            _mockCars.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(carsData.Provider);
            _mockCars.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(carsData.Expression);
            _mockCars.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(carsData.ElementType);
            _mockCars.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(carsData.GetEnumerator());

            // Mockowanie metod Add, Remove i Find
            _mockCars.Setup(m => m.Add(It.IsAny<Car>())).Callback<Car>(car => carsData.ToList().Add(car));
            _mockCars.Setup(m => m.Remove(It.IsAny<Car>())).Callback<Car>(car => carsData.ToList().Remove(car));
            _mockCars.Setup(m => m.Find(It.IsAny<object[]>())).Returns((object[] ids) => carsData.FirstOrDefault(c => c.Id == (int)ids[0]));

            // Mockowanie ApplicationDbContext
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockContext.Setup(c => c.Cars).Returns(_mockCars.Object);

            // Inicjalizacja kontrolera
            _controller = new CarController(_mockContext.Object);
        }

        [TestMethod]
        public void Index_ReturnsListOfCars()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Car>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Toyota", model[0].Make);
        }

        [TestMethod]
        public void Create_ReturnsView()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create_ValidCar_ReturnsRedirectToIndex()
        {
            // Arrange
            var newCar = new Car { Make = "BMW", Model = "X5", Year = 2022, DailyPrice = 150 };

            // Act
            var result = _controller.Create(newCar) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockCars.Verify(m => m.Add(It.IsAny<Car>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Create_InvalidCar_ReturnsView()
        {
            // Arrange
            var newCar = new Car { Make = "", Model = "", Year = 2022, DailyPrice = 150 }; // Nieprawidłowe dane
            _controller.ModelState.AddModelError("Make", "Marka jest wymagana");

            // Act
            var result = _controller.Create(newCar) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newCar, result.Model);
        }

        [TestMethod]
        public void Delete_CarExists_ReturnsRedirectToIndex()
        {
            // Arrange
            var carId = 1;

            // Act
            var result = _controller.Delete(carId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockCars.Verify(m => m.Remove(It.Is<Car>(c => c.Id == carId)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
