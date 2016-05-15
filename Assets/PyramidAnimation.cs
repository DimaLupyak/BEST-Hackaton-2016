using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PyramidAnimation : MonoBehaviour 
{
	public Transform cam;
	public int currentLevel = 1;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow) && currentLevel < 19)
		{
			currentLevel++;
			MoveMapToLevel(currentLevel);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow) && currentLevel > 1)
		{
			currentLevel--;
			MoveMapToLevel(currentLevel);
		}
	}

	void Start () 
	{
		FirstAnimation();
	}

	void FirstAnimation()
	{
		TweenParms parms = new TweenParms();
		parms.Prop("rotation", new Vector3(0, 180, 0));
		HOTween.To(cam.parent, 8, parms);
		HOTween.To(cam, 8, "localPosition", new Vector3(0, 30, -120));
	}

	void MoveMapToLevel(int level)
	{
		HOTween.Kill(cam.parent);
		HOTween.Kill(cam);
		HOTween.To(cam.parent, 2, "rotation", new Vector3(0, 180 + ((level - 1) % 6) * (-360f / 6f) , 0));
		HOTween.To(cam, 2, "localPosition", new Vector3(0, 15 + (level - 1) / 6 * (100 / 3), -120));
	}
}
