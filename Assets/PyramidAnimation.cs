using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.SceneManagement;

public class PyramidAnimation : MonoBehaviour 
{
	public Transform cam, oko;
	public int currentLevel = 1;
	public float firstSpeed, navigationSpeed;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			SceneManager.LoadScene(1);
		}
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
		HOTween.To(cam.parent, firstSpeed, parms);
		HOTween.To(cam, firstSpeed, "localPosition", new Vector3(0, 15, -120));
	}

	void MoveMapToLevel(int level)
	{
		HOTween.Kill(cam.parent);
		HOTween.Kill(cam);
		HOTween.To(cam.parent, navigationSpeed, "rotation", new Vector3(0, 180 + ((level - 1) % 6) * (-360f / 6f) , 0));
		HOTween.To(cam, navigationSpeed, "localPosition", new Vector3(0, 15 + (level - 1) / 6 * (105 / 3), -120 + (level - 1) / 6 * (15)));
	}
}