//This class interact with the hotel db
package com.example.hotel;

import java.util.ArrayList;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class Ops1 extends SQLiteOpenHelper {


    public Ops1(Context context) {
        super( context, "hotel.db", null, 1 );
    }

    @Override
    public void onCreate(SQLiteDatabase arg0) {

        String q = "create table hotel (name text primary key,pic intger,rate integer,address text,price integer,locw real,loch real,used integer,size integer);";
        arg0.execSQL( q );
        arg0.execSQL( "insert into hotel values (\"Porto Bello\"," + R.drawable.portobellohotel + ",5,\"Bir Tatil Mahallesi\",170,36.851295,30.622607,450,450);" );
        arg0.execSQL( "insert into hotel values (\"Ramada Blaza\"," + R.drawable.ramadaplazahotel + ",6,\"Gençlik Mahallesi, Fevzi Çakmak\",190,36.876732,30.707939,700,700);" );
        arg0.execSQL( "insert into hotel values (\"Nar Sarisu Apart\"," + R.drawable.nar + ",4,\"Sarısu Mahallesi\",140,36.841129,30.589482,261,270);" );
        arg0.execSQL( "insert into hotel values (\"Hayal Residence Apart\"," + R.drawable.hayal + ",3,\"Şirinyalı Mahallesi\",100,36.856564,30.744475,676,800);" );
        arg0.execSQL( "insert into hotel values (\"Rixos Downtown Antalya\"," + R.drawable.rixos + ",6,\"Deniz Mahallesi\",200,36.886053,30.674133,397,560);" );
        arg0.execSQL( "insert into hotel values (\"Yazar Lara Hotel\"," + R.drawable.yazar + ",2,\"Çağlayan Mahallesi\",80,36.849171,30.766642,340,400);" );

        arg0.execSQL( "insert into hotel values (\"The Marmara\"," + R.drawable.themarmara + ",2,\"Şirinyalı Mahallesi, Muratpaşa/Antalya, Turkey\",120,36.856759, 30.738848,295,566);" );
        arg0.execSQL( "insert into hotel values (\"Hotel Su\"," + R.drawable.hotelsu + ",5,\"Meltem Mahallesi, Muratpaşa/Antalya, Turkey\",88,36.878453, 30.662058,1404,1600);" );
        arg0.execSQL( "insert into hotel values (\"Delphin BE Grand Resort\"," + R.drawable.delphinbegrandresort + ",4,\"Güzeloba Mahallesi, Muratpaşa/Antalya, Turkey\",99,36.855209, 30.863801,2054,2600);" );
        arg0.execSQL( "insert into hotel values (\"Alp Pasa Old Town\"," + R.drawable.alppasaoldtown + ",3,\"Barbaros Mah\",150,36.883838, 30.707447,154,300);" );
        arg0.execSQL( "insert into hotel values (\"Prime Boutique Hotel\"," + R.drawable.primeboutiquehotel + ",5,\"Genclik Mah\",190,36.874176, 30.715519,507,750);" );
        arg0.execSQL( "insert into hotel values (\"White Garden\"," + R.drawable.whitegarden + ",5,\"Kaleiçi Kılıçaslan Mah. Hesapçi Geçidi No:9\",60,36.882418, 30.704450,897,1200);" );
    }

    @Override
    public void onUpgrade(SQLiteDatabase arg0, int arg1, int arg2) {
        String q = "drop table hotel;";
        arg0.execSQL( q );
        onCreate( arg0 );
    }

    int[] pics() {

        String q = "select * from hotel;";
        SQLiteDatabase db1 = getReadableDatabase();
        Cursor c = db1.rawQuery( q, null );
        int[] array = new int[c.getCount()];
        int i = 0;
        while (c.moveToNext()) {
            int uname = c.getInt( c.getColumnIndex( "pic" ) );
            array[i] = uname;
            i++;
        }

        return array;
    }

    String[] names() {

        String q = "select * from hotel;";
        SQLiteDatabase db1 = getReadableDatabase();
        Cursor c = db1.rawQuery( q, null );
        String[] array = new String[c.getCount()];
        int i = 0;
        while (c.moveToNext()) {
            String uname = c.getString( c.getColumnIndex( "name" ) );
            array[i] = uname;
            i++;
        }

        return array;
    }

    void info(String n,hotel i) {
        String q = "select * from hotel where name=\"" + n + "\";";
        SQLiteDatabase db1 = getReadableDatabase();
        Cursor c = db1.rawQuery( q, null );
        if (c.moveToFirst()) {// if there is data, moveToFirst() moves cursor to first row  and returns true , if there is no data it returns false
            i.setPic(c.getInt(1));
            i.setRate(c.getInt(2));
            i.setAddress(c.getString(3));
            i.setPrice(c.getInt(4));
            i.setLocw(c.getDouble(5));
            i.setLoch(c.getDouble(6));
            i.setUsed(c.getInt(7));
            i.setSize(c.getInt(8));

        }
    }

    int[] innerpic(String n) {
        int[]s=new int[]{};
        if(n.equals("Porto Bello"))
            s=new int[]{R.drawable.portobellolaunge,R.drawable.portobellorestaurant1,R.drawable.portobelloroom};
        else
        if(n.equals("Ramada Blaza"))
            s=new int[]{R.drawable.ramadaplazaroom,R.drawable.ramadaplazaroom2,R.drawable.ramadaplaza1,R.drawable.ramadaplaza2};
        else
        if(n.equals("Nar Sarisu Apart"))
            s=new int[]{R.drawable.narsarisu1,R.drawable.narsarisuroom};
        else
        if(n.equals("Hayal Residence Apart"))
            s=new int[]{R.drawable.hayallivingroom,R.drawable.hayalroom};
        else
        if(n.equals("Rixos Downtown Antalya"))
            s=new int[]{R.drawable.rixosdowntownantalya,R.drawable.rixosroom,R.drawable.rixosroom2};
        else
        if(n.equals("Yazar Lara Hotel"))
            s=new int[]{R.drawable.yazar2,R.drawable.yazarroom2};
        else
        if(n.equals("The Marmara"))
            s=new int[]{R.drawable.marmar1,R.drawable.marmar2,R.drawable.marmar3};
        else
        if(n.equals("Hotel Su"))
            s=new int[]{R.drawable.su1,R.drawable.su2};
        else
        if(n.equals("Delphin BE Grand Resort"))
            s=new int[]{R.drawable.delphin1,R.drawable.delphin2};
        else
        if(n.equals("Alp Pasa Old Town"))
            s=new int[]{R.drawable.alp1,R.drawable.alp2};
        else
        if(n.equals("Prime Boutique Hotel"))
            s=new int[]{R.drawable.prime1,R.drawable.prime2};
        else
        if(n.equals("White Garden"))
            s=new int[]{R.drawable.whitegarden1,R.drawable.whitegarden2};
        return s;
    }

    ArrayList<Integer>selectusedAndSize(String n)
    {
        SQLiteDatabase rdb =  this.getReadableDatabase();
        String s1 = "select used from hotel where name = \"" + n + "\";";
        String s2 = "select size from hotel where name = \"" + n + "\";";
        Cursor c1 = rdb.rawQuery(s1,null);
        Cursor c2 = rdb.rawQuery(s2,null);
        ArrayList<Integer>list= new ArrayList<Integer>();
        if(c1.moveToFirst())
        {
        list.add(c1.getInt(c1.getColumnIndex("used")));
    }
        if(c2.moveToFirst())
        {
            list.add(c2.getInt(c2.getColumnIndex("size")));
        }

        return list;
    }

    void updateUsed(String n)
    {
        SQLiteDatabase wdb =  this.getWritableDatabase();
        String s1 = "update hotel set used = used"+"+"+1+" where name=\"" + n + "\";";
        wdb.execSQL(s1);
    }

    void removeUsed(String n)
    {
        SQLiteDatabase wdb =  this.getWritableDatabase();
        String s1 = "update hotel set used = used"+"-"+1+" where name=\"" + n + "\";";
        wdb.execSQL(s1);
    }
}



