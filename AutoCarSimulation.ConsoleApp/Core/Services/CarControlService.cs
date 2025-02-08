using AutoCarSimulation.ConsoleApp.Core.Interfaces;
using AutoCarSimulation.ConsoleApp.Domain.Enums;
using AutoCarSimulation.ConsoleApp.Domain.Models;

namespace AutoCarSimulation.ConsoleApp.Core.Services
{
    /// <inheritdoc />
    public class CarControlService : ICarControlService
    {
        /// <inheritdoc />
        public void ProcessNextCommand(Car car, Field field)
        {
            if (!car.HasMoreCommands)
                return;

            char command = car.NextCommand.Value;
            switch (command)
            {
                case 'L':
                    car.Direction = TurnLeft(car.Direction);
                    break;
                case 'R':
                    car.Direction = TurnRight(car.Direction);
                    break;
                case 'F':
                    var newPosition = car.Position.Move(car.Direction);
                    if (field.IsWithinBounds(newPosition))
                    {
                        car.Position = newPosition;
                    }
                    break;
                default:
                    // Ignore invalid commands.
                    break;
            }
            car.AdvanceCommand();
        }

        /// <inheritdoc />
        public Direction TurnLeft(Direction direction) => direction switch
        {
            Direction.N => Direction.W,
            Direction.W => Direction.S,
            Direction.S => Direction.E,
            Direction.E => Direction.N,
            _ => direction,
        };

        /// <inheritdoc />
        public Direction TurnRight(Direction direction) => direction switch
        {
            Direction.N => Direction.E,
            Direction.E => Direction.S,
            Direction.S => Direction.W,
            Direction.W => Direction.N,
            _ => direction,
        };
    }
}