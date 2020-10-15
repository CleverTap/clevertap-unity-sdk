package com.clevertap.unity;

import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import androidx.appcompat.app.AppCompatActivity;
import com.unity3d.player.UnityPlayer;

/**
 * Unity Player activity to support AndroidX features
 */
public class AppCompatUnityPlayerActivity
        extends AppCompatActivity {

    protected UnityPlayer mUnityPlayer;

    protected void onCreate(Bundle bundle) {
        this.requestWindowFeature(1);
        super.onCreate(bundle);
        this.getWindow().setFormat(2);
        this.mUnityPlayer = new UnityPlayer(this);
        this.setContentView(this.mUnityPlayer);
        this.mUnityPlayer.requestFocus();
    }

    protected void onDestroy() {
        this.mUnityPlayer.quit();
        super.onDestroy();
    }

    protected void onPause() {
        super.onPause();
        this.mUnityPlayer.pause();
    }

    protected void onResume() {
        super.onResume();
        this.mUnityPlayer.resume();
    }

    public void onConfigurationChanged(Configuration configuration) {
        super.onConfigurationChanged(configuration);
        this.mUnityPlayer.configurationChanged(configuration);
    }

    public void onWindowFocusChanged(boolean bl) {
        super.onWindowFocusChanged(bl);
        this.mUnityPlayer.windowFocusChanged(bl);
    }

    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
        if (keyEvent.getAction() == 2) {
            return this.mUnityPlayer.injectEvent(keyEvent);
        }
        return super.dispatchKeyEvent(keyEvent);
    }

    public boolean onKeyUp(int n2, KeyEvent keyEvent) {
        return this.mUnityPlayer.injectEvent(keyEvent);
    }

    public boolean onKeyDown(int n2, KeyEvent keyEvent) {
        return this.mUnityPlayer.injectEvent(keyEvent);
    }

    public boolean onTouchEvent(MotionEvent motionEvent) {
        return this.mUnityPlayer.injectEvent(motionEvent);
    }

    public boolean onGenericMotionEvent(MotionEvent motionEvent) {
        return this.mUnityPlayer.injectEvent(motionEvent);
    }
}