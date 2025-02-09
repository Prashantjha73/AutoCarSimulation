using AutoCarSimulation.ConsoleApp.Core.Interfaces;
using AutoCarSimulation.ConsoleApp.Domain.Enums;
using AutoCarSimulation.ConsoleApp.Domain.Models;
using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;

namespace AutoCarSimulation.ConsoleApp
{
    public class SimulationRunner(ICarStore carStore, IFieldStore fieldStore, ISimulationEngine simulationEngine)
    {
        private readonly ICarStore _carStore = carStore;
        private readonly IFieldStore _fieldStore = fieldStore;
        private readonly ISimulationEngine _simulationEngine = simulationEngine;

        /// <summary> Orchestrates the simulation </summary>
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Field? field = PromptForField();
                if (field == null) continue;

                _fieldStore.Clear();
                _fieldStore.SetField(field);
                _carStore.Clear();

                ProcessCarInput();

                exit = !PromptStartOver();
            }
        }

        private Field? PromptForField()
        {
            Console.WriteLine("Welcome to Auto Driving Car Simulation!");
            Console.WriteLine("Please enter the width and height of the simulation field in x y format:");
            return ParseFieldInput(Console.ReadLine());
        }

        private Field? ParseFieldInput(string? input)
        {
            string[]? parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts == null || parts.Length < 2 || !int.TryParse(parts[0], out int width) || !int.TryParse(parts[1], out int height))
            {
                Console.WriteLine("Invalid input. Please enter valid integers.");
                return null;
            }

            try
            {
                Console.WriteLine($"You have created a field of {width} x {height}.");
                return new Field(width, height);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void ProcessCarInput()
        {
            bool addingCars = true;
            while (addingCars)
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("[1] Add a car to field");
                Console.WriteLine("[2] Run simulation");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddCar();
                        break;
                    case "2":
                        RunSimulationAndDisplayResults();
                        addingCars = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }

        private void AddCar()
        {
            Console.WriteLine("Please enter the name of the car:");
            string? name = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Car name cannot be empty");
                return;
            }

            Console.WriteLine($"Please enter initial position of car {name} in x y Direction (N,S,E,W) format:");
            if (!ParseCarInput(Console.ReadLine(), out Position pos, out Direction direction))
                return;

            Field currentField = _fieldStore.GetField();
            if (!currentField.IsWithinBounds(pos))
            {
                Console.WriteLine("Position is out of field bounds.");
                return;
            }

            Console.WriteLine($"Please enter the commands for car {name}. eg. FFRRLF (F-Forward, L-Turn Left, R-Turn Right):");
            string? commands = Console.ReadLine();

            _carStore.AddCar(new Car(name, pos, direction, commands ?? string.Empty));
            Console.WriteLine($"Car {name} added.");
        }

        private bool ParseCarInput(string? input, out Position pos, out Direction direction)
        {
            pos = null;
            direction = Direction.N; // Set a valid default to avoid incorrect checks

            string[]? parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts == null || parts.Length < 3 || !int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                Console.WriteLine("Invalid position format.");
                return false;
            }

            if (!Enum.TryParse(parts[2], true, out direction) || !Enum.IsDefined(typeof(Direction), direction))
            {
                Console.WriteLine("Invalid direction. Use N, S, E, or W.");
                return false;
            }

            pos = new Position(x, y);
            return true;
        }

        private void RunSimulationAndDisplayResults()
        {
            var cars = _carStore.GetCars();
            if (cars == null || !cars.Any())
            {
                Console.WriteLine("There is no car on field for simulation.");
                return;
            }

            Console.WriteLine("Your current list of cars are:");
            foreach (var car in _carStore.GetCars())
            {
                Console.WriteLine($"- {car.Name}, ({car.Position.X},{car.Position.Y}) {car.Direction}, {car.CommandString}");
            }

            _simulationEngine.RunSimulation();

            Console.WriteLine("After simulation, the result is:");
            foreach (var car in _carStore.GetCars())
            {
                Console.WriteLine("- " + car.ToString());
            }
        }

        private bool PromptStartOver()
        {
            Console.WriteLine("Please choose from the following options:\n[1] Start over\n[2] Exit\nOr press any key to exit");
            string? postOption = Console.ReadLine();

            if (postOption == "1")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Thank you for running the simulation. Goodbye!");
                return false;
            }
        }
    }
}