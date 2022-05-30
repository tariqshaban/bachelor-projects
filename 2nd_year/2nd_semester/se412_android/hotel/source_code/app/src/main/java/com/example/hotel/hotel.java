//A class that contains the hotel and it's information (stored in a db)
package com.example.hotel;

public class hotel {
    private String name;
    private int pic;
    private int rate;
    private String address;
    private int price;
    private Double locw;
    private Double loch;
    private int size;
    private int used;

    public hotel() {
        super();
    }

    public hotel(String a,int b,int c,String d,int e,Double f,Double g,int h,int i) {
        super();
        name=a;
        pic=b;
        rate=c;
        address=d;
        price=e;
        locw=f;
        loch=g;
        used=h;
        size=i;

    }
    public hotel(String a) {
        super();
        name=a;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getPic() {
        return pic;
    }

    public void setPic(int pic) {
        this.pic = pic;
    }

    public int getRate() {
        return rate;
    }

    public void setRate(int rate) {
        this.rate = rate;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public int getPrice() {
        return price;
    }

    public void setPrice(int price) {
        this.price = price;
    }

    public Double getLocw() {
        return locw;
    }

    public void setLocw(Double locw) {
        this.locw = locw;
    }

    public Double getLoch() {
        return loch;
    }

    public void setLoch(Double loch) {
        this.loch = loch;
    }

    public int getSize() {
        return size;
    }

    public void setSize(int size) {
        this.size = size;
    }

    public int getUsed() {
        return used;
    }

    public void setUsed(int used) {
        this.used = used;
    }
}


