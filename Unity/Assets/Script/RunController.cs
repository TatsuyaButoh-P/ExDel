using UnityEngine;
using System.Collections;

public class RunController : MonoBehaviour {

	// 動作させるプレハブ
	private GameObject moveBackGround;

	void Start () {
		// 動作するプレハブを設定する
		var go = Resources.Load ("Prefab/BackGround/BackGround_Town_01");
		moveBackGround = Instantiate (go) as GameObject;
	}

	void Update () {
		if (moveBackGround != null) {
			var position = moveBackGround.transform.position;
			position.z -= 0.1f;
			moveBackGround.transform.position = position;
		}
	}
}
