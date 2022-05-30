//A confirmation dialog that is triggered when the user chooses 'unbook' from the 'hotelsinfo' activity
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

public class Unbook extends DialogFragment {

    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        AlertDialog.Builder b=new AlertDialog.Builder(getActivity());
        //The message is meaningless due to the fact that we can't handle the credit card number, requires more research
        b.setTitle(getString(R.string.deletedial)).setMessage(getString(R.string.warn)+"\n\n\n"+getString(R.string.warn2))
                .setPositiveButton( getString(R.string.understand), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        SharedPreferences auto =getActivity().getSharedPreferences( "Auto", MODE_PRIVATE );
                        SharedPreferences confirm =getActivity().getSharedPreferences( "Confirm", MODE_PRIVATE );
                        Ops2 f2=new Ops2(getActivity());
                        //Deletes the book from the db
                        f2.removeBooked(auto.getString( "User", "Error!" ),confirm.getString( "Hotel", "Error!" ));
                        f2.close();

                        Ops1 f1=new Ops1(getActivity());
                        //Decrement the visitors of this hotel
                        f1.removeUsed(confirm.getString( "Hotel", "Error!" ));
                        f1.close();

                        Intent u=new Intent(getActivity(),grid.class);
                        startActivity(u);
                        getActivity().finish();
                    }
                } )
                .setNegativeButton( getString(R.string.cancel), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                    }
                } );
        return b.create();
    }

}
