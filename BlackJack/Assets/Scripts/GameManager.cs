using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    Player player;

    public List<GameObject> deckCards = new List<GameObject>();
    public GameObject deck;

    [UnityEngine.SerializeField]
    private List<int> playedCards;

    public List<Character> characters = new List<Character>();
    int characterTurnIndex;

    public List<Transform> handPositions;

    bool gameStarting = false;
    bool handInProgress = false;
    float waitTime = 0.75f;
    float waitTimer = 0f;

    public GameObject gameResultPanel;
    public TextMeshProUGUI gameResultText;

    public AudioSource audioSource;
    public AudioClip audioClip;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this;

        // Get characters
        player = new Player(handPositions[0].position);
        characters.Add(player);
        characters.Add(new Dealer(handPositions[1].position));

        foreach (Transform child in deck.GetComponentsInChildren<Transform>()) {
            deckCards.Add(child.gameObject);
        }

        deckCards.RemoveAt(0); // Removes the deck itself from the list
    }

    // Update is called once per frame
    void Update()
    {
        if (handInProgress == false && gameStarting == false) {
            if (!Input.GetKeyDown(KeyCode.Space)) {
                return;
            }

            StartNewHand();

            return;
        }

        if (handInProgress == true) {
            waitTimer += Time.deltaTime;
            if (waitTimer < waitTime) {
                return;
            }

            // Characters take turns
            Character characterTurn = characters[characterTurnIndex];

            int turnComplete = characterTurn.TakeTurn();

            if (turnComplete == 1) {
                waitTimer = 0f;
                return;
            } else if (turnComplete == 2) {
                Debug.Log(characterTurn.name + "'s turn complete");

                characterTurnIndex += 1;
                waitTimer = 0f;

                if (characterTurnIndex < characters.Count) {
                    // Still more Character's turns to take
                    return;
                }
            } else {
                return;
            }


            // Get winner
            // PrintUpdate();

            Dealer dealer = (Dealer)characters[characters.Count-1];

            bool playerBust = player.hand.GetValue() > 21 ? true : false;
            bool dealerBust = dealer.hand.GetValue() > 21 ? true : false;

            if (playerBust && dealerBust) {
                //Debug.Log("Dealer and Player bust. Tie hand.");
                gameResultText.text = "Dealer and Player bust. Tie hand.";
            } else if (playerBust) {
                //Debug.Log("Player bust. The winner is the Dealer");
                gameResultText.text = "Player bust. The winner is the Dealer";
            } else if (dealerBust) {
                //Debug.Log("The dealer bust. The winner is Player");
                gameResultText.text = "The dealer bust. You won";
            } else if (player.hand.GetValue() > dealer.hand.GetValue()) {
                //Debug.Log("The winner is Player");
                gameResultText.text = "You won";
            } else if (player.hand.GetValue() < dealer.hand.GetValue()) {
                //Debug.Log("The winner is the Dealer");
                gameResultText.text = "The winner is the Dealer";
            } else {
                //Debug.Log("It was a tie hand");
                gameResultText.text = "It was a tie hand";
            }

            handInProgress = false;
            gameResultPanel.SetActive(true);
        }



    }

    void StartNewHand() {
        Debug.Log("Starting new hand");

        gameResultPanel.SetActive(false);

        // Clear old hands
        foreach(Character character in characters) {
            character.hand.ClearHand();
            playedCards.Clear();
        }

        // Deal starting hands
        int cardsDealt = 0;
        for (int i = 0; i < 2; i++) {
            foreach(Character character in characters) {
                // Get a random unplayed card and give to character
                CardDetails card = DealCard();
                //character.hand.AddCard(card);
                StartCoroutine(StartingDeal(character, card, cardsDealt*0.3f));
                cardsDealt++;
            }
        }

        // PrintUpdate();
        StartCoroutine(StartHand(cardsDealt*0.3f));
        gameStarting = true;

        // characterTurnIndex = 0;
        // handInProgress = true;
    }

    private IEnumerator StartingDeal(Character character, CardDetails card, float delay) {
        yield return new WaitForSeconds(delay);

        character.AddCard(card);
    }

    private IEnumerator StartHand(float delay) {
        yield return new WaitForSeconds(delay);

        characterTurnIndex = 0;
        handInProgress = true;
        gameStarting = false;
    }


    public CardDetails DealCard() {
        int selectedCard = -1;

        while (selectedCard == -1) {
            int randCard = Random.Range(0, 52);

            if (!playedCards.Contains(randCard)) {
                selectedCard = randCard;
            }
        }

        playedCards.Add(selectedCard);

        return new CardDetails(selectedCard, deckCards[selectedCard]);
    }




    void PrintUpdate() {
        string temp = "";

        foreach (CardDetails card in characters[1].hand.cards) {
            temp += card.GetValue() + ", ";
        }

        temp = temp.Remove(temp.Length - 2);

        Debug.Log("Dealer has: " + temp);
        
        temp = "";

        foreach (CardDetails card in characters[0].hand.cards) {
            temp += card.GetValue() + ", ";
        }

        temp = temp.Remove(temp.Length - 2);

        Debug.Log("Player has: " + temp);
    }
}

public struct CardDetails {
    public int number;

    public GameObject objCard;
    public Card card;
    public Vector3 startingPos;

    public CardDetails(int number, GameObject objCard) {
        this.number = number;

        this.objCard = objCard;
        this.card = objCard.GetComponent<Card>();
        this.startingPos = objCard.transform.position;
    }

    public int GetValue() {
        int value = number % 13;

        value += 1;

        if (value > 10) {
            value = 10;
        }

        return value;
    }

    public string GetName() {
        int suitInt = number / 13;
        
        return "";
    }
}

public enum Suits {
    Spades,
    Clubs,
    Hearts,
    Diamonds
}
