//This class interact with the booked db
package com.example.hotel;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class Ops2 extends SQLiteOpenHelper {


    public Ops2(Context context) {
        super( context, "Booked.db", null, 1 );
    }

    @Override
    public void onCreate(SQLiteDatabase arg0) {

        String q = "create table Booked (user text,hotel text,start text,stop text,done integer, primary key(user, hotel));";
        arg0.execSQL( q );
    }

    @Override
    public void onUpgrade(SQLiteDatabase arg0, int arg1, int arg2) {
        String q = "drop table Booked;";
        arg0.execSQL( q );
        onCreate( arg0 );
    }

    void addBooked(Booked p){
        String myQuery="insert into Booked values (\""+p.getUser()+"\",\""+p.getHotel()+"\", \""+p.getStart()+"\",\""+p.getStop()+"\",\""+p.getDone()+"\");";
        SQLiteDatabase db=getWritableDatabase();
        db.execSQL(myQuery);
    }

    boolean Exist(Booked p)
    {
        String q="select * from Booked where user=\""+p.getUser()+"\" and hotel=\""+p.getHotel()+"\";";
        SQLiteDatabase db1=getReadableDatabase();
        Cursor c=db1.rawQuery(q, null);
        return c.moveToFirst();
    }

    String returnDate(Booked p)
    {
        String q="select * from Booked where user=\""+p.getUser()+"\" and hotel=\""+p.getHotel()+"\";";
        SQLiteDatabase db1=getReadableDatabase();
        Cursor c=db1.rawQuery(q, null);
        c.moveToFirst();
        return c.getString(2);
    }

    void removeBooked(String a,String b){
        String myQuery="delete from Booked where user=\""+a+"\" and hotel=\""+b+"\";";
        SQLiteDatabase db=getWritableDatabase();
        db.execSQL(myQuery);
    }

    int returnDone(String a,String b){
        String q="select * from Booked where user=\""+a+"\" and hotel=\""+b+"\";";
        SQLiteDatabase db1=getReadableDatabase();
        Cursor c=db1.rawQuery(q, null);
        c.moveToFirst();
        return c.getInt(4);
    }

}



