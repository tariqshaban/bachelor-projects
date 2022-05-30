//An activity that shows the hotel position in a map in full view
package com.example.hotel;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

public class Expand extends AppCompatActivity implements OnMapReadyCallback {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_expand );

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager().findFragmentById( R.id.map );
        //Shows the map
        mapFragment.getMapAsync( this );
    }

    //A function that is called as soon the map fragment is created
    @Override
    public void onMapReady(GoogleMap googleMap) {
        GoogleMap map = googleMap;
        Intent i = getIntent();
        //Retrieves a specific position of a certain hotel by using 'getExtra' from 'hotelsinfo'
        Double Height = i.getDoubleExtra(  "Height" ,0);
        Double Width = i.getDoubleExtra(  "Width",0);
        //Sets the position of the hotel in the map
        LatLng Hotel = new LatLng( Width, Height );
        //Marks the position
        map.addMarker( new MarkerOptions().position( Hotel ).title( "Hotel" ) );
        //Changes the map zoom
        map.moveCamera( CameraUpdateFactory.newLatLngZoom( Hotel, 16 ) );
    }
}
