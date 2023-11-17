using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MifareReaderApp.Models;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Results;
using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic
{
    public class UserLogic : BaseLogic
    {
        public DbOperationResult<User> Find(string cardNumber)
        {
            var result = new DbOperationResult<User>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            var users = DbContext.Users.Where(x => x.Card == cardNumber).ToList();

            if (users.Count > 1)
            {
                result.IsSuccess = false;
                result.Message = $"Обнаружено более одной записи с номером карты {cardNumber}";
                return result;
            }
            else if (!users.Any())
            {
                result.Message = $"Не обнаружено записей в таблице {nameof(DbContext.Users)} с {nameof(User.Card)} = {cardNumber}";
                result.NotFound = true;
                return result;
            }

            var user = users.SingleOrDefault();
            result.Entity = user;

            return result;
        }

        public DbOperationResult<User> Add(User user)
        {
            var result = new DbOperationResult<User>() { Message = "Успешно сохранено" };

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            var foundUser = Find(user.Card);
            var userNotFound = foundUser.NotFound;
            if (userNotFound == false)
            {
                DbContext.ChangeTracker.Clear();
                var updateResult = this.Update(user);

                return updateResult;
            }

            user.Id = GenerateId();
            user.Dt = DateTime.Now;

            var place = new HelperEntityLogic<Place>().Get(AppConfig.Instance.DefaultPlace);

            if (place == null)
            {
                result.IsSuccess = false;
                result.Message = $"Не обнаружены доступные записи тип {nameof(Place)}";

                return result;
            }

            user.PlaceId = place.Id;

            try
            {
                DbContext.Add(user);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Instance.LogError($"Во время добавления сущности {nameof(User)} возникла ошибка", e);
                result.IsSuccess = false;
                result.Message = e.Message;
            }

            return result;
        }

        public DbOperationResult<User> Update(User user)
        {
            var result = new DbOperationResult<User>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                DbContext.Update(user);
                DbContext.SaveChanges();

                result.Message = $"Успешно обновлено";
            }
            catch (Exception e)
            {
                Logger.Instance.LogError($"При обновлени сущности {nameof(User)} возникла ошибка", e);

                result.IsSuccess = false;
                result.Message = $"При обновлени сущности {nameof(User)} возникла ошибка\n{e.Message}";
            }

            return result;
        }

        public DbOperationResult<User> Delete(User user)
        {
            var result = new DbOperationResult<User>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                DbContext.Remove(user);
                DbContext.SaveChanges();

                result.Message = "Успешно удалено";
            }
            catch (Exception e)
            {
                Logger.Instance.LogError($"При удалении сущности {nameof(User)} возникла ошибка", e);

                result.IsSuccess = false;
                result.Message = $"При удалении сущности {nameof(User)} возникла ошибка\n{e.Message}";
            }

            return result;
        }

        public DbOperationResult<List<User>> GetAllIncluded(Expression<Func<User, bool>> predicate)
        {
            var result = new DbOperationResult<List<User>>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                var set = DbContext.Users.Where(predicate).Include(x => x.Place).ToList();
                result.Entity = set;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError("Во время получения сущностей возникла ошибка", e);
                result.IsSuccess = false;
                result.Message = e.Message;
            }

            return result;
        }
    }
}
