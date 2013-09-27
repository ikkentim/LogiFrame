using System;
using LogiFrame;
using System.Diagnostics;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a renderable component for LogiFrame.Frame 
    /// </summary>
    public abstract class Component : IDisposable
    {

        #region Fields

        private Location location = new Location();
        private Size size = new Size();
        private Bytemap bytemap;

        private bool topEffect;
        private bool transparent;
        private bool visible = true;
        private bool hasChanged = true;
        private bool isRendering;
        private bool disposed;

        #endregion

        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Component class.
        /// </summary>
        public Component()
        {
            location.Changed += new EventHandler(location_LocationChanged);
            size.Changed += new EventHandler(size_SizeChanged);
        }

        /// <summary>
        /// Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        ~Component()
        {
            Dispose();
        }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when a property has changed or the LogiFrame.Components.Component needs to be refreshed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Occurs when the location of the LogiFrame.Components.Component has changed.
        /// </summary>
        public event EventHandler LocationChanged;
        #endregion

        #region Properties

        /// <summary>
        /// The LogiFrame.Location this LogiFrame.Components.Comonent should 
        /// be rendered at in the parrent LogiFrame.Components.Container.
        /// </summary>
        public virtual Location Location
        {
            get { return location; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Components.Component.Size cannot be set to null.");

                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (location == value)
                    return;

                location.Changed -= location_LocationChanged;
                value.Changed += location_LocationChanged;

                location = value;

                if(LocationChanged != null)
                    LocationChanged(value, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The LogiFrame.Size of this LogiFrame.Components.Component.
        /// </summary>
        public virtual Size Size
        {
            get { return size; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Components.Component.Size cannot be set to null.");

                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (size == value)
                    return;

                size.Changed -= size_SizeChanged;
                value.Changed += size_SizeChanged;

                size = value;
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Components.Component should have Bytemap.TopEffect enabled.
        /// </summary>
        public bool TopEffect
        {
            get { return topEffect; }
            set
            {

                topEffect = value;

                HasChanged = true;
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Components.Component should have Bytemap.Transparent enabled.
        /// </summary>
        public bool Transparent
        {
            get { return transparent; }
            set
            {
                transparent = value;
                HasChanged = true;
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Components.Component should be visible.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;

                HasChanged = true;
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Component has been changed since the last render.
        /// </summary>
        public bool HasChanged
        {
            get { return hasChanged; }
            protected set
            {
                if (Disposed || IsRendering)
                    return;

                bool notify = !hasChanged;

                hasChanged = true;

                if (notify = Changed != null)
                    Changed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Component is in the process of rendering itself.
        /// </summary>
        public bool IsRendering
        {
            get { return isRendering; } 
        }

        /// <summary>
        /// Gets or sets the object that contains data about the component.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Whether this LogiFrame.Components.Component has been disposed.
        /// </summary>
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Gets the rendered LogiFrame.Bytemap of the current LogiFrame.Components.Component.
        /// </summary>
        public Bytemap Bytemap
        {
            get
            {
                if (hasChanged)
                {
                    Refresh(false);
                    hasChanged = false;
                }
                return Visible ? bytemap : Bytemap.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary.
        /// </summary>
        /// <param name="forceRefresh">Force the LogiFrame.Components.Component.Bytemap being rerendered even if it hasn't changed</param>
        public virtual void Refresh(bool forceRefresh)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            if (forceRefresh || HasChanged)
            {
                Debug.WriteLine("[DEBUG] Rendering " + ToString() + " at " + Location + ", IsRendering: " + IsRendering.ToString());
                isRendering = true;
                bytemap = Render();
                bytemap.Transparent = Transparent;
                bytemap.TopEffect = TopEffect;
                isRendering = false;
            }
        }

        /// <summary>
        /// Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary. 
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        /// Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                DisposeComponent();

                disposed = true;
            }
        }

        /// <summary>
        /// Stub for child components. This overridable method can be used to dispose resources.
        /// </summary>
        protected virtual void DisposeComponent()
        {
            //Stub
        }

        /// <summary>
        /// Renders all grahpics of the current LogiFrame.Components.Component
        /// </summary>
        /// <returns>The rendered LogiFrame.Bytemap</returns>
        protected abstract Bytemap Render();

        #endregion

        #region Private methods
        //Callbacks
        private void size_SizeChanged(object sender, EventArgs e)
        {
            HasChanged = true;
        }

        private void location_LocationChanged(object sender, EventArgs e)
        {
            if (!Disposed && LocationChanged != null)
                LocationChanged(sender, e);
        }
        #endregion

    }
}
