using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform player;

    void Update()
    {
        transform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
