using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public GameObject newTerrain;
    public GameObject obstacle;
    public Material color2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.name);
        if (other.name == "Player")
        {
            GetComponent<Renderer>().material = color2;

            Destroy(obstacle);
            newTerrain.SetActive(true);
        }
    }
}


