using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public abstract class JavaScriptGlobalVariable : IJavaScriptGlobalVariable
    {
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }

        public string GetJsSetterString()
        {
            return Value == null ? null : string.Format("ReferEngineGlobals.{0} = \"{1}\";", Name, Value);
        }
    }
}