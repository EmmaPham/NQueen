using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{

    [SerializeField]
    private Text CurrentSpot;

    [SerializeField]
    private Text OtherOption;

    
    public GameObject mChecked;

    private int mCurrentRow;
    private int mCurrentCol;

    
    public List<GameObject> mOtherOptionList;


    public void SetCurrentSpot (int r, int c)
    {
        mCurrentRow = r; mCurrentCol = c;
    }

    public int GetCurrentRow()
    {
        return mCurrentRow;
    }

    public int GetCurrentCol()
    {
        return mCurrentCol;
    }

    void Update()
    {
        CurrentSpot.text = "(" + mCurrentRow + "," + mCurrentCol + ")";
        OtherOption.text = "" + mOtherOptionList.Count;
    }

}

