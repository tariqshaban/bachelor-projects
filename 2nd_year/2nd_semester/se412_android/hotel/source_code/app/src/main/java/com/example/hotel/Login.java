//An activity that allows a registered user to login
package com.example.hotel;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;


public class Login extends AppCompatActivity {

    //Defining necessary variables
    SharedPreferences auto;
    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),Color.parseColor("#0040FF")};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_login );

        //A shared preferences that retrieves the background and text color
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c2=auto.getInt("Text",4);
        final EditText Huser=findViewById(R.id.editText2);
        final EditText Hpass=findViewById(R.id.editText3);
        Huser.setTextColor(colors[c2]);
        Hpass.setTextColor(colors[c2]);
        int c=auto.getInt("Color", Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);


        Button B=findViewById(R.id.button);
        //Handles the login button
        B.setOnClickListener( new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Calls the user db
                Ops q = new Ops( getApplicationContext() );
                //Search if the user have registered
                user a = q.searchUser( Huser.getText().toString() );
                q.close();

                //A long commands that validates the input
                final TextInputLayout U=findViewById(R.id.f1);
                final TextInputLayout P=findViewById(R.id.f2);
                String UI=Huser.getText().toString();
                String PI=Hpass.getText().toString();
                if(UI.isEmpty())
                    U.setError(getString(R.string.required));
                else
                if(a.getName() == null)
                    U.setError(getString(R.string.invalid_user));

                if(PI.isEmpty())
                    P.setError(getString(R.string.required));
                else
                if(!Hpass.getText().toString().equals(a.getPass()))
                    P.setError(getString(R.string.invalid_pass));

                if(!UI.isEmpty() && a.getName()!=null)
                    U.setError(null);
                if(!PI.isEmpty())
                    if(Hpass.getText().toString().equals(a.getPass()))
                        P.setError(null);

                    if(!UI.isEmpty() && a.getName()!=null && !PI.isEmpty() && Hpass.getText().toString().equals(a.getPass()))
                    {
                        SharedPreferences auto=getSharedPreferences("Auto",MODE_PRIVATE);
                        SharedPreferences.Editor editor=auto.edit();
                        editor.putString( "User",Huser.getText().toString());
                        editor.putString("Pass",Hpass.getText().toString());
                        editor.apply();
                        Intent i = new Intent( getApplicationContext(), grid.class );
                        startActivity( i );
                    }
                }
        } );
    }

    //Handles app restart events
    @Override
    protected void onRestart() {

        super.onRestart();
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c=auto.getInt("Color",Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);
        int c2=auto.getInt("Text",4);
        EditText Huser=findViewById(R.id.editText2);
        EditText Hpass=findViewById(R.id.editText3);
        Huser.setTextColor(colors[c2]);
        Hpass.setTextColor(colors[c2]);
    }
}
