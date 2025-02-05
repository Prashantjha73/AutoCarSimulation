
namespace AutoCarSimulation.ConsoleApp.Infrastructure.Data
{
    using AutoCarSimulation.ConsoleApp.Domain.Models;
    using AutoCarSimulation.ConsoleApp.Infrastructure.Interface;
    public class FieldStore : IFieldStore
    {
        private Field _field;

        public void SetField(Field field)
        {
            _field = field;
        }

        public Field GetField() => _field;

        public void Clear() => _field = null;
    }
}