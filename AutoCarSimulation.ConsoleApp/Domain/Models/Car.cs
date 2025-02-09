namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    public class Car
    {
        /// <summary>
        /// The name of the car.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The position.
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// The direction.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// The command string (e.g., "FFRFFFFRRL") to be processed.
        /// </summary>
        public string CommandString { get; }

        /// <summary>
        /// The index of the next command to execute.
        /// </summary>
        public int CommandIndex { get; set; }

        /// <summary>
        /// True if car collides else false.
        /// </summary>
        public bool IsCollided { get; set; } = false;

        /// <summary>
        /// Index of collision step.
        /// </summary>
        public int? CollisionStep { get; set; }

        /// <summary>
        /// The collision message.
        /// </summary>
        public string CollisionMessage { get; set; } = string.Empty;

        /// <summary>
        /// The car.
        /// </summary>
        /// <param name="name">Name of the car</param>
        /// <param name="position">Initial position on field</param>
        /// <param name="direction">Initial direction (N,S,E,W)</param>
        /// <param name="commandString">Commands</param>
        public Car(string name, Position position, Direction direction, string commandString)
        {
            Name = name;
            Position = position;
            Direction = direction;
            CommandString = commandString.ToUpper();
            CommandIndex = 0;
        }

        /// <summary>
        /// Has commands to execute
        /// </summary>
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