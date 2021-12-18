using CleanArchitecture.Infrastructure;
using System;

namespace CleanArchitecture.WebApi.EndToEndTests
{
    /// <summary>
    /// 
    /// </summary>
    public class DataBaseFixture : IDisposable
    {
        private bool _isDisposed;

        public BlogDbContext BlogDbContext { get; set; }

        public DataBaseFixture()
        {
        }

        public void Dispose()
        {
            BlogDbContext?.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;


            if (disposing)
            {
                BlogDbContext.Dispose();
            }

            _isDisposed = true;
        }

        ~DataBaseFixture()
        {
            Dispose(false);
        }
    }
}
