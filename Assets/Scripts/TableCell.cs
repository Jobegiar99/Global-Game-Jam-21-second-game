using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TableCell : MonoBehaviour
{

    private GameObject Controller;
    public void UpdateTableCell()
    {
        Controller = GameObject.Find("GameController");
        GameController controller = Controller.GetComponent<GameController>();
        GameObject caller = transform.GetChild(0).gameObject;
        TextMeshProUGUI callerText = caller.GetComponent<TextMeshProUGUI>(); // .GetComponent<TextMeshProUGUI>();
        if (controller.GetPlayerMoves() > 0 && controller.PlayerCanClick)
        {
            UpdateTableCellPlayerHelper(controller, callerText);
        }
    }

    public void UpdateTableCellPlayerHelper( GameController controller, TextMeshProUGUI callerText)
    {
        if (callerText.text == "")
        {
            string stateChange = " ";
            if (controller.Ones > 0)
            {

                stateChange = "1";
                controller.RemoveAOne();
            }
            else
            {

                stateChange = "0";
            }
            controller.UpdateGameState(gameObject.name[7].ToString(), stateChange);
            callerText.text = stateChange;
            controller.ReducePlayerMoves();
            if (controller.GetPlayerMoves() == 0)
            {
                StartCoroutine(controller.CalculateRoundWinner());
            }
        }
        
    }
}
