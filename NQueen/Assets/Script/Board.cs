using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    //-------Vectors ----------------------------
    public static GameObject[,] BoardVector;
    public static GameObject[] StatusVector;

    //-------Scenes------------------------------
    [SerializeField]
    private GameObject SetUpManager;

    [SerializeField]
    private GameObject mBoard;

    //-------Instantiates------------------------

    [SerializeField]
    private GameObject frame;

    [SerializeField]
    private GameObject checkBox;

    [SerializeField]
    private GameObject dark;

    [SerializeField]
    private GameObject light;

    //-------Setting Values---------------------

    private int BoardSize;
    private int BoxSize;

    //-------Debug-------------------------------
    [SerializeField]
    private Text txtDebug;

    //---------------------------------------------------------------------------------------------------------------
    void Start()
    {
        BoardSize = global::SetUpManager.InputBoardSize;
        BoxSize = global::SetUpManager.InputBoxSize;
        StatusVector = new GameObject[BoardSize];
        BoardVector = new GameObject[BoardSize, BoardSize];
        DisplayBoard();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void DisplayBoard()
    {
        for (int b = 0; b < BoardSize; b++)
        {
            GameObject CheckBox = Instantiate(checkBox, new Vector2(BoxSize * b + 700, 1600), transform.rotation);
            CheckBox.transform.SetParent(frame.transform);
            //myCheckBox.GetComponent<Status>().myBoxNumber = b;
            StatusVector[b] = CheckBox;
            CheckBox.transform.name = "Row: " + b.ToString();
        }

        for (int c = 0; c < BoardSize; c++)
        {
            for (int r = 0; r < BoardSize; r++)
            {
                if (c % 2 == 0)
                {
                    if (r % 2 == 0)//even number
                    {
                        GameObject mDark = Instantiate(dark, new Vector2(BoxSize * r + 700 , -BoxSize * c + 1400), transform.rotation);
                        mDark.transform.SetParent(frame.transform);
                        mDark.gameObject.GetComponent<ChessBlock>().SetBlock(r, c);
                        BoardVector[r, c] = mDark;
                        mDark.transform.name = "(" + r.ToString() + "," + c.ToString() + ")";
                    }
                    else //odd number
                    {
                        GameObject mLight = Instantiate(light, new Vector2(BoxSize * r + 700, -BoxSize * c + 1400), transform.rotation);
                        mLight.transform.SetParent(frame.transform);
                        mLight.gameObject.GetComponent<ChessBlock>().SetBlock(r, c);
                        BoardVector[r, c] = mLight;
                        mLight.transform.name = "(" + r.ToString() + "," + c.ToString() + ")";
                    }
                }
                else
                {
                    if (r % 2 == 0)//even number
                    {
                        GameObject mLight = Instantiate(light, new Vector2(BoxSize * r + 700, -BoxSize * c + 1400), transform.rotation);
                        mLight.transform.SetParent(frame.transform);
                        mLight.gameObject.GetComponent<ChessBlock>().SetBlock(r, c);
                        BoardVector[r, c] = mLight;
                        mLight.transform.name = "(" + r.ToString() + "," + c.ToString() + ")";
                    }
                    else //odd number
                    {
                        GameObject mDark = Instantiate(dark, new Vector2(BoxSize * r + 700, -BoxSize * c + 1400), transform.rotation);
                        mDark.transform.SetParent(frame.transform);
                        mDark.gameObject.GetComponent<ChessBlock>().SetBlock(r, c);
                        BoardVector[r, c] = mDark;
                        mDark.transform.name = "(" + r.ToString() + "," + c.ToString() + ")";
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    public void CheckUnderAttack(int row, int col)
    {
        bool result = false;
        for (int i = 0; i < BoardSize; i++)
        {
            if (BoardVector[i, col].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN && (i != row))
                result = true; //check the row
            else if (BoardVector[row, i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN && (i != col))
                result = true; //check the col
            else if ((row - col >= 0) && (row - col + i < BoardSize) && BoardVector[row - col + i, 0 + i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN)
                result = true; //diagnal check 
            else if ((row - col < 0) && (Mathf.Abs(row - col) + i < BoardSize) && BoardVector[0 + i, Mathf.Abs(row - col) + i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN)
                result = true; 
            else if ((row + col < BoardSize) && (row + col - i >= 0) && BoardVector[row + col - i, 0 + i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN)
                result = true;
            else if ((row + col >= BoardSize) && (row + col - i < BoardSize) && (i < BoardSize) && BoardVector[row + col - i, i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.QUEEN)
                result = true;
        }

        if (result)//if the spot is underattack
            BoardVector[row, col].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.UNDERATTACK;
        else // if it is safe
            BoardVector[row, col].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.SAFE;
    }

    //---------------------------------------------------------------------------------------------------------------
    void Update() //run every frame
    {
        for (int c = 0; c < BoardSize; c++)
        {
            for (int r = 0; r < BoardSize; r++)
            {
                if (letsClear) // When I clicked "Clear"
                {
                    for (int i = 0; i < BoardSize; i++) // Cleaning up mCheckBoxes
                    { StatusVector[i].gameObject.GetComponent<Status>().mChecked.SetActive(false); }
                    mRow = 0; mCol = 0;
                    BoardVector[r, c].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.SAFE;
                    if (r == BoardSize - 1 && c == BoardSize - 1)
                        letsClear = false; //Done Cleaning.
                }
                else //All the time = Normal Situation
                {
                    switch (BoardVector[r, c].gameObject.GetComponent<ChessBlock>().MSpot)
                    {
                        case ChessBlock.Spot.UNDERATTACK:
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mMarked.SetActive(true);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mQueen.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mNotThisSpot.SetActive(false);
                            CheckUnderAttack(r, c);
                            break;
                        case ChessBlock.Spot.SAFE:
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mMarked.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mQueen.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mNotThisSpot.SetActive(false);
                            CheckUnderAttack(r, c);
                            break;
                        case ChessBlock.Spot.QUEEN:
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mMarked.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mQueen.SetActive(true);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mNotThisSpot.SetActive(false);
                            break;
                        case ChessBlock.Spot.NOTTHISSPOT:
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mMarked.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mQueen.SetActive(false);
                            BoardVector[r, c].gameObject.GetComponent<ChessBlock>().mNotThisSpot.SetActive(true);
                            break;
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    public int mRow, mCol;
    public int safeSpots;

    public void PlaceTheQueen()
    {
        safeSpots = 0;//When ever this function is called, starts at 0
        for (int c = BoardSize-1; c >= 0; c--) // Count back from bottom -> so I can get the most top at the end.
        {
            if (BoardVector[mRow, c].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.SAFE)
            {
                mCol = c; 
                safeSpots++;
            }
        }
        /*This is Fail situation. 
          I increase myRow, myCol ++ after I place my Queen. So when it reaches the maximum size. 
          When there are no safeSpot, that Column is fail*/
        if (mRow >= BoardSize && mCol >= BoardSize || mCol >= BoardSize || safeSpots == 0)
        {
            Debug.Log("Im looking here Row: " + mRow + "And there are no safe Spot, so fail");
            Debug.Log("My Last Queen R: " + mRow + " C:" + mCol + "is not working");
            //I Failed this Col but there are other options
            if (StatusVector[mRow].gameObject.GetComponent<Status>().mOtherOptionList.Count > 0)
            { 
                BoardVector[mRow, mCol].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.NOTTHISSPOT;
                StatusVector[mRow].gameObject.GetComponent<Status>().mOtherOptionList.RemoveAt(0);
            }
            //I Failed and there are no other options
            else 
            { 
                for(int i = 0; i<BoardSize; i++)
                {
                    //set the whole spot in that col Safe. => even if everythin is safe, they will be re-checked in Update frame
                    BoardVector[mRow, i].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.SAFE;
                    //uncheck my CheckBox
                    StatusVector[mRow].gameObject.GetComponent<Status>().mChecked.SetActive(false);
                }
                mRow--; //Since I increase after my place down, I have to -1 to get current Row.
            }
            //If I didnt reach the fail&no other option <- because I clear the fail part I need current Row,Col with -1 
            mRow = StatusVector[mRow].gameObject.GetComponent<Status>().GetCurrentRow();
            mCol = StatusVector[mRow].gameObject.GetComponent<Status>().GetCurrentCol();
        }
        //Safe -> Place Queen 
        else if (BoardVector[mRow, mCol].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.SAFE)
        {
            for(int i = 0; i<BoardSize; i++)
            {
                //Getting other Option except itself.
                if(i!=mCol && BoardVector[mRow, i].gameObject.GetComponent<ChessBlock>().MSpot == ChessBlock.Spot.SAFE)
                    StatusVector[mRow].gameObject.GetComponent<Status>().mOtherOptionList.Add(BoardVector[mRow, i]);
            }
            BoardVector[mRow, mCol].gameObject.GetComponent<ChessBlock>().MSpot = ChessBlock.Spot.QUEEN ;
            StatusVector[mRow].gameObject.GetComponent<Status>().mChecked.SetActive(true);//Check the Box ON After I place a Queen
            StatusVector[mRow].gameObject.GetComponent<Status>().SetCurrentSpot(mRow, mCol);
            txtDebug.text = "Placed Queen R: " + mRow + " C: " + mCol;
            mRow++; // myRow => next Row to go. 
        }
        else if (mRow < BoardSize)//when I cannot place the queen, move to next Col Col=Y
            mCol++;
    }

    //---------------------------------------------------------------------------------------------------------------
    private bool letsClear;

    public void ClearTheBoard()
    {
        letsClear = true;
        for(int i = 0; i < BoardSize; i++)
        {
            StatusVector[i].gameObject.GetComponent<Status>().SetCurrentSpot(0, 0);
            //myStatusVector[i].gameObject.GetComponent<Status>().SetOtherSpot(0, 0);
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    public float timer;

    public GameObject btnAuto;
    public GameObject btnStopAuto;
    public void Auto()
    {
        //InvokeRepeating("PlaceTheQueen", 0f, 0f);
        InvokeRepeating("PlaceTheQueen", 0.5f, 0.5f);
        btnAuto.SetActive(false);
        btnStopAuto.SetActive(true);

    }

    //---------------------------------------------------------------------------------------------------------------
    public GameObject btnFast;
    public GameObject btnStopFast;
    public void SuperFast()
    {
       
        InvokeRepeating("PlaceTheQueen", 0.1f, 0.1f);
        btnFast.SetActive(false);
        btnStopFast.SetActive(true);

    }

    //---------------------------------------------------------------------------------------------------------------
    public void StopAuto()
    {
        CancelInvoke("PlaceTheQueen");
        btnStopAuto.SetActive(false);
        btnStopFast.SetActive(false);
        btnAuto.SetActive(true);
        btnFast.SetActive(true);
    }
}
