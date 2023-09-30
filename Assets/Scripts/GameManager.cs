using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pallets;

    public int ghostMultiplier {get; private set; } = 1;

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

    public void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    public void NewRound()
    {
        foreach(Transform pallet in pallets)
        {
            pallet.gameObject.SetActive(true);
        }
        ResetState();
    }

    public void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    public void GameOver()
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
        int points = ghost.points * ghostMultiplier;

        ghost.gameObject.SetActive(false);
        SetScore(Score + points);
        ghostMultiplier++;
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

    public void PalletEaten(Pallet pallet)
    {
        pallet.gameObject.SetActive(false);

        SetScore(Score + pallet.points);

        if(!HasRemainingPallets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound),3.0f);
        }
    }

    public void PowerPalletEaten(PowerPallet powerPallet)
    {
        PalletEaten(powerPallet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), powerPallet.duration);
    }

    private bool HasRemainingPallets()
    {
        foreach(Transform pallet in pallets)
        {
            if(pallet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
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
