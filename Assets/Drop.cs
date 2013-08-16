using UnityEngine;
using System.Collections;

public enum DropColor{
	Red = 1,
	Blue,
	Yellow,
	Green,
	White
}

public class Drop : Board {

public int DropIndex{get;set;}

	int row = 0;
	public int Row{
		get{
			return row;
		}
		set{
			this.row = value;
			var pos = transform.localPosition;
			transform.localPosition = new Vector3(dropSize * value,pos.y,pos.z);
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
			transform.localPosition = new Vector3(pos.x,dropSize * value,pos.z);
			
		}
	}
	public DropColor DropColor{get;set;}
	public bool Dragging{get;set;}
	
	public int dropSize = 2;
	
	
	
	// Use this for initialization
	void Start () {
		//rigidbody.isKinematic = true;
		tk2dSprite sprite = GetComponent<tk2dSprite>();
		if(DropColor == DropColor.Red)sprite.SetSprite(3);
		if(DropColor == DropColor.Blue)sprite.SetSprite(11);
		if(DropColor == DropColor.Yellow)sprite.SetSprite(14);
		if(DropColor == DropColor.Green)sprite.SetSprite(7);
		if(DropColor == DropColor.White)sprite.SetSprite(13);
	}
	
	private void Update(){
		if(Dragging){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 pos = ray.origin - ray.direction *(ray.origin.z / ray.direction.z+0.3f);
			if(pos.x<=0.0f-(float)(dropSize)/2.0f) 
				pos.x = 0.0f - (float)dropSize/2.0f;
			if(pos.y<=0.0f-(float)(dropSize)/2.0f) 
				pos.y = 0.0f - (float)dropSize/2.0f;
			if(pos.x>=5.0f*(float)(dropSize)+(float)(dropSize)/2.0f) 
				pos.x = 5.0f*(float)(dropSize) + (float)dropSize/2.0f;
			if(pos.y>=4.0f*(float)(dropSize)+(float)(dropSize)/2.0f) 
				pos.y = 4.0f*(float)(dropSize) + (float)dropSize/2.0f;
			transform.position = pos;
		}
	}

	void OnMouseDown(){
		Debug.Log("MouseDown!!!");
		Dragging = true;
		//collider.isTrigger = true;
	}
	
	void OnMouseUp(){
		Dragging = false;
		//collider.isTrigger = false;
		/*transform.localPosition = new Vector3(
			(float)(Math.Round(transform.localPosition.x/2))*2, 
			(float)(Math.Round(transform.localPosition.y/2))*2, 
			0.0f); */
		transform.localPosition = new Vector3(row*2, column*2, 0.0f);
		Debug.Log("column = "+column +" , row = "+ row);
	}
	
	public void DeleteDrop(){
		tk2dSprite drop = GetComponent<tk2dSprite>();
		
		var pos = transform.localPosition;
		transform.localPosition = new Vector3(pos.x,pos.y,pos.z+1);
		Destroy(gameObject);
	}
}