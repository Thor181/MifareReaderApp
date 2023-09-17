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
    public class QREventLogic : BaseLogic
    {
        public DbOperationResult<List<Qrevent>> GetAllIncluded(Expression<Func<Qrevent, bool>> predicate)
        {
            var result = new DbOperationResult<List<Qrevent>>();

            if (!DbAvailable)
            {
                result.IsSuccess = false;
                result.Message = "База данных недоступна";
                return result;
            }

            try
            {
                var set = DbContext.Qrevents.Where(predicate).Include(x => x.Point).Include(x => x.Pay).Include(x => x.Type).ToList();
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
