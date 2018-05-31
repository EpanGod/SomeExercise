namespace SytUI
{
    using UnityEngine;
    using System.Collections;

    public class UIbind : MonoBehaviour
    {

        static bool isBind = false;

        public static void Bind()
        {
            if(!isBind)
            {
                isBind = true;

                UIbase.delegateSyncLoadUI = Resources.Load;

            }
        }
    }
}
