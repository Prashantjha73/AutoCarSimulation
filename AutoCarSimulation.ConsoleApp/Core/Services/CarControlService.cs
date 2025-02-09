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

            ExecuteCommand(car, field, car.NextCommand.Value);
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

        private void ExecuteCommand(Car car, Field field, char command)
        {
            switch (command)
            {
                case 'L':
                    car.Direction = TurnLeft(car.Direction);
                    break;
                case 'R':
                    car.Direction = TurnRight(car.Direction);
                    break;
                case 'F':
                    TryMoveForward(car, field);
                    break;
            }
        }

        private void TryMoveForward(Car car, Field field)
        {
            var newPosition = car.Position.Move(car.Direction);
            if (field.IsWithinBounds(newPosition))
            {
                car.Position = newPosition;
            }
        }
    }
}