using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerGolf : MonoBehaviour
{
    [SerializeField]GameObject gameOverPanel;
    [SerializeField]TMP_Text gameOverText;
    [SerializeField] PlayerController player;
    [SerializeField]Hole hole;
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if(hole.Entered&&gameOverPanel.activeInHierarchy==false)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "Finished!\n Shoot Count: "+player.ShootCount;
        }
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }

    public void PlayNext()
    {
        SceneLoader.LoadNextLevel();
    }
}
