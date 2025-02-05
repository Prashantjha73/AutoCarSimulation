namespace AutoCarSimulation.Tests.Core
{
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Core.Services;
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    public class CarControlServiceTests
    {
        private readonly ICarControlService _carController;

        public CarControlServiceTests()
        {
            _carController = new CarControlService();
        }

        [Fact]
        public void TurnLeft_FromNorth_ReturnsWest()
        {
            // Arrange
            var direction = Direction.N;

            // Act
            var result = _carController.TurnLeft(direction);

            // Assert
            Assert.Equal(Direction.W, result);
        }

        [Fact]
        public void TurnRight_FromNorth_ReturnsEast()
        {
            // Arrange
            var direction = Direction.N;

            // Act
            var result = _carController.TurnRight(direction);

            // Assert
            Assert.Equal(Direction.E, result);
        }

        [Fact]
        public void ProcessNextCommand_LeftCommand_ChangesDirection()
        {
            // Arrange
            Field field = new Field(5, 5);
            // Create a car with a left command.
            Car car = new Car("TestCar", new Position(1, 1), Direction.N, "L");

            // Act
            _carController.ProcessNextCommand(car, field);

            // Assert: after processing "L", the car’s direction should change from N to W.
            Assert.Equal(Direction.W, car.Direction);
            Assert.Equal(1, car.CommandIndex);
        }

        [Fact]
        public void ProcessNextCommand_ForwardCommand_MovesCarWithinBounds()
        {
            // Arrange
            Field field = new Field(5, 5);
            // Create a car with a forward command.
            Car car = new Car("TestCar", new Position(1, 1), Direction.N, "F");

            // Act
            _carController.ProcessNextCommand(car, field);

            // Assert: The car should move forward from (1,1) to (1,2).
            Assert.Equal(new Position(1, 2), car.Position);
            Assert.Equal(1, car.CommandIndex);
        }

        [Fact]
        public void ProcessNextCommand_ForwardCommand_IgnoresOutOfBounds()
        {
            // Arrange
            Field field = new Field(5, 5);
            // Place the car at the top edge and facing North.
            Car car = new Car("TestCar", new Position(1, 4), Direction.N, "F");

            // Act
            _carController.ProcessNextCommand(car, field);

            // Assert: The car should not move beyond the field boundaries.
            Assert.Equal(new Position(1, 4), car.Position);
            Assert.Equal(1, car.CommandIndex);
        }
    }
}