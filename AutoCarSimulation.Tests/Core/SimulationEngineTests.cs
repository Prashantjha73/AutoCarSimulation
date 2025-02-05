
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
            // Initialize the field and car stores.
            _fieldStore = new FieldStore();
            _carStore = new CarStore();

            // Create a mock for ICarController.
            _mockCarController = new Mock<ICarControlService>();

            // Create the simulation engine using the injected field store, car store, and mocked car controller.
            _simulationEngine = new SimulationEngine(_fieldStore, _carStore, _mockCarController.Object);
        }

        [Fact]
        public void RunSimulation_ProcessesCommands_WithoutCollision()
        {
            // Arrange: Set the field in the field store.
            Field field = new Field(10, 10);
            _fieldStore.SetField(field);

            // Add a car with a single forward command.
            Car car = new Car("A", new Position(1, 1), Direction.N, "F");
            _carStore.AddCar(car);

            // Configure the mock: simulate processing by advancing the command pointer
            // and, if the command is 'F', moving the car forward.
            _mockCarController.Setup(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()))
                .Callback<Car, Field>((c, f) =>
                {
                    c.AdvanceCommand();
                    // Check if the processed command was 'F' (using the command at the previous index).
                    if (c.CommandString[c.CommandIndex - 1] == 'F')
                    {
                        var newPos = c.Position.Move(c.Direction);
                        if (f.IsWithinBounds(newPos))
                        {
                            c.Position = newPos;
                        }
                    }
                });

            // Act: Run the simulation.
            _simulationEngine.RunSimulation();

            // Assert:
            // The controller should have been invoked exactly once (because the car had a single command).
            _mockCarController.Verify(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()), Times.Exactly(1));
            Assert.False(car.HasMoreCommands);
            // The car should have moved from (1,1) to (1,2).
            Assert.Equal(new Position(1, 2), car.Position);
            // No collision should have occurred.
            Assert.False(car.IsCollided);
        }

        [Fact]
        public void RunSimulation_SetsCollision_WhenCarsCollide()
        {
            // Arrange: Set the field.
            Field field = new Field(10, 10);
            _fieldStore.SetField(field);

            // Add two cars at the same starting position with a single forward command.
            Car car1 = new Car("A", new Position(1, 1), Direction.N, "F");
            Car car2 = new Car("B", new Position(1, 1), Direction.N, "F");
            _carStore.AddCar(car1);
            _carStore.AddCar(car2);

            // Configure the mock: simply advance the command pointer without moving the car.
            // This ensures that both cars remain at (1,1) so a collision is detected.
            _mockCarController.Setup(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()))
                .Callback<Car, Field>((c, f) => c.AdvanceCommand());

            // Act: Run the simulation.
            _simulationEngine.RunSimulation();

            // Assert:
            // Each car had one command so the controller should have been called twice in total.
            _mockCarController.Verify(m => m.ProcessNextCommand(It.IsAny<Car>(), It.IsAny<Field>()), Times.Exactly(2));
            // Both cars should be marked as collided.
            Assert.True(car1.IsCollided);
            Assert.True(car2.IsCollided);
            // Both cars should have the same collision step.
            Assert.NotNull(car1.CollisionStep);
            Assert.Equal(car1.CollisionStep, car2.CollisionStep);
            // Their positions remain equal (and still (1,1) in this test).
            Assert.Equal(car1.Position, car2.Position);
        }
    }
}