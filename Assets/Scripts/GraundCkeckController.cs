using UnityEngine;
using System.Collections;

public class GraundCkeckController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            gameObject.GetComponentInParent<PlayerController>().Jump();
        }
            
    }
}
