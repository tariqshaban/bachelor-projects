//A dialog that sets the text color which is called from the navigation drawer
package com.example.hotel;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.NavigationView;
import android.support.v4.app.DialogFragment;

import static android.content.Context.MODE_PRIVATE;

public class Colour extends DialogFragment {
    //Result is a value that is set when the user selects a color, retrieved in showVotingDialog function
    int result=0;
    SharedPreferences auto;
    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        auto = getActivity().getSharedPreferences( "UI", MODE_PRIVATE );
        String[] a = {getString(R.string.blue),getString(R.string.red),getString(R.string.cyan),getString(R.string.green),getString(R.string.lgreen)};
        //'c' is an integer that is retrieved from the shared preferences which indicate the current text color
        int c = auto.getInt( "Text", 4 );
        //Position informs the dialog of the current text color
        int pos = 0;
        if (c == 4)
            pos = 0;
        else if (c == 0)
            pos = 1;
        else if (c == 1)
            pos = 2;
        else if (c == 2)
            pos = 3;
        else if (c == 3)
            pos = 4;

        AlertDialog.Builder b = new AlertDialog.Builder( getActivity() );
        b.setTitle( "Change Color" )
                .setSingleChoiceItems( a, pos, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        switch (i) {
                            case 0:
                                result=4;
                                getDialog().dismiss();
                                break;
                            case 1:
                                result=0;
                                getDialog().dismiss();
                                break;
                            case 2:
                                result=1;
                                getDialog().dismiss();
                                break;
                            case 3:
                               result=2;
                                getDialog().dismiss();
                                break;
                            case 4:
                                result=3;
                                getDialog().dismiss();
                                break;
                        }
                        //Calls a confirmation dialog
                        showVotingDialog(getActivity());
                    }
                } );
        return b.create();
    }

    private void showVotingDialog(final Activity e) {
        //A confirmation dialog that when 'yes' is clicked, 'result will be stored a shared preferences'
        AlertDialog.Builder b = new AlertDialog.Builder(e);
        b.setTitle(getString(R.string.restart)).setMessage(getString(R.string.restart1))
                .setPositiveButton( getString(R.string.yes), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        auto.edit().putInt( "Text", result ).apply();
                        Intent h = new Intent(e, MainActivity.class );
                        e.startActivity( h );
                    }
                } )
                .setNegativeButton( getString(R.string.no), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {

                    }
                } );
        b.show();
    }
}