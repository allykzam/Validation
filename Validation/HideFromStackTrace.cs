using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    /// <summary>
    /// Hide a method and all sub-frames from stack traces generated in the <seealso cref="ValidationException"/> class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited=false)]
    internal class HideFromStackTrace : Attribute { }
    internal static class HideFromStackTraceTools
    {
        internal static bool ShouldHide(this System.Reflection.MethodBase method)
        {
            return method.IsDefined(typeof(HideFromStackTrace), true);
        }
    }
}
