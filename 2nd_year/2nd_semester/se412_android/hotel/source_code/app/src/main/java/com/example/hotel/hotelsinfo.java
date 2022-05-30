//A long, painful activity that shows the hotel's characteristics and handles booking
package com.example.hotel;

import android.app.DatePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.design.widget.Snackbar;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.DialogFragment;
import android.support.v4.view.GravityCompat;
import android.support.v4.view.ViewPager;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.text.method.ScrollingMovementMethod;
import android.view.MenuItem;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.PopupMenu;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Scroller;
import android.widget.TextView;
import android.widget.Toast;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import java.util.ArrayList;
import java.util.Calendar;

public class hotelsinfo extends AppCompatActivity  implements PopupMenu.OnMenuItemClickListener, NavigationView.OnNavigationItemSelectedListener, OnMapReadyCallback,DatePickerDialog.OnDateSetListener {
        //Defining necessary variables
        final int[] colors = {Color.RED, Color.CYAN, Color.parseColor( "#207245" ), Color.parseColor( "#ADFF2F" ), Color.parseColor( "#0040FF" )};
        private DrawerLayout drawer;
        SharedPreferences auto;
        Button book;
        TextView name, address, price, rate,capacity;
        GoogleMap map;
        ViewPager viewPager;
        hotel a;
        int mode=0;
        Ops2 f1;
        Ops1 ops1;
        AlertDialog dialog;
        TextView app;
        TextInputLayout days;
        String gleft="";


    @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate( savedInstanceState );
            setContentView( R.layout.activity_hotelsinfo );

            //A shared preferences that retrieves the background and text color
            auto = getSharedPreferences( "UI", MODE_PRIVATE );
            final int c = auto.getInt( "Color", Color.parseColor( "#333333" ) );
            getWindow().getDecorView().setBackgroundColor( c );
            final int c2 = auto.getInt( "Text", 4 );

            //Sets up the imageButtons
            ImageButton more = findViewById( R.id.imageButton );
            more.setImageResource( R.drawable.ic_more_vert_black_24dp );
            more.setBackgroundColor( 0 );
            ImageButton more2 = findViewById( R.id.imageButton2 );
            more2.setImageResource( R.drawable.ic_map_black_24dp );
            more2.setBackgroundColor( 0 );
            ImageButton more3 = findViewById( R.id.imageButton3 );
            more3.setImageResource( R.drawable.ic_unfold_more_black_24dp );
            more3.setBackgroundColor( 0 );

            //Sets the after booking report
            app=findViewById(R.id.textView);

            //Sets up the navigation drawer
            Toolbar toolbar = findViewById( R.id.toolbar );
            setSupportActionBar( toolbar );
            drawer = findViewById( R.id.drawer_layout );
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle( this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close );
            drawer.addDrawerListener( toggle );
            toggle.syncState();
            NavigationView nav = findViewById( R.id.nav_view );
            nav.setBackgroundColor( c );
            nav.setItemTextColor( ColorStateList.valueOf( colors[c2] ) );
            nav.setItemIconTintList( ColorStateList.valueOf( colors[c2] ) );
            nav.setNavigationItemSelectedListener( this );

            //Bond xml views with java
            book = findViewById( R.id.book );
            name = findViewById( R.id.name );
            address = findViewById( R.id.address );
            price = findViewById( R.id.price );
            rate = findViewById( R.id.rate );
            capacity=findViewById(R.id.textView2);
            name.setTextColor( colors[c2] );
            address.setTextColor( colors[c2] );
            price.setTextColor( colors[c2] );
            rate.setTextColor( colors[c2] );
            capacity.setTextColor( colors[c2] );
            app.setTextColor(colors[c2]);

            //Retrieves the hotel name from an intent
            Intent i=getIntent();
            final String hotelname=i.getStringExtra("hotelname");
            a=new hotel(hotelname);
            //Calls the hotel db
            Ops1 f=new Ops1(this);
            //Calls the booked db
            f1=new Ops2(this);
            f.info(hotelname,a);
            f.close();
            //f1 will be closed later

            //Retrieved data from the db is set here
            name.setText(hotelname);
            address.setText(a.getAddress());
            rate.setText(a.getRate()+" "+getString(R.string.stars));
            price.setText(a.getPrice()+" "+getString(R.string.JD));
            capacity.setText(a.getUsed()+"/"+a.getSize());

            //Defined the empty fields dialog
            final Fill d = new Fill();

            //Redefined the hotel db
            ops1= new Ops1(this);

            //Retrieved the username
            auto = getSharedPreferences( "Auto", MODE_PRIVATE );
            //Retrieves the users-booked hotels, if found
            Booked user=new Booked(auto.getString( "User", "Error!" ),i.getStringExtra("hotelname"));
            //Defines the Booked db
            Ops2 f2=new Ops2(this);
            //Checked whether the user has booked in that hotel or not
            boolean check=f2.Exist(user);
            //closes the db
            f2.close();

            //If the user did book in that hotel the activity loads the after book report from the Booked db
            if (check) {
                String date;
                SharedPreferences auto = getSharedPreferences( "Auto", MODE_PRIVATE );
                Booked book1 = new Booked( auto.getString( "User", "Error!" ), a.getName() );
                date = f1.returnDate( book1 );
                Calendar cal = Calendar.getInstance();
                int year;
                int month;
                int dayOfMonth;
                String[] separated = date.split( "/" );
                dayOfMonth = Integer.parseInt( separated[0] );
                month = Integer.parseInt( separated[1] );
                year = Integer.parseInt( separated[2] );
                int done = f1.returnDone( auto.getString( "User", "Error!" ), a.getName() );
                done-=30;
                double y, m, day;
                double addition = year * 365 + (month+1) * 30 + dayOfMonth;
                y = Math.floor( (done + addition) / 365 );
                m = Math.floor( ((done + addition) - 365 * y) / 30 );
                day = Math.floor( (done + addition) - 365 * y - 30 * m );
                String end = (int) day + "/" + (int) m + "/" + (int) y;
                String start = dayOfMonth + "/" + month + "/" + year;
                int minus = 0;
                minus = (cal.get( Calendar.YEAR ) - year) * 365 + (cal.get( Calendar.MONTH ) - month) * 30 + cal.get( Calendar.DAY_OF_MONTH ) - dayOfMonth;

                    if (cal.get( Calendar.YEAR ) > year || cal.get( Calendar.YEAR ) == year && cal.get( Calendar.MONTH ) > month || cal.get( Calendar.YEAR ) == year && cal.get( Calendar.MONTH ) == month && cal.get( Calendar.DAY_OF_MONTH ) > dayOfMonth)
                        minus = (cal.get( Calendar.YEAR ) - year) * 365 + (cal.get( Calendar.MONTH ) - month) * 30 + cal.get( Calendar.DAY_OF_MONTH ) - dayOfMonth;
                    String left;
                    if (minus >= done) {
                        f1.removeBooked( auto.getString( "User", "Error!" ), a.getName() );
                        left = getString(R.string.end);
                    }
                    else {
                        //Sets the book button text to 'Cancel Reserve', If the language was english
                        book.setText(getString(R.string.creserve));
                        left = done - minus +" "+getString(R.string.startend)+"\n"+getString(R.string.start_end)+" "+ start +"\n"+getString(R.string.endstart)+" "+ end;
                    }
                    gleft = left;
                    app.setText( gleft );
                    f1.close();
                }




            //Handles the book button
            book.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    //Checks if the user has already booked in that hotel
                    if(book.getText().toString().equals(getString(R.string.creserve))) {
                        SharedPreferences confirm = getSharedPreferences( "Confirm", MODE_PRIVATE );
                        SharedPreferences.Editor adder=confirm.edit();
                        adder.putString("Hotel",a.getName());
                        adder.apply();
                        //Shows the 'unbook' dialog
                        Unbook s = new Unbook();
                        s.show( getSupportFragmentManager(), "lel" );
                    }
                    else
                    //Proceeds to booking
                    {
                    //Shows the book dialog
                    AlertDialog.Builder b = new AlertDialog.Builder(hotelsinfo.this);
                    View view=getLayoutInflater().inflate(R.layout.layout_checkin_items,null);
                    //Binds the view elements
                    final TextInputLayout cardNumber = view.findViewById(R.id.cardNumber);
                    days = view.findViewById(R.id.daysofstay);
                    final EditText cardNumberc = view.findViewById(R.id.cardNumberc);
                    final EditText daysc = view.findViewById(R.id.daysofstayc);
                    final RadioButton visa = view.findViewById(R.id.visa);
                    final RadioButton master = view.findViewById(R.id.master);
                    final RadioButton paypal = view.findViewById(R.id.paypal);
                    final RadioButton suite = view.findViewById(R.id.high);
                    final RadioButton standard = view.findViewById(R.id.mid);
                    final RadioButton economical = view.findViewById(R.id.low);
                    final RadioGroup span = view.findViewById(R.id.span);
                    final TextView total = view.findViewById(R.id.result);
                    //Sets the text color based on the text color shared preferences
                    cardNumberc.setTextColor(colors[c2]);
                    daysc.setTextColor(colors[c2]);
                    Button ok = view.findViewById(R.id.ok);
                    Button cancel = view.findViewById(R.id.cancel);
                    b.setView(view);
                    dialog= b.create();
                    dialog.show();
                    cancel.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            dialog.dismiss();
                        }
                    });

                    daysc.addTextChangedListener( new TextWatcher() {
                        @Override
                        public void beforeTextChanged(CharSequence s, int start, int count, int after) {
                        }

                        @Override
                        public void onTextChanged(CharSequence s, int start, int before, int count) {
                            //Makes bill calculations real-time, while typing in the days AND selected an offer
                            if(suite.isChecked()||standard.isChecked()||economical.isChecked()) {
                                Intent i = getIntent();
                                final String hotelname = i.getStringExtra("hotelname");
                                a = new hotel(hotelname);
                                Ops1 f = new Ops1(getApplicationContext());
                                f.info(hotelname, a);
                                f.close();
                                if (days.getEditText().getText().toString().isEmpty())
                                    total.setText("");
                                else if (Integer.parseInt(days.getEditText().getText().toString()) > 3650) {
                                    total.setText(getString(R.string.max));
                                    total.setTextColor(Color.RED);
                                } else {
                                    int num = Integer.parseInt(days.getEditText().getText().toString());
                                    double bonus = 1;
                                    if (suite.isChecked())
                                        bonus = 1.5;
                                    else if (economical.isChecked())
                                        bonus = 0.7;
                                    total.setText((int) (num * a.getPrice() * bonus) +" "+ getString(R.string.JD));
                                    total.setTextColor(Color.BLACK);
                                }
                            }
                        }



                        @Override
                        public void afterTextChanged(Editable s) {
                        }
                    } );

                    span.setOnCheckedChangeListener( new RadioGroup.OnCheckedChangeListener() {
                        //Makes bill calculations real-time, while typing in the days AND selected an offer
                        @Override
                        public void onCheckedChanged(RadioGroup group, int checkedId) {
                            Intent i=getIntent();
                            final String hotelname=i.getStringExtra("hotelname");
                            a=new hotel(hotelname);
                            Ops1 f=new Ops1(getApplicationContext());
                            f.info(hotelname,a);
                            f.close();
                            if(days.getEditText().getText().toString().isEmpty())
                                total.setText("");
                            else
                            if(Integer.parseInt(days.getEditText().getText().toString())>3650)
                            {
                                total.setText(getString(R.string.max));
                                total.setTextColor(Color.RED);
                            }
                            else {
                                int num=Integer.parseInt( days.getEditText().getText().toString() );
                                double bonus=1;
                                if(suite.isChecked())
                                    bonus=1.5;
                                else if(economical.isChecked())
                                    bonus=0.7;
                                total.setText( (int)(num*a.getPrice()*bonus) +" JD" );
                            }
                        }
                    } );

                    ok.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            //Validations
                            if(cardNumber.getEditText().getText().toString().isEmpty()||
                                    days.getEditText().getText().toString().isEmpty()||!visa.isChecked()&&!master.isChecked()&&!paypal.isChecked()||!suite.isChecked()&&!standard.isChecked()&&!economical.isChecked())
                            {
                                //Shows fill dialog
                                d.show(getSupportFragmentManager(),"lel");
                            }
                            else if(total.getText().toString().equals(getString(R.string.max)))
                                Toast.makeText(getBaseContext(),"Maximum Days Reached",Toast.LENGTH_LONG).show();
                            else {
                                //Increment the number of visitors in that hotel
                                ArrayList<Integer> list = ops1.selectusedAndSize(name.getText().toString());
                                //Checks for hotel capacity
                                if (list.get(0) >= list.get(1)) {
                                    Toast.makeText(getApplicationContext(),getString(R.string.maxc), Toast.LENGTH_LONG).show();
                                    dialog.dismiss();
                                }
                               else
                                {
                                    //Shows the calendar
                                    DialogFragment datepicker=new DatePickerFragment();
                                    datepicker.show(getSupportFragmentManager(),"lel");
                                }

                            }
                        }
                    });
                }
            }});

            //An imported picasso library that shows images in a dynamic view
            viewPager = findViewById( R.id.view_pager );
            pager adapter = new pager( this, f.innerpic(a.getName()));
            viewPager.setAdapter( adapter );
            viewPager.bringToFront();

            //Calls in the map
            SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager().findFragmentById( R.id.map );
            //Shows the map
            mapFragment.getMapAsync( this );
        }

        //The 'more' imageButton listener
        public void onClick(View v) {
            PopupMenu menu = new PopupMenu( getApplicationContext(), v );
            menu.setOnMenuItemClickListener( this );
            menu.inflate( R.menu.more );
            menu.show();
        }

        //The 'grid' imageButton listener
        public void onClick2(View v) {
            ImageButton more2=findViewById(R.id.imageButton2);
            if(mode==0) {
                more2.setImageResource( R.drawable.ic_image_black_24dp);
                findViewById(R.id.map).bringToFront();
                findViewById(R.id.imageButton3).bringToFront();
                mode=1;
            }
            else {
                more2.setImageResource( R.drawable.ic_map_black_24dp);
                viewPager.bringToFront();
                mode=0;
            }}

        //The 'expand map' imageButton listener
        public void onClick3(View v) {
            Intent i=new Intent(getApplicationContext(),Expand.class).putExtra("Height",a.getLoch()).putExtra("Width",a.getLocw());
            startActivity(i);
        }

        //The 'more' imageButton ITEMS listener
        @Override
        public boolean onMenuItemClick(MenuItem menuItem) {
            switch (menuItem.getItemId()) {
                case R.id.Logout:
                    //Logs out by setting the shared preferences to null
                    SharedPreferences auto = getSharedPreferences( "Auto", MODE_PRIVATE );
                    SharedPreferences.Editor edit = auto.edit();
                    edit.putString( "User", null );
                    edit.putString( "Pass", null );
                    edit.apply();
                    Intent i = new Intent( getApplicationContext(), Intro.class );
                    startActivity( i );
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
            if (drawer.isDrawerOpen( GravityCompat.START ))
                drawer.closeDrawer( GravityCompat.START );
            else
                super.onBackPressed();
        }

        //Handles the navigation drawer items click
        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
            NavigationView nav = findViewById( R.id.nav_view );
            switch (menuItem.getItemId()) {
                case R.id.theme:
                //Shows the change theme dialog
                    Theme w = new Theme();
                    w.show( getSupportFragmentManager(), "lel" );
                    break;
                case R.id.color:
                //Shows the change text color dialog
                    Colour v = new Colour();
                    v.show( getSupportFragmentManager(), "lel" );
                    break;
                case R.id.help:
                //Shows the help dialog, scrollable
                    AlertDialog dialog = new AlertDialog.Builder( this )
                            .setTitle(getString(R.string.help))
                            .setMessage( "temp" )
                            .setPositiveButton( android.R.string.yes, new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            } )
                            .setIcon( android.R.drawable.ic_menu_help)
                            .show();
                    Window window = dialog.getWindow();
                    window.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000 );
                    TextView textView = dialog.findViewById( android.R.id.message );
                    textView.setText( "This\n\n\n\n\n\nIs\n\n\n\n\n\nHelp" );
                    textView.setScroller( new Scroller( this ) );
                    textView.setVerticalScrollBarEnabled( true );
                    textView.setMovementMethod( new ScrollingMovementMethod() );
                    break;
                case R.id.about:
                //Shows the about dialog, scrollable
                    AlertDialog dialog1 = new AlertDialog.Builder( this )
                            .setTitle(getString(R.string.about))
                            .setMessage( "temp" )
                            .setPositiveButton( android.R.string.yes, new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            } )
                            .setIcon( android.R.drawable.ic_menu_info_details )
                            .show();
                    TextView textView1 = dialog1.findViewById( android.R.id.message );
                    Window window1 = dialog1.getWindow();
                    window1.setLayout( DrawerLayout.LayoutParams.MATCH_PARENT, 1000 );
                    textView1.setText( "This\n\n\n\n\n\nIs\n\n\n\n\n\nabout" );
                    textView1.setScroller( new Scroller( this ) );
                    textView1.setVerticalScrollBarEnabled( true );
                    textView1.setMovementMethod( new ScrollingMovementMethod() );
                    break;

            }
            //Closes the drawer when an item is selected
            drawer.closeDrawer( GravityCompat.START );
            return true;
        }

        //A function that is called as soon the map fragment is created
        @Override
        public void onMapReady(GoogleMap googleMap) {
            map = googleMap;
            //Sets the position of the hotel in the map
            LatLng Hotel = new LatLng( a.getLocw(), a.getLoch() );
            //Marks the position
            map.addMarker( new MarkerOptions().position( Hotel ).title( "Hotel" ) );
            //Changes the map zoom
            map.moveCamera( CameraUpdateFactory.newLatLngZoom( Hotel, 16 ) );
        }

    //Triggered when the user chooses a date, sets the after book report, inserts the date in the db and sets the book button to 'Cancel Reservation'
    @Override
    public void onDateSet(DatePicker view, int year, int month, int dayOfMonth) {
        Calendar cal=Calendar.getInstance();
        if(cal.get(Calendar.YEAR)>year || cal.get(Calendar.YEAR)==year && cal.get(Calendar.MONTH)>month || cal.get(Calendar.YEAR)==year && cal.get(Calendar.MONTH)==month && cal.get(Calendar.DAY_OF_MONTH)>dayOfMonth) {
            Toast.makeText(getApplicationContext(),getString(R.string.wrong), Toast.LENGTH_SHORT ).show();
        }
        else {
            SharedPreferences auto = getSharedPreferences( "Auto", MODE_PRIVATE );

            int done=Integer.parseInt(days.getEditText().getText().toString());
            double y,m,d;
            double addition=year*365+(month+1)*30+dayOfMonth;
            y=Math.floor((done+addition)/365);
            m=Math.floor(((done+addition)-365*y)/30);
            d=Math.floor((done+addition)-365*y-30*m);
            String end=(int)d+"/"+(int)m+"/"+(int)y;
            String start=dayOfMonth+"/"+(month+1)+"/"+year;
            String left=done+" "+getString(R.string.startend)+"\n"+getString(R.string.start_end)+" "+ start +"\n"+getString(R.string.endstart)+" "+ end;
            gleft=left;
            app.setText(left);
            book.setText(getString(R.string.creserve));
            Booked book = new Booked(auto.getString( "User", "Error!" ),a.getName(), start, end,done);
            f1.addBooked( book );
            f1.close();
            ops1.updateUsed( name.getText().toString() );
            dialog.dismiss();
            Snackbar.make(findViewById(R.id.drawer_layout),getString(R.string.approved),Snackbar.LENGTH_SHORT)
                    .setAction("Action",null).show();

            int low=a.getUsed()+1;
            int high=a.getSize();
            capacity.setText(low+"/"+high);
        }
    }
}
