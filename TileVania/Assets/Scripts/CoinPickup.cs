using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickups = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToCoins(pointsForCoinPickups);
        AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
