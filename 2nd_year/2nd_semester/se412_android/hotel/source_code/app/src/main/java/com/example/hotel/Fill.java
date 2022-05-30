//A reminder dialog that is triggered in 'hotelsinfo' when the user books a hotel but didn't fill all the fields
package com.example.hotel;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.DialogFragment;

import static android.content.Context.MODE_PRIVATE;

public class Fill extends DialogFragment {
    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        AlertDialog.Builder b=new AlertDialog.Builder(getActivity());
        b.setTitle(getString(R.string.fill)).setMessage(getString(R.string.fill1))
                .setPositiveButton( getString(R.string.ok), new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                    }
                } );
        return b.create();
    }

}
