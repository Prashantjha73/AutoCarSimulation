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
    }
}