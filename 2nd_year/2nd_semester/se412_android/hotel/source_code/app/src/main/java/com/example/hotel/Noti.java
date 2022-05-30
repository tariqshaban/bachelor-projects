//This class handles the notification chanel when the user register
package com.example.hotel;

import android.app.Application;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.os.Build;

public class Noti extends Application {
    public static final String ChannelID="Channel1";
    @Override
    public void onCreate() {
        super.onCreate();
        createNoti();
    }
    private void createNoti()
    {
        //Checks for the android version
        if(Build.VERSION.SDK_INT>=Build.VERSION_CODES.O)
        {
            NotificationChannel Channel1=new NotificationChannel(
                    ChannelID,"Channel 1", NotificationManager.IMPORTANCE_HIGH
            );
            Channel1.setDescription("Main Channel");
            NotificationManager manager=getSystemService(NotificationManager.class);
            manager.createNotificationChannel(Channel1);
        }
    }
}
