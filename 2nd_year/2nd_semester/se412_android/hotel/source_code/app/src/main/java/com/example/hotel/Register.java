//An activity that shows registers a new user
package com.example.hotel;

import android.app.Notification;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.os.VibrationEffect;
import android.os.Vibrator;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.NotificationManagerCompat;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.TextView;
import android.widget.Toast;

import java.util.regex.Pattern;

import static com.example.hotel.Noti.ChannelID;


public class Register extends AppCompatActivity {

    //Defining necessary variables
    private NotificationManagerCompat notificationManager;
    Ops q=new Ops(this);
    //A regular expression for the E-mail field
    public static final Pattern EMAIL_ADDRESS=Pattern.compile("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])");
    //A regular expression for the password field
    public static final Pattern PASS=Pattern.compile("^(?=.*[0-9])(?=.*[a-z])(?=\\S+$).{6,}$");
    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),Color.parseColor("#0040FF")};
    SharedPreferences auto;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_register );

        //A shared preferences that retrieves the background and text color
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c1=auto.getInt("Color", Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c1);
        int c2=auto.getInt("Text",4);
        TextView a=findViewById(R.id.editText12);
        TextView b=findViewById(R.id.editText22);
        TextView c=findViewById(R.id.editText32);
        TextView d=findViewById(R.id.editText42);
        a.setTextColor(colors[c2]);
        b.setTextColor(colors[c2]);
        c.setTextColor(colors[c2]);
        d.setTextColor(colors[c2]);

        //Sets up the notification
        notificationManager=NotificationManagerCompat.from(this);

        final Button B=findViewById(R.id.button);
        final CheckBox C=findViewById(R.id.checkBox);
        //Disables the register button and gray it out until the checkbox is checked
        B.setEnabled(false);
        B.setAlpha(.5f);
        //Enables the register button upon enabling the checkbox
        C.setOnClickListener( new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(C.isChecked())
                {
                    B.setEnabled(true);
                    B.setAlpha(1);
                }
                else
                {
                    B.setEnabled(false);
                    B.setAlpha(.5f);
                }
            }
        } );

        //Text input layout that offers a better visual and features
        final TextInputLayout U=findViewById(R.id.editText1);
        final TextInputLayout E=findViewById(R.id.editText2);
        final TextInputLayout P=findViewById(R.id.editText3);
        final TextInputLayout P2=findViewById(R.id.editText4);

        //Handles the register button, includes along validation of inputs
        B.setOnClickListener( new View.OnClickListener() {
            @Override
            public void onClick(View view) {

             String UI=U.getEditText().getText().toString().replace(" ", "");
             user s=q.searchUser(UI);
             if(!UI.equals(U.getEditText().getText().toString()))
                 U.setError(getString(R.string.space));
             else
             if(UI.isEmpty())
                 U.setError(getString(R.string.required));
             else
             if(UI.length()>15)
                 U.setError(getString(R.string.long_));
             else
                 if(s.getName()!=null)
                     U.setError(getString(R.string.exist));
                 else
                 U.setError(null);
            String EI=E.getEditText().getText().toString().replace(" ", "");
            if(!EI.equals(E.getEditText().getText().toString()))
                E.setError(getString(R.string.space));
            else
            if(EI.isEmpty())
                E.setError(getString(R.string.required));
            else
            if(!EMAIL_ADDRESS.matcher(EI).matches())
                E.setError(getString(R.string.invalid_email));
            else
                E.setError(null);

            String PI=P.getEditText().getText().toString().replace(" ", "");
            if(!PI.equals(P.getEditText().getText().toString()))
                P.setError(getString(R.string.space));
            else
            if(PI.isEmpty())
                P.setError(getString(R.string.required));
            else
            if(PI.length()>15)
                P.setError(getString(R.string.long_));
            else
            if(!PASS.matcher(PI).matches())
                P.setError(getString(R.string.invalid_pass_req));
            else
                P.setError(null);


            String P2I=P2.getEditText().getText().toString().replace(" ", "");
            if(!P2I.equals(P2.getEditText().getText().toString()))
                P2.setError(getString(R.string.space));
            else
            if(P2I.isEmpty())
                P2.setError(getString(R.string.required));
            else
            if(P2I.length()>15)
                P2.setError(getString(R.string.long_));
            else
            if(!PASS.matcher(P2I).matches())
                P2.setError(getString(R.string.invalid_pass_req));
            else
                P2.setError(null);

            if(!PI.equals(P2I) && PASS.matcher(PI).matches() && PASS.matcher(P2I).matches() && (P.getEditText().getText().toString().equals(PI) || P2.getEditText().getText().toString().equals(P2I)))
            {
                P.setError(getString(R.string.pass_match));
                P2.setError(getString(R.string.pass_match));
            }
            else
            if(PI.equals(P2I) && PASS.matcher(PI).matches() && PASS.matcher(P2I).matches() && P.getEditText().getText().toString().equals(PI) && P2.getEditText().getText().toString().equals(P2I))
            {
                P.setError(null);
                P2.setError(null);
            }

            if(U.getError()==null && E.getError()==null && P.getError()==null && P2.getError()==null)
            {
                //When the input is valid, the username and password will be saved in the db and in the shared preferences
                SharedPreferences auto=getSharedPreferences("Auto",MODE_PRIVATE);
                SharedPreferences.Editor editor=auto.edit();
                editor.putString( "User",UI);
                editor.putString("Pass",PI);
                editor.apply();
                user u=new user(EI,UI,PI);
                q.addUser(u);
                q.close();
                q.close();

                //Fires the notification
                Notification add=new NotificationCompat.Builder(getApplicationContext(),ChannelID)
                        .setSmallIcon(R.drawable.ic_stat_name).setContentTitle(getString(R.string.complete))
                        .setContentText(getString(R.string.thanks))
                        .setStyle(new NotificationCompat.BigTextStyle().bigText(getString(R.string.thanks1)))
                        .build();
                add.contentIntent=  PendingIntent.getActivity(getApplicationContext(), 0,
                        new Intent(getApplicationContext(), MainActivity.class), PendingIntent.FLAG_UPDATE_CURRENT);
                notificationManager.notify(1,add);

                //Vibrates the phone upon receiving the notification for 1000 milliseconds
                Vibrator v = (Vibrator) getSystemService(getApplication().VIBRATOR_SERVICE);
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    v.vibrate( VibrationEffect.createOneShot(500, VibrationEffect.DEFAULT_AMPLITUDE));
                } else {
                    v.vibrate(1000);
                }

                //Launches the 'grid' activity
                Intent a=new Intent(getApplicationContext(),grid.class);
                startActivity(a);
            }
            }
        } );

    }



}
