using UnityEngine;
using System.Collections;
public enum EnemyType {SmallRat, BigRat}

public class EnemyController : MonoBehaviour
{
    public float speed = 10f;
    public float attackRange = 2;
    public float health = 100;
	public float jumpForce = 2;

    public int power = 10;
    public EnemyType type;
	public Transform headPlace;

	public SpriteRenderer occupatedSprite;
    private Animator anim;
	public BoxCollider2D groundChecker;
	[HideInInspector]
	public Rigidbody2D rigi;
    private PlayerController player;
    private float distance;
	public bool isOccupated = false;
    void Start()
	{
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerController>();
    }
		
    void Update()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "Player" && !isOccupated)
        {
            StartCoroutine(Hit());
            player.Jump();
        }   
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
		if (coll.gameObject.tag == "Fliper" && type == EnemyType.SmallRat)
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
//        anim.SetBool("Kick", true);
        player.GetDamage(power);
        
        yield return new WaitForSeconds(0.2f);
  //      anim.SetBool("Kick", false);
    }

    void Move()
    {
		if(type == EnemyType.BigRat)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x)>1)
                rigi.velocity = new Vector2(speed * (int)(Mathf.Sign(player.transform.position.x - transform.position.x)), rigi.velocity.y);
        }
		else if (type == EnemyType.SmallRat)
        {
            rigi.velocity = new Vector2(speed * Mathf.Sign(transform.localScale.x), rigi.velocity.y);
        }
    }

	public void Jump()
	{
		rigi.velocity = new Vector2(rigi.velocity.x, 0);
		rigi.AddForce(new Vector2(0, jumpForce));
	}
}
