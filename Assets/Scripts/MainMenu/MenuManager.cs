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
        gm.ChangeScene(Scene.WorldView);

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
