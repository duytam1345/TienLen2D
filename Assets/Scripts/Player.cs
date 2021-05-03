using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<DataCard> dataCard;

    public bool coLuot;

    public string namePlayer;
    public int money;

    public bool hetBai;

    public GameObject info;

    public void SetName()
    {
        info.transform.GetChild(2).GetComponent<Text>().text = name;
    }
    public void SetMoney()
    {
        info.transform.GetChild(3).GetComponent<Text>().text = money.ToString();   
    }
}
