using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float CharacterJumpPower = 7f;
    const int MaxJump = 2;
    int RemainJump = 0;
    GameManager GM;

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && RemainJump > 0)
        {
            Jump(CharacterJumpPower);
            RemainJump --;
        }
    }

    // Jump with power
    void Jump(float power)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, CharacterJumpPower, 0), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            RemainJump = MaxJump;
        }
        if (col.gameObject.CompareTag("Obstacle"))
        {
            GM.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Point"))
        {
            GM.GetPoint(1);
            Destroy(col.gameObject);
            
        }
    }
}
