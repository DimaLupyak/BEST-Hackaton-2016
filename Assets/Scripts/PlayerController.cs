﻿using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
	public float attackRadius = 0.5f;
    public float speed = 10f;
    public AudioSource hurt, coin;
    public float jumpForce = 600f;
    public float groundRadius = 0.2f;
	public BoxCollider2D groundChecker;
	public int power = 10;
	public float hitDistance = 2f;
    public GameObject slider;
    public Text scoreText;
    private bool blockHit;
    public float health = 100;
    private Animator anim;
    private Rigidbody2D rigi;
    private bool isGrounded = true;    
	public SpriteRenderer mainSprite, glassesSprite, occupatedSpriteRat, occupatedSpriteBrain;
	private Transform currentTransform;
	private bool occupating;
	private Collider2D currentCollider;
	private MainController controller;
    private int score = 0;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
		currentTransform = transform;
		controller = GameObject.FindObjectOfType<MainController>();
    }

	void HeadOccupation(EnemyController enemy)
	{
		this.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, this.transform.position.z);
		Destroy(enemy.gameObject);
		rigi.velocity = Vector2.zero;
		mainSprite.enabled = false;
		glassesSprite.enabled = false;
		occupatedSpriteRat.enabled = true;
		occupatedSpriteBrain.enabled = true;
	}

	void JumpAndUnoccupate()
	{
		//this.transform.Translate(0, 1, 0);
		mainSprite.enabled = true;
		glassesSprite.enabled = true;
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
        if (coll.gameObject.tag == "Shipi")
        {
            
            SceneManager.LoadScene("DemoLvl");
        }
        if (coll.gameObject.tag == "item")
        {
            coin.Play();
            health += 7;
            score += 50;
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "door")
        {
            SceneManager.LoadScene("Tower");
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
            health += 7;
            score += 50;
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Enemy")
		{
			if (coll.gameObject.GetComponent<EnemyController>().type == EnemyType.SmallRat && coll.transform.position.y < this.transform.position.y)
			{
				rigi.AddForce(new Vector2(0, 150));
				Destroy(coll.gameObject);
			}
			else if (coll.gameObject.GetComponent<EnemyController>().type == EnemyType.BigRat && coll.transform.position.y < this.transform.position.y)

			{
				coll.GetComponent<EnemyController>().GetDamage(power, false);
				rigi.AddForce(new Vector2(0, 200));
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		currentCollider = null;
	}

	private void Update()
	{
        if (health < 0)
            SceneManager.LoadScene("DemoLvl");
        scoreText.text = score.ToString("D5");
        slider.GetComponent<Slider>().value = health;
        anim.SetFloat("Gravity", rigi.gravityScale);
		//JUMP
		if (Input.GetButtonDown("Jump") && groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")))
			Jump();	
		if (Input.GetButton("Jump") && rigi.velocity.y < 0 && !groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")))
			rigi.gravityScale = 0.1f;
		else 
			rigi.gravityScale = 1f;
		anim.SetBool("Ground", groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")));
		anim.SetFloat("vSpeed", rigi.velocity.y);
		if (!occupating)
		{
			if (Input.GetButtonDown("Fire2") && currentCollider != null && currentCollider.tag == "BigRatHead")
			{
				//currentCollider.transform.parent.GetComponent<EnemyController>().isOccupated = true;
				occupating = true;
				HeadOccupation(currentCollider.transform.parent.GetComponent<EnemyController>());
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire2"))
				JumpAndUnoccupate();
		}
		if (Input.GetButtonDown("Fire1") && !blockHit)
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
        this.GetComponent<AudioSource>().Play();
        rigi.velocity = new Vector2(rigi.velocity.x, 0);
        rigi.AddForce(new Vector2(0, jumpForce));
    }
    public void Jump(float ez)
    {
        this.GetComponent<AudioSource>().Play();
        rigi.velocity = new Vector2(rigi.velocity.x, 0);
        rigi.AddForce(new Vector2(0, ez));
    }



    public void GetDamage(int damage, EnemyController enemy)
    {
        health -= damage;
		StartCoroutine(RedLight());
		var sign = enemy.transform.position.x > this.transform.position.x ? -1 : 1;
		rigi.AddForce(new Vector2(150 * sign, 50));
    }

	IEnumerator RedLight()
	{
		for (int i = 0; i < 3; i++)
		{
			HOTween.To(mainSprite, 0.1f, "color", new Color(1, 0.5f, 0.5f));
			yield return new WaitForSeconds(0.1f);
			HOTween.To(mainSprite, 0.1f, "color", new Color(1, 1f, 1f));
			yield return new WaitForSeconds(0.1f);			
		}
	}

	public void Hit()
	{
		for (int i = 0; i < controller.enemies.Count; i++)
		{
			if (controller.enemies[i] == null)
				continue;
			if (Mathf.Abs(controller.enemies[i].transform.position.x - this.transform.position.x) < attackRadius
				&& Mathf.Abs(controller.enemies[i].transform.position.y - this.transform.position.y) < 0.3f)
			{
                hurt.Play();
				controller.enemies[i].GetDamage(power, true);
				StartCoroutine(BlockHit(1));
			}
		}
	}

	IEnumerator BlockHit(float delay)
	{
		blockHit = true;
		yield return new WaitForSeconds(delay);	
		blockHit= false;
	}

}
