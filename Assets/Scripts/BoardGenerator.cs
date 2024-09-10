using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BoardGenerator : MonoBehaviour
{

    [SerializeField] private List<Sprite> colors;
    [SerializeField] private List<GameObject> textAreas;
    [SerializeField] public List<GameObject> tiles;

    [SerializeField] private Sprite ButtonOK;
    [SerializeField] private Sprite ButtonNOK;

    [SerializeField] private GameObject Button_6_8;
    [SerializeField] private GameObject Button_2_12;
    [SerializeField] private GameObject Button_Same_Res;
    [SerializeField] private GameObject Button_Same_Num;


    private bool canTouch_6_8;
    private bool canTouch_2_12;
    private bool canTouch_Same_Numbers;
    private bool canTouch_Same_Resources;

    private void Start()
    {
        canTouch_6_8 = true;
        canTouch_2_12 = true;
        canTouch_Same_Numbers = true;
        canTouch_Same_Resources = true;

        Generate();
        
    }

    public void Generate()
    {
        Vibrate("hard");
        GenerateTiles();
        GenerateNumbers();

    }

    private void GenerateTiles()
    {
    again:

        List<int> tileColors = new()
        {
            0,1,1,1,1,2,2,2,2,3,3,3,4,4,4,5,5,5,5
        };

        List<int> tmpTileColors = new List<int>();

        List<GameObject> tmpTiles = new List<GameObject>();

        foreach (var item in tiles)
        {
            tmpTiles.Add(item);
        }


        System.Random random = new System.Random();

        while (tmpTiles.Count > 0)
        {

            int randomIndex = random.Next(tmpTiles.Count);

            GameObject selectedTile = tmpTiles[randomIndex];

            tmpTileColors.Clear();

            foreach (var item in tileColors)
            {
                tmpTileColors.Add(item);
            }

            if (canTouch_Same_Resources)
            {
                int randomColorIndex = random.Next(tileColors.Count);

                int selectedColor = tileColors[randomColorIndex];


                tmpTiles.RemoveAt(randomIndex);

                tileColors.RemoveAt(randomColorIndex);


                selectedTile.GetComponent<Image>().sprite = colors[selectedColor];
            }
            else
            {
                foreach (var item in selectedTile.GetComponent<tile>().neighboors)
                {
                    Sprite color = item.GetComponent<Image>().sprite;

                    Sprite theColor = colors.Find(a => a == color);


                    if (theColor != null)
                    {
                        int colorNum = colors.IndexOf(theColor);

                        tmpTileColors.RemoveAll(x => x == colorNum);

                    }

                }

                if (tmpTileColors.Count == 0)
                {
                    goto again;
                }

                int randomColorIndex = random.Next(tmpTileColors.Count);

                int selectedColor = tmpTileColors[randomColorIndex];

                tileColors.Remove(selectedColor);

                selectedTile.GetComponent<Image>().sprite = colors[selectedColor];

                tmpTiles.Remove(selectedTile);

            }

        }
    }

    private void GenerateNumbers()
    {
    again:

        int counter = 0;

        List<int> tileNumbers = new()
        {
            2,3,3,4,4,5,5,6,6,8,8,9,9,10,10,11,11,12
        };

        List<int> tmpTileNumbers = new List<int>();

        List<GameObject> tmpTexts = new List<GameObject>();

        foreach (var item in textAreas)
        {
            item.GetComponent<TextMeshProUGUI>().text = "0";
            tmpTexts.Add(item);
        }

        foreach (var item in textAreas)
        {
            item.SetActive(true);
        }

        System.Random random = new System.Random();

        while (tmpTexts.Count > 0)
        {

            int randomIndex = random.Next(tmpTexts.Count);

            GameObject selectedTile = tmpTexts[randomIndex];

            GameObject parent = selectedTile.transform.parent.gameObject;

            tmpTileNumbers.Clear();

            foreach (var item in tileNumbers)
            {
                tmpTileNumbers.Add(item);
            }

            if (canTouch_Same_Numbers && canTouch_6_8 && canTouch_2_12)
            {

                if (parent.GetComponentInParent<Image>().sprite != colors[0])
                {

                    int randomNumIndex = random.Next(tileNumbers.Count);

                    int selectedNum = tileNumbers[randomNumIndex];

                    selectedTile.GetComponentInChildren<TextMeshProUGUI>().color = GiveColor(selectedNum);

                    tileNumbers.RemoveAt(randomNumIndex);

                    selectedTile.GetComponentInChildren<TextMeshProUGUI>().text = selectedNum.ToString();

                }
                else
                {
                    tmpTexts[randomIndex].SetActive(false);
                }

                tmpTexts.Remove(selectedTile);

            }
            else
            {
                Debug.Log("NEIGHBOOR COUNT = " + parent.GetComponent<tile>().neighboors.Count);

                foreach (var neighboor in parent.GetComponent<tile>().neighboors)
                {
                    GameObject neighboorChild = neighboor.transform.GetChild(0).gameObject;
                    int theNumber = int.Parse(neighboorChild.GetComponent<TextMeshProUGUI>().text);

                    Debug.Log(counter + " neighboors = " + theNumber);

                    if (theNumber != 0)
                    {
                        if (theNumber == 6 || theNumber == 8)
                        {
                            if (!canTouch_6_8)
                            {
                                tmpTileNumbers.RemoveAll(x => x == 6);
                                tmpTileNumbers.RemoveAll(x => x == 8);

                            }

                        }
                        if (theNumber == 2 || theNumber == 12)
                        {
                            if (!canTouch_2_12)
                            {
                                tmpTileNumbers.RemoveAll(x => x == 2);
                                tmpTileNumbers.RemoveAll(x => x == 12);
                            }

                        }
                        if (theNumber == 3 || theNumber == 4 || theNumber == 5 || theNumber == 9 || theNumber == 10 || theNumber == 11)
                        {
                            if (!canTouch_Same_Numbers)
                            {
                                tmpTileNumbers.RemoveAll(x => x == theNumber);

                            }

                        }

                    }

                }

                if (tmpTileNumbers.Count == 0)
                {
                    goto again;
                }

                if (parent.GetComponentInParent<Image>().sprite != colors[0])
                {

                    int randomNumIndex = random.Next(tmpTileNumbers.Count);

                    int selectedNum = tmpTileNumbers[randomNumIndex];

                    String iCanBe;

                    iCanBe = "I can be:";

                    foreach (var item in tmpTileNumbers)
                    {
                        iCanBe += " ";
                        iCanBe += item.ToString();
                    }

                    Debug.Log(iCanBe);

                    Debug.Log(counter + " I am: " + selectedNum);
                    counter++;

                    selectedTile.GetComponentInChildren<TextMeshProUGUI>().color = GiveColor(selectedNum);

                    tmpTileNumbers.Remove(selectedNum);

                    tileNumbers.Remove(selectedNum);

                    selectedTile.GetComponentInChildren<TextMeshProUGUI>().text = selectedNum.ToString();

                    tmpTexts.Remove(selectedTile);

                }
                else
                {
                    tmpTexts[randomIndex].SetActive(false);
                    tmpTexts.Remove(tmpTexts[randomIndex]);
                }


            }

        }
    }

    private Color32 GiveColor(int num)
    {
        if (num == 8 || num == 6)
        {

            return new Color32(230, 70, 64, 255);
        }
        else
        {
            return new Color32(250, 250, 250, 255);
        }

    }

    public void canTouch_6_8_Button()
    {
        Vibrate("soft");
        if (canTouch_6_8)
        {
            canTouch_6_8 = false;
            Button_6_8.GetComponent<Image>().sprite = ButtonNOK;
        }
        else
        {
            canTouch_6_8 = true;
            Button_6_8.GetComponent<Image>().sprite = ButtonOK;
        }
    }
    public void canTouch_2_12_Button()
    {
        Vibrate("soft");
        if (canTouch_2_12)
        {
            canTouch_2_12 = false;
            Button_2_12.GetComponent<Image>().sprite = ButtonNOK;
        }
        else
        {
            canTouch_2_12 = true;
            Button_2_12.GetComponent<Image>().sprite = ButtonOK;
        }
    }
    public void canTouch_Same_Res_Button()
    {
        Vibrate("soft");
        if (canTouch_Same_Resources)
        {
            canTouch_Same_Resources = false;
            Button_Same_Res.GetComponent<Image>().sprite = ButtonNOK;
        }
        else
        {
            canTouch_Same_Resources = true;
            Button_Same_Res.GetComponent<Image>().sprite = ButtonOK;
        }
    }
    public void canTouch_Same_Num_Button()
    {
        Vibrate("soft");
        if (canTouch_Same_Numbers)
        {
            canTouch_Same_Numbers = false;
            Button_Same_Num.GetComponent<Image>().sprite = ButtonNOK;
        }
        else
        {
            canTouch_Same_Numbers = true;
            Button_Same_Num.GetComponent<Image>().sprite = ButtonOK;
        }
    }

    public void Vibrate(string type)
    {
        switch (type)
        {
            case "soft":

                Taptic.Light();

                break;

            case "hard":

                Taptic.Medium();

                break;
        }
    }
}
