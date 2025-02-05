using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    public class Field
    {
        public int Width { get; }
        public int Height { get; }

        public Field(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Field dimensions must be positive.");
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(Position position) =>
            position.X >= 0 && position.X < Width &&
            position.Y >= 0 && position.Y < Height;
    }
}