using System;

namespace LogiFrame.Components
{

    public abstract class Component
    {
        public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);

        public event ComponentChangedEventHandler ComponentChanged;

        #region Properties
        /// <summary>
        /// The LogiFrame.Location this LogiFrame.Components.Comonent should 
        /// be rendered at in the parrent LogiFrame.Components.Container.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// The LogiFrame.Size of this LogiFrame.Components.Component.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Whether this LogiFrame.Component has been changed since the last render.
        /// </summary>
        public bool HasChanged { 
            get
        {
                //TODO: Context of this method
                return false;
        }
            set
            {
                //TODO: Context of this method
            }
        }

        public Bytemap Bytemap
        {
            get
            {
                /*
                 * if changed :
                 *      rerender
                 */

                //TODO: Context of this method
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Provides data for the LogiFrame.Components.Component.ComponentChanged event.
    /// </summary>
    public class ComponentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.ComponentChangedEventArgs class.
        /// </summary>
        public ComponentChangedEventArgs()
        {

        }
    }
}
