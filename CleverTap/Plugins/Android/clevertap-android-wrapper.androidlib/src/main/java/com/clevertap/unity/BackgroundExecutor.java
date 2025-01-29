package com.clevertap.unity;

import android.os.Handler;
import android.os.Looper;

import java.util.concurrent.Executor;
import java.util.concurrent.Executors;

import kotlin.jvm.functions.Function0;

public class BackgroundExecutor {

    private final Executor executor = Executors.newSingleThreadExecutor(runnable -> {
        Thread newThread = Executors.defaultThreadFactory().newThread(runnable);
        newThread.setName("CleverTapUnityPluginBackgroundThread");
        return newThread;
    });

    private final Handler mainHandler = new Handler(Looper.getMainLooper());

    public <T> void execute(final Function0<T> backgroundWork,
                            final ResultCallback<T> resultCallback) {
        executor.execute(() -> {
            T result = backgroundWork.invoke();
            mainHandler.post(() -> resultCallback.onResult(result));
        });
    }

    @FunctionalInterface
    public interface ResultCallback<T> {
        void onResult(T result);
    }
}
