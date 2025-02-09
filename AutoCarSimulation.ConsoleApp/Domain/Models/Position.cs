namespace AutoCarSimulation.ConsoleApp.Domain.Models
{
    using AutoCarSimulation.ConsoleApp.Domain.Enums;
    public record Position(int X, int Y)
    {
        public Position Move(Direction direction) => direction switch
        {
            Direction.N => this with { Y = this.Y + 1 },
            Direction.S => this with { Y = this.Y - 1 },
            Direction.E => this with { X = this.X + 1 },
            Direction.W => this with { X = this.X - 1 },
            _ => this,
        };
    }
}