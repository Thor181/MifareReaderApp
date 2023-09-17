using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MifareReaderApp.DataLogic.Stuff;
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
    public class BaseLogic : IDisposable
    {
        private bool _disposed = false;

        public bool DbAvailable { get => GetDbAvailability(); }

        protected MfRADbContext DbContext { get; set; }

        public BaseLogic()
        {
            DbContext = new MfRADbContext();
        }

        protected Guid GenerateId() => Guid.NewGuid();

        public bool GetDbAvailability()
        {
            DbContext = new MfRADbContext();
            return DbContext.Database.CanConnect();
        }

        public virtual DbOperationResult<List<T>> GetAll<T>() where T : class
        {
            var result = new DbOperationResult<List<T>>();

            if (!DbAvailable)
                return result;

            result.Entity = DbContext.Set<T>().ToList();

            return result;
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }

                _disposed = true;
            }
        }
        #endregion
    }
}
