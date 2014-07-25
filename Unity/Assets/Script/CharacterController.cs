using UnityEngine;
using System.Collections;

//----------------------------------------------
///@brief Characterコントローラー
public class CharacterController : MonoBehaviour
{
	[SerializeField]
	public bool IsPlayer; //!< プレイヤーかどうか

	[SerializeField]
	public GameObject leftDeliverCollision; //!< 左のデリバーアクション当たり判定

	[SerializeField]
	public GameObject rightDeliverCollision; //!< 右のデリバーアクション当たり判定
}
