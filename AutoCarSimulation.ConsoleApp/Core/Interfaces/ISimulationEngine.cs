using AutoCarSimulation.ConsoleApp.Domain.Models;

namespace AutoCarSimulation.ConsoleApp.Core.Interfaces
{
    /// <summary>
    /// Represents the simulation engine responsible for executing 
    /// </summary>
    public interface ISimulationEngine
    {
        /// <summary>
        /// Runs the simulation by processing the commands of all cars in the simulation.
        /// </summary>
        void RunSimulation();

        /// <summary>
        /// Add car in the simulation
        /// </summary>
        /// <param name="car">Car.</param>
        void AddCarInSimulation(Car car);

        /// <summary>
        /// Create a field for simulation.
        /// </summary>
        /// <param name="field">Field</param>
        void CreateFieldForSimulation(Field field);

        /// <summary>
        /// Get all cars in the simulation.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Car> GetCarsInSimulation();

        /// <summary>
        /// Initialises the simulation.
        /// </summary>
        void InitialiseSimulation();
    }
}