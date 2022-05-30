//A splash screen activity
package com.example.hotel;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends AppCompatActivity {
    //Defining necessary variables
    private static int Splash=5200;
    int Progress=0;
    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),Color.parseColor("#0040FF")};
    SharedPreferences auto;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_main );

        //A shared preferences that retrieves the background and text color
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c=auto.getInt("Color", Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);
        ImageView a1=findViewById(R.id.imageView);
        int c2=auto.getInt("Text",4);
        TextView a=findViewById(R.id.choosedreamhotel);
        a.setTextColor(colors[c2]);
        final ProgressBar ProgressBar=findViewById(R.id.progressBar);
        ProgressBar.getProgressDrawable().setColorFilter(colors[c2], android.graphics.PorterDuff.Mode.SRC_IN);

        //Starts an audio
        MediaPlayer mp = MediaPlayer.create(this, R.raw.intro);
        mp.start();

        //Sets the image
        a1.setImageResource(R.drawable.icon);

        //Increments the progress bar every 100 milliseconds
        Timer t=new Timer();
        TimerTask tt=new TimerTask() {
            @Override
            public void run() {
                Progress++;
                ProgressBar.setProgress(Progress);
                if(Progress==100)cancel();
            }
        };
        t.schedule(tt,0,50);


        //After the progress bar completes this function will run and starts the 'Intro' activity
        new Handler().postDelayed( new Runnable() {
            @Override
            public void run() {
               Intent Start=new Intent(MainActivity.this,Intro.class);
               startActivity(Start);
               finish();
            }
        },Splash );



    }
}
