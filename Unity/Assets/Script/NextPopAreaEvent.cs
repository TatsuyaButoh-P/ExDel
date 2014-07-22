using UnityEngine;
using System.Collections;

//----------------------------------------------
///@brief 次のポップイベント
public class NextPopAreaEvent : MonoBehaviour
{
	[SerializeField]
	private GameObject target; //!< イベント等の発行先

	[SerializeField]
	private bool isRotate; //!< 回転フラグ

	[SerializeField]
	private float rotate; //!< 回転する角度

	//----------------------------------------------
	///@brief 衝突判定
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("NextPopAreaEvent : OnTriggerEnter");

		// 回転させる
		if (!!isRotate) {
			//target.transform.eulerAngles = new Vector3(0, rotate, 0);
			target.transform.RotateAround(this.gameObject.transform.position, target.transform.up, rotate);
		}

		target.SendMessage("pop");

		// 自壊する
		GameObject.Destroy(this.gameObject);
	}
}
