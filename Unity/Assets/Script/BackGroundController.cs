using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------
///@brief BackGroundコントローラー
public class BackGroundController : MonoBehaviour
{
	// イベントの発行先
	private GameObject target;

	// 削除するZ座標
	private const float Destroy_Z_Pos = -100.0f;

	// 初期Z座標
	private const float Initial_Z_Pos = 50.0f;

	void Update()
	{
		// 自壊処理
		// TODO 後々は最初に生成して使いまわす形を想定している
		if (this.transform.position.z < Destroy_Z_Pos) {
			GameObject.Destroy(this);
		}
	}

	//----------------------------------------------
	///@brief 初期化処理
	///@param [in] t ターゲット
	///@param [in] isFirst 最初のオブジェクトかどうか
	public void init(GameObject t, bool isFirst)
	{
		target = t;
		var position = this.transform.position;
		if (!isFirst) {
			position.z = Initial_Z_Pos;
		}
		this.transform.position = position;
	}

	//----------------------------------------------
	///@brief ポップイベント
	public void pop()
	{
		Debug.Log("BackGroundController : pop");
		target.SendMessage("addBackGround", "Prefab/BackGround/BackGround_Town_01");
	}
}
