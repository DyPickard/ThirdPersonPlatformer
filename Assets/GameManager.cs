using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        HideCursor();
    }

    private void HideCursor(){
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    }
}
