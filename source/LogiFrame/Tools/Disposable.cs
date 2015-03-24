using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogiFrame.Tools
{
    /// <summary>
    ///     Defines methods to release allocated resources and to check whether this resource has been disposed.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        ///     Finalizes an instance of the <see cref="Disposable" /> class.
        /// </summary>
        ~Disposable()
        {
            if (IsDisposed)
            {
                //We've been desposed already. Abort further disposure.
                return;
            }

            Dispose(false);

            //We don't care to set IsDisposed value; Resource is being collected by GC anyways.
        }

        /// <summary>
        /// Occurs when disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///     Gets whether this resource has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                //We've been desposed already. Abort further disposure.
                return;
            }
            //Dispose all native and managed resources.
            Dispose(true);

            //Remember we've been disposed.
            IsDisposed = true;

            //Notify listeners.
            OnDisposed(EventArgs.Empty);

            //Suppress finalisation; We already disposed our  resources.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Checks whether this instance has been disposed. If it has, it throws an exception.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when this instance has been disposed.</exception>
        protected void AssertNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().ToString());
        }

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected abstract void Dispose(bool disposing);

        protected virtual void OnDisposed(EventArgs args)
        {
            if (Disposed != null)
                Disposed(this, args);
        }
    }
}
