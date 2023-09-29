using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pallets;

    public int Score { get; private set; }
    public int Lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if(Lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }

    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach(Transform pallet in pallets)
        {
            pallet.gameObject.SetActive(true);
        }
        ResetState();
    }

    private void ResetState()
    {
        for(int i=0; i<ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(true);
        }
        pacman.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        foreach(Transform pallet in pallets)
        {
            pallet.gameObject.SetActive(false);
        }

        for(int i=0; i<ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }
        pacman.gameObject.SetActive(false);
    }

    public void GhostEaten(Ghost ghost)
    {
        ghost.gameObject.SetActive(false);
        SetScore(Score + ghost.points);
    }

    public void PacmanEaten()
    {
        pacman.gameObject.SetActive(false);
        SetLives(Lives - 1);

        if(Lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    private void SetScore(int score)
    {
        Score= score;
    }

    private void SetLives(int lives)
    {
        Lives= lives;
    }

}
