//The custom grid view calls this function to inflates views
package com.example.hotel;

import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import static android.content.Context.MODE_PRIVATE;

public class pichotel extends ArrayAdapter<String> {
    public pichotel( Context context, int resource,  String[] objects) {
        super(context, resource, objects);
    }


    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),Color.parseColor("#0040FF")};
    @Override
    public View getView(int position,  View convertView,  ViewGroup parent) {
        LayoutInflater inflater=LayoutInflater.from(getContext());
        View v = inflater.inflate( R.layout.activity_pichotel,parent,false);
        ImageView im=v.findViewById( R.id.pic);
        TextView tview=v.findViewById( R.id.text);
        Ops1 q = new Ops1(getContext());
        int hotelspics[]=q.pics();
        im.setImageResource(hotelspics[position]);
        tview.setText(getItem(position));
        SharedPreferences auto=getContext().getSharedPreferences("UI",MODE_PRIVATE);
        int c2=auto.getInt("Text",4);
        tview.setTextColor(colors[c2]);

        return v;
    }
}


