package com.clevertap.unity.example;

import static com.clevertap.android.geofence.Logger.DEBUG;

import android.Manifest;
import android.content.pm.PackageManager;
import android.location.Location;
import android.os.Bundle;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import com.clevertap.android.geofence.CTGeofenceAPI;
import com.clevertap.android.geofence.CTGeofenceSettings;
import com.clevertap.android.geofence.interfaces.CTGeofenceEventsListener;
import com.clevertap.android.geofence.interfaces.CTLocationUpdatesListener;
import com.clevertap.android.sdk.CleverTapAPI;
import com.clevertap.unity.CleverTapOverrideActivity;

import org.json.JSONObject;

public class GeofenceExampleActivity extends CleverTapOverrideActivity implements CTGeofenceAPI.OnGeofenceApiInitializedListener, CTGeofenceEventsListener, CTLocationUpdatesListener {

    private static final int REQUEST_CODE_LOCATION_PERMISSION = 3212;

    private CTGeofenceAPI geofenceAPI;
    private boolean didAskForPermission = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        geofenceAPI = CTGeofenceAPI.getInstance(getApplicationContext());
        geofenceAPI.setOnGeofenceApiInitializedListener(this);
        geofenceAPI.setCtGeofenceEventsListener(this);
        geofenceAPI.setCtLocationUpdatesListener(this);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (didAskForPermission) {
            return;
        }

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED) {
            initGeofenceApi();
        } else {
            didAskForPermission = true;
            ActivityCompat.requestPermissions(
                    this,
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION},
                    REQUEST_CODE_LOCATION_PERMISSION
            );
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == REQUEST_CODE_LOCATION_PERMISSION && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
            initGeofenceApi();
        }
    }

    private void initGeofenceApi() {
        CleverTapAPI cleverTapAPI = CleverTapAPI.getDefaultInstance(this);
        CTGeofenceSettings ctGeofenceSettings = new CTGeofenceSettings.Builder()
                .setLogLevel(DEBUG)
                .build();
        geofenceAPI.init(ctGeofenceSettings, cleverTapAPI);
    }

    @Override
    public void OnGeofenceApiInitialized() {
        Toast.makeText(this, "Geofence Api Initialized", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onGeofenceEnteredEvent(JSONObject geofenceEnteredEventProperties) {
        Toast.makeText(this, "Geofence Entered", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onGeofenceExitedEvent(JSONObject geofenceExitedEventProperties) {
        Toast.makeText(this, "Geofence Exited", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onLocationUpdates(Location location) {
        Toast.makeText(this, "Location update " + location, Toast.LENGTH_SHORT).show();
    }
}
