using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Toolbox;

public class MoveCube : MonoBehaviour
{
    [SerializeField] private float moveTime = 1.0f;

    private LerpData moveData = null;
    private bool canMove = false;
    private bool backward = false;

    private void Awake()
    {
        moveData = LerpHelper.SetLerp(this, transform.position, transform.position + Vector3.forward * 5.0f, moveTime, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveOrStopCube();
        }

        if (canMove)
        {
            if (!moveData.infos.isRunning)
            {
                backward = !backward;

                if (backward)
                {
                    moveData = LerpHelper.SetLerp(this, transform.position, transform.position - Vector3.forward * 5.0f, moveTime);
                }
                else
                {
                    moveData = LerpHelper.SetLerp(this, transform.position, transform.position + Vector3.forward * 5.0f, moveTime);
                }
            }

            transform.position = moveData.infos.vec3;
        }
    }

    private void MoveOrStopCube()
    {
        canMove = !canMove;

        if (canMove)
        {
            LerpHelper.ResumeLerp(this, moveData);
        }
        else
        {
            LerpHelper.StopLerp(this, moveData);
        }
    }
}
