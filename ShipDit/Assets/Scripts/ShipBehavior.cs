using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    public int shipLength;
    int hitAmount;
    public OccupaationType type;

    private void Start()
    {
        hitAmount = shipLength;
    }
    bool IsSunk()
    {
        return hitAmount <= 0;
    }

    public bool IsHit()
    {
        return hitAmount < shipLength && hitAmount > 0;
    }

    public bool TakeDamage()
    {
        hitAmount--;
        if(IsSunk() )
        {
            GetComponent<MeshRenderer>().enabled = true;
            return true;
        }
        return false;
    }
}
