namespace AutoCarSimulation.ConsoleApp.Core.Interfaces
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    using AutoCarSimulation.ConsoleApp.Domain.Models;

    /// <summary>
    /// Defines operations for controlling a car within the simulation.
    /// </summary>
    public interface ICarControlService
    {
        /// <summary>
        /// Processes the next command for the given car within the specified field.
        /// </summary>
        /// <param name="car">Car.</param>
        /// <param name="field">The field.</param>
        void ProcessNextCommand(Car car, Field field);

        /// <summary>
        /// Rotates the car 90 degrees to the left based on its current direction.
        /// </summary>
        /// <param name="direction">The current direction of the car.</param>
        /// <returns>The new direction after turning left.</returns>
        Direction TurnLeft(Direction direction);

        /// <summary>
        /// Rotates the car 90 degrees to the right based on its current direction.
        /// </summary>
        /// <param name="direction">The current direction of the car.</param>
        /// <returns>The new direction after turning right.</returns>
        Direction TurnRight(Direction direction);
    }
}