using AutoCarSimulation.ConsoleApp.Core.Interfaces;
using AutoCarSimulation.ConsoleApp.Core.Services;
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
        /// Runs the command-line simulation.
        /// </summary>
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to Auto Driving Car Simulation!");
                Console.WriteLine("Please enter the width and height of the simulation field in x y format:");
                string fieldInput = Console.ReadLine();
                string[] fieldParts = fieldInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (fieldParts.Length < 2 ||
                    !int.TryParse(fieldParts[0], out int width) ||
                    !int.TryParse(fieldParts[1], out int height))
                {
                    Console.WriteLine("Invalid input. Please enter valid integers.");
                    continue;
                }

                Field field;
                try
                {
                    field = new Field(width, height);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                Console.WriteLine($"You have created a field of {width} x {height}.");

                // Clear and set the current field.
                _fieldStore.Clear();
                _fieldStore.SetField(field);

                // Clear any existing cars.
                _carStore.Clear();

                bool addingCars = true;
                while (addingCars)
                {
                    Console.WriteLine("Please choose from the following options:");
                    Console.WriteLine("[1] Add a car to field");
                    Console.WriteLine("[2] Run simulation");
                    string option = Console.ReadLine();

                    if (option == "1")
                    {
                        Console.WriteLine("Please enter the name of the car:");
                        string name = Console.ReadLine();

                        Console.WriteLine($"Please enter initial position of car {name} in x y Direction format:");
                        string posInput = Console.ReadLine();
                        string[] posParts = posInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (posParts.Length < 3 ||
                            !int.TryParse(posParts[0], out int x) ||
                            !int.TryParse(posParts[1], out int y))
                        {
                            Console.WriteLine("Invalid position format.");
                            continue;
                        }

                        char dirChar = posParts[2].ToUpper()[0];
                        Direction direction = dirChar switch
                        {
                            'N' => Direction.N,
                            'S' => Direction.S,
                            'E' => Direction.E,
                            'W' => Direction.W,
                            _ => throw new ArgumentException("Invalid direction. Use N, S, E, or W.")
                        };

                        Position pos = new Position(x, y);
                        if (!field.IsWithinBounds(pos))
                        {
                            Console.WriteLine("Position is out of field bounds.");
                            continue;
                        }

                        Console.WriteLine($"Please enter the commands for car {name}:");
                        string commands = Console.ReadLine();

                        var car = new Car(name, pos, direction, commands);
                        _carStore.AddCar(car);
                    }
                    else if (option == "2")
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

                        Console.WriteLine("Please choose from the following options:");
                        Console.WriteLine("[1] Start over");
                        Console.WriteLine("[2] Exit");
                        string postOption = Console.ReadLine();
                        if (postOption == "1")
                        {
                            addingCars = false; // restart simulation
                        }
                        else
                        {
                            exit = true;
                            addingCars = false;
                            Console.WriteLine("Thank you for running the simulation. Goodbye!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please choose again.");
                    }
                }
            }
        }
    }
}