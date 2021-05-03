using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    Controller controller;

    public SpriteRenderer sprite;

    public DataCard data;

    private void Start()
    {
        controller = FindObjectOfType<Controller>();
    }

    public void Select()
    {
        controller.selected.Add(this);
        transform.position += Vector3.up;
    }

    public void UnSelect()
    {
        controller.selected.Remove(this);
        transform.position -= Vector3.up;
    }
}
