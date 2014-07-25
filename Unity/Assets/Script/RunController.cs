using UnityEngine;
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
	private const float Ax_Left_Pos = -0.25f;
	private const float Ax_Right_Pos = 0.25f;

	// プレイヤー
	[SerializeField]
	private GameObject player;

	// プレイヤーのアニメーター
	private Animator playerAnimator;

	// アクション中フラグ
	private bool isActioning = false;
	private bool isLeftDelivering = false;
	private bool isRightDelivering = false;

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
			playerAnimator.SetBool("LeftDeliver", false);
			playerAnimator.SetBool("RightDeliver", false);

			isActioning = false;
			isLeftDelivering = false;
			isRightDelivering = false;

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

		// アクション中フラグの更新
		isActioning = playerAnimator.GetBool("Jumping") || playerAnimator.GetBool("Sliding") || 
						playerAnimator.GetBool("LeftDeliver") || playerAnimator.GetBool("RightDeliver");

		// デリバーアクション当たり判定の発生
		effectiveDeliverCollision();
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

			// 既に右にいる場合はデリバーアクション
			if (!isActioning && playerAx == Ax_Right) {
				playerAnimator.SetBool("RightDeliver", true);
				isRightDelivering = true;
				constantProcessTimer = 0.0f;
			} else if (!isActioning && playerAx == Ax_Left) {
				var position = player.transform.position;
				position.x = Ax_Right_Pos;
				playerAx = Ax_Right;
				player.transform.position = position;
			}

		}
		// 左移動ボタン
		if (GUI.Button(new Rect(25, 430, 100, 50), "MoveLeft")) {
			Debug.Log("Click => MoveLeft");

			// 既に左にいる場合はデリバーアクション
			if (!isActioning && playerAx == Ax_Left) {
				playerAnimator.SetBool("LeftDeliver", true);
				isLeftDelivering = true;
				constantProcessTimer = 0.0f;
			} else if (!isActioning && playerAx == Ax_Right) {
				var position = player.transform.position;
				position.x = Ax_Left_Pos;
				playerAx = Ax_Left;
				player.transform.position = position;
			}
		}
	}

	//----------------------------------------------
	///@brief デリバーアクションの当たり判定の制御を行う
	private void effectiveDeliverCollision() {
		// 左
		if (!!isLeftDelivering) {
			player.GetComponent<CharacterController>().leftDeliverCollision.gameObject.SetActive(true);
		} else {
			player.GetComponent<CharacterController>().leftDeliverCollision.gameObject.SetActive(false);
		}
		// 右
		if (!!isRightDelivering) {
			player.GetComponent<CharacterController>().rightDeliverCollision.gameObject.SetActive(true);
		} else {
			player.GetComponent<CharacterController>().rightDeliverCollision.gameObject.SetActive(false);
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
