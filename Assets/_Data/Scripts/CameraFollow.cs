using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float boundX = 0.5f;
    [SerializeField] private float boundY = 0.2f;

    private void LateUpdate()
    {
        if (this.target != null)
        {
            this.FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector3 delta = Vector3.zero;
        float deltaX = this.target.position.x - transform.position.x;
        float deltaY = this.target.position.y - transform.position.y;

        if (deltaX > this.boundX || deltaX < -this.boundX)
        {
            if (transform.position.x < this.target.position.x)
                delta.x = deltaX - this.boundX;
            else
                delta.x = deltaX + this.boundX;
        }

        if (deltaY > this.boundY || deltaY < -this.boundY)
        {
            if (transform.position.y < this.target.position.y)
                delta.y = deltaY - this.boundY;
            else
                delta.y = deltaY + this.boundY;
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
