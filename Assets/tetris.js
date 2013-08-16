//#pragma strict
var blocks = [ [
		[1,1],
		[0,1],
		[0,1]
	], [
		[1,1],
		[1,0],
		[1,0]
	], [
		[1,1],
		[1,1]
	], [
		[1,0],
		[1,1],
		[1,0]
	], [
		[1,0],
		[1,1],
		[0,1]
	], [
		[0,1],
		[1,1],
		[1,0]
	], [
		[1],
		[1],
		[1],
		[1]
	] ];

private var block : Array;
private var posx : int = 0;
private var posy : int = 0;
private var mapWidth : int = 10;
private var mapHeight : int = 20;
private var map = new Array();

var blockPrefab : GameObject;
private var block_prefabs = new Array();

private var deltaTime : float = 0;
private var interval : float = 0.3;

private var keyDownDelta : float = 0;
private var keyDownInterval : float = 0.1;

function Start () {
	load();
}

function Update () {
	var key: int = 0;
	if (Input.GetAxis("Horizontal") > 0) {
		key = 37;
	}
	if (Input.GetAxis("Horizontal") < 0) {
		key = 39;
	}

	if (Input.GetAxis("Vertical") > 0) {
		key = 38;
	}
	if (Input.GetAxis("Vertical") < 0) {
		key = 40;
	}

	keyDown(key);

	deltaTime += Time.deltaTime;
	if (deltaTime > interval) {
		deltaTime = 0;
		paint();
	}
}

function load() {
	block = blocks[Mathf.RoundToInt(Random.Range(0, blocks.Length-1))];
	map = new Array(mapHeight);
	for (var y = 0; y < mapHeight; y++) {
		map[y] = new Array(mapWidth);
		for (var x = 0; x < mapWidth; x++) {
			map[y][x] = 0;
		}
	}
}

function paintMatrix(matrix : Array, offsetx : int, offsety : int, color) {
	for (var y = 0; y < matrix.length; y ++) {
		for (var x = 0; x < matrix[y].length; x ++) {
			if (matrix[y][x]) {
				block_prefabs.push(Instantiate(blockPrefab, Vector3(x + offsetx, 0, y + offsety), transform.rotation));
			}
		}
	}
}

function check(map : Array, block : Array, offsetx : int, offsety : int) {
	if (offsetx < 0 || offsety < 0 ||
			mapHeight < offsety + block.length ||
			mapWidth < offsetx + block[0].length) {
		return false;
	}

	for (var y = 0; y < block.length; y ++) {
		for (var x = 0; x < block[y].length; x ++) {
			if (block[y][x] && map[y + offsety][x + offsetx]) { 
				return false;
			}
		}
	}
	return true;
}

function mergeMatrix(map : Array, block : Array, offsetx : int, offsety : int) {
	var yl : int = 0;
	var xl : int = 0;
	for (var y = 0; y < mapHeight; y ++) {
		for (var x = 0; x < mapWidth; x ++) {
			if (y - offsety < 0 || block.length <= y - offsety) continue;
			if (x - offsetx < 0 || block[y - offsety].length <= x - offsetx) continue;
			if (block[y - offsety][x - offsetx] > 0) {
				map[y][x] += 1;
			}
		}
	}
}

function clearRows() {
	for (var y = 0; y < mapHeight; y ++) {
		var full = true;
		for (var x = 0; x < mapWidth; x ++) {
			if (!map[y][x]) {
				full = false;
			}
		}
		if (full) {
			map.RemoveAt(y);
			var newRow = new Array();
			for (var i = 0; i < mapWidth; i ++) {
				newRow.push(0);
			}
			map.unshift(newRow);
		}
	}
}

function clearRect() {
	for (var i=0; i<block_prefabs.length; i++) {
		Destroy(block_prefabs[i]);
	}
	block_prefabs.clear();
}

function paint() {
	clearRect();
	paintMatrix(block, posx, posy, 'rgb(255, 0, 0)');
	paintMatrix(map, 0, 0, 'rgb(128, 128, 128)');
	/**************** DEBUG ****************/
	var mapstring = '';
	for (y=0; y<mapHeight; y++) {
		var line : String = '';
		for (x=0; x<mapWidth; x++) {
			line = line + ' ' + map[y][x];
		}
		mapstring += line + '\n';
	}
	Debug.Log(mapstring);
	/**************** DEBUG ****************/

	if (check(map, block, posx, posy + 1)) {
		posy = posy + 1;
	}
	else {
		mergeMatrix(map, block, posx, posy);
		clearRows();
		posx = 0; posy = 0;
		block = blocks[Mathf.RoundToInt(Random.Range(0, blocks.Length-1))];
	}
}

function rotate(block : Array) {
	var rotated = new Array();
	for (var x = 0; x < block[0].length; x++) {
		rotated.push(Array());
		for (var y = 0; y < block.length; y ++) {
			rotated[x][block.length - y - 1] = block[y][x];
		}
	}
	return rotated;
}

function keyDown(keyCode) {
	keyDownDelta += Time.deltaTime;
	if (keyDownDelta < keyDownInterval) {
		return;
	}
	keyDownDelta = 0;

	switch (keyCode) {
		case 38:
			if (!check(map, rotate(block), posx, posy)) {
				return;
			}
			block = rotate(block);
			break;
		case 39:
			if (!check(map, block, posx + 1, posy)) {
				return;
			}
			posx = posx + 1;
			break;
		case 37:
			if (!check(map, block, posx - 1, posy)) {
				return;
			}
			posx = posx - 1;
			break;
		case 40:
			var y = posy;
			while (check(map, block, posx, y)) { y++; }
			posy = y - 1;
			break;
		default:
			return;
	}
	clearRect();
	paintMatrix(block, posx, posy, 'rgb(255, 0, 0)');
	paintMatrix(map, 0, 0, 'rgb(128, 128, 128)');
}