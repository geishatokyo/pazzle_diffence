using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	bool dragging = false;
	Vector3 dragStart;
	Vector3 thisPosition;
	
	Vector3 MousePositionOnXY{
		get{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			return ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		var vec = Input.mousePosition;
		vec.z = 0;
		var worldPosition = Camera.main.ScreenToWorldPoint(vec);
		if (Input.GetMouseButtonDown(0)) Debug.Log(vec);
		
		
		if(Input.GetMouseButtonDown(0) && !dragging && vec.y >=353){
			dragging = true;
			dragStart = MousePositionOnXY;
			thisPosition = transform.localPosition;
		}else if(dragging && Input.GetMouseButtonUp(0)){
			dragging = false;
		}
		
		if(dragging){
			var delta = MousePositionOnXY - dragStart;
			delta = new Vector3(delta.x,0,0);
			this.transform.localPosition = thisPosition+ delta;
		}
		
		
	
	}
}
