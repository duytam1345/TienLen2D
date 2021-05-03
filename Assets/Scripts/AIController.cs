using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Player p;

    public Manager manager;

    public List<GameObject> card;

    public bool showCard;

    public void ToiLuotDanh()
    {
        StartCoroutine(DanhCo());
    }

    public void XoeBai()
    {
        card = new List<GameObject>();

        for (int i = 0; i < 13; i++) {
            GameObject g = Instantiate(Resources.Load("LaBai") as GameObject, Vector3.zero, Quaternion.identity);
            g.GetComponent<Card>().data = p.dataCard[i];
            string sSprite = "Sprite/" + p.dataCard[i].nut.ToString() + p.dataCard[i].chat.ToString();
            if (!showCard) {
                sSprite = "Sprite/ASD";
            }
            Sprite s = Resources.Load<Sprite>(sSprite);
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

        if (name == "Player 3") {
            if (showCard) {
                int t = card.Count / 2;
                t = -(t);
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(t, 3.5f, 0);
                    t++;
                }
            } else {
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(0, 3.5f, 0);
                }
            }
        } else if (name == "Player 2") {
            if (showCard) {
                int t = card.Count / 2;
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(8, t * .5f, 0);
                    t--;
                }
            } else {
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(8, 0, 0);
                }
            }
        } else if (name == "Player 4") {
            if (showCard) {
                int t = card.Count / 2;
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(-8, t * .5f, 0);
                    t--;
                }
            } else {
                for (int i = 0; i < card.Count; i++) {
                    card[i].transform.position = new Vector3(-8,0, 0);
                }
            }
        }
    }

    IEnumerator DanhCo()
    {
        yield return new WaitForSeconds(.5f);

        List<Card> cardDanh = new List<Card>();

        if (manager.groupCard.p != null) {

            switch (manager.groupCard.a) {
                case GroupCard.AA._Coi:
                //Nếu là cội 2 thì tìm đôi thông chặt or tứ quý
                if (manager.groupCard.data[0].nut == DataCard.Nut._2) {
                    //Tìm tứ quý
                    for (int i = 0; i < card.Count; i++) {
                        bool tuQuy = true;
                        for (int j = 1; j <= 3; j++) {
                            if (i + j < card.Count) {
                                if (card[i].GetComponent<Card>().data.nut != card[i + j].GetComponent<Card>().data.nut) {
                                    tuQuy = false;
                                }
                            } else {
                                tuQuy = false;
                                break;
                            }
                        }

                        if (tuQuy) {
                            print(card[i].GetComponent<DataCard>().nut);
                        }
                    }
                    //Tìm đôi thông
                }

                //Nếu k có tứ quý or đôi thông thì tìm cội thường ăn
                //Up: cội đó phải không trùng vào sảnh or đôi,...
                foreach (var item in card) {
                    if (item.GetComponent<Card>().data.nut == manager.groupCard.data[0].nut) {
                        if (item.GetComponent<Card>().data.chat > manager.groupCard.data[0].chat) {
                            cardDanh.Add(item.GetComponent<Card>());
                            break;
                        }
                    } else if (item.GetComponent<Card>().data.nut > manager.groupCard.data[0].nut) {
                        cardDanh.Add(item.GetComponent<Card>());
                        break;
                    }
                }
                break;
                case GroupCard.AA._Doi:
                //Tim doi
                List<Card> c1 = new List<Card>();
                List<Card> c2 = new List<Card>();

                foreach (var item in card) {
                    foreach (var jtem in card) {
                        if (item.GetComponent<Card>().data.chat != jtem.GetComponent<Card>().data.chat) {
                            if (item.GetComponent<Card>().data.nut == jtem.GetComponent<Card>().data.nut) {
                                c1.Add(item.GetComponent<Card>());
                                c2.Add(jtem.GetComponent<Card>());
                            }
                        }
                    }
                }
                //So sanh
                for (int i = 0; i < c1.Count; i++) {
                    if (c1[i].data.nut == manager.groupCard.data[0].nut) { //= nut thi so chat
                        if (c1[i].data.chat > manager.groupCard.data[0].chat && c1[i].data.chat > manager.groupCard.data[1].chat) {
                            cardDanh.Add(c1[i]);
                            cardDanh.Add(c2[i]);
                            break;
                        }
                    } else if (c1[i].data.nut > manager.groupCard.data[0].nut) { // hon nut thi lay
                        cardDanh.Add(c1[i]);
                        cardDanh.Add(c2[i]);
                        break;
                    }
                }
                break;
                case GroupCard.AA._3Cay:
                bool b = false;
                for (int i = 0; i < card.Count; i++) {
                    if (b) {
                        break;
                    }
                    for (int j = i + 1; j < card.Count; j++) {
                        if (b) {
                            break;
                        }
                        for (int n = j + 1; n < card.Count; n++) {
                            if (card[i].GetComponent<Card>().data.nut == card[j].GetComponent<Card>().data.nut &&
                                card[j].GetComponent<Card>().data.nut == card[n].GetComponent<Card>().data.nut) {

                                if (card[i].GetComponent<Card>().data.nut > manager.groupCard.data[0].nut) {
                                    cardDanh.Add(card[i].GetComponent<Card>());
                                    cardDanh.Add(card[j].GetComponent<Card>());
                                    cardDanh.Add(card[n].GetComponent<Card>());

                                    b = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
                case GroupCard.AA._Sanh3:
                List<Card> c3 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c3.Count != 0) {
                    foreach (var item in c3) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh4:
                List<Card> c4 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c4.Count != 0) {
                    foreach (var item in c4) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh5:
                List<Card> c5 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c5.Count != 0) {
                    foreach (var item in c5) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh6:
                List<Card> c6 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c6.Count != 0) {
                    foreach (var item in c6) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh7:
                List<Card> c7 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c7.Count != 0) {
                    foreach (var item in c7) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh8:
                List<Card> c8 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c8.Count != 0) {
                    foreach (var item in c8) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh9:
                List<Card> c9 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c9.Count != 0) {
                    foreach (var item in c9) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh10:
                List<Card> c10 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c10.Count != 0) {
                    foreach (var item in c10) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh11:
                List<Card> c11 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c11.Count != 0) {
                    foreach (var item in c11) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh12:
                List<Card> c12 = TimSanh(manager.groupCard.data.Count, new DataCard(DataCard.Chat._Bich, DataCard.Nut._2));
                if (c12.Count != 0) {
                    foreach (var item in c12) {
                        cardDanh.Add(item);
                    }
                }
                break;
                case GroupCard.AA._Sanh13:
                break;
                case GroupCard.AA._3DoiThong:
                break;
                case GroupCard.AA._4DoiThong:
                break;
                case GroupCard.AA._5DoiThong:
                break;
                case GroupCard.AA._6DoiThong:
                break;
                case GroupCard.AA._TuQuy:
                break;
            }


        } else {
            foreach (var item in DanhNgauNhien()) {
                cardDanh.Add(item);
            }
        }

        //if(name == "Player 3") {
        //    foreach (var item in card) {
        //        print(item);
        //    }
        //}

        if (cardDanh.Count == 0) {
            p.coLuot = false;
            manager.BoLuot();
        } else {
            foreach (var item in cardDanh) {
                item.GetComponent<Card>().sprite.sprite = Resources.Load<Sprite>("Sprite/" + item.GetComponent<Card>().data.nut.ToString() + item.GetComponent<Card>().data.chat.ToString());
            }

            for (int i = 0; i < cardDanh.Count; i++) {
                for (int j = 0; j < card.Count; j++) {
                    if (cardDanh[i].data.nut == card[j].GetComponent<Card>().data.nut &&
                        cardDanh[i].data.chat == card[j].GetComponent<Card>().data.chat) {
                        card.RemoveAt(j);
                        j--;
                    }
                }
            }

            if (card.Count == 0) {
                p.hetBai = true;

                if (name == "Player 2") {
                    print("2");
                    manager.PlayerHetBai(2);
                } else if (name == "Player 3") {
                    print("3");
                    manager.PlayerHetBai(3);
                } else if (name == "Player 4") {
                    print("4");
                    manager.PlayerHetBai(4);
                }
            }

            manager.Danh(cardDanh, p);
        }
    }

    List<Card> TimSanh(int doDaiSanh, DataCard lonNhat)
    {
        List<Card> dtDeTimSanh = new List<Card>();

        //tim la bai lon nhat cua sanh
        if (lonNhat.nut == DataCard.Nut._2) {
            lonNhat = manager.groupCard.data[0];

            foreach (var item in manager.groupCard.data) {
                if (lonNhat.nut < item.nut) {
                    lonNhat = item;
                }
            }
        }

        //tim nhung la bai co the an duoc la lon nhat cua sanh
        for (int i = 0; i < card.Count; i++) {
            if (card[i].GetComponent<Card>().data.nut == lonNhat.nut) {
                if (card[i].GetComponent<Card>().data.chat > lonNhat.chat) {
                    dtDeTimSanh.Add(card[i].GetComponent<Card>());
                }
            } else if (card[i].GetComponent<Card>().data.nut > lonNhat.nut) {
                dtDeTimSanh.Add(card[i].GetComponent<Card>());
            }
        }

        //loai bo la 2
        for (int i = 0; i < dtDeTimSanh.Count; i++) {
            if (dtDeTimSanh[i].data.nut == DataCard.Nut._2) {
                dtDeTimSanh.RemoveAt(i);
                i--;
            }
        }



        //Tim sanh
        for (int i = 0; i < dtDeTimSanh.Count; i++) {
            //i = so lan lap lai tat ca cac la lon hon da tim duoc
            //n = so thu tu la bai dang kiem tra sanh trong bai cua nguoi choi
            //j lap lai so lan = so la bai cua sanh
            //z la thu tu la bai de kiem tra sanh,z = n-j
            int n = 0;
            for (int m = 0; m < card.Count; m++) {
                if (card[m].GetComponent<Card>().data.nut == dtDeTimSanh[i].data.nut &&
                    card[m].GetComponent<Card>().data.chat == dtDeTimSanh[i].data.chat) {
                    n = m;
                    break;
                }
            }

            List<Card> sanh = new List<Card>();

            int nut = (int)dtDeTimSanh[i].data.nut - 1;

            for (int j = 1; j <= doDaiSanh; j++) {
                if (sanh.Count == doDaiSanh - 1) {
                    //print(dtDeTimSanh[i].data.nut + sanh[0].data.nut.ToString() + sanh[1].data.nut.ToString());
                    sanh.Add(dtDeTimSanh[i]);
                    return sanh;
                }

                for (int z = n - j; z >= 0; z--) {
                    if ((int)card[z].GetComponent<Card>().data.nut == nut) {
                        sanh.Insert(0, (card[z].GetComponent<Card>()));
                        nut--;
                        break;
                    }
                }
            }


        }

        return new List<Card>();
    }

    List<Card> DanhNgauNhien()
    {
        if (manager.firstRound) {
            for (int i = 0; i < card.Count; i++) {
                if (card[i].GetComponent<Card>().data.nut == DataCard.Nut._3 && card[i].GetComponent<Card>().data.chat == DataCard.Chat._Bich) {
                    return new List<Card>() { card[i].GetComponent<Card>() };
                }
            }
        }

        //uu tien sanh
        List<Card> sanh3 = TimSanh(3, new DataCard(DataCard.Chat._Bich, DataCard.Nut._5));
        List<Card> sanh4 = TimSanh(4, new DataCard(DataCard.Chat._Bich, DataCard.Nut._6));
        List<Card> sanh5 = TimSanh(5, new DataCard(DataCard.Chat._Bich, DataCard.Nut._7));
        List<Card> sanh6 = TimSanh(6, new DataCard(DataCard.Chat._Bich, DataCard.Nut._8));
        List<Card> sanh7 = TimSanh(7, new DataCard(DataCard.Chat._Bich, DataCard.Nut._9));
        List<Card> sanh8 = TimSanh(8, new DataCard(DataCard.Chat._Bich, DataCard.Nut._10));

        if (sanh8.Count > 0) {
            return sanh7;
        } else {
            if (sanh7.Count > 0) {
                return sanh7;
            } else {
                if (sanh6.Count > 0) {
                    return sanh6;
                } else {
                    if (sanh5.Count > 0) {
                        return sanh5;
                    } else {
                        if (sanh4.Count > 0) {
                            return sanh4;
                        } else {
                            if (sanh3.Count > 0) {
                                return sanh3;
                            }
                        }
                    }
                }
            }
        }

        //uu tien doi
        List<Card> c1 = new List<Card>();
        List<Card> c2 = new List<Card>();

        foreach (var item in card) {
            foreach (var jtem in card) {
                if (item.GetComponent<Card>().data.chat != jtem.GetComponent<Card>().data.chat) {
                    if (item.GetComponent<Card>().data.nut == jtem.GetComponent<Card>().data.nut) {
                        c1.Add(item.GetComponent<Card>());
                        c2.Add(jtem.GetComponent<Card>());
                    }
                }
            }
        }

        if (c1.Count > 0) {
            return new List<Card>() { c1[0], c2[0] };
        }
        //

        //coi
        List<Card> c = new List<Card>();
        c.Add(card[0].GetComponent<Card>());
        //

        return c;
    }
}
