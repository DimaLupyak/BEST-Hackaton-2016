using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 600f;
    public bool isFacingRight = true;
    public float groundRadius = 0.2f;
	public BoxCollider2D groundChecker;
	public SpriteRenderer mySprite;

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
		this.transform.position = new Vector3(enemy.transform.position.x, -2.8f, this.transform.position.z);
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
	}

    private void FixedUpdate()
    {
		float move = Input.GetAxis("Horizontal");
		if (move != 0)
			currentTransform.localScale = Mathf.Abs(currentTransform.localScale.x) * new Vector3(move > 0 ? 1 : -1, 1, 1);
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
}
