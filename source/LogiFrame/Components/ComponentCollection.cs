using System.Collections.ObjectModel;

namespace LogiFrame.Components
{
    /// <summary>
    ///  Represents a dynamic collection of LogiFrame.Components.Component.
    /// </summary>
    /// <typeparam name="T">An instance of LogiFrame.Components.Component</typeparam>
    public class ComponentCollection<T> : ObservableCollection<T>
    where T : Component
    {

    }
}
