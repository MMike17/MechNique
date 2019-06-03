using UnityEngine;
using UnityEngine.UI;

public class AdjustsColor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Color full;
    [SerializeField] Color empty;

    [Header("Assing in Inspector")]
    [SerializeField] Image aim;
    [SerializeField] Image hp,hp_fill;
    [SerializeField] Slider overheat_left,overheat_right;

    [Header("Test")]
    [Range(1,0)]
    [SerializeField] float percentile_test;

    void OnDrawGizmos ()
    {
        SetColor(percentile_test);
    }

    /// <summary>Set's UI color depending on left health</summary>
    /// <param name="percentile">percentile of heath left</param>
    public void SetColor (float percentile)
    {
        HSVColor full_hsv=new HSVColor(),empty_hsv=new HSVColor(),displacement=new HSVColor();

        Color.RGBToHSV(full,out full_hsv.h,out full_hsv.s,out full_hsv.v);
        Color.RGBToHSV(empty,out empty_hsv.h,out empty_hsv.s,out empty_hsv.v);

        HSVColor difference=new HSVColor(empty_hsv.h+((full_hsv.h-empty_hsv.h)*percentile),empty_hsv.s+((full_hsv.s-empty_hsv.s)*percentile),empty_hsv.v+((full_hsv.v-empty_hsv.v)*percentile));

        SetImageColor(aim,difference);
        SetImageColor(hp,difference);
        SetImageColor(hp_fill,difference);
        
        foreach(Image image in overheat_left.GetComponentsInChildren<Image>())
            SetImageColor(image,difference);

        foreach(Image image in overheat_right.GetComponentsInChildren<Image>())
            SetImageColor(image,difference);
    }

    void SetImageColor (Image ui,HSVColor displacement)
    {
        Color temp=ui.color;
        temp=displacement.ToRGB();
        temp.a=ui.color.a;
        ui.color=temp;
    }
}

public class HSVColor
{
    public float h,s,v;

    public HSVColor ()
    {
        h=0;
        s=0;
        v=0;
    }

    public HSVColor (float h,float s, float v)
    {
        this.h=h;
        this.s=s;
        this.v=v;
    }

    public static HSVColor operator +(HSVColor a,HSVColor b)
    {
        return new HSVColor(a.h+b.h,a.s+b.s,a.v+b.v);
    }

    public Color ToRGB ()
    {
        return Color.HSVToRGB(h,s,v);
    }
}