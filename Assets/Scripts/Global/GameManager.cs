using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int lifeblood, maxLifeblood = 100;
    public Scene currentScene;
    public Card[] possibleCards;
    public List<Card> deck;
    public List<Card> enemyDeck;
    public EnemyCardController[,] enemyCards = new EnemyCardController[(int)Board.Width, (int)Board.Height * 2];
    public GameObject enemyCardPrefab;

    public bool placedEnemyDeck = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeScene(Scene.MainMenu);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeScene(Scene.WorldView);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeScene(Scene.InGame);
        }

        if (placedEnemyDeck == false && currentScene == Scene.InGame)
        {
            PlaceEnemyDeck();
        }
    }

    public void ChangeScene(Scene scene)
    {
        currentScene = scene;
        SceneManager.LoadScene(Enum.GetName(typeof(Scene), scene));
    }

    public void InitialiseDeck(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            int random = UnityEngine.Random.Range(0, possibleCards.Length);
            deck.Add(possibleCards[random]);
        }
    }

    public void InitialiseEnemyDeck(int difficulty)
    {
        enemyDeck = new List<Card>();

        int count = difficulty + 2;
        int i = 0;
        while (i < count)
        {
            int r = UnityEngine.Random.Range(0, possibleCards.Length);
            Card cardSelected = possibleCards[r];
            if (cardSelected.energyCost + i <= count)
            {
                i += cardSelected.energyCost;
                enemyDeck.Add(cardSelected);
            }

        }
    }

    public void PlaceEnemyDeck()
    {
        int boardSlots = (int)Board.Width * (int)Board.Height;
        var availableLocations = Enumerable.Range(0, boardSlots - 1).ToList();

        foreach (Card card in enemyDeck)
        {
            int r = UnityEngine.Random.Range(0, availableLocations.Count);

            int index = availableLocations[r];

            int xPos = index % (int)Board.Width;
            int yPos = ((int)Board.Height * 2) - (index / (int)Board.Width) - 1;

            var transforms = GameObject.FindObjectOfType<EnemyBoardPositions>();
            var enemyCard = Instantiate(enemyCardPrefab, transforms.bp[index]);

            EnemyCardController enemyCardController = enemyCard.GetComponent<EnemyCardController>();
            enemyCardController.card = card;

            enemyCards[xPos, yPos] = enemyCardController;

            availableLocations.RemoveAt(r);
        }

        placedEnemyDeck = true;
    }


}