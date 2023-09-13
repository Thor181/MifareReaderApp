using Microsoft.EntityFrameworkCore;
using MifareReaderApp.Models;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.DataLogic
{
    public class BaseLogic : IDisposable
    {
        private bool _disposed = false;

        protected MfRADbContext DbContext { get; set; }

        public BaseLogic()
        {
            DbContext = new MfRADbContext();
        }

        protected Guid GenerateId() => Guid.NewGuid();

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
