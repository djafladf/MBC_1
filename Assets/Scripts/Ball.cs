using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = new Vector2(0,-2);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        var v = rigid.linearVelocity;
        if (col.CompareTag("VWall")) v.x *= -1;
        if (col.CompareTag("HWall")) v.y *= -1;
        if (col.CompareTag("Player"))
        {

        }
        rigid.linearVelocity = v;
    }
}
