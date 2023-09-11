using MifareReaderApp.Models;
using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
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
                result.Message = $"Обнаружено более одной записи с {nameof(User.Card)} = {cardNumber}";
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

        //public DbOperationResult<User> AddUser(User user)
        //{
            
        //}
    }
}
