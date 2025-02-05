namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    public class Car
    {
        public string Name { get; }
        public Position Position { get; set; }
        public Direction Direction { get; set; }

        /// <summary>
        /// The command string (e.g., "FFRFFFFRRL") to be processed.
        /// </summary>
        public string CommandString { get; }

        /// <summary>
        /// The index of the next command to execute.
        /// </summary>
        public int CommandIndex { get; set; }
        public bool IsCollided { get; set; } = false;
        public int? CollisionStep { get; set; }
        public string CollisionMessage { get; set; } = string.Empty;

        public Car(string name, Position position, Direction direction, string commandString)
        {
            Name = name;
            Position = position;
            Direction = direction;
            CommandString = commandString.ToUpper();
            CommandIndex = 0;
        }

        public bool HasMoreCommands => CommandIndex < CommandString.Length;

        /// <summary>
        /// Returns the next command (if any).
        /// </summary>
        public char? NextCommand => HasMoreCommands ? CommandString[CommandIndex] : null;

        /// <summary>
        /// Advances the command pointer.
        /// </summary>
        public void AdvanceCommand() => CommandIndex++;

        public override string ToString() =>
            IsCollided
                ? $"{Name}, {CollisionMessage}"
                : $"{Name}, ({Position.X},{Position.Y}) {Direction}";
    }
}