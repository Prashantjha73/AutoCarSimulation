namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;

    public class Car(string name, Position position, Direction direction, string commandString)
    {
        /// <summary>
        /// The name of the car.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// The position.
        /// </summary>
        public Position Position { get; set; } = position;

        /// <summary>
        /// The direction.
        /// </summary>
        public Direction Direction { get; set; } = direction;

        /// <summary>
        /// The command string (e.g., "FFRFFFFRRL") to be processed.
        /// </summary>
        public string CommandString { get; } = commandString.ToUpper();

        /// <summary>
        /// The index of the next command to execute.
        /// </summary>
        public int CommandIndex { get; set; } = 0;

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