using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static int playerNumber;
    public static bool playeble;
    private static int[,] matrix;
    private static int sizeX, sizeY;
    private static int moveNumber;
    private static GameObject textObj;
    private static Vector2Int lastMove;
    public UnityEngine.Material material;

    void Start()
    {
        playerNumber = 1;
        sizeX = sizeY = 3;
        matrix = new int[sizeX, sizeY];
        playeble = true;
        moveNumber = 0;
        textObj = GameObject.Find("Text Background");
        textObj.SetActive(false);
    }

    void Update()
    {
        if (playeble == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    public static void makeMove(Vector2Int cellPosition)
    {
        moveNumber++;
        matrix[cellPosition.x, cellPosition.y] = playerNumber;
        lastMove = cellPosition;
        int winner = checkWinner();
        if (winner > 0)
        {
            Debug.Log("Player " + winner + " is the winner");
            endGame(winner);
        }

        if(moveNumber == 9)
        {
            endGame(0);
        }

        playerNumber = playerNumber % 2 + 1;
    }

    public static void undo()
    {
        if (lastMove != new Vector2Int(-1, -1))
        {
            moveNumber--;
            playerNumber = playerNumber % 2 + 1;
            matrix[lastMove.x, lastMove.y] = 0;
            int cellNr = lastMove.x * 3 + lastMove.y;
            var cellObj = GameObject.Find("Cell (" + cellNr + ")");
            cellObj.GetComponent<Cell>().deleteSprite();
            lastMove = new Vector2Int(-1, -1);
        }
    }

    private static int checkWinner()
    {
        int x, y;
        Vector3 point1 = new Vector3(0, 0, 1);
        Vector3 point2 = new Vector3(0, 0, 1);
        bool wins;
        int winner = 0;

        // check lines
        for(x = 0; x < sizeX; x++) {
            wins = true;
            for(y = 1; y < sizeY; y++) {
                if (matrix[x, y] != matrix[x, y - 1])
                    wins = false;
            }

            if (wins && matrix[x, 0] > 0)
            {
                winner = matrix[x, 0];
                point1 = new Vector3(-0.5f, 2 - x, 1);
                point2 = new Vector3(2.5f, 2 - x, 1);
            }
        }
        // check columns
        for (y = 0; y < sizeX; y++)
        {
            wins = true;
            for (x = 1; x < sizeY; x++)
            {
                if (matrix[x, y] != matrix[x - 1, y])
                    wins = false;
            }

            if (wins && matrix[0, y] > 0)
            {
                winner = matrix[0, y];
                point1 = new Vector3(y, -0.5f, 1);
                point2 = new Vector3(y, 2.5f, 1);
            }
        }
        // diagonal 1
        wins = true;
        for (x = y = 1; x < sizeX; x++, y++)
        {
            if (matrix[x, y] != matrix[x - 1, y - 1])
                wins = false;
        }
        if (wins && matrix[0, 0] > 0)
        {
            winner = matrix[0, 0];
            point1 = new Vector3(-0.5f, 2.5f, 1);
            point2 = new Vector3(2.5f, -0.5f, 1);
        }
        // diagonal 2
        
        wins = true;
        for (x = 1, y = 1; x < sizeX; x++, y--)
        {
            if (matrix[x, y] != matrix[x - 1, y + 1])
                wins = false;
        }
        if (wins && matrix[0, 2] > 0)
        {
            winner = matrix[0, 2];
            point1 = new Vector3(-0.5f, -0.5f, 1);
            point2 = new Vector3(2.5f, 2.5f, 1);
        }

        if (winner > 0)
        {
            // draw line
            point1 -= new Vector3(1, 1, 0);
            point2 -= new Vector3(1, 1, 0);

            point1 *= 2.5f;
            point2 *= 2.5f;

            point1 -= new Vector3(0, 0.6f, 0);
            point2 -= new Vector3(0, 0.6f, 0);

            GameObject board = GameObject.Find("Board");
            
            LineRenderer l = board.AddComponent<LineRenderer>();

            List<Vector3> pos = new List<Vector3>();
            pos.Add(point1);
            pos.Add(point2);
            l.startWidth = 0.2f;
            l.endWidth = 0.2f;
            l.SetPositions(pos.ToArray());
            l.useWorldSpace = true;
            l.material = board.GetComponent<GameMaster>().material;
            l.sortingOrder = 4;
            var col = new Color(30f/255f, 30f/255f, 30f/255f);
            l.startColor = col;
            l.endColor = col;
        }

        return winner;
    }

    private static void endGame(int winner)
    {
        playeble = false;
        textObj.SetActive(true);
        var text = GameObject.Find("End Message").GetComponent<TMP_Text>();
        Debug.Log("winner is " + winner);
        if (winner > 0)
            text.text = "Player " + winner + " wins!";
        else
            text.text = "Its a tie!";

        if (winner == 1)
            MainManager.score.Item1++;
        else if (winner == 2)
            MainManager.score.Item2++;
    }
}
