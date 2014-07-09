using UnityEngine;
using System.Collections;

public class RunController : MonoBehaviour {

	// 動作させるプレハブ
	private GameObject backGround;

	// スピード
	private float speed = 0.0f;

	// 最初のスピード
	private const float DEFAULT_SPEED = 0.1f;

	// プレイヤーのアニメーター
	[SerializeField]
	private Animator playerAnimator;

	void Start () {
		// 動作するプレハブを設定する
		var go = Resources.Load ("Prefab/BackGround/BackGround_Town_01");
		backGround = Instantiate (go) as GameObject;

		// アニメーターのミッション開始フラグを立てる
		playerAnimator.SetBool ("MissionStart", true);
	}

	void Update () {
		// スピードを設定する
		if (!!playerAnimator.GetBool ("MissionStart")) {
			speed = DEFAULT_SPEED;
		}
		// プレハブを動かす
		moveBackGround ();
	}

	void OnGUI ()
	{
		// Jumpボタン
		if (GUI.Button(new Rect(125, 400, 100, 50), "Jump")) {
			Debug.Log("Click => Jump");
		}
		// Slidingボタン
		if (GUI.Button(new Rect(125, 460, 100, 50), "Sliding")) {
			Debug.Log("Click => Sliding");
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
