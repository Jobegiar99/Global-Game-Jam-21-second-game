using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public byte Ones;
    public List<List<string>> GameState;
    public byte CurrentRound;
    private byte PlayerMoves;
    public bool PlayerCanClick;
    byte ScientistVictories = 0;
    byte HackerVictories = 0;
    string GameWinner;
    public GameObject GameOver;

    public List<List<GameObject>> UIButtons;

    // Start is called before the first frame update
    public void Start()
    {
        PlayerMoves = 6;
        UIButtons = new List<List<GameObject>>();
        CurrentRound = 0;
        PlayerCanClick = false;
        GameState = new List<List<string>>
        {
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "},
           new List<string>{" ", " ", " ", " ", " ", " "," "," "}
        };
        GenerateUIButtonsList();
        StartCoroutine(LoadNextButtonRow());
    }

    public void Update()
    {

    }

    public byte GetPlayerMoves()
    {
        return PlayerMoves;
    }
    public void ReducePlayerMoves()
    {
        PlayerMoves--;
    }

    public void GenerateNumberOfOnes()
    {
        Ones = (byte)Random.Range(1, 7);
        GameObject.Find("ONES").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "ONES: " + ((Ones).ToString());
    }

    public void RemoveAOne()
    {
        if (Ones > 0)
            Ones -= 1;
        GameObject.Find("ONES").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "ONES: " + ((Ones).ToString());
    }

    public void ChangePlayer()
    {
        PlayerMoves = 6;
    }

    public IEnumerator LoadNextButtonRow()
    {
        PlayerCanClick = false;
        foreach (GameObject uiItem in UIButtons[CurrentRound])
        {
            uiItem.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
        CurrentRound++;
        if (CurrentRound < 7)
        {
            for (int i = 0; i < 30; i++)
            {
                GenerateNumberOfOnes();
                yield return new WaitForSeconds(0.05f);
            }
        
            PlayerCanClick = true;
            PlayerMoves = 6;
        }
        else
        {
           DecideWinner();
        }
    }

    public void DecideWinner()
    {
        byte Scientific1 = ObtainColumnOnes(0);
        byte A1 = ObtainColumnOnes(1);
        byte B1 = ObtainColumnOnes(2);
        byte C1 = ObtainColumnOnes(3);
        byte D1 = ObtainColumnOnes(4);
        byte E1 = ObtainColumnOnes(5);
        byte F1 = ObtainColumnOnes(6);
        byte Hacker1 = ObtainColumnOnes(7);

        StartCoroutine(DecideWinnerHelper(Scientific1, Hacker1, 0, 7));
        StartCoroutine(DecideWinnerHelper(A1, B1, 1, 2));
        StartCoroutine(DecideWinnerHelper(C1, D1, 3, 4));
        StartCoroutine(DecideWinnerHelper(E1, F1, 5, 6));
        if( ScientistVictories > HackerVictories)
        {
            GameWinner = "Scientist";
        }
        else if ( ScientistVictories < HackerVictories)
        {
            GameWinner = "Hacker";
        }
        else
        {
            GameWinner = "None";
        }

        StartCoroutine(OpenGameOver());
    }

    public IEnumerator OpenGameOver()
    {
        string passwordResult = "";
        List<string> passwords = GeneratePassword(ref passwordResult);
        yield return new WaitForSeconds(4);
        GameObject gameOver = Instantiate(GameOver, transform.position, Quaternion.identity);
        string password = "";
        foreach ( string pass in passwords)
        {
            password += pass + "\n";
        }
        if( GameWinner == "None")
        {
            gameOver.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Noone trespassed the security system.\n\n";
            gameOver.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "The attemped password was:\n" + password + "\n"+ passwordResult;
        }
        else
        {
            gameOver.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "The security system was trespassed by the " + GameWinner;
            gameOver.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "The password is \n" + password + "\n" + passwordResult;
        }
    }

    public List<string> GeneratePassword(ref string passwordResult)
    {
        List<string> passwords = new List<string>();
        for(int i = 1; i < 7; i++)
        {
            string binary = "";

            for(int j = 0; j < GameState[i].Count;j++)
            {
                binary += GameState[i][j];
            }
            passwords.Add(binary + " = " + binaryToDecimal(binary, ref passwordResult));
        }
        return passwords;
    }

    public string binaryToDecimal( string binary, ref string passwordResult)
    {
        int number = 0;
        int increment = 0;
        for(int i = binary.Length -1; i >= 0; i--)
        {
            if( binary[i] == '1')
            {
                number += (int)(Mathf.Pow(2, increment));
            }
            increment++;
        }
        passwordResult += number.ToString() + " ";
        return number.ToString();
    }

    public IEnumerator DecideWinnerHelper(byte optionA, byte optionB, byte optionAI, byte optionBI)
    {
        if (optionA > optionB)
        {
            ScientistVictories++;
            UIButtons[CurrentRound - 1][optionAI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
            yield return new WaitForSeconds(0.5f);
            UIButtons[CurrentRound - 1][optionBI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
        }
        else if( optionA < optionB)
        {
            HackerVictories++;
            UIButtons[CurrentRound - 1][optionAI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
            yield return new WaitForSeconds(0.5f);
            UIButtons[CurrentRound - 1][optionBI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
        }
        else
        {
            UIButtons[CurrentRound - 1][optionAI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
            yield return new WaitForSeconds(0.5f);
            UIButtons[CurrentRound - 1][optionBI].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
        }
        yield return new WaitForSeconds(0.5f);
    }


    public byte ObtainColumnOnes(int column)
    {
        byte count = 0;
        for(int i =0; i < GameState[0].Count; i++)
        {
            if (GameState[i][column] == "1")
                count++;
        }
        return count;
    }

    
    public byte GetRound()
    {
        return CurrentRound;
    }
    public IEnumerator CalculateRoundWinner()
    {
        byte playerPoints =(byte)( byte.Parse(GameState[CurrentRound][1]) + byte.Parse(GameState[CurrentRound][3]) + byte.Parse(GameState[CurrentRound][5]));
        byte hackerPoints = (byte)(byte.Parse(GameState[CurrentRound][2]) + byte.Parse(GameState[CurrentRound][4]) + byte.Parse(GameState[CurrentRound][6]));
        string playerResult, hackerResult;
        if( playerPoints == hackerPoints)
        {
            playerResult = "0";
            hackerResult = "0";
        }
        else if ( playerPoints > hackerPoints)
        {
            playerResult = "1";
            hackerResult = "0";
        }
        else
        {
            playerResult = "0";
            hackerResult = "1";
        }

        GameState[CurrentRound][0] = playerResult;
        GameState[CurrentRound][7] = hackerResult;
        UIButtons[CurrentRound - 1][0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = playerResult;
        yield return new WaitForSeconds(0.25f);
        UIButtons[CurrentRound -1][7].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = hackerResult;
        StartCoroutine(LoadNextButtonRow());
    }

    public void UpdateGameState(string yS, string change)
    {
        byte Y = byte.Parse(yS);
        Y++;
        GameState[CurrentRound][Y] = change;
    }

    private void GenerateUIButtonsList()
    {
        for (int i = 0; i <= 6; i++)
        {
            List<GameObject> tempUIButtons = new List<GameObject>();
            GameObject scientist = GameObject.Find("scientist" + i.ToString() + 0.ToString());
            tempUIButtons.Add(scientist);
            GameObject hacker = GameObject.Find("hacker" + i.ToString() + 0.ToString());
            for (int j = 0; j < 6; j++)
            {
                GameObject button = GameObject.Find("button" + i.ToString() + j.ToString());
                button.gameObject.SetActive(false);
                tempUIButtons.Add(button);
            }
            scientist.gameObject.SetActive(false);
            hacker.gameObject.SetActive(false);

            tempUIButtons.Add(hacker);
            UIButtons.Add(tempUIButtons);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    
}
