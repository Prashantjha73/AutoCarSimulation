namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    public class Car
    {
        public string Name { get; }
        public Position Position { get; set; }
        public Direction Direction { get; set; }
        public string CommandString { get; }

        public Car(string name, Position position, Direction direction, string commandString)
        {
            Name = name;
            Position = position;
            Direction = direction;
            CommandString = commandString.ToUpper();
        }
    }
}