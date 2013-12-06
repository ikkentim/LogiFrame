using System;

namespace LogiFrame.Components.Book
{
    [AttributeUsage(AttributeTargets.Class |
                           AttributeTargets.Struct)
    ]
    public class PageInfo : Attribute
    {
        public PageInfo(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
