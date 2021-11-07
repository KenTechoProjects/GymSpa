using System;

namespace Utilities
{
    [System.AttributeUsage(System.AttributeTargets.Class | AttributeTargets.Method |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)  // multiuse attribute
]
    public class NotImplemented : System.Attribute
    {
        public NotImplemented(string displayingMessage = null)
        {
            NotIMplemented = null;
        }

        public Object NotIMplemented { get; set; }
    }
}