
namespace AutoCarSimulation.Tests.Core
{
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Core.Services;
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Data;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;
    using Moq;

    public class SimulationEngineTests
    {
        private readonly IFieldStore _fieldStore;
        private readonly ICarStore _carStore;
        private readonly Mock<ICarControlService> _mockCarController;
        private readonly ISimulationEngine _simulationEngine;

        public SimulationEngineTests()
        {
            _fieldStore = new FieldStore();
            _carStore = new CarStore();
            _mockCarController = new Mock<ICarControlService>();
            _simulationEngine = new SimulationEngine(_fieldStore, _carStore, _mockCarController.Object);
        }

        [Fact]
        public void RunSimulation_ProcessesCommands_WithoutCollision()
        {
            // Arrange
            Field field = new Field(10, 10);
            _fieldStore.SetField(field);

            Car car = new Car("A", new Position(1, 1), Direction.N, "F");
            _carStore.AddCar(car);

            _mockCarController.Setup(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()))
                .Callback<Car, Field>((c, f) =>
                {
                    c.AdvanceCommand();
                    if (c.CommandString[c.CommandIndex - 1] == 'F')
                    {
                        var newPos = c.Position.Move(c.Direction);
                        if (f.IsWithinBounds(newPos))
                        {
                            c.Position = newPos;
                        }
                    }
                });

            // Act
            _simulationEngine.RunSimulation();

            // Assert:
            _mockCarController.Verify(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()), Times.Exactly(1));
            Assert.False(car.HasMoreCommands);
            Assert.Equal(new Position(1, 2), car.Position);
            Assert.False(car.IsCollided);
        }

        [Fact]
        public void RunSimulation_SetsCollision_WhenCarsCollide()
        {
            // Arrange
            Field field = new Field(10, 10);
            _fieldStore.SetField(field);
            Car car1 = new Car("A", new Position(1, 1), Direction.N, "F");
            Car car2 = new Car("B", new Position(1, 1), Direction.N, "F");
            _carStore.AddCar(car1);
            _carStore.AddCar(car2);

            _mockCarController.Setup(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()))
                .Callback<Car, Field>((c, f) => c.AdvanceCommand());

            // Act
            _simulationEngine.RunSimulation();

            // Assert:
            _mockCarController.Verify(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()), Times.Exactly(2));
            Assert.True(car1.IsCollided);
            Assert.True(car2.IsCollided);
            Assert.NotNull(car1.CollisionStep);
            Assert.Equal(car1.CollisionStep, car2.CollisionStep);
            Assert.Equal(car1.Position, car2.Position);
        }
    }
}