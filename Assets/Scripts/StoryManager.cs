using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    GameManager gm;
    public GameObject storyDialog;
    public TextMeshPro storyText;
    void Start()
    {
        gm = GameManager.instance;
    }


    // Update is called once per frame
    void Update()
    {
    }
}
