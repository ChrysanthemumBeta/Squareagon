using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Destoyself());
    }

    IEnumerator Destoyself()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
