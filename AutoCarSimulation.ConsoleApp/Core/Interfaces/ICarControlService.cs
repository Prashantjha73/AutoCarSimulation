
namespace AutoCarSimulation.ConsoleApp.Core.Interfaces
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    public interface ICarControlService
    {
        void ProcessNextCommand(Car car, Field field);
        Direction TurnLeft(Direction direction);
        Direction TurnRight(Direction direction);
    }
}