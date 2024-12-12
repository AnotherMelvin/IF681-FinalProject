using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    int itemCount = 0;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(player);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = 270f;
        transform.rotation = Quaternion.Euler(rotation);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    // void OnTriggerEnter(Collider other)
    // {
    //     if(other.tag == "Player") // Jangan lupa dibuat tag "player"
    //     {
    //         itemCount++;
    //         Destroy(this.gameObject);
    //     }
    // }
}
