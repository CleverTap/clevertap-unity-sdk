#if UNITY_IOS
using CleverTap.Common;
using CleverTap.Utilities;

namespace CleverTap.IOS {
    internal class IOSPlatformVariable : CleverTapPlatformVariable {
        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue) {
            IOSDllImport.CleverTap_defineVariable(name, kind, Json.Serialize(defaultValue));
            Var<T> result = new IOSVar<T>(name, kind, defaultValue);
            varCache.Add(name, result);
            return result;
        }
    }
}
#endif
