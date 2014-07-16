using UnityEngine;
using System.Collections;

//----------------------------------------------
///@brief Runコントローラー
using System.Collections.Generic;


public class RunController : MonoBehaviour
{

	// 動作させるプレハブ
	private List<GameObject> backGroundList;

	// タイマー
	private float timer = 0.0f;

	// スピード
	private float speed = 0.0f;

	// 初期化フラグ
	private bool initFlg = true;

	// 現在の軸
	private string playerAx;

	// 最初のスピード
	private const float Default_Speed = 0.1f;

	// 定期処理時間
	private const int Constant_Process_Time = 1;

	// 左右真ん中軸
	private const string Ax_Center = "0";
	private const string Ax_Left = "1";
	private const string Ax_Right = "2";

	// 左右真ん中軸座標
	private const float Ax_Center_Pos = 0.0f;
	private const float Ax_Left_Pos = 1.0f;
	private const float Ax_Right_Pos = -1.0f;

	// プレイヤー
	[SerializeField]
	private GameObject player;

	// プレイヤーのアニメーター
	private Animator playerAnimator;

	void Start ()
	{
		// アニメーター設定
		playerAnimator = player.GetComponent<Animator>();

		// 動作するプレハブを設定する
		backGroundList = new List<GameObject>();

		var load = Resources.Load("Prefab/BackGround/BackGround_Town_01");
		var go = Instantiate(load) as GameObject;
		go.GetComponent<BackGroundController>().init(this.gameObject, true);
		backGroundList.Add(go);

		// 軸指定
		playerAx = Ax_Center;
	}

	void Update ()
	{
		// タイマー加算
		timer += Time.deltaTime;

		// 定期処理
		if (timer > Constant_Process_Time) {
			if (!!initFlg) {
				speed = Default_Speed;
				initFlg = false;
				playerAnimator.SetBool("Running", true);
			}

			playerAnimator.SetBool("Jumping", false);
			playerAnimator.SetBool("Sliding", false);

			timer = 0.0f;
		}

		// プレハブを動かす
		moveBackGround();
	}

	void OnGUI ()
	{
		// Jumpボタン
		if (GUI.Button(new Rect(125, 400, 100, 50), "Jump")) {
			Debug.Log("Click => Jump");
			playerAnimator.SetBool("Jumping", true);
		}
		// Slidingボタン
		if (GUI.Button(new Rect(125, 460, 100, 50), "Sliding")) {
			Debug.Log("Click => Sliding");
			playerAnimator.SetBool("Sliding", true);
		}

		// 右移動ボタン
		if (GUI.Button(new Rect(225, 430, 100, 50), "MoveRight")) {
			Debug.Log("Click => MoveRight");
			var position = player.transform.position;
			switch (playerAx) {
			case Ax_Left : 
				position.x = Ax_Center_Pos;
				playerAx = Ax_Center;
				break;
			case Ax_Center : 
				position.x = Ax_Right_Pos;
				playerAx = Ax_Right;
				break;
			default : 
				break;
			}
			player.transform.position = position;
		}
		// 左移動ボタン
		if (GUI.Button(new Rect(25, 430, 100, 50), "MoveLeft")) {
			Debug.Log("Click => MoveLeft");
			var position = player.transform.position;
			switch (playerAx) {
			case Ax_Center : 
				position.x = Ax_Left_Pos;
				playerAx = Ax_Left;
				break;
			case Ax_Right : 
				position.x = Ax_Center_Pos;
				playerAx = Ax_Center;
				break;
			default : 
				break;
			}
			player.transform.position = position;
		}
	}

	//----------------------------------------------
	///@brief 動かすプレハブを追加する
	///@param [in] プレハブのパス
	private void addBackGround(string path)
	{
		var load = Resources.Load(path);
		var go = Instantiate(load) as GameObject;
		go.GetComponent<BackGroundController>().init(this.gameObject, false);
		backGroundList.Add(go);
	}

	//----------------------------------------------
	///@brief プレハブを動かす
	private void moveBackGround()
	{
		foreach (var backGround in backGroundList) {
			if (backGround != null) {
				var position = backGround.transform.position;
				position.z -= speed;
				backGround.transform.position = position;
			}
		}
	}
}
