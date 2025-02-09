namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    /// <summary>
    /// Field for the car simulation.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Width of the field.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the field.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The field.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <exception cref="ArgumentException">Invalid height or width.</exception>
        public Field(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Field dimensions must be positive.");
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Is the car within bounds.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <returns>True or false.</returns>
        public bool IsWithinBounds(Position position) =>
            position.X >= 0 && position.X < Width &&
            position.Y >= 0 && position.Y < Height;
    }
}