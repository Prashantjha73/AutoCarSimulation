using AutoCarSimulation.ConsoleApp.Core.Interfaces;
using AutoCarSimulation.ConsoleApp.Domain.Enums;
using AutoCarSimulation.ConsoleApp.Domain.Models;
using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;

namespace AutoCarSimulation.ConsoleApp
{
    public class SimulationRunner
    {
        private readonly ICarStore _carStore;
        private readonly IFieldStore _fieldStore;
        private readonly ISimulationEngine _simulationEngine;

        public SimulationRunner(ICarStore carStore, IFieldStore fieldStore, ISimulationEngine simulationEngine)
        {
            _carStore = carStore;
            _fieldStore = fieldStore;
            _simulationEngine = simulationEngine;
        }

        /// <summary>
        /// The main method which orchestrates the simulation.
        /// </summary>
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Field field = PromptForField();
                if (field == null)
                {
                    continue; // Ask again if field input is invalid.
                }

                // Store the field and clear any previous state.
                _fieldStore.Clear();
                _fieldStore.SetField(field);
                _carStore.Clear();

                ProcessCarInput();

                // After simulation, decide whether to restart or exit.
                exit = !PromptStartOver();
            }
        }

        /// <summary>
        /// Prompts the user for the simulation field dimensions and returns a new Field instance.
        /// Returns null if the input is invalid.
        /// </summary>
        private Field PromptForField()
        {
            Console.WriteLine("Welcome to Auto Driving Car Simulation!");
            Console.WriteLine("Please enter the width and height of the simulation field in x y format:");
            string input = Console.ReadLine();
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2 ||
                !int.TryParse(parts[0], out int width) ||
                !int.TryParse(parts[1], out int height))
            {
                Console.WriteLine("Invalid input. Please enter valid integers.");
                return null;
            }

            try
            {
                Field field = new Field(width, height);
                Console.WriteLine($"You have created a field of {width} x {height}.");
                return field;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Processes user input to add cars or run the simulation.
        /// </summary>
        private void ProcessCarInput()
        {
            bool addingCars = true;
            while (addingCars)
            {
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("[1] Add a car to field");
                Console.WriteLine("[2] Run simulation");
                string? option = Console.ReadLine();

                if (option == "1")
                {
                    AddCar();
                }
                else if (option == "2")
                {
                    RunSimulationAndDisplayResults();
                    addingCars = false;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose again.");
                }
            }
        }

        /// <summary>
        /// Prompts the user to input a new car and adds it to the car store.
        /// </summary>
        private void AddCar()
        {
            Console.WriteLine("Please enter the name of the car:");
            string name = Console.ReadLine();

            Console.WriteLine($"Please enter initial position of car {name} in x y Direction format:");
            string? posInput = Console.ReadLine();
            string[] posParts = posInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (posParts.Length < 3 ||
                !int.TryParse(posParts[0], out int x) ||
                !int.TryParse(posParts[1], out int y))
            {
                Console.WriteLine("Invalid position format.");
                return;
            }

            char dirChar = posParts[2].ToUpper()[0];
            Direction direction;
            switch (dirChar)
            {
                case 'N':
                    direction = Direction.N;
                    break;
                case 'S':
                    direction = Direction.S;
                    break;
                case 'E':
                    direction = Direction.E;
                    break;
                case 'W':
                    direction = Direction.W;
                    break;
                default:
                    Console.WriteLine("Invalid direction. Use N, S, E, or W.");
                    return;
            }

            Position pos = new Position(x, y);
            Field currentField = _fieldStore.GetField();
            if (!currentField.IsWithinBounds(pos))
            {
                Console.WriteLine("Position is out of field bounds.");
                return;
            }

            Console.WriteLine($"Please enter the commands for car {name}:");
            string commands = Console.ReadLine();

            Car car = new Car(name, pos, direction, commands);
            _carStore.AddCar(car);
            Console.WriteLine($"Car {name} added.");
        }

        /// <summary>
        /// Runs the simulation using the current cars and displays the results.
        /// </summary>
        private void RunSimulationAndDisplayResults()
        {
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

        /// <summary>
        /// Prompts the user whether to start over or exit.
        /// Returns true if the user wants to start over.
        /// </summary>
        private bool PromptStartOver()
        {
            Console.WriteLine("Please choose from the following options:");
            Console.WriteLine("[1] Start over");
            Console.WriteLine("[2] Exit");
            string postOption = Console.ReadLine();

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