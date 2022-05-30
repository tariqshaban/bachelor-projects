//An activity that shows all the hotels in a custom gridView which is retrieved from a db
package com.example.hotel;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.method.ScrollingMovementMethod;
import android.view.MenuItem;
import android.view.View;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.GridView;
import android.widget.ImageButton;
import android.widget.PopupMenu;
import android.widget.Scroller;
import android.widget.TextView;
import android.widget.Toast;

public class grid extends AppCompatActivity implements PopupMenu.OnMenuItemClickListener, NavigationView.OnNavigationItemSelectedListener {

    //Defining necessary variables
    ArrayAdapter<String> p;
    GridView grid;
    private DrawerLayout drawer;
    SharedPreferences auto,gridstat;
    TextView textdream;
    int[] colors={Color.RED,Color.CYAN,Color.parseColor("#207245"),Color.parseColor("#ADFF2F"),
            Color.parseColor("#0040FF")};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView( R.layout.activity_grid);
        //Sets a fade-in animation for the textView and the gridView
        textdream=findViewById(R.id.choosedreamhotel);
        textdream.animate().alpha(1f).setDuration(3000);
        grid=findViewById( R.id.grid);
        grid.animate().alpha(1f).setDuration(3000);

        //A shared preferences that retrieves the background and text color
        auto=getSharedPreferences("UI",MODE_PRIVATE);
        int c=auto.getInt("Color",Color.parseColor("#333333"));
        getWindow().getDecorView().setBackgroundColor(c);
        int c2=auto.getInt("Text",4);
        textdream.setTextColor(colors[c2]);

        //A shared preferences that retrieves the number of columns of the gridView
        gridstat=getSharedPreferences("grid",MODE_PRIVATE);

        //Sets up the navigation drawer
        Toolbar toolbar=findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        drawer=findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle=new ActionBarDrawerToggle(this,drawer,toolbar,R.string.navigation_drawer_open,R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();
        NavigationView nav =findViewById(R.id.nav_view);
        nav.setBackgroundColor(c);
        nav.setItemTextColor( ColorStateList.valueOf(colors[c2]));
        nav.setItemIconTintList(ColorStateList.valueOf(colors[c2]));
        nav.setNavigationItemSelectedListener(this);

        //Sets up the imageButtons
        ImageButton more=findViewById(R.id.imageButton);
        more.setImageResource(R.drawable.ic_more_vert_black_24dp);
        more.setBackgroundColor(0);
        ImageButton more2=findViewById(R.id.imageButton2);
        //Changes the imageButton accordingly with the gridView's number of columns
        if(gridstat.getBoolean("grid",false)==false) {
            more2.setImageResource( R.drawable.ic_grid_on_black_24dp );
            grid.setNumColumns(1);
        }
        else {
            more2.setImageResource( R.drawable.ic_menu_black_24dp );
            grid.setNumColumns(2);
        }
        more2.setBackgroundColor(0);

        //Retrieves the hotel db
        Ops1 q = new Ops1(this);
        final String hotelsnames[]=q.names();
        q.close();

        //Custom gridView setup
        p= new pichotel(this, R.layout.activity_pichotel,hotelsnames);
        grid.setAdapter(p);
        grid.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Intent i = new Intent(getApplicationContext(),hotelsinfo.class);
                i.putExtra("hotelname",hotelsnames[position]);
                startActivity(i);
            }
        });


    }

    //The 'more' imageButton listener
    public void onClick(View v) {
        PopupMenu menu=new PopupMenu(getApplicationContext(),v);
        menu.setOnMenuItemClickListener(this);
        menu.inflate(R.menu.more);
        menu.show();}

    //The 'grid' imageButton listener
    public void onClick2(View v) {
        ImageButton more2=findViewById(R.id.imageButton2);
        if(gridstat.getBoolean("grid",false)==false) {
            more2.setImageResource( R.drawable.ic_menu_black_24dp);
            gridstat.edit().putBoolean("grid",true).apply();
            grid.setNumColumns(2);
        }
        else {
            more2.setImageResource( R.drawable.ic_grid_on_black_24dp );
            gridstat.edit().putBoolean("grid",false).apply();
            grid.setNumColumns(1);
        }}

    //The 'more' imageButton ITEMS listener
    @Override
    public boolean onMenuItemClick(MenuItem menuItem) {
        SharedPreferences auto=getSharedPreferences("Auto",MODE_PRIVATE);
        switch(menuItem.getItemId())
        {case R.id.Logout:
            //Logs out by setting the shared preferences to null
            SharedPreferences.Editor edit=auto.edit();
            edit.putString("User",null);
            edit.putString("Pass",null);
            edit.apply();
            Intent i=new Intent(getApplicationContext(),Intro.class);
            startActivity(i);
            return true;
        case R.id.Delete:
            //Deletes the user by using user db, won't delete his reservations though -for storage purposes
            Del a=new Del();
            a.show(getSupportFragmentManager(),"lel");
            return true;
            default:
                return false;
        }

    }

    //Closes the drawer when the back button is pressed
    @Override
    public void onBackPressed() {
        if (drawer.isDrawerOpen( GravityCompat.START))
            drawer.closeDrawer(GravityCompat.START);
        else
        super.onBackPressed();
    }

    //Handles the navigation drawer items click
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
        switch(menuItem.getItemId()) {
            case R.id.theme:
            //Shows the change theme dialog
                Theme w=new Theme();
                w.show(getSupportFragmentManager(),"lel");
                break;
            case R.id.color:
            //Shows the change text color dialog
                Colour v=new Colour();
                v.show(getSupportFragmentManager(),"lel");
                break;
            case R.id.help:
            //Shows the help dialog, scrollable
                AlertDialog dialog = new AlertDialog.Builder(this)
                        .setTitle(getString(R.string.help))
                        .setMessage("temp")
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        })
                        .setIcon(android.R.drawable.ic_menu_help)
                        .show();
                Window window = dialog.getWindow();
                window.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000);
                TextView textView =dialog.findViewById(android.R.id.message);
                textView.setText("This\n\n\n\n\n\nIs\n\n\n\n\n\nHelp");
                textView.setScroller(new Scroller(this));
                textView.setVerticalScrollBarEnabled(true);
                textView.setMovementMethod(new ScrollingMovementMethod());
                break;
            case R.id.about:
            //Shows the about dialog, scrollable
                AlertDialog dialog1 = new AlertDialog.Builder(this)
                        .setTitle(getString(R.string.about))
                        .setMessage("temp")
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        })
                        .setIcon(android.R.drawable.ic_menu_info_details)
                        .show();
                TextView textView1 =dialog1.findViewById(android.R.id.message);
                Window window1 = dialog1.getWindow();
                window1.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000);
                textView1.setText("This\n\n\n\n\n\nIs\n\n\n\n\n\nabout");
                textView1.setScroller(new Scroller(this));
                textView1.setVerticalScrollBarEnabled(true);
                textView1.setMovementMethod(new ScrollingMovementMethod());
                break;

        }
        //Closes the drawer when an item is selected
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }
}
