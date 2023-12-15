﻿#if UNITY_IOS
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.IOS {
    internal class IOSPlatformVariable : CleverTapPlatformVariable {
        internal override void SyncVariables() =>
            IOSDllImport.CleverTap_syncVariables();

<<<<<<< HEAD
        internal override void SyncVariables(bool isProduction) =>
            IOSDllImport.CleverTap_syncVariablesProduction(isProduction);

=======
>>>>>>> 58f5a2b (Feature/sdk 3323/variables bindings (#54))
        internal override void FetchVariables(int callbackId) =>
            IOSDllImport.CleverTap_fetchVariables(callbackId);

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue) {
            IOSDllImport.CleverTap_defineVar(name, kind, Json.Serialize(defaultValue));
            Var<T> result = new IOSVar<T>(name, kind, defaultValue);
            varCache.Add(name, result);
            return result;
        }
    }
}
#endif
