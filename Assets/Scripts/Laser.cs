using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        boxCollider.enabled = false;
        Invoke("ColliderChanger",1);
    }

    private void ColliderChanger()
    {
        circleCollider.enabled = false;
        boxCollider.enabled = true;
    }
}
