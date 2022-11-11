using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using UnityEngine;

public class CoinTag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var playerTag = other.GetComponent<PlayerTag>();
        if (playerTag)
        {
            Destroy(gameObject);
        }
    }

}
