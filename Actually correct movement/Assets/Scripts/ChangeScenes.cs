using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public Transform toolbar;
    public void GoToSceneFinalMinigame()
    {
        SceneManager.LoadScene("FinalMinigame");

    }

    public void GoToSceneRoomLayout()
    {
        SceneManager.LoadScene("RoomLayout");

    }
}
