namespace AutoCarSimulation.ConsoleApp.Infrastructure.Interface
{
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    public interface ICarStore
    {
        void AddCar(Car car);
        IReadOnlyList<Car> GetCars();
        void Clear();
    }
}