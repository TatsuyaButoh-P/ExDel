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

	// 最初のスピード
	private const float DEFAULT_SPEED = 0.1f;

	// 定期処理時間
	private const int CONSTANT_PROCESS_TIME = 1;

	// 左右移動幅
	private const float MOVE_DISTANCE = 1.0f;

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
			position.x -= MOVE_DISTANCE;
			backGround.transform.position = position;
		}
		// 左移動ボタン
		if (GUI.Button(new Rect(25, 430, 100, 50), "MoveLeft")) {
			Debug.Log("Click => MoveLeft");
			var position = backGround.transform.position;
			position.x += MOVE_DISTANCE;
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
