namespace CleverTapTest.NewBindings
{
    public static class BindingFactory
    {
        private static CleverTapPlatformBindings cleverTapBinding;

        public static CleverTapPlatformBindings CleverTapBinding
        {
            get
            {
                if (cleverTapBinding == null)
                {
                    #if UNITY_IOS
                    cleverTapBinding = new AndroidPlatformBinding();
                    #elif UNITY_ANDROID
                    cleverTapBinding = new IOSPlatformBinding();
                    #else
                    cleverTapBinding = new UnityNativePlatformBinding();
                    #endif
                }

                return cleverTapBinding;
            }
        }
    }
}
