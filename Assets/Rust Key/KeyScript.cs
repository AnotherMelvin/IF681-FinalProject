using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public Outline outline;
    private Rigidbody rb;
    private bool isInteracted;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        outline.enabled = false;
    }

    public void Select()
    {
        if (isInteracted) return;

        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }

    public void Interact()
    {
        isInteracted = true;
        rb.useGravity = true;
        rb.AddForce(Random.Range(-200f, 200f), Random.Range(300f, 400f), -transform.forward.z);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Player" && isInteracted)
        {
            this.gameObject.SetActive(false);
        }
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //         this.gameObject.SetActive(false);
    //     }
    // }
}
