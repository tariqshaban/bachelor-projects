//A dialog that sets the background color which is called from the navigation drawer
package com.example.hotel;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.NavigationView;
import android.support.v4.app.DialogFragment;

import static android.content.Context.MODE_PRIVATE;

public class Theme extends DialogFragment {
    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        String[] a={getString(R.string.gray),getString(R.string.white),getString(R.string.black)};
        final SharedPreferences auto=getActivity().getSharedPreferences("UI",MODE_PRIVATE);
        //'c' is an integer that is retrieved from the shared preferences which indicate the current background color
        int c=auto.getInt("Color", Color.parseColor("#333333"));
        final NavigationView nav =getActivity().findViewById(R.id.nav_view);
        //Position informs the dialog of the current text color
        int pos=0;
        if(c==Color.parseColor("#333333"))
            pos=0;
        else
        if(c==Color.WHITE)
            pos=1;
        else
        if(c==Color.BLACK)
            pos=2;

        AlertDialog.Builder b=new AlertDialog.Builder(getActivity());
        b.setTitle("Change Theme")
                .setSingleChoiceItems(a, pos, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        //Apply the selected color and saves it in the shared preferences
                        switch (i)
                        {
                            case 0:
                                auto.edit().putInt("Color",Color.parseColor("#333333")).apply();
                                getActivity().getWindow().getDecorView().setBackgroundColor(auto.getInt("Color",Color.BLACK));
                                nav.setBackgroundColor(auto.getInt("Color",Color.parseColor("#333333")));
                                getDialog().dismiss();
                                break;
                            case 1:
                                auto.edit().putInt("Color",Color.WHITE).apply();
                                getActivity().getWindow().getDecorView().setBackgroundColor(auto.getInt("Color",Color.BLACK));
                                nav.setBackgroundColor(auto.getInt("Color",Color.parseColor("#333333")));
                                getDialog().dismiss();
                                break;
                            case 2:
                                auto.edit().putInt("Color",Color.BLACK).apply();
                                getActivity().getWindow().getDecorView().setBackgroundColor(auto.getInt("Color",Color.BLACK));
                                nav.setBackgroundColor(auto.getInt("Color",Color.parseColor("#333333")));
                                getDialog().dismiss();
                                break;
                        }
                    }
                } );
        return b.create();
    }
}
