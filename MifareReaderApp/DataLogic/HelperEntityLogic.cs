using MifareReaderApp.Models.Interfaces;

namespace MifareReaderApp.DataLogic
{
    public class HelperEntityLogic<T> : BaseLogic where T : class, IHelperEntity
    {
        public T? Get(string name)
        {
            if (!GetDbAvailability())
                return null;

            var entity = DbContext.Set<T>().SingleOrDefault(x => x.Name == name);

            return entity;
        }

        public T? First()
        {
            if (!GetDbAvailability())
                return null;

            var entity = DbContext.Set<T>().FirstOrDefault();

            return entity;
        }

        public List<T> GetAll()
        {
            var available = GetDbAvailability();
            if (available == false)
                return new List<T>();

            return DbContext.Set<T>().ToList();
        }
    }
}
