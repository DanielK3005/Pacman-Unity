using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passages : MonoBehaviour
{

    [SerializeField] private Transform spawnPlace;

private void OnTriggerEnter2D(Collider2D other) {
        Vector3 position = spawnPlace.position;
        position.z = other.transform.position.z;
        other.transform.position = position;
}
}
