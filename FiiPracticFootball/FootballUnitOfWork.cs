using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiiPracticFootball
{
    public class FootballUnitOfWork : IFootballUnitOfWork
    {
        private bool disposedValue;
        private readonly FootballContext footballContext;

        public FootballUnitOfWork(FootballContext footballContext)
        {
            this.footballContext = footballContext;
        }

        public void SaveChanges()
        {
            footballContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    footballContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
