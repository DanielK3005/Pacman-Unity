using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pallets;

    public int ghostMultiplier {get; private set; } = 1;

    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;

    [SerializeField] AudioSource gameOverAudio;

    
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
        gameOverText.enabled = false;

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
        gameOverAudio.Play();
        gameOverText.enabled = true;

        for(int i=0; i<ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;

        SetScore(Score + points);
        ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();
        
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
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(powerPallet.duration);
        }


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
        Score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        Lives = lives;
        livesText.text = "x" + lives.ToString();
    }

}
