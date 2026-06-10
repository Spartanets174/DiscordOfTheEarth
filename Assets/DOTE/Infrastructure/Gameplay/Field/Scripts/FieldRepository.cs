using DOTE.Gameplay.Domain.Field;

namespace DOTE.Gameplay.Infrastructure
{
    public class FieldRepository : IFieldRepository
    {
        private Field currentField;

        public FieldRepository(Field currentField)
        {
            this.currentField = currentField;
        }

        public Field GetCurrentField()
        {
            return currentField;
        }
    }
}
