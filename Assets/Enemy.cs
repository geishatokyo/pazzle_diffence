using UnityEngine;
using System.Collections;

public class Enemy : Board {
	
	public int HP{get;set;}
	public int Power{get;set;}
	public int Level{get;set;}
	public int Speed{get;set;}
	public bool Attacking{get;set;}
	public bool Moving{get;set;}
	public int UnitID{get;set;}
	public float animTime;
	public int[,] UnitPic;
	public int animIndex;

	// Use this for initialization
	void Start () {
		animTime = 0;
		animIndex = 0;
		Moving = true;
		UnitPic = new int[5,4];
		UnitPic[0,0] = 24;
		UnitPic[0,1] = 23;
		UnitPic[0,2] = 24;
		UnitPic[0,3] = 22;
		Power = 100;
	}
	
	// Update is called once per frame
	void Update () {
		tk2dSprite unit = GetComponent<tk2dSprite>();
		var pos = transform.localPosition;
		transform.Translate(Vector3.left * 1 * Time.deltaTime);
		animTime += Time.deltaTime;
		
		if(Moving && animTime > 1f){
			animTime = 0;
			animIndex = (animIndex+1) % 4;
			unit.SetSprite(UnitPic[0,animIndex]);
		}
	}
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "Unit"){
			//collision.
			Destroy(gameObject);
		}
	}
}
