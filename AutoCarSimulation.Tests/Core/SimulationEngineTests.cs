
namespace AutoCarSimulation.Tests.Core
{
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Core.Services;
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;
    using Moq;

    public class SimulationEngineTests
    {
        private readonly Mock<IFieldStore> _mockFieldStore;
        private readonly Mock<ICarStore> _mockCarStore;
        private readonly ICarControlService _mockCarController;
        private readonly SimulationEngine _simulationEngine;

        public SimulationEngineTests()
        {
            _mockFieldStore = new Mock<IFieldStore>();
            _mockCarStore = new Mock<ICarStore>();
            _mockCarController = new CarControlService();

            _simulationEngine = new SimulationEngine(
                _mockFieldStore.Object,
                _mockCarStore.Object,
                _mockCarController
            );
        }

        [Fact]
        public void RunSimulation_ShouldCompleteWithoutErrors_WhenNoCarsPresent()
        {
            // Arrange
            _mockFieldStore.Setup(f => f.GetField()).Returns(new Field(5, 5));
            _mockCarStore.Setup(c => c.GetCars()).Returns(new List<Car>());

            // Act
            _simulationEngine.RunSimulation();

            // Assert
            _mockCarStore.Verify(c => c.GetCars(), Times.AtLeastOnce);
        }

        [Fact]
        public void RunSimulation_ShouldProcessCommands_WhenCarsHaveCommands()
        {
            // Arrange
            var car = new Car("A", new Position(1, 2), Direction.N, "FFRFFFFRRL");
            _mockFieldStore.Setup(f => f.GetField()).Returns(new Field(10, 10));
            _mockCarStore.Setup(c => c.GetCars()).Returns(new List<Car> { car });

            // Act
            _simulationEngine.RunSimulation();

            // Assert
            Assert.Equal(new Position(5, 4), car.Position);
            Assert.Equal(Direction.S, car.Direction);
            Assert.False(car.IsCollided);
        }

        [Fact]
        public void RunSimulation_ShouldStop_WhenAllCarsCollide()
        {
            // Arrange
            var car1 = new Car("Car1", new Position(1, 2), Direction.N, "FFRFFFFRRL");
            var car2 = new Car("Car2", new Position(7, 8), Direction.W, "FFLFFFFFFF");
            _mockFieldStore.Setup(f => f.GetField()).Returns(new Field(10, 10));
            _mockCarStore.Setup(c => c.GetCars()).Returns(new List<Car> { car1, car2 });

            // Act
            _simulationEngine.RunSimulation();

            // Assert
            Assert.Equal(car1.CollisionStep, car2.CollisionStep);
            Assert.Equal(car1.Position, car2.Position);
        }

        [Fact]
        public void HandleCollisions_ShouldThrowException_WhenCarsOccupySamePosition()
        {
            // Arrange
            var car1 = new Car("Car1", new Position(2, 2), Direction.N, "F");
            var car2 = new Car("Car2", new Position(2, 2), Direction.S, "F");
            _mockFieldStore.Setup(f => f.GetField()).Returns(new Field(5, 5));
            _mockCarStore.Setup(c => c.GetCars()).Returns(new List<Car> { car1 });

            // Act
            var exception = Assert.Throws<Exception>(() => _simulationEngine.AddCarInSimulation(car2));
        }

        [Fact]
        public void ProcessCarCommands_ShouldNotMoveCar_WhenOutOfBounds()
        {
            // Arrange
            var car = new Car("EdgeCar", new Position(4, 4), Direction.E, "F");
            _mockFieldStore.Setup(f => f.GetField()).Returns(new Field(5, 5));
            _mockCarStore.Setup(c => c.GetCars()).Returns(new List<Car> { car });

            // Act
            _simulationEngine.RunSimulation();

            // Assert
            Assert.Equal(4, car.Position.X);
        }
    }
}