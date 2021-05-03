using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataCard
{
    public Player player;

    public enum Chat
    {
        _Bich,
        _Chuon,
        _Ro,
        _Co
    }

    public enum Nut
    {
        _3, _4, _5, _6, _7, _8, _9, _10, _J, _Q, _K, _A, _2,
    }

    public Chat chat;
    public Nut nut;

    public DataCard(Chat newA, Nut newB)
    {
        chat = newA;
        nut = newB;
    }
}
