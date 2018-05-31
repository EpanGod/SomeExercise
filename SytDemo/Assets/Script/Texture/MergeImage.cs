using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MergeImage : MonoSingleton<MergeImage>
{
    public override void Init()
    {
        base.Init();
    }

    /// <summary>  
    /// 多张Texture2D合成一张Texture2D  
    /// </summary>  
    /// <param name="tex">Texture2D图片数组</param>  
    /// <returns></returns>  
    public Texture2D Merge(Texture2D[] tex)
    {

        if (tex.Length == 0) return null;
        //定义新图的宽高  
        int width = 0, height = 0;

        for (int i = 0; i < tex.Length; i++)
        {
            //Debug.Log(tex[i].ToString());  
            //新图的宽度  
            width += tex[i].width;
            if (i > 0)
            {
                //新图的高度，这里筛选为最高  
                if (tex[i].height > tex[i - 1].height)
                    height = tex[i].height;
            }
            else height = tex[i].height; //只有一张图  
        }

        //初始Texture2D  
        Texture2D texture2D = new Texture2D(width, height);

        int x = 0, y = 0;
        for (int i = 0; i < tex.Length; i++)
        {
            //取图  
            Color32[] color = tex[i].GetPixels32(0);

            //赋给新图  
            if (i > 0) texture2D.SetPixels32(x += tex[i - 1].width, y, tex[i].width, tex[i].height, color);
            else texture2D.SetPixels32(x, y, tex[i].width, tex[i].height, color);
        }

        //应用  
        texture2D.Apply();


        return texture2D;
    }
    public Texture2D Merge2Row(Texture2D[] tex)
    {

        if (tex.Length == 0) return null;
        if (tex.Length % 2 != 0)
        {
            Debug.LogError("Merge2Row 方法参数需为2倍数图片数组！");
            return null;
        }
        //定义新图的宽高  
        int width = tex.Length * 256, height = 1024;


        //初始Texture2D  
        Texture2D texture2D = new Texture2D(width, height);

        int x = 0, x2 = 0, y = 0;
        for (int i = 0; i < tex.Length; i++)
        {
            //取图  
            Color32[] color = tex[i].GetPixels32(0);
            if (i < tex.Length * 0.5f)
            {
                //赋给新图  
                if (i > 0) texture2D.SetPixels32(x += 512, y, tex[i].width, tex[i].height, color);
                else texture2D.SetPixels32(x, y, tex[i].width, tex[i].height, color);
            }
            else
            {
                //赋给新图  
                if (i > tex.Length * 0.5f) texture2D.SetPixels32(x2 += 512, 512, tex[i].width, tex[i].height, color);
                else texture2D.SetPixels32(x2, 512, tex[i].width, tex[i].height, color);
            }
        }

        //应用  
        texture2D.Apply();


        return texture2D;
    }
}
