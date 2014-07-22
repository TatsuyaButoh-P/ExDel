﻿using UnityEngine;
using System.Collections;

//----------------------------------------------
///@brief Runコントローラー
using System.Collections.Generic;


public class RunController : MonoBehaviour
{

	// 動作させるプレハブ
	private List<GameObject> backGroundList;

	// 定期処理タイマー
	private float constantProcessTimer = 0.0f;

	// 調整処理タイマー
	private float adjustTimer = 0.0f;

	// スピード
	private float speed = 0.0f;

	// 初期化フラグ
	private bool initFlg = true;

	// 現在の軸
	private string playerAx;

	// 最初のスピード
	private const float Default_Speed = 0.1f;

	// 定期処理時間
	private const float Constant_Process_Time = 0.8f;

	// 調整時間(定期処理時間)
	private const float Adjust_Time = 0.1f;

	// 左右軸
	private const string Ax_Left = "0";
	private const string Ax_Right = "1";

	// 左右真ん中軸座標
	private const float Ax_Left_Pos = -0.5f;
	private const float Ax_Right_Pos = 0.5f;

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

		var load = Resources.Load("Prefab/BackGround/BackGround_Town_02");
		var go = Instantiate(load) as GameObject;
		go.GetComponent<BackGroundController>().init(this.gameObject, true);
		backGroundList.Add(go);

		// 軸指定
		playerAx = Ax_Left;
	}

	void Update ()
	{
		// タイマー加算
		constantProcessTimer += Time.deltaTime;
		adjustTimer += Time.deltaTime;

		// 定期処理
		if (constantProcessTimer > Constant_Process_Time) {
			if (!!initFlg) {
				speed = Default_Speed;
				initFlg = false;
				playerAnimator.SetBool("Running", true);
			}

			playerAnimator.SetBool("Jumping", false);
			playerAnimator.SetBool("Sliding", false);

			constantProcessTimer = 0.0f;
		}

		// 調整処理
		if (adjustTimer > Adjust_Time) {
			// 位置を定期的に調整する
			if (playerAx == Ax_Left) {
				player.transform.position = new Vector3(Ax_Left_Pos, 0, 0);
			} else {
				player.transform.position = new Vector3(Ax_Right_Pos, 0, 0);
			}
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
			constantProcessTimer = 0.0f;
		}
		// Slidingボタン
		if (GUI.Button(new Rect(125, 460, 100, 50), "Sliding")) {
			Debug.Log("Click => Sliding");
			playerAnimator.SetBool("Sliding", true);
			constantProcessTimer = 0.0f;
		}
		
		// 右移動ボタン
		if (GUI.Button(new Rect(225, 430, 100, 50), "MoveRight")) {
			Debug.Log("Click => MoveRight");
			var position = player.transform.position;
			if (!isPlayerAction() && playerAx == Ax_Left) {
				position.x = Ax_Right_Pos;
				playerAx = Ax_Right;
			}
			player.transform.position = position;
		}
		// 左移動ボタン
		if (GUI.Button(new Rect(25, 430, 100, 50), "MoveLeft")) {
			Debug.Log("Click => MoveLeft");
			var position = player.transform.position;
			if (!isPlayerAction() && playerAx == Ax_Right) {
				position.x = Ax_Left_Pos;
				playerAx = Ax_Left;
			}
			player.transform.position = position;
		}
	}

	//----------------------------------------------
	///@brief プレイヤーがアクションしているかどうか
	///@retval true アクションしている
	///@retval false アクションしていない
	///@return プレイヤーがアクションしているかどうか
	private bool isPlayerAction()
	{
		return playerAnimator.GetBool("Jumping") || playerAnimator.GetBool("Sliding");
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
