using Microsoft.EntityFrameworkCore;
using MifareReaderApp.Models;
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
    public class OperatorEventLogic : BaseLogic
    {
        public DbOperationResult<List<OperatorEvent>> GetAllIncluded(Expression<Func<OperatorEvent, bool>> predicate)
        {
            var result = new DbOperationResult<List<OperatorEvent>>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                var set = DbContext.OperatorEvents.Where(predicate)
                    .Include(x => x.Point)
                    .Include(x => x.EventType)
                    .ToList();

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
