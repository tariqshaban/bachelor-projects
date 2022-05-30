//An activity that shows a video and allows the user to log in or register, a homepage in short
package com.example.hotel;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.method.ScrollingMovementMethod;
import android.view.MenuItem;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.Scroller;
import android.widget.TextView;
import android.widget.VideoView;


public class Intro extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener {
    //Defining necessary variables
    private DrawerLayout drawer;
    SharedPreferences auto;
    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),Color.parseColor("#0040FF")};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_intro );

        //A shared preferences that retrieves the username and password
        auto = getSharedPreferences( "Auto", MODE_PRIVATE );
        String user=auto.getString("User","Null");
        String pass=auto.getString("Pass","Null");
        Intent i=new Intent(getApplicationContext(),grid.class);
        //Auto log in if the user have already registered on this device
        if(user != "Null" && pass != "Null")
            startActivity(i);

        //A shared preferences that retrieves the background and text color
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c=auto.getInt("Color",Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);
        int c2=auto.getInt("Text",4);
        TextView a=findViewById(R.id.choosedreamhotel);
        a.setTextColor(colors[c2]);

        //Defining necessary variables
        Button Login=findViewById(R.id.Login);
        Button Register=findViewById(R.id.Register);
        VideoView VideoView=findViewById(R.id.videoView);

        //Sets up the video player
        VideoView.setVideoURI( Uri.parse("android.resource://"+getPackageName()+"/"+R.raw.video));
        VideoView.start();
        VideoView.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
            @Override
            public void onPrepared(MediaPlayer mp) {
                mp.setVolume(0, 0);
                mp.setLooping(true);
            }
        });

        //Handles log in button
        Login.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent L=new Intent(getApplicationContext(),Login.class);
                startActivity(L);
            }
        } );

        //Handles register button
        Register.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent R=new Intent(getApplicationContext(),Register.class);
                startActivity(R);
            }
        } );

        //Sets up the navigation drawer
        Toolbar toolbar=findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        drawer=findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle=new ActionBarDrawerToggle(this,drawer,toolbar,R.string.navigation_drawer_open,R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();
        NavigationView nav =findViewById(R.id.nav_view);
        nav.setBackgroundColor(c);
        nav.setItemTextColor(ColorStateList.valueOf(colors[c2]));
        nav.setItemIconTintList(ColorStateList.valueOf(colors[c2]));
        nav.setNavigationItemSelectedListener(this);
            }

    //Handles app restart events
    @Override
    protected void onRestart() {
        super.onRestart();
        VideoView VideoView=findViewById(R.id.videoView);
        VideoView.start();
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c=auto.getInt("Color",Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);
        int c2=auto.getInt("Text",4);
        TextView a=findViewById(R.id.choosedreamhotel);
        a.setTextColor(colors[c2]);
    }

    //Shows the close app dialog
    @Override
    public void onBackPressed() {
        if (drawer.isDrawerOpen(GravityCompat.START))
            drawer.closeDrawer(GravityCompat.START);
        else {
            Confirm a=new Confirm();
            a.show(getSupportFragmentManager(),"lel");
        }
    }

    //Handles the navigation drawer items click
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
        NavigationView nav =findViewById(R.id.nav_view);
        switch(menuItem.getItemId()) {
            case R.id.theme:
            //Shows the change theme dialog
                Theme w=new Theme();
                w.show(getSupportFragmentManager(),"lel");
                break;
            case R.id.color:
            //Shows the change text color dialog
                Colour v=new Colour();
                v.show(getSupportFragmentManager(),"lel");
                break;
            case R.id.help:
            //Shows the help dialog, scrollable
                AlertDialog dialog = new AlertDialog.Builder(this)
                        .setTitle(getString(R.string.help))
                        .setMessage("temp")
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        })
                        .setIcon(android.R.drawable.ic_menu_help)
                        .show();
                Window window = dialog.getWindow();
                window.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000);
                TextView textView =dialog.findViewById(android.R.id.message);
                textView.setText("This\n\n\n\n\n\nIs\n\n\n\n\n\nHelp");
                textView.setScroller(new Scroller(this));
                textView.setVerticalScrollBarEnabled(true);
                textView.setMovementMethod(new ScrollingMovementMethod());
                break;
            case R.id.about:
            //Shows the about dialog, scrollable
                AlertDialog dialog1 = new AlertDialog.Builder(this)
                        .setTitle(getString(R.string.about))
                        .setMessage("temp")
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        })
                        .setIcon(android.R.drawable.ic_menu_info_details)
                        .show();
                TextView textView1 =dialog1.findViewById(android.R.id.message);
                Window window1 = dialog1.getWindow();
                window1.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000);
                textView1.setText("This\n\n\n\n\n\nIs\n\n\n\n\n\nabout");
                textView1.setScroller(new Scroller(this));
                textView1.setVerticalScrollBarEnabled(true);
                textView1.setMovementMethod(new ScrollingMovementMethod());
                break;

        }
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }
}
