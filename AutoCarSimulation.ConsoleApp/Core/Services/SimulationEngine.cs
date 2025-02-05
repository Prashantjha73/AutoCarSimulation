
namespace AutoCarSimulation.ConsoleApp.Core.Services
{
    using AutoCarSimulation.ConsoleApp.Core.Interfaces;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;

    public class SimulationEngine : ISimulationEngine
    {
        private readonly IFieldStore _fieldStore;
        private readonly ICarStore _carStore;
        private readonly ICarControlService _carController;

        public SimulationEngine(IFieldStore fieldStore, ICarStore carStore, ICarControlService carController)
        {
            _fieldStore = fieldStore;
            _carStore = carStore;
            _carController = carController;
        }

        public void RunSimulation()
        {
            Field field = _fieldStore.GetField();
            int step = 1;
            while (_carStore.GetCars().Any(car => !car.IsCollided && car.HasMoreCommands))
            {
                // Process one command per active car.
                foreach (var car in _carStore.GetCars().Where(car => !car.IsCollided && car.HasMoreCommands))
                {
                    _carController.ProcessNextCommand(car, field);
                }

                // Check for collisions: any position with more than one active car.
                var groups = _carStore.GetCars()
                                      .Where(c => !c.IsCollided)
                                      .GroupBy(c => c.Position)
                                      .Where(g => g.Count() > 1);

                foreach (var group in groups)
                {
                    var collidedCars = group.ToList();
                    foreach (var car in collidedCars)
                    {
                        if (!car.IsCollided)
                        {
                            string otherNames = string.Join(" and ",
                                collidedCars.Where(c => c != car).Select(c => c.Name));
                            car.IsCollided = true;
                            car.CollisionStep = step;
                            car.CollisionMessage = $"collides with {otherNames} at ({car.Position.X},{car.Position.Y}) at step {step}";
                        }
                    }
                }
                step++;
            }
        }
    }
}