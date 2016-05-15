using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum EnemyType {SmallRat, BigRat}

public class EnemyController : MonoBehaviour
{
    public float speed = 10f;
    public float attackRange = 0.3f;
	public float attackFrequency = 1;
    public float health = 100;
	public float jumpForce = 2;
	public float moveDistance = 3;
	public int power = 10;
	public SpriteRenderer occupatedSprite, mainSprite;
	public EnemyType type;
	public Transform headPlace;
	public BoxCollider2D groundChecker;
	public Rigidbody2D rigi;
	private Animator anim;
    private PlayerController player;
	private bool blockMove;
	private bool isDied;

    void Start()
	{
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerController>();
		StartCoroutine(CheckAttack());
    }
		
	IEnumerator CheckAttack()
	{
		while (!isDied)
		{
			yield return new WaitForSeconds(attackFrequency);
			if (Mathf.Abs(transform.position.x - player.transform.position.x) < attackRange && Mathf.Abs(transform.position.y - player.transform.position.y) < 0.3f)
			{
				Hit();
				blockMove = true;
			}
			else blockMove = false;
		}
	}

    void Update()
    {
		if (isDied)
			return;
		if (!blockMove)
        	Move();
		var sign = player.transform.position.x < this.transform.position.x ? 1 : -1;
		this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x) * sign, this.transform.localScale.y, this.transform.localScale.z);
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

	public void GetDamage(int damage, bool byWhip = false)
	{
		health -= damage;
		if (health < 0)
		{
			Die();
			return;
		}
		StartCoroutine(RedLight());
		var sign = player.transform.position.x > this.transform.position.x ? -1 : 1;
		if (byWhip)
			rigi.AddForce(new Vector2(100 * sign, 80));
		StartCoroutine(BlockMove(0.5f));
	}


	void Die()
	{
		//HOTween.To(mainSprite, 0.05f, "color", new Color(1, 1, 1, 0));
		Destroy(this.gameObject);
		//GetComponent<BoxCollider2D>().enabled = false;

	}

	IEnumerator BlockMove(float delay)
	{
		blockMove = true;
		yield return new WaitForSeconds(delay);	
		blockMove = false;
	}

	IEnumerator RedLight()
	{
		for (int i = 0; i < 3; i++)
		{
			HOTween.To(mainSprite, 0.05f, "color", new Color(1, 0.5f, 0.5f));
			yield return new WaitForSeconds(0.1f);
			HOTween.To(mainSprite, 0.05f, "color", new Color(1, 1f, 1f));
			yield return new WaitForSeconds(0.1f);			
		}
	}

	void Hit()
	{
        anim.SetTrigger("Hit");
		player.GetDamage(power, this);
	}

    void Move()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(rigi.velocity.x));
        if (type == EnemyType.BigRat)
        {
			if (Mathf.Abs(player.transform.position.x - transform.position.x) >= attackRange && Mathf.Abs(player.transform.position.x - transform.position.x) < moveDistance)
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
