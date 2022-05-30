//A class that contains the user and his password (stored in a db)
package com.example.hotel;

public class user {
    private String mail;
    private String name;
    private String pass;

    public user() {
        super();
    }

    public user(String mail,String name, String pass) {
        super();
        this.mail=mail;
        this.name = name;
        this.pass = pass;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPass() {
        return pass;
    }

    public void setPass(String pass) {
        this.pass = pass;
    }

    public String getMail() {
        return mail;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }
}

