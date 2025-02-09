
namespace AutoCarSimulation.ConsoleApp.Core.Services
{
    using System.Collections.Generic;
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;

    /// <inheritdoc />
    public class SimulationEngine(IFieldStore fieldStore, ICarStore carStore, ICarControlService carControlService) : ISimulationEngine
    {

        /// <inheritdoc />
        public void RunSimulation()
        {
            Field field = fieldStore.GetField();
            int step = 1;

            while (HasActiveCars())
            {
                ProcessCarCommands(field);
                HandleCollisions(step);
                step++;
            }
        }

        /// <inheritdoc />
        public void AddCarInSimulation(Car car)
        {
            Field currentField = fieldStore.GetField();
            if (!currentField.IsWithinBounds(car.Position))
            {
                throw new Exception("Position is out of field bounds.");
            }

            if (carStore.GetCars().ToList().Exists(c => c.Name == car.Name))
            {
                throw new Exception($"Car name {car.Name} already exists");
            }

            if (carStore.GetCars().ToList().Exists(c => c.Position == car.Position))
            {
                throw new Exception($"{car.Position} is already occupied");
            }

            carStore.AddCar(car);
        }

        /// <inheritdoc />
        public void CreateFieldForSimulation(Field field)
        {
            fieldStore.SetField(field);
        }

        /// <inheritdoc />
        public IReadOnlyList<Car> GetCarsInSimulation()
        {
            return carStore.GetCars();
        }

        /// <inheritdoc />
        public void InitialiseSimulation()
        {
            fieldStore.Clear();
            carStore.Clear();
        }

        private bool HasActiveCars()
        {
            return carStore.GetCars().Any(car => !car.IsCollided && car.HasMoreCommands);
        }

        private void ProcessCarCommands(Field field)
        {
            foreach (var car in carStore.GetCars().Where(car => !car.IsCollided && car.HasMoreCommands))
            {
                carControlService.ProcessNextCommand(car, field);
            }
        }

        private void HandleCollisions(int step)
        {
            var groupedCars = carStore.GetCars()
                                       .Where(c => !c.IsCollided)
                                       .GroupBy(c => c.Position)
                                       .Where(g => g.Count() > 1);

            foreach (var group in groupedCars)
            {
                var collidedCars = group.ToList();
                foreach (var car in collidedCars)
                {
                    if (!car.IsCollided)
                    {
                        car.IsCollided = true;
                        car.CollisionStep = step;
                        car.CollisionMessage = $"collides with {string.Join(" and ", collidedCars.Where(c => c != car).Select(c => c.Name))} at ({car.Position.X},{car.Position.Y}) at step {step}";
                    }
                }
            }
        }
    }
}