#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal class UnityNativePlatformVariable : CleverTapPlatformVariable
    {
        private readonly UnityNativeVarCache nativeVarCache;
        private UnityNativeEventManager unityNativeEventManager;
        private CleverTapCallbackHandler callbackHandler;

        internal UnityNativePlatformVariable() : base()
        {
            nativeVarCache = new UnityNativeVarCache();
        }

        internal void Load(UnityNativeEventManager unityNativeEventManager,
            CleverTapCallbackHandler callbackHandler,
            UnityNativeCoreState coreState)
        {
            this.unityNativeEventManager = unityNativeEventManager;
            this.callbackHandler = callbackHandler;

            nativeVarCache.SetCoreState(coreState);
            nativeVarCache.LoadDiffs();
        }

        internal void SwitchUser()
        {
            nativeVarCache.Reset();
            nativeVarCache.LoadDiffs();
        }

        internal void HandleVariablesResponse(object varsResponse)
        {
            if (varsResponse is IDictionary<string, object> vars)
            {
                HandleVariablesResponseSuccess(vars);
            }
            else
            {
                HandleVariablesResponseError();
            }
        }

        private void HandleVariablesResponseSuccess(IDictionary<string, object> varsJson)
        {
            nativeVarCache.SetHasVarsRequestCompleted(true);
            var diffs = UnityNativeVariableUtils.ConvertDictionaryToNestedDictionaries(varsJson);

            nativeVarCache.ApplyVariableDiffs(diffs);
            nativeVarCache.SaveDiffs();

            TriggerVariablesChanged();
            TriggerVariablesFetched(true);
        }

        internal void HandleVariablesResponseError()
        {
            if (!nativeVarCache.HasVarsRequestCompleted)
            {
                nativeVarCache.SetHasVarsRequestCompleted(true);
                TriggerVariablesChanged();
            }

            TriggerVariablesFetched(false);
        }

        private void TriggerVariablesChanged()
        {
            callbackHandler.CleverTapVariablesChanged("VariablesChanged");
            callbackHandler.CleverTapVariablesChangedAndNoDownloadsPending("VariablesChangedAndNoDownloadsPending");
        }

        private void TriggerVariablesFetched(bool success)
        {
            var fetchedMessage = new Dictionary<string, object>
            {
                { "isSuccess", success }
            };
            callbackHandler.CleverTapVariablesFetched(Json.Serialize(fetchedMessage));
        }

        internal override void SyncVariables()
        {
            SyncVariables(false);
        }

        internal override void SyncVariables(bool isProduction)
        {
            if (isProduction)
            {
                if (Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("Calling SyncVariables(true) from Debug build. Do not use (isProduction: true) in this case.");
                }
                else
                {
                    CleverTapLogger.Log("Calling SyncVariables(true) from Release build. Do not release this build and use with caution.");
                }
            }
            else
            {
                if (!Debug.isDebugBuild)
                {
                    CleverTapLogger.Log("SyncVariables() can only be called from Debug builds.");
                    return;
                }
            }

            if (unityNativeEventManager != null)
            {
                if (nativeVarCache.VariablesCount == 0)
                {
                    CleverTapLogger.Log("CleverTap: No Variables defined.");
                }
                unityNativeEventManager.SyncVariables(nativeVarCache.GetDefineVarsPayload());
            }
            else
            {
                CleverTapLogger.LogError("CleverTap Error: Cannot sync variables. The unityNativeWrapper is null.");
            }
        }

        internal override void FetchVariables(int callbackId)
        {
            CleverTapLogger.LogError("CleverTap Error: FetchVariables is not supported for this platform.");
        }

        protected override Var<T> DefineVariable<T>(string name, string kind, T defaultValue)
        {
            UnityNativeVar<T> result = new UnityNativeVar<T>(name, kind, defaultValue, nativeVarCache);
            varCache.Add(name, result);
            return result;
        }

        protected override Var<T> GetOrDefineVariable<T>(string name, T defaultValue)
        {
            var variable = base.GetOrDefineVariable<T>(name, defaultValue);
            return variable;
        }

        protected override Var<string> GetOrDefineFileVariable(string name)
        {
            CleverTapLogger.LogError("CleverTap Error: File Variables are not supported for this platform.");
            return null;
        }
    }
}
#endif
