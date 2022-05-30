//A confirmation dialog that is triggered when the user chooses 'delete user' from the drop down menu in the upper right of the screen (position varies depending on language)
package com.example.hotel;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.Snackbar;
import android.support.v4.app.DialogFragment;
import android.widget.Toast;

import static android.content.Context.MODE_PRIVATE;

public class Del extends DialogFragment {

        @NonNull
        @Override
        public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
            AlertDialog.Builder b=new AlertDialog.Builder(getActivity());
            b.setTitle(getString(R.string.deletedial)).setMessage(R.string.deletedial1)
                    .setPositiveButton( R.string.delete_app, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {
                            Ops q = new Ops( getActivity());
                            SharedPreferences auto=getActivity().getSharedPreferences("Auto",MODE_PRIVATE);
                            //Deletes the user from the database
                            q.deleteUser(auto.getString("User",""));
                            //Deletes the user from the shared preferences
                            SharedPreferences.Editor edit1=auto.edit();
                            edit1.putString("User",null);
                            edit1.putString("Pass",null);
                            edit1.apply();
                            //returns to 'Intro' activity
                            Intent u=new Intent(getActivity(),Intro.class);
                            startActivity(u);
                        }
                    } )
                    .setNegativeButton( R.string.cancel, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {

                        }
                    } );
            return b.create();
        }

    }
