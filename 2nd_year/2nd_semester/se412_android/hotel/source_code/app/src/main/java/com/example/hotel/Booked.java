//A class that contains the user and his booked hotels (stored in a db)
package com.example.hotel;

public class Booked {
    private String user;
    private String hotel;
    private String start;
    private String stop;
    private int done;

    public Booked(String a, String b, String c, String d, int e) {
        super();
        user=a;
        hotel=b;
        start=c;
        stop=d;
        done=e;
    }

    public Booked(String a, String b) {
        super();
        user=a;
        hotel=b;
    }

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

    public String getHotel() {
        return hotel;
    }

    public void setHotel(String hotel) {
        this.hotel = hotel;
    }

    public String getStart() {
        return start;
    }


    public String getStop() {
        return stop;
    }


    public int getDone() {
        return done;
    }
}


