using UnityEngine;
using System.Collections;

public class Unit : Board {
	
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
		UnitPic[0,0] = 20;
		UnitPic[0,1] = 19;
		UnitPic[0,2] = 20;
		UnitPic[0,3] = 21;
		
		UnitPic[1,0] = 28;
		UnitPic[1,1] = 26;
		UnitPic[1,2] = 28;
		UnitPic[1,3] = 27;
		
		UnitPic[2,0] = 33;
		UnitPic[2,1] = 34;
		UnitPic[2,2] = 33;
		UnitPic[2,3] = 32;
		
		UnitPic[3,0] = 1;
		UnitPic[3,1] = 0;
		UnitPic[3,2] = 1;
		UnitPic[3,3] = 25;
		
		UnitPic[4,0] = 29;
		UnitPic[4,1] = 30;
		UnitPic[4,2] = 29;
		UnitPic[4,3] = 31;
	}
	
	// Update is called once per frame
	void Update () {
		tk2dSprite unit = GetComponent<tk2dSprite>();
		var pos = transform.localPosition;
		transform.Translate(Vector3.right * 1 * Time.deltaTime);
		animTime += Time.deltaTime;
		
		if(Moving && animTime > 0.5f){
			animTime = 0;
			animIndex = (animIndex+1) % 4;
			unit.SetSprite(UnitPic[UnitID,animIndex]);
		}
		
		if(HP <= 0){
			Destroy(gameObject);
		}
	}
	
	public void SetUnit(int id, int level, int combo){
		
		UnitPic = new int[5,4];
		UnitPic[0,0] = 20;
		UnitPic[0,1] = 19;
		UnitPic[0,2] = 20;
		UnitPic[0,3] = 21;
		
		UnitPic[1,0] = 28;
		UnitPic[1,1] = 26;
		UnitPic[1,2] = 28;
		UnitPic[1,3] = 27;
		
		UnitPic[2,0] = 33;
		UnitPic[2,1] = 34;
		UnitPic[2,2] = 33;
		UnitPic[2,3] = 32;
		
		UnitPic[3,0] = 1;
		UnitPic[3,1] = 0;
		UnitPic[3,2] = 1;
		UnitPic[3,3] = 25;
		
		UnitPic[4,0] = 29;
		UnitPic[4,1] = 30;
		UnitPic[4,2] = 29;
		UnitPic[4,3] = 31;
		
		UnitID = id-1;
		Level = level;
		int rate = (int)Mathf.Pow(1.2f, (float)combo * Level/2f);
		
		HP = rate * 100;
		Power = rate * 10;
		Speed = 10 + Level/3;
		
		tk2dSprite unit = GetComponent<tk2dSprite>();
		unit.SetSprite(UnitPic[UnitID,animIndex]);
		var scl = unit.transform.localScale;
		unit.transform.localScale = new Vector3(scl.x*(level*0.4f + 1f),scl.y*(level*0.4f + 1f),scl.z);
		var pos = unit.transform.localPosition;
		unit.transform.localPosition = new Vector3(pos.x,pos.y + scl.y*(level*0.1f + 1f) - scl.y,pos.z);
		if(level > 3){
			unit.particleSystem.Play();
		}
	}
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "Enemy"){
			HP-=100;
		}
	}
}

/*public class UnitData {
	int[] hp = new int[4];
	int[] power = new int[4];
	
	//00 Red Fighter
	hp[0] = 100;
}*/