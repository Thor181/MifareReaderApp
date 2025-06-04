using Microsoft.EntityFrameworkCore;
using MifareReaderApp.Models;
using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic
{
    public class CardEventLogic : BaseLogic
    {
        public DbOperationResult<List<CardEvent>> GetAllIncluded(Expression<Func<CardEvent, bool>> predicate)
        {
            var result = new DbOperationResult<List<CardEvent>>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                var set = DbContext.CardEvents.Where(predicate).Include(x => x.Point).Include(x => x.Type).ToList();
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

        public DbOperationResult<List<KeyValuePair<CardEvent, User>>> GetAllWithUserIncluded(Expression<Func<CardEvent, bool>> predicate)
        {
            var result = new DbOperationResult<List<KeyValuePair<CardEvent, User>>>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                var users = DbContext.Users;

                var set = DbContext.CardEvents.Where(predicate)
                    .Include(x => x.Point)
                    .Include(x => x.Type)
                    .Select(x => KeyValuePair.Create(x, users.SingleOrDefault(y => y.Card == x.Card)))
                    //.Join(DbContext.Users, x => x.Card, y => y.Card, (cardEvent, user) => KeyValuePair.Create(cardEvent, user))
                    .ToList();

                result.Entity = set;
                return result;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Во время получения сущностей возникла ошибка", ex);
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
