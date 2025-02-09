
namespace AutoCarSimulation.ConsoleApp.Infrastructure.Interface
{
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    public interface IFieldStore
    {
        /// <summary>
        /// Sets the current simulation field.
        /// </summary>
        void SetField(Field field);

        /// <summary>
        /// Retrieves the current simulation field.
        /// </summary>
        Field GetField();

        /// <summary>
        /// Clears the stored field.
        /// </summary>
        void Clear();
    }
}