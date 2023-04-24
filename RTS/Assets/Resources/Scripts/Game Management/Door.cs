using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision, false);
    }
}
