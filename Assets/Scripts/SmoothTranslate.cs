using UnityEngine;
using System.Collections;

public class SmoothTranslate : MonoBehaviour 
{
	public Transform mainObj;
	public float speed = 0.2f;
	void Update () 
	{
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, mainObj.localPosition + new Vector3 (3, -mainObj.localPosition.y, -10), speed);
	}
}
