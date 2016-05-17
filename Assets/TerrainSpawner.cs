using UnityEngine;
using System.Collections;

public class TerrainSpawner : MonoBehaviour 
{
	// terrain references	
	public GameObject jumpTerrain;
	public GameObject climbTerrain;
	public GameObject platformTerrain;

	public float[] height;
	// starting position for terrain, number found from tweaking in the editor
	public float startSpawnPosition = 11.2f;
	public float distance;
	// y position that all terrain will be spawned
	// my terrain is all joined at the same level
	// you can change this if here and the spawn method 
	// if you need terrain at different heights
	public float spawnYPos;
	
	// random number that is used for selecting the terrain
	int randomChoice;
	// keep track of the last position terrain was generated
	float lastPosition;
	// camera reference
	GameObject cam;
	// used to check if terrain can be generated depending on the camera position and lastposition
	bool canSpawn = true;
	
	void Start()
	{
		// make the lastposition start at start spawn position
		lastPosition = startSpawnPosition;
		// pair camera to camera reference
		cam = GameObject.Find("Main Camera");
	}
	
	void Update()
	{
		// if the camera is farther than the number last position minus 16 terrain is spawned
		// a lesser number may make the terrain 'pop' into the scene too early
		// showing the player the terrain spawning which would be unwanted
		if (cam.transform.position.x >= lastPosition - 16 && canSpawn == true)
		{
			// turn off spawning until ready to spawn again
			canSpawn = false;
			// we choose the random number that will determine what terrain is spawned
			randomChoice = Random.Range(1, 10);
			// SpawnTerrain is called and passed the randomchoice number
			SpawnTerrain(randomChoice);
		}
	}
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnTerrain(int rand)
	{
		if (rand >= 1 && rand <= 4)
		{
			Instantiate(jumpTerrain, new Vector3(lastPosition, height[0], 0), Quaternion.Euler(0, 0, 0));
			// same as start spawn position as starting terrain
			// is the same length as the rest of the terrain prefabs
			lastPosition += distance;
		}
		
		if (rand >= 5 && rand <= 8)
		{
			Instantiate(climbTerrain, new Vector3(lastPosition, height[1], 0), Quaternion.Euler(0, 0, 0));
			lastPosition += distance;
		}
		
		if (rand >= 9 && rand <= 10)
		// the platform terrain is more difficult to traverse
		// so we will lessen the chances of it spawning
		{
			Instantiate(platformTerrain, new Vector3(lastPosition, height[2], 0), Quaternion.Euler(0, 0, 0));
			lastPosition += distance;
		}
		
		// script is now ready to spawn more terrain
		canSpawn = true;
	}
}

