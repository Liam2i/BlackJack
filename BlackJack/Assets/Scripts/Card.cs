using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool flipped;

    public bool move = false;
    public Vector3 startingPos;
    public Vector3 targetPos;
    public float moveTimer = 0f;

    public bool flip = false;

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move == true) {
            moveTimer += Time.deltaTime * 2; // 0.5 seconds
            DoMove();
        } else if (flip == true) {
            DoFlip();
        }
    }

    public void Move(Vector3 startingPosition, Vector3 targetPosition) {
        startingPos = startingPosition;
        targetPos = targetPosition;
        move = true;
        GameManager.gameManager.audioSource.PlayOneShot(GameManager.gameManager.audioClip);
    }

    public void DoMove() {
        transform.position = Vector3.Lerp(startingPos, targetPos, moveTimer);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f) {
            transform.position = targetPos;
            move = false;
            moveTimer = 0f;
        }
    }

    public void Flip() {
        flip = true;
    }

    public void DoFlip() {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        //animator.SetTrigger("Flip");
        flip = false;
    }
}
