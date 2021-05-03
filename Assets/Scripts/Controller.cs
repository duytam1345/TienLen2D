using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Player p;

    public List<GameObject> card;

    public List<Card> selected = new List<Card>();

    public Manager manager;

    public void XoeBai()
    {
        card = new List<GameObject>();

        for (int i = 0; i < 13; i++) {
            GameObject g = Instantiate(Resources.Load("LaBai") as GameObject, Vector3.zero, Quaternion.identity);
            g.GetComponent<Card>().data = p.dataCard[i];
            Sprite s = Resources.Load<Sprite>("Sprite/" + p.dataCard[i].nut.ToString() + p.dataCard[i].chat.ToString());
            g.GetComponent<Card>().sprite.sprite = s;
            card.Add(g);
        }

        XepBai();
    }

    public void XepBai()
    {
        List<GameObject> newCard = new List<GameObject>();

        for (int i = 0; i < 13; i++) {
            for (int j = 0; j < 4; j++) {
                foreach (var item in card) {
                    if ((int)item.GetComponent<Card>().data.chat == j && (int)item.GetComponent<Card>().data.nut == i) {
                        newCard.Add(item);
                        break;
                    }
                }
            }
        }

        card.Clear();
        card = newCard;

        for (int i = 0; i < card.Count; i++) {
            card[i].GetComponent<Card>().sprite.sortingOrder = i;
        }

        float t = 13f / (13f/4.5f);
        t = -(t);
        for (int i = 0; i < card.Count; i++) {
            card[i].transform.position = new Vector3(t, -3.5f, 0);
            t += .75f;
        }
    }

    public void ClickOnCard(Card c)
    {
        if (selected.Contains(c)) {
            c.UnSelect();
        } else {
            c.Select();
        }
    }

    public void Danh()
    {
        List<Card> cDanh = new List<Card>();

        if (manager.turnPlayer == Manager.TurnPlayer.P1) {
            if (selected.Count > 0) {
                if (manager.Check(selected)) {

                    foreach (var item in selected) {
                        cDanh.Add(item);

                        for (int i = 0; i < card.Count; i++) {
                            if (item.data.chat == card[i].GetComponent<Card>().data.chat &&
                                item.data.nut == card[i].GetComponent<Card>().data.nut) {
                                card.RemoveAt(i);
                            }
                        }
                    }

                    if (card.Count == 0) {
                        p.hetBai = true;
                        manager.PlayerHetBai(1);
                    }

                    manager.Danh(cDanh, p);

                    selected.Clear();
                } else {
                    GameObject g = Instantiate(Resources.Load("Text Khong hop le") as GameObject, GameObject.Find("Canvas").transform);
                    Destroy(g, 1f);
                }
            }
        }
    }

    //2 loi~
    //1_ bai cua p1 khong ai an nhung p1 phai an dc la bai do moi dc danh tiep
    //2_ p1 da het bai nhung van toi luot danh


    public void Bo()
    {
        if (manager.turnPlayer == Manager.TurnPlayer.P1) {
            if (manager.groupCard.p != null && manager.groupCard.p != p)
                p.coLuot = false;
            manager.BoLuot();
        }
    }
}
