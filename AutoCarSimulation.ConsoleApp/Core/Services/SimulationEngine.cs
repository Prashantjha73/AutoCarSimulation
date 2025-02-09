
namespace AutoCarSimulation.ConsoleApp.Core.Services
{
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;

    /// <inheritdoc />
    public class SimulationEngine(IFieldStore fieldStore, ICarStore carStore, ICarControlService carController) : ISimulationEngine
    {
        private readonly IFieldStore _fieldStore = fieldStore;
        private readonly ICarStore _carStore = carStore;
        private readonly ICarControlService _carController = carController;

        /// <inheritdoc />
        public void RunSimulation()
        {
            Field field = _fieldStore.GetField();
            int step = 1;

            while (HasActiveCars())
            {
                ProcessCarCommands(field);
                HandleCollisions(step);
                step++;
            }
        }

        private bool HasActiveCars()
        {
            return _carStore.GetCars().Any(car => !car.IsCollided && car.HasMoreCommands);
        }

        private void ProcessCarCommands(Field field)
        {
            foreach (var car in _carStore.GetCars().Where(car => !car.IsCollided && car.HasMoreCommands))
            {
                _carController.ProcessNextCommand(car, field);
            }
        }

        private void HandleCollisions(int step)
        {
            var groupedCars = _carStore.GetCars()
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