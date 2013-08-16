using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	public GameObject sphere;
	public tk2dSprite sprite;
	public tk2dSprite unitprefab;
	public Color[] color = new Color[5];
	public tk2dSprite enemyprefab;
	
	Drop[] drops = new Drop[30];
	List<Unit> unitlist = new List<Unit>();
	List<Enemy> enemylist = new List<Enemy>();
		
	float timer =0;
	
	
	// Use this for initialization
	void Start () {
		
		color[0] = new Color(1.0f,0.2f,0.2f,1.0f);
		color[1] = new Color(0.2f,1.0f,0.2f,1.0f);
		color[2] = new Color(0.2f,0.2f,1.0f,1.0f);
		color[3] = new Color(1.0f,1.0f,0.2f,1.0f);
		color[4] = new Color(0.9f,0.9f,0.9f,1.0f);
		
		int index = 0;
		for(int i=0; i<6; i++){
			for(int j=0; j<5; j++){
				tk2dSprite drop = (tk2dSprite)Instantiate(sprite, new Vector3(i*2, j*2, 0), Quaternion.identity);
				var component = drop.GetComponent<Drop>();
				drops[index] = component;
				component.Row = i;
				component.Column = j;
				component.DropIndex = index++;
				component.DropColor = (DropColor)UnityEngine.Random.Range(1,6);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButton(0)){
			for(int i=0; i<30; i++){
				int xaxis = (int)System.Math.Round(drops[i].transform.position.x/drops[i].dropSize);
				int yaxis = (int)System.Math.Round(drops[i].transform.position.y/drops[i].dropSize);
				
				//ドロップのはみ出しを抑制
				if(xaxis >= 5) xaxis = 5;
				if(yaxis >= 4) yaxis = 4;
				if(xaxis <= 0) xaxis = 0;
				if(yaxis <= 0) yaxis = 0;
				
				//ドロップ操作
				if(drops[i].Dragging == true){
					//Debug.Log(i.ToString() +"     " +xaxis.ToString()+", "+yaxis.ToString() + "  row : "
					//	+ drops[i].Row + "  column : " + drops[i].Column);
					if(drops[i].Row !=  xaxis|| drops[i].Column != yaxis){
						int j;
						for(j=0; j<30; j++) 
							if(drops[j].Row == xaxis && drops[j].Column == yaxis) break;
						drops[j].Row = drops[i].Row;
						drops[j].Column = drops[i].Column;
						drops[i].Row = xaxis;
						drops[i].Column = yaxis;
					}
				}
			}
			
		}
		
		var vec = Input.mousePosition;
		vec.z = 0;
		var worldPosition = Camera.main.ScreenToWorldPoint(vec);
		if (Input.GetMouseButtonDown(0)) Debug.Log(vec);
		
		//マウスが離されたらmatch3判定
		if(Input.GetMouseButtonUp(0) && vec.y < 353){
			Drop[,] board = new Drop[6,5];
			for(int i=0; i<30; i++){
				board[drops[i].Row,drops[i].Column] = drops[i];
			}
			scan (board);
			BoardReset();
		}
		
		
			
		timer += Time.deltaTime;
	
		if(timer > 1f){
			tk2dSprite unit = (tk2dSprite)Instantiate(enemyprefab, new Vector3(30f, 11.5f, 3f), Quaternion.identity);
			var component = unit.GetComponent<Enemy>();
			GameObject ground = GameObject.FindWithTag("Ground");
			var pos = ground.transform;
			component.transform.Translate(pos.localPosition.x-6.718847f,0f,0f);
			component.transform.parent = ground.transform;
			timer = 0f;
		}
		
	}
	
	void BoardReset(){
		
		for(int i=0; i<30; i++){
			//drops[index].renderer.enabled = false;
			//var component = GetComponent("Drop");
			//Destroy(component);
			drops[i].DeleteDrop();
		}
		
		int index = 0;
		for(int i=0; i<6; i++){
			for(int j=0; j<5; j++){
				tk2dSprite drop = (tk2dSprite)Instantiate(sprite, new Vector3(i*2, j*2, 0), Quaternion.identity);
				var component = drop.GetComponent<Drop>();
				//drops[index] = new Drop();
				drops[index] = component;
				component.Row = i;
				component.Column = j;
				component.DropIndex = index++;
				component.DropColor = (DropColor)UnityEngine.Random.Range(1,6);
			}
		}
	}
	
	void scan(Drop[,] board){
		List<Drop> scanlist_tmp = new List<Drop>();
		List<List<Drop>> scanlist_column = new List<List<Drop>>();
		List<List<Drop>> scanlist_row = new List<List<Drop>>();
		List<List<Drop>> scan_result = new List<List<Drop>>();
		List<int> DeleteList = new List<int>();
		
		
		
		//scanlist_b[i]
		int length = 0;
		int continue_color;
		
		bool flag = false;
		
		//縦の検索
		for(int i=0; i<6; i++){
			for(int j=0; j<4; j++){
				int k=0;
				length = 1;
				if(board[i,j].DropColor == board[i,j+1].DropColor){
					continue_color = (int)board[i,j].DropColor;
					for(k=j+1; k<5; k++){
						if((int)board[i,k].DropColor == continue_color){
							length++;
						}
						else{
							break;
						}
					}
					
					if(length >= 3){
						for(int l=j; l<k; l++){
							scanlist_tmp.Add(board[i,l]);
							//board[i,l].transform.localScale = new Vector3(0.3f,0.3f,1.0f);
						}
						scanlist_column.Add(scanlist_tmp);
						scanlist_tmp = new List<Drop>();
					}
					j = k-1;
				}
			}
		}
		
		//縦のコンボを連結
		//iとjの要素を総当りで比較
		for(int i=0; i<scanlist_column.Count-1; i++){
			for(int j=i+1; j<scanlist_column.Count; j++){
				
				
				//コンボの色が同じなら比較
				if(scanlist_column[i][0].DropColor == scanlist_column[j][0].DropColor ){
					//コンボが隣り合っているか総当りで比較
					for(int k=0; k<scanlist_column[i].Count; k++){
						for(int l=0; l<scanlist_column[j].Count; l++){
							
							
							//コンボ同士が隣り合っていたら
							if(scanlist_column[i][k].Column == scanlist_column[j][l].Column&&
								scanlist_column[i][k].Row+1 == scanlist_column[j][l].Row){
								
								for(int m=0; m<scanlist_column[j].Count; m++){
									if(scanlist_column[i].Contains(scanlist_column[j][m]) == false)
										scanlist_column[i].Add(scanlist_column[j][m]);
								}
								flag = true;
								scanlist_column.RemoveAt(j);
								j--;
								break;
							}
						}
						if(flag){
							flag = false;
							break;
						}
					}
				}
			}
		}
		
		
		
		//横の検索
		for(int i=0; i<5; i++){
			for(int j=0; j<5; j++){
				int k=0;
				length = 1;
				if(board[j,i].DropColor == board[j+1,i].DropColor){
					continue_color = (int)board[j,i].DropColor;
					for(k=j+1; k<6; k++){
						if((int)board[k,i].DropColor == continue_color){
							length++;
						}
						else{
							break;
						}
					}
					
					if(length >= 3){
						for(int l=j; l<k; l++){
							scanlist_tmp.Add(board[l,i]);
							//board[l,i].transform.localScale = new Vector3(0.3f,0.3f,1.0f);
						}
						scanlist_row.Add(scanlist_tmp);
						scanlist_tmp = new List<Drop>();
					}
					j = k - 1;
				}
			}  
		}
		
		
		//横のコンボを連結
		//iとjの要素を総当りで比較
		for(int i=0; i<scanlist_row.Count-1; i++){
			for(int j=i+1; j<scanlist_row.Count; j++){
				
				
				//コンボの色が同じなら比較
				if(scanlist_row[i][0].DropColor == scanlist_row[j][0].DropColor ){
					//コンボが隣り合っているか総当りで比較
					for(int k=0; k<scanlist_row[i].Count; k++){
						for(int l=0; l<scanlist_row[j].Count; l++){
							
							
							//コンボ同士が隣り合っていたら
							if(scanlist_row[i][k].Row == scanlist_row[j][l].Row&&
								scanlist_row[i][k].Column+1 == scanlist_row[j][l].Column){
								
								for(int m=0; m<scanlist_row[j].Count; m++){
									if(scanlist_row[i].Contains(scanlist_row[j][m]) == false)
										scanlist_row[i].Add(scanlist_row[j][m]);
								}
								flag = true;
								scanlist_row.RemoveAt(j);
								j--;
								break;
							}
						}
						if(flag){
							flag = false;
							break;
						}
					}
				}
			}
		}
		
		//縦と横のコンボを連結
		for(int i=0; i<scanlist_column.Count; i++){
			for(int j=0; j<scanlist_row.Count; j++){
				
				//色が同じなら重なったドロップがあるか比較
				if(scanlist_column[i][0].DropColor == scanlist_row[j][0].DropColor){
					for(int k=0; k<scanlist_row[j].Count; k++){
						
						//重なってたら連結
						if(scanlist_column[i].Contains(scanlist_row[j][k])){
							for(int l=0; l<scanlist_row[j].Count; l++){
								if(scanlist_column[i].Contains(scanlist_row[j][l]) == false){
									scanlist_column[i].Add(scanlist_row[j][l]);
								}
							}
							
							DeleteList.Add(j);
							break;
						}
					}
				}
			}
		}
		
		for(int i=0; i<scanlist_row.Count; i++){
			if(DeleteList.Contains(i) == false) scanlist_column.Add(scanlist_row[i]);
		}
		
		
		for(int i=0; i<scanlist_column.Count; i++){
			for(int j=0; j<scanlist_column[i].Count; j++){
				scanlist_column[i][j].transform.localScale = new Vector3(0.3f,0.3f,1.0f);
			}
		}
		
		for(int i=0; i<scanlist_row.Count; i++){
			for(int j=0; j<scanlist_row[i].Count; j++){
				//scanlist_row[i][j].transform.localScale = new Vector3(0.3f,0.3f,1.0f);
			}
		}
		
		int size_column = scanlist_column.Count;
		int size_row = scanlist_row.Count;
		Debug.Log(size_column+" Combo");
		
		//ユニット生成
		StartCoroutine(GenerateUnits(scanlist_column));
		//float timerdrop = 0;
		//while(timerdrop < 10){
		//	timerdrop+= Time.deltaTime;
		//	timerdrop = 0;
		//}
		
		scanlist_row.Clear();
		scanlist_tmp.Clear();
	}
	
	//public int count{get;set;}
	IEnumerator GenerateUnits(List<List<Drop>> scanlist){
		int combo = scanlist.Count;
		DispCombo.Count = combo;
		
		for(int i=0; i<combo; i++){
			//yield return new WaitForSeconds(1.5f);
			Debug.Log ("count : "+ i + "   combo : " + scanlist.Count);
			tk2dSprite unit = (tk2dSprite)Instantiate(unitprefab, new Vector3(2f, 10.8f, 3f), Quaternion.identity);
			var component = unit.GetComponent<Unit>();
			component.SetUnit((int)scanlist[i][0].DropColor,scanlist[i].Count,combo);
			GameObject ground = GameObject.FindWithTag("Ground");
			var pos = ground.transform;
			component.transform.Translate(pos.localPosition.x-6.718847f,0f,0f);
			component.transform.parent = ground.transform;
			unitlist.Add(component);
			yield return new WaitForSeconds(1.5f);
		}
		scanlist.Clear();
	}
	
	//void OnGUI(){
	//	GUI.Box(new Rect(9,17,20,90), 3 + " Combo!!!");
	//}
}