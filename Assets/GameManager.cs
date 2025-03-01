using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    void Start()
    {
        HideCursor();
        // set the score text to coins + score
        scoreText.text = "Coins: " + score;
    }

    private void HideCursor(){
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    }
    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Coins: " + score;
    }
}
