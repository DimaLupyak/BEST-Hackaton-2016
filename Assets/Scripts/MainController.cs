using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour 
{
	public List<EnemyController> enemies;

	void Start()
	{
		enemies = new List<EnemyController>();
		foreach (var enemy in GameObject.FindObjectsOfType<EnemyController>())
			enemies.Add(enemy);
	}
}
