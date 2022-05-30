//This class interact with the user db
package com.example.hotel;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class Ops extends SQLiteOpenHelper {


        public Ops(Context context) {
                super(context, "user.db", null, 1);

            }



            @Override
            public void onCreate(SQLiteDatabase arg0) {

                String q="create table user (mail text,user text primary key,password text);";
                arg0.execSQL(q);
            }

            @Override
            public void onUpgrade(SQLiteDatabase arg0, int arg1, int arg2) {
                String q="drop table user;";
                arg0.execSQL(q);
                onCreate(arg0);
            }


            void addUser(user p){
                String myQuery="insert into user values (\""+p.getMail()+"\",\""+p.getName()+"\", \""+p.getPass()+"\")";
                SQLiteDatabase db=getWritableDatabase();
                db.execSQL(myQuery);
            }

        user searchUser(String n){
            String q="select * from user where user=\""+n+"\";";
            SQLiteDatabase db1=getReadableDatabase();
            Cursor c=db1.rawQuery(q, null);
            user p=new user();
            if(c.moveToFirst()){
                p.setMail(c.getString(0));
                p.setName(c.getString(1));
                p.setPass(c.getString(2));
            }
            return p;
        }

        void deleteUser(String p){
            String qq="delete from user where user=\""+p+"\";";
            SQLiteDatabase myDB=getWritableDatabase();
            myDB.execSQL(qq);

        }
    }



