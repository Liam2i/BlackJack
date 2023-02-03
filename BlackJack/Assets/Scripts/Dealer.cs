using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : Character
{
    bool flippedCard = false;

    public Dealer(Vector3 handPosition) : base(handPosition) {
        name = "Dealer";
    }

	public override int TakeTurn()
	{
        if (flippedCard == false) {
            hand.cards[0].card.Flip();
            flippedCard = true;
        }

        finishedTurn = 0;
        
        if (hand.GetValue() < 17) {
            Hit();
        } else {
            Stand();
        }

        if (finishedTurn == 2) {
            flippedCard = false;
        }

		return finishedTurn;
	}

	public override void AddCard(CardDetails card) {
		if (hand.cards.Count == 0) {
            hand.AddCard(card, false);
        } else {
            hand.AddCard(card, true);
        }
	}
}
