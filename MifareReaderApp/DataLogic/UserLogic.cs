using MifareReaderApp.Models;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Results;
using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic
{
    public class UserLogic : BaseLogic
    {
        public DbOperationResult<User> FindUser(string cardNumber)
        {
            var result = new DbOperationResult<User>();
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

        public DbOperationResult<User> AddUser(User user)
        {
            var result = new DbOperationResult<User>();

            var userNotFound = FindUser(user.Card).NotFound;
            if (userNotFound == false)
            {
                result.IsSuccess = false;
                result.Message = $"Пользователь с номером карты {user.Card} уже существует";
                return result;
            }

            user.Id = GenerateId();
            user.Dt = DateTime.Now;

            var place = new HelperEntityLogic<Place>().First();

            if (place == null)
            {
                result.IsSuccess = false;
                result.Message = $"Не обнаружен доступных записей {nameof(Place)}";
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

            result.Message = "Успешно сохранено";

            return result;
        }
    }
}
