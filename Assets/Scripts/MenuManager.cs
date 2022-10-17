using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameManager gm;
    public GameObject noSaveGameDialog;
    private string levelToLoad;

    void Start()
    {
        gm = GameManager.instance;
    }

    void Update()
    {
        
    }

    public void NewGameDialogYes()
    {
        gm.InitialiseDeck(3);
        gm.ChangeScene("WorldView");
        
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSaveGameDialog.SetActive(true);
        }

    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
