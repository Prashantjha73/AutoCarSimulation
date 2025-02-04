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
            Width = width;
            Height = height;
        }
    }
}