using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.SceneManagement;

public class PyramidAnimation : MonoBehaviour 
{
	public Transform cam, oko;
	public int currentLevel = 1;
	public float firstSpeed, navigationSpeed;
    bool blokChoose;
    public GameObject text;
    public UnityEngine.UI.Text levelText;
	void Update()
	{
        if (!text.gameObject.activeInHierarchy)
            levelText.text = "Choosed level: " + currentLevel.ToString();
        if (Input.GetButton("Fire3"))
        {
            SceneManager.LoadScene("ez");
        }
        if (Input.GetButton("Fire2"))
		{
			SceneManager.LoadScene(0);
		}
        if (Input.GetButton("Fire1"))
        {
            FirstAnimation();
        }
        if (Input.GetButton("Jump"))
		{
			SceneManager.LoadScene(1);
		}
        float side = Input.GetAxis("Horizontal");
		if (side > 0 && currentLevel < 19 && !blokChoose)
		{
			currentLevel++;
			MoveMapToLevel(currentLevel);
		}
		else if(side < 0 && currentLevel > 1 && !blokChoose)
		{
			currentLevel--;
			MoveMapToLevel(currentLevel);
		}
	}
		
	void Start () 
	{
		
	}

	void FirstAnimation()
	{
        text.SetActive(false);
		TweenParms parms = new TweenParms();
		parms.Prop("rotation", new Vector3(0, 180, 0));
		HOTween.To(cam.parent, firstSpeed, parms);
		HOTween.To(cam, firstSpeed, "localPosition", new Vector3(0, 15, -120));
	}

	void MoveMapToLevel(int level)
	{
        blokChoose = true;
        StartCoroutine(Unlock());
        HOTween.To(this, navigationSpeed, "blokChoose", false);
        HOTween.Kill(cam.parent);
		HOTween.Kill(cam);
		HOTween.To(cam.parent, navigationSpeed, "rotation", new Vector3(0, 180 + ((level - 1) % 6) * (-360f / 6f) , 0));
		HOTween.To(cam, navigationSpeed, "localPosition", new Vector3(0, 15 + (level - 1) / 6 * (105 / 3), -100 + (level - 1) / 6 * (15)));
	}

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(navigationSpeed);
        blokChoose = false;
    }
}