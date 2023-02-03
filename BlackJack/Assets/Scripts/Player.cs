using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Player(Vector3 handPosition) : base(handPosition) {
        name = "Player";
    }

	public override int TakeTurn()
	{
        finishedTurn = 0;

        if (Input.GetKeyDown(KeyCode.H)) {
            Hit();
        } else if (Input.GetKeyDown(KeyCode.S)) {
            Stand();
        }

		return finishedTurn;
	}
}
