using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int cellPosition;
    public GameObject child;
    public Sprite[] sprites;
    private SpriteRenderer sr;
    private bool occupied;

    void Start()
    {
        sr = child.AddComponent<SpriteRenderer>() as SpriteRenderer;
        occupied = false;
    }

    void Update()
    {
    }

    void OnMouseOver()
    {
        if ((Input.GetButtonDown("Fire1") && occupied == false) && GameMaster.playeble) {
            occupied = true;
            sr.sprite = sprites[GameMaster.playerNumber - 1];
            sr.transform.localScale = new Vector3(0.12f, 0.12f, 1);
            sr.sortingOrder = 2;
            GameMaster.makeMove(cellPosition);
        }
    }

    public void deleteSprite()
    {
        occupied = false;
        sr.sprite = null;
    }
}
