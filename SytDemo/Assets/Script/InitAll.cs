using UnityEngine;
using System.Collections;

public static class InitAll 
{
    public static void Init()
    {
        IniManager.instance.enabled = true;
        UIManager.instance.enabled = true;
        ModelManager.instance.enabled = true;
        AnimationManager.instance.enabled = true;
    }

}
