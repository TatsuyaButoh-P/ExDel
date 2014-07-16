using UnityEngine;
using System.Collections;

//----------------------------------------------
///@brief 次のポップイベント
public class NextPopAreaEvent : MonoBehaviour
{
	[SerializeField]
	private GameObject target; //!< イベントの発行先

	//----------------------------------------------
	///@brief 衝突判定
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("NextPopAreaEvent : OnTriggerEnter");
		target.SendMessage("pop");
	}
}
