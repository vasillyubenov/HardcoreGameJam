using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn1, playerSpawn2;
    [SerializeField] private Health player1Health, player2Health;
    [SerializeField] private int pointsToWin;

    private int player1Score;
    private int player2Score;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(10);
        }
    }

    public void OnPlayerDeath(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player2Score++;
            if (player2Score >= pointsToWin)
            {
                PlayCelebrationSequence(2);
            }
            return;
        }
        player1Score++;
        if (player1Score >= pointsToWin)
        {
            PlayCelebrationSequence(1);
        }
    }

    public void PlayCelebrationSequence(int playerWon)
    {

    }

    public void Respawn(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player1Health.transform.position = playerSpawn1.position;
            player1Health.Revive();
            return;
        }

        player2Health.transform.position = playerSpawn2.position;
        player2Health.Revive();
    }
}