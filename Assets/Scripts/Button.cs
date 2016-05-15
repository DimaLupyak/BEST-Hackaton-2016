using UnityEngine;
using System.Collections;
using System.IO;
using Holoville.HOTween;
using UnityEngine.SceneManagement;

public enum ButtonActionType {StartLevel, Pause}

public class Button : MonoBehaviour 
{
	public ButtonActionType action;
	const float popDiff = 0.95f;
	private bool clicked = false;
	private Vector3 startScale;
	private SpriteRenderer iconRenderer, thisRenderer;

	void OnMouseUp()
	{
		ButtonAction();
	}

	void OnMouseExit()
	{
		if (clicked)
		{
			clicked = false;
		}
	}

	void ButtonAction()
	{
		switch (action)
		{
		case ButtonActionType.StartLevel:
			SceneManager.LoadScene(1);
			break;
		default:
			Debug.LogWarning(action.ToString());
			break;
		}
	}
}