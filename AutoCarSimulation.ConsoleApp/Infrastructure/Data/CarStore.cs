
namespace AutoCarSimulation.ConsoleApp.Infrastructure.Data
{
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;
    public class CarStore : ICarStore
    {
        private readonly List<Car> _cars = [];

        public void AddCar(Car car)
        {
            _cars.Add(car);
        }

        public IReadOnlyList<Car> GetCars() => _cars.AsReadOnly();

        public void Clear() => _cars.Clear();
    }
}