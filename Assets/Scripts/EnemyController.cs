using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private float speed = 0.02f;
    private float attackRange = 20;
    public float health = 100;
    public int power = 10;
    public float hitDelta = 50;
    private Animator anim;
    private Rigidbody2D rigi;
    private PlayerController player;
    private float minDistance, distance, hitTimer;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        distance =  Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
        if (distance < attackRange)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitDelta)
                Hit();
        }
    }



    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Killer")
            Destroy(gameObject);

    }

    public void GetDamage(int damage)
    { 
        health -= damage;
    }

    IEnumerator Hit()
    {
            hitTimer = 0;
            anim.SetBool("Kick", true);
            player.GetDamage(power);
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("Kick", false);
    }
}
