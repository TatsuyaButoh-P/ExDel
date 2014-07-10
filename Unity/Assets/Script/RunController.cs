using UnityEngine;
using System.Collections;

public class RunController : MonoBehaviour {

	// 動作させるプレハブ
	private GameObject backGround;

	// タイマー
	private float timer = 0.0f;

	// スピード
	private float speed = 0.0f;

	// 初期化フラグ
	private bool initFlg = true;

	// 現在の軸
	private string playerAx;

	// 最初のスピード
	private const float DEFAULT_SPEED = 0.1f;

	// 定期処理時間
	private const int CONSTANT_PROCESS_TIME = 1;

	// 左右真ん中軸
	private const string AX_CENTER = "0";
	private const string AX_LEFT = "1";
	private const string AX_RIGHT = "2";

	// 左右真ん中軸座標
	private const float AX_CENTER_POS = 0.0f;
	private const float AX_LEFT_POS = 1.0f;
	private const float AX_RIGHT_POS = -1.0f;

	// プレイヤー
	[SerializeField]
	private GameObject player;

	// プレイヤーのアニメーター
	private Animator playerAnimator;

	void Start () {
		// アニメーター設定
		playerAnimator = player.GetComponent<Animator> ();

		// 動作するプレハブを設定する
		var go = Resources.Load ("Prefab/BackGround/BackGround_Town_01");
		backGround = Instantiate (go) as GameObject;

		// 軸指定
		playerAx = AX_CENTER;
	}

	void Update () {
		// タイマー加算
		timer += Time.deltaTime;

		// 定期処理
		if (timer > CONSTANT_PROCESS_TIME) {
			if (!!initFlg) {
				speed = DEFAULT_SPEED;
				initFlg = false;
				playerAnimator.SetBool ("Running", true);
			}

			playerAnimator.SetBool ("Jumping", false);
			playerAnimator.SetBool ("Sliding", false);

			timer = 0.0f;
		}

		// プレハブを動かす
		moveBackGround ();
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
			var position = backGround.transform.position;
			switch (playerAx) {
			case AX_LEFT : 
				position.x = AX_CENTER_POS;
				playerAx = AX_CENTER;
				break;
			case AX_CENTER : 
				position.x = AX_RIGHT_POS;
				playerAx = AX_RIGHT;
				break;
			default : 
				break;
			}
			backGround.transform.position = position;
		}
		// 左移動ボタン
		if (GUI.Button(new Rect(25, 430, 100, 50), "MoveLeft")) {
			Debug.Log("Click => MoveLeft");
			var position = backGround.transform.position;
			switch (playerAx) {
			case AX_CENTER : 
				position.x = AX_LEFT_POS;
				playerAx = AX_LEFT;
				break;
			case AX_RIGHT : 
				position.x = AX_CENTER_POS;
				playerAx = AX_CENTER;
				break;
			default : 
				break;
			}
			backGround.transform.position = position;
		}
	}

	// プレハブを動かす
	private void moveBackGround()
	{
		if (backGround != null) {
			var position = backGround.transform.position;
			position.z -= speed;
			backGround.transform.position = position;
		}
	}
}
