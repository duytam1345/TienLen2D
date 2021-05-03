using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToPlayer : MonoBehaviour
{
    public Vector3 v;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,v)>.1f) {
            transform.position = Vector3.MoveTowards(transform.position, v, Time.deltaTime*20);
        }
    }
}
