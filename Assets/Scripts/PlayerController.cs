using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 600f;
    public bool isFacingRight = true;
    public float groundRadius = 0.2f;
	public BoxCollider2D groundChecker;
	public int power = 10;
	public float hitDistance = 2f;

    public float health = 100;
    private Animator anim;
    private Rigidbody2D rigi;
    private bool isGrounded = true;    
	public SpriteRenderer mainSprite, occupatedSpriteRat, occupatedSpriteBrain;
	private Transform currentTransform;
	private bool occupating;
	private Collider2D currentCollider;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
		currentTransform = transform;
    }

	void HeadOccupation(EnemyController enemy)
	{
		this.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, this.transform.position.z);
		Destroy(enemy.gameObject);
		rigi.velocity = Vector2.zero;
		mainSprite.enabled = false;
		occupatedSpriteRat.enabled = true;
		occupatedSpriteBrain.enabled = true;
	}

	void JumpAndUnoccupate()
	{
		this.transform.Translate(0, 1, 0);
		mainSprite.enabled = true;
		occupatedSpriteRat.enabled = false;
		occupatedSpriteBrain.enabled = false;
		rigi.AddForce(new Vector2(0, jumpForce));
		occupating = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "BigRatHead")
		{
			currentCollider = coll;
		}
		else if (coll.gameObject.tag == "Enemy")
		{
			if (coll.gameObject.GetComponent<EnemyController>().type == EnemyType.SmallRat)
				Destroy(coll.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		currentCollider = null;
	}

	private void Update()
	{
		//JUMP
		if (Input.GetButtonDown("Jump") && groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")))
			Jump();	
		anim.SetBool("Ground", groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")));
		anim.SetFloat("vSpeed", rigi.velocity.y);
		if (!occupating)
		{
			if (Input.GetKeyDown(KeyCode.G) && currentCollider != null && currentCollider.tag == "BigRatHead")
			{
				//currentCollider.transform.parent.GetComponent<EnemyController>().isOccupated = true;
				occupating = true;
				HeadOccupation(currentCollider.transform.parent.GetComponent<EnemyController>());
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.G))
				JumpAndUnoccupate();
		}
		if (Input.GetButtonDown("Fire1"))
		{
			anim.SetTrigger("Hit");
			Hit();
		}

	}

    private void FixedUpdate()
    {
		float move = Input.GetAxis("Horizontal");
		if (move != 0)
			currentTransform.localScale = Mathf.Abs(currentTransform.localScale.x) * new Vector3(move > 0 ? 1 : -1, 1, 1);
		move = Mathf.Abs(move) > 0.3f ? Mathf.Sign(move) : move;
		anim.SetFloat("hSpeed", Mathf.Abs(move));
		//RUN
		rigi.velocity = new Vector2(move * speed, rigi.velocity.y);
    }

    public void Jump()
    {
        rigi.velocity = new Vector2(rigi.velocity.x, 0);
        rigi.AddForce(new Vector2(0, jumpForce));
    }

    public void GetDamage(int damage)
    {
        health -= damage;
    }

	public void Hit()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, isFacingRight ? Vector3.right : Vector3.left, hitDistance);
		if (hit == false) return;
		if (hit.collider.tag == "Enemy" && hit.distance <= hitDistance)
		{
			EnemyController enemy = hit.collider.gameObject.GetComponent<EnemyController>();
			enemy.GetDamage(power);
		}

	}

}
