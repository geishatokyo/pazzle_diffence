using UnityEngine;
using System.Collections;

public class DispCombo : MonoBehaviour 
{
    private static GameObject instance;
 
    private void Awake()
    {
        instance = this.gameObject;
    }
 
    public static int count = 0;
    public static int Count
    {
        get { return count; }
        set
        {
            count = value;
            instance.gameObject.guiText.text = count.ToString() + " COMBO!!!";
        }
    }
}
