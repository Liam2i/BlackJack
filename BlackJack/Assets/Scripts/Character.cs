using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public string name;
    protected int finishedTurn;

    public Hand hand;

    protected Character(Vector3 handPosition) {
        hand = new Hand(new List<CardDetails>(), handPosition);
        finishedTurn = 0;
    }

    public abstract int TakeTurn();

    public virtual void AddCard(CardDetails card) {
        hand.AddCard(card, true);
    }

    protected void Hit() {
        CardDetails card = GameManager.gameManager.DealCard();
        hand.AddCard(card, true);

        // Debug.Log("Hit: " + card.GetValue());
        finishedTurn = 1;

        if (hand.GetValue() > 21) {
            Stand();
        }

    }

    protected void Stand() {
        finishedTurn = 2;

        Debug.Log("Stand: " + name + " has " + hand.PrintHand());
    }
}

public struct Hand {
    public Vector3 position;
    public List<CardDetails> cards;

    public Hand(List<CardDetails> cards, Vector3 position) {
        this.cards = cards;
        this.position = position;
    }

    public void AddCard(CardDetails card, bool flip)
	{
        card.card.Move(card.objCard.transform.position, new Vector3(position.x + (cards.Count * 1.5f), position.y, position.z));
        if (flip) {
            card.card.Flip();
        }
		
        cards.Add(card);
	}

    public void ClearHand() {
        foreach(CardDetails card in cards) {
            card.objCard.transform.position = card.startingPos;
            card.objCard.transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        cards.Clear();
    }

    public int GetValue() {
        // Finds the highest point total of cards, including Aces
        int total = 0;
        bool ace = false;

        foreach(CardDetails card in cards) {
            total += card.GetValue();

            if (card.GetValue() == 1) {
                ace = true;
            }
        }

        // Check if Ace can be 11
        if (ace == true && total <= 11) {
            total += 10;
        }

        return total;
    }

    public string PrintHand() {
        string temp = "";

        foreach (CardDetails card in cards) {
            temp += card.GetValue() + ", ";
        }

        return temp.Remove(temp.Length - 2);
    }
}
