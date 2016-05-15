using UnityEngine;
using System.Collections;

public class SmoothTranslate : MonoBehaviour 
{
    public float height;
	public Transform mainObj;
	public float speed = 0.2f;
	public float borderLeft, borderRight;

	void Update () 
	{
		var newPos = Vector3.Lerp(this.transform.localPosition, mainObj.localPosition + new Vector3 (0, -mainObj.localPosition.y -height, -10), speed);
		if (newPos.x > borderLeft && newPos.x < borderRight)
			this.transform.localPosition = newPos;
	}
}
