using UnityEngine;

public class Hand : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] private int damage;

    private void Start()
    {
        Invoke(nameof(DestroyHand), 2);
    }


    void Update()
    {
        if (transform.rotation.y == 0)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    private void DestroyHand()
    {
        Destroy(gameObject);
    }
}
