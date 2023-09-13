using MifareReaderApp.Models.Interfaces;

namespace MifareReaderApp.DataLogic
{
    public class HelperEntityLogic<T> : BaseLogic where T : class, IHelperEntity
    {
        public T? Get(string name)
        {
            var entity = DbContext.Set<T>().SingleOrDefault(x => x.Name == name);

            return entity;
        }

        public T? First()
        {
            var entity = DbContext.Set<T>().FirstOrDefault();

            return entity;
        }

        public List<T> GetAll()
        {
            return DbContext.Set<T>().ToList();
        }
    }
}
