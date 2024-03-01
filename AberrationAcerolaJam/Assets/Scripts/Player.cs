using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float SpeedAccumulated, SpeedAccumulation, SpeedCap = 0.3f, PunchRate;
    private float dash;
    private Rigidbody2D MyRigidBody;
    private Vector3 Direction;
    private bool punch;

    void Awake()
    {
        Rigidbody2D rig = this.gameObject.GetComponent<Rigidbody2D>();

        if (rig == null)
            rig = this.gameObject.AddComponent<Rigidbody2D>();

        MyRigidBody = rig;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        bool Moving = Movement != Vector3.zero;

        if (Moving && dash <= 0f)
        {
            Direction = Vector3.Normalize(Movement);

            if (SpeedAccumulated < SpeedCap)
            {
                SpeedAccumulated += SpeedAccumulation;

                if (SpeedAccumulated > SpeedCap * 0.65f && !punch)
                {
                    SpeedAccumulated = SpeedCap;
                }

            }
            MyRigidBody.MovePosition(this.transform.position + Direction * (MovementSpeed + SpeedAccumulated));
        }
        else
        {
            if (SpeedAccumulated > 0f)
            {
                SpeedAccumulated -= SpeedAccumulation * 4f;
            }
            else
            {
                SpeedAccumulated = 0f;
            }

        }

        if (dash > 0f)
        {
            dash -= SpeedAccumulation * 8f;
            MyRigidBody.MovePosition(this.transform.position + Direction * (MovementSpeed + SpeedAccumulated + dash));
        }

    }

    void Update()
    {
        if (Input.GetKeyDown("c") && dash <= 0f )
        {
            dash = 0.2f;
            SpeedAccumulated = SpeedCap;
        }
        if (Input.GetKeyDown("x") && !punch)
        {
            punch = true;
            StartCoroutine(pnch());
            if(SpeedAccumulated < SpeedCap * 0.65f)
            {
                SpeedAccumulated = SpeedAccumulated * 0.8f;
            }
            else
            {
                SpeedAccumulated = SpeedAccumulated * 0.4f;
            }
            
        }
    }

    public IEnumerator pnch()
    {
        float timer = 0f;

        while (timer < PunchRate)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        punch = false;

    }
}
