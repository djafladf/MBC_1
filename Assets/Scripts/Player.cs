using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float Speed;
    BoxCollider2D coll;
    Rigidbody2D rigid;
    RectTransform rect;
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>(); rigid = GetComponent<Rigidbody2D>(); rect = GetComponent<RectTransform>();
    }
    Vector2 Dir = Vector2.zero;
    Vector2 NextPos = Vector2.zero;
    void FixedUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x + Dir.x * Time.fixedDeltaTime * Speed, -8.75f, 8.75f), -3.9f);
    }

    void OnMove(InputValue value)
    {
        //-8.75 ~ 8.75
        Dir = value.Get<Vector2>();
    }
}
