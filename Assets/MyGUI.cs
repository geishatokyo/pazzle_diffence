using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour {
	public GameObject cube;
    public void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 100, 50), "GENERATE"))
        {
            float x = Random.Range(-6, 6);
            float y = 6;
            float z = 0;
            
            Instantiate(this.cube, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}
