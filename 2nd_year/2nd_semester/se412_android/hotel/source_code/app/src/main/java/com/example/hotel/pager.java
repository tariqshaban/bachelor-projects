//Picasso class that handles images and offers a dynamic imageViews
package com.example.hotel;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v4.view.PagerAdapter;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;

import com.squareup.picasso.Picasso;

public class pager extends PagerAdapter {
    private Context context;
    private int[] pics;

    pager(Context context,int[] pics)
    {
        this.context=context;
        this.pics=pics;
    }

    @Override
    public int getCount() {
        return pics.length;
    }

    @Override
    public boolean isViewFromObject(@NonNull View view, @NonNull Object o) {
        return view==o;
    }

    @NonNull
    @Override
    public Object instantiateItem(@NonNull ViewGroup container, int position) {
        ImageView img=new ImageView(context);
        Picasso.get().load(pics[position]).fit().centerCrop().into(img);
        container.addView(img);
        return img;
    }

    @Override
    public void destroyItem(@NonNull ViewGroup container, int position, @NonNull Object object) {
        container.removeView((View)object);
    }
}
