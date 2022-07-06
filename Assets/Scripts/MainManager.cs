using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public static (int, int) score;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        var textScore = GameObject.Find("Score").GetComponent<TMP_Text>();
        textScore.text = score.Item1 + " - " + score.Item2;
    }
}
