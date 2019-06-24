using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SetUpManager : MonoBehaviour
{
    public static int InputBoardSize = 1;
    public static int InputBoxSize = 1;

    [SerializeField]
    private InputField mInput;

    [SerializeField]
    private GameObject Board;

    [SerializeField]
    private GameObject SetUp;

    public void GetInput()
    {
        if (mInput.text == "")
        {
            mInput.text = "8";
        }
        else if (mInput.text == "0")
        {
            mInput.text = "Should be greater than 3";
        }
        else if (mInput.text == "1")
        {
            mInput.text = "Should be greater than 3";
        }
        else if (mInput.text == "2")
        {
            mInput.text = "Should be greater than 3";
        }
        else if (mInput.text == "3")
        {
            mInput.text = "Should be greater than 3";
        }

        InputBoardSize = Convert.ToInt32(mInput.text);
        InputBoxSize = 960 / InputBoardSize;
        Board.SetActive(true);
        SetUp.SetActive(false);
    }
}
