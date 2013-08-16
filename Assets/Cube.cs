using UnityEngine;
using System.Collections;
using System;



public class Cube : Board {
	
	
	public int CubeIndex{get;set;}
	
	int row = 0;
	public int Row{
		get{
			return row;
		}
		set{
			this.row = value;
			var pos = transform.localPosition;
			transform.localPosition = new Vector3(pos.x,dropSize * value,pos.z);
		}
	}
	int column = 0;
	public int Column{
	
		get{
			return column;
		}
		set{
			this.column = value;
			var pos = transform.localPosition;
			transform.localPosition = new Vector3(dropSize * value,pos.y,pos.z);
			
		}
	}
	public DropColor DropColor{get;set;}
	bool dragging;
	public bool Dragging{get;set;}
	
	public int dropSize = 2;
	
	
	
	// Use this for initialization
	void Start () {
		rigidbody.isKinematic = true;
		if(DropColor == DropColor.Red)renderer.material.color = Color.red;
		if(DropColor == DropColor.Blue)renderer.material.color = Color.blue;
		if(DropColor == DropColor.Yellow)renderer.material.color = Color.yellow;
		if(DropColor == DropColor.Green)renderer.material.color = Color.green;
		if(DropColor == DropColor.White)renderer.material.color = Color.white;
	}
	
	private void Update(){
		if(dragging){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				transform.position = ray.origin - ray.direction *(ray.origin.z / ray.direction.z+0.3f);
		}
	}

	void OnMouseDown(){
		Debug.Log("MouseDown!!!");
		dragging = true;
		collider.isTrigger = true;
	}
	
	void OnMouseUp(){
		dragging = false;
		collider.isTrigger = false;
		/*transform.localPosition = new Vector3(
			(float)(Math.Round(transform.localPosition.x/2))*2, 
			(float)(Math.Round(transform.localPosition.y/2))*2, 
			0.0f); */
		transform.localPosition = new Vector3(column*2, row*2, 0.0f);
		Debug.Log(transform.position);
	}
}