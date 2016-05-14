using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{


    public float attackRange = 2;
    public float health = 100;
    public int power = 10;
    public float hitDelta = 50;
    private Animator anim;
    private Rigidbody2D rigi;
    private PlayerController player;
    private float distance;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StartCoroutine(Hit());
            player.Jump();
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
        anim.SetBool("Kick", true);
        player.GetDamage(power);
        
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Kick", false);
    }
}
