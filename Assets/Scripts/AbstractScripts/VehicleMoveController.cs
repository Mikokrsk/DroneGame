using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleMoveController : MonoBehaviour
{
    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void TurnLeft();
    public abstract void TurnRight();
}
