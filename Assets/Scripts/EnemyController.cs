using UnityEngine;
using System.Collections;
public enum EnemyType { Mario, Following }

public class EnemyController : MonoBehaviour
{

    public float speed = 10f;
    public float attackRange = 2;
    public float health = 100;
    public int power = 10;
    public EnemyType type;
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
        Move();
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
        if (coll.gameObject.tag == "Fliper" && type == EnemyType.Mario)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
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

    void Move()
    {
       if(type == EnemyType.Following)
        {
            if(Mathf.Abs(player.transform.position.x - transform.position.x)>1)
                rigi.velocity = new Vector2(speed * (int)(Mathf.Sign(player.transform.position.x - transform.position.x)), rigi.velocity.y);
        }
       else if (type == EnemyType.Mario)
        {
            rigi.velocity = new Vector2(speed*Mathf.Sign(transform.localScale.x), rigi.velocity.y);
        }
    }
}
