using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GroupCard
{
    public enum AA
    {
        _Coi,
        _Doi,
        _3Cay,
        _Sanh3,
        _Sanh4,
        _Sanh5,
        _Sanh6,
        _Sanh7,
        _Sanh8,
        _Sanh9,
        _Sanh10,
        _Sanh11,
        _Sanh12,
        _Sanh13,
        _3DoiThong,
        _4DoiThong,
        _5DoiThong,
        _6DoiThong,
        _TuQuy,
    }

    public AA a;
    public List<DataCard> data;

    public Player p;
}

public class Manager : MonoBehaviour
{
    public Text text;

    public enum StateGame
    {
        DoiVanMoi,
        ChiaBai,
        ChoiBai,
        KetQua
    }

    public StateGame stateGame;

    public enum TurnPlayer
    {
        P1,
        P2,
        P3,
        P4,
    }

    public TurnPlayer turnPlayer;

    public Controller p1;
    public AIController p2;
    public AIController p3;
    public AIController p4;

    public int sortI;
    public GroupCard groupCard;

    public bool wait;

    public int indexP1;
    public int indexP2;
    public int indexP3;
    public int indexP4;

    public GameObject panelSpaceToPlay;

    public GameObject panelKetQua;
    public Text nameP1;
    public Text nameP2;
    public Text nameP3;
    public Text nameP4;

    public bool firstRound;

    private void Start()
    {
        p1.p.SetName();
        p2.p.SetName();
        p3.p.SetName();
        p4.p.SetName();
    }

    public void Update()
    {
        text.text = turnPlayer.ToString();

        if (Input.GetKeyDown(KeyCode.T)) {

        }

        if (Input.GetKeyDown(KeyCode.Q)) {

            foreach (Transform item in GameObject.Find("Group Card").transform) {
                Destroy(item.gameObject);
            }

            groupCard.p = null;

            firstRound = true;
            TaoBai();
        }

        switch (stateGame) {
            case StateGame.DoiVanMoi:
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (panelSpaceToPlay.activeInHierarchy) {
                    panelSpaceToPlay.SetActive(false);
                }

                stateGame = StateGame.ChiaBai;
                ChiaBai();
            }
            break;
            case StateGame.ChiaBai:
            break;
            case StateGame.ChoiBai:
            break;
            case StateGame.KetQua:
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (panelKetQua.activeInHierarchy) {
                    panelKetQua.SetActive(false);
                }
                stateGame = StateGame.DoiVanMoi;

                if (!panelSpaceToPlay.activeInHierarchy) {
                    panelSpaceToPlay.SetActive(true);
                }
            }
            break;
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.Length > 0) {
                Card c = hit[0].collider.GetComponent<Card>();
                foreach (var item in hit) {
                    if (item.collider.GetComponent<Card>().sprite.sortingOrder > c.sprite.sortingOrder) {
                        c = item.collider.GetComponent<Card>();
                    }

                }
                p1.ClickOnCard(c);
            }
        }
    }

    void ChiaBai()
    {
        StartCoroutine(ChiaBaiCo());
    }

    IEnumerator ChiaBaiCo()
    {
        int i = 52;
        int p = 1;

        List<GameObject> l = new List<GameObject>();

        float z = Random.Range(-15f, 15f);

        while (i > 0) {
            GameObject g = Instantiate(Resources.Load("Empty Card") as GameObject, Vector3.zero, Quaternion.identity);
            l.Add(g);
            g.transform.eulerAngles = new Vector3(0, 0, z);
            if (p == 1) {
                g.transform.DOMove(p1.transform.position, .2f);
            } else if (p == 2) {
                g.transform.DOMove(p2.transform.position, .2f);
            } else if (p == 3) {
                g.transform.DOMove(p3.transform.position, .2f);
            } else if (p == 4) {
                g.transform.DOMove(p4.transform.position, .2f);
            }

            i--;

            p++;
            if (p >= 5) {
                p = 1;
            }

            yield return new WaitForSeconds(.1f);

            z = Random.Range(-15f, 15f);
        }

        yield return new WaitForSeconds(.2f);

        foreach (var item in l) {
            Destroy(item);
        }

        TaoBai();
        stateGame = StateGame.ChoiBai;
    }

    void TaoBai()
    {
        foreach (var item in p1.card) {
            Destroy(item);
        }
        p1.p.dataCard.Clear();
        foreach (var item in p2.card) {
            Destroy(item);
        }
        p2.p.dataCard.Clear();
        foreach (var item in p3.card) {
            Destroy(item);
        }
        p3.p.dataCard.Clear();
        foreach (var item in p4.card) {
            Destroy(item);
        }
        p4.p.dataCard.Clear();

        List<DataCard> datas = new List<DataCard>();

        for (int a = 0; a < 4; a++) {
            for (int b = 0; b < 13; b++) {
                datas.Add(new DataCard((DataCard.Chat)a, (DataCard.Nut)b));
            }
        }

        int r = Random.Range(0, datas.Count);

        for (int i = 0; i < 13; i++) {
            datas[r].player = p1.p;
            p1.p.dataCard.Add(datas[r]);
            datas.RemoveAt(r);
            r = Random.Range(0, datas.Count);
        }

        for (int i = 0; i < 13; i++) {
            datas[r].player = p2.p;
            p2.p.dataCard.Add(datas[r]);
            datas.RemoveAt(r);
            r = Random.Range(0, datas.Count);
        }

        for (int i = 0; i < 13; i++) {
            datas[r].player = p3.p;
            p3.p.dataCard.Add(datas[r]);
            datas.RemoveAt(r);
            r = Random.Range(0, datas.Count);
        }

        for (int i = 0; i < 13; i++) {
            datas[0].player = p4.p;
            p4.p.dataCard.Add(datas[0]);
            datas.RemoveAt(0);
        }

        p1.XoeBai();
        p2.XoeBai();
        p3.XoeBai();
        p4.XoeBai();

        if (firstRound) {
            //nguoi choi cam la 3 bich duoc di truoc
            foreach (var item in FindObjectsOfType<Card>()) {
                if (item.data.nut == DataCard.Nut._3 && item.data.chat == DataCard.Chat._Bich) {
                    if (item.data.player == p1.p) {
                        turnPlayer = TurnPlayer.P1;
                    } else if (item.data.player == p2.p) {
                        turnPlayer = TurnPlayer.P2;
                        p2.ToiLuotDanh();
                    } else if (item.data.player == p3.p) {
                        turnPlayer = TurnPlayer.P3;
                        p3.ToiLuotDanh();
                    } else if (item.data.player == p4.p) {
                        turnPlayer = TurnPlayer.P4;
                        p4.ToiLuotDanh();
                    }
                    break;
                }
            }
        } else {
            switch (turnPlayer) {
                case TurnPlayer.P1:
                break;
                case TurnPlayer.P2:
                p2.ToiLuotDanh();
                break;
                case TurnPlayer.P3:
                p3.ToiLuotDanh();
                break;
                case TurnPlayer.P4:
                p4.ToiLuotDanh();
                break;
            }
        }
    }

    bool CheckSanh(List<DataCard> cards)
    {
        //Co la 2 thi loai bo
        foreach (var item in cards) {
            if (item.nut == DataCard.Nut._2) {
                return false;
            }
        }

        //sap xep
        List<DataCard> d = new List<DataCard>();

        int nNhoNhat = 0;
        int index = 0;
        while (cards.Count > 0 && index <= 1000) {
            index++;
            bool b = false;
            for (int i = 0; i < cards.Count; i++) {
                if (cards[i].nut == (DataCard.Nut)nNhoNhat) {
                    b = true;
                    d.Add(cards[i]);
                    cards.RemoveAt(i);
                }
            }
            if (!b) {
                nNhoNhat++;
            }
        }

        //

        //kiem tra
        int n = (int)d[0].nut + 1;
        for (int i = 1; i < d.Count; i++) {
            if (d[i].nut != (DataCard.Nut)n) {
                return false;
            }
            n++;
        }
        return true;
        //
    }

    bool CheckDoiThong(List<DataCard> cards)
    {
        //sap xep
        List<DataCard> d = new List<DataCard>();

        int nNhoNhat = 0;
        int index = 0;
        while (cards.Count > 0 && index <= 1000) {
            index++;
            bool b = false;
            for (int i = 0; i < cards.Count; i++) {
                if (cards[i].nut == (DataCard.Nut)nNhoNhat) {
                    b = true;
                    d.Add(cards[i]);
                    cards.RemoveAt(i);
                }
            }
            if (!b) {
                nNhoNhat++;
            }
        }

        //

        //kiem tra
        int indexChan = 0;
        int indexLe = 1;

        while (indexChan < d.Count && indexLe < d.Count) {
            if (d[indexChan].nut != d[indexLe].nut) {
                return false;
            } else {
                indexChan += 2;
                indexLe += 2;
            }
        }

        for (int i = 0; i < d.Count - 2; i++) {
            if ((int)d[i].nut + 1 != (int)d[i + 2].nut) {
                return false;
            }
        }

        return true;
        //
    }

    public bool Check(List<Card> c)
    {
        if (groupCard.p == null) {
            if (firstRound) {
                foreach (var item in c) {
                    if (item.data.nut == DataCard.Nut._3 && item.data.chat == DataCard.Chat._Bich) {
                        return true;
                    }
                }
                return false;
            }

            if (c.Count == 2) { //2 la
                if (c[0].data.nut == c[1].data.nut) {
                    return true;
                } else {
                    return false;
                }
            } else if (c.Count == 3) { //3 la
                if (c[0].data.nut == c[1].data.nut && c[1].data.nut == c[2].data.nut) {
                    return true;
                } else {
                    List<DataCard> dtCard = new List<DataCard>();
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }
                    return CheckSanh(dtCard);
                }
            } else if (c.Count == 4) {//4 la
                if (c[0].data.nut == c[1].data.nut &&
                     c[1].data.nut == c[2].data.nut &&
                    c[2].data.nut == c[3].data.nut) {
                    return true;
                } else {
                    List<DataCard> dtCard = new List<DataCard>();
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }
                    return CheckSanh(dtCard);
                }
            } else if (c.Count == 5) {//5 la
                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                return CheckSanh(dtCard);
            } else if (c.Count == 6) {//6 la

                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }

                if (CheckSanh(dtCard)) {
                    return true;
                } else {
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }
                    if (CheckDoiThong(dtCard)) {
                        return true;
                    } else {
                        return false;
                    }
                }
            } else if (c.Count == 7) {//7 la
                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                return CheckSanh(dtCard);
            } else if (c.Count == 8) {//8 la

                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }

                if (CheckSanh(dtCard)) {
                    return true;
                } else {
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }
                    if (CheckDoiThong(dtCard)) {
                        return true;
                    } else {
                        return false;
                    }
                }
            } else if (c.Count == 9) {//9 la
                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                return CheckSanh(dtCard);
            } else if (c.Count == 10) {//10 la

                List<DataCard> dtCard = new List<DataCard>();
                foreach (var item in c) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }

                if (CheckSanh(dtCard)) {
                    return true;
                } else {
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }
                    if (CheckDoiThong(dtCard)) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            return true;
        } else {
            switch (groupCard.a) {
                case GroupCard.AA._Coi:
                if (groupCard.data[0].nut == DataCard.Nut._2) {
                    if (c.Count == 8) {
                        //4 đôi thông
                        return false;
                    } else if (c.Count == 6) {
                        //3 đôi thông
                        List<DataCard> dtCard = new List<DataCard>();
                        foreach (var item in c) {
                            dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                        }
                        if (CheckDoiThong(dtCard)) {
                            return true;
                        } else {
                            return false;
                        }
                    } else if (c.Count == 4) {
                        //tứ quý
                        if (c[0].data.nut == c[1].data.nut &&
                            c[1].data.nut == c[2].data.nut &&
                            c[2].data.nut == c[3].data.nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else if (c.Count == 1) {

                        if (c[0].data.nut == groupCard.data[0].nut) {
                            if (c[0].data.chat <= groupCard.data[0].chat) {
                                return false;
                            } else {
                                return true;
                            }
                        } else if (c[0].data.nut > groupCard.data[0].nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    if (c.Count == 1) {

                        if (c[0].data.nut == groupCard.data[0].nut) {
                            if (c[0].data.chat <= groupCard.data[0].chat) {
                                return false;
                            } else {
                                return true;
                            }
                        } else if (c[0].data.nut > groupCard.data[0].nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }
                case GroupCard.AA._Doi:
                if (c.Count != 2) {
                    return false;
                } else {
                    if (c[0].data.nut > groupCard.data[0].nut) {
                        return true;
                    } else if (c[0].data.nut == groupCard.data[0].nut) {
                        DataCard.Chat cLonNhat = c[0].data.chat > c[1].data.chat ? c[0].data.chat : c[1].data.chat;
                        if (cLonNhat > groupCard.data[0].chat && cLonNhat > groupCard.data[1].chat) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                }
                case GroupCard.AA._3Cay:
                if (c.Count == 3) {
                    if (c[0].data.nut == c[1].data.nut && c[1].data.nut == c[2].data.nut &&
                        c[1].data.nut > groupCard.data[0].nut) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                case GroupCard.AA._Sanh3:
                if (c.Count == 3) {
                    List<DataCard> dtCard = new List<DataCard>();
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }

                    if (CheckSanh(dtCard)) {
                        DataCard c1 = groupCard.data[0];
                        foreach (var item in groupCard.data) {
                            if (item.nut > c1.nut) {
                                c1 = item;
                            }
                        }

                        DataCard c2 = c[0].data;
                        foreach (var item in c) {
                            if (item.data.nut > c2.nut) {
                                c2 = item.data;
                            }
                        }

                        if (c2.nut == c1.nut) {
                            if (c2.chat > c1.chat) {
                                return true;
                            } else {
                                return false;
                            }
                        } else if (c2.nut > c1.nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                case GroupCard.AA._Sanh4:
                if (c.Count == 4) {
                    List<DataCard> dtCard = new List<DataCard>();
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }

                    if (CheckSanh(dtCard)) {
                        DataCard c1 = groupCard.data[0];
                        foreach (var item in groupCard.data) {
                            if (item.nut > c1.nut) {
                                c1 = item;
                            }
                        }

                        DataCard c2 = c[0].data;
                        foreach (var item in c) {
                            if (item.data.nut > c2.nut) {
                                c2 = item.data;
                            }
                        }

                        if (c2.nut == c1.nut) {
                            if (c2.chat > c1.chat) {
                                return true;
                            } else {
                                return false;
                            }
                        } else if (c2.nut > c1.nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                case GroupCard.AA._Sanh5:
                if (c.Count == 5) {
                    List<DataCard> dtCard = new List<DataCard>();
                    foreach (var item in c) {
                        dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                    }

                    if (CheckSanh(dtCard)) {
                        DataCard c1 = groupCard.data[0];
                        foreach (var item in groupCard.data) {
                            if (item.nut > c1.nut) {
                                c1 = item;
                            }
                        }

                        DataCard c2 = c[0].data;
                        foreach (var item in c) {
                            if (item.data.nut > c2.nut) {
                                c2 = item.data;
                            }
                        }

                        if (c2.nut == c1.nut) {
                            if (c2.chat > c1.chat) {
                                return true;
                            } else {
                                return false;
                            }
                        } else if (c2.nut > c1.nut) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
                case GroupCard.AA._Sanh6:
                return true;
                case GroupCard.AA._Sanh7:
                return true;
                case GroupCard.AA._Sanh8:
                return true;
                case GroupCard.AA._Sanh9:
                return true;
                case GroupCard.AA._Sanh10:
                return true;
                case GroupCard.AA._Sanh11:
                return true;
                case GroupCard.AA._Sanh12:
                return true;
                case GroupCard.AA._Sanh13:
                return true;
                case GroupCard.AA._3DoiThong:
                return true;
                case GroupCard.AA._4DoiThong:
                return true;
                case GroupCard.AA._5DoiThong:
                return true;
                case GroupCard.AA._6DoiThong:
                return true;
                case GroupCard.AA._TuQuy:
                return true;
                default:
                return true;
            }
        }
    }

    public void ResetLuot()
    {
        foreach (Transform item in GameObject.Find("Group Card").transform) {
            Destroy(item.gameObject);
        }

        sortI = 1;

        p1.p.coLuot = true;
        p2.p.coLuot = true;
        p3.p.coLuot = true;
        p4.p.coLuot = true;
    }

    public void BoLuot()
    {
        NextPlayer();
    }

    public void Danh(List<Card> cards, Player newP)
    {
        firstRound = false;

        int x = -(cards.Count / 2);
        for (int i = 0; i < cards.Count; i++) {
            //set sorting order
            cards[i].sprite.sortingOrder = sortI;
            sortI++;

            //set position
            cards[i].gameObject.transform.DOMove(new Vector3(x * .4f, 0, 0), .2f);
            x += 1;

            //set euler angele
            cards[i].transform.eulerAngles = new Vector3(0, 0, Random.Range(-10f, 10f));

            //set scale
            cards[i].transform.DOScale(.8f, .3f);

            //set parent
            cards[i].transform.SetParent(GameObject.Find("Group Card").transform);
        }


        GroupCard g = new GroupCard();
        g.data = new List<DataCard>();

        foreach (var item in cards) {
            g.data.Add(item.data);
        }

        if (g.data.Count == 1) {
            g.a = GroupCard.AA._Coi;
        } else if (g.data.Count == 2) {
            g.a = GroupCard.AA._Doi;
        } else if (g.data.Count == 3) {
            if (g.data[0].nut == g.data[1].nut) {
                g.a = GroupCard.AA._3Cay;
            } else {
                g.a = GroupCard.AA._Sanh3;
            }
        } else if (g.data.Count == 4) {
            if (g.data[0].nut == g.data[1].nut) {
                g.a = GroupCard.AA._TuQuy;
            } else {
                g.a = GroupCard.AA._Sanh4;
            }
        } else if (g.data.Count == 5) {
            g.a = GroupCard.AA._Sanh5;
        } else if (g.data.Count == 6) {

            List<DataCard> dtCard = new List<DataCard>();

            foreach (var item in cards) {
                dtCard.Add(new DataCard(item.data.chat, item.data.nut));
            }

            if (CheckSanh(dtCard)) {
                g.a = GroupCard.AA._Sanh6;
            } else {
                foreach (var item in cards) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                if (CheckDoiThong(dtCard)) {
                    g.a = GroupCard.AA._3DoiThong;
                } else {
                    print("dell");
                }
            }
        } else if (g.data.Count == 7) {
            g.a = GroupCard.AA._Sanh7;
        } else if (g.data.Count == 8) {
            List<DataCard> dtCard = new List<DataCard>();

            foreach (var item in cards) {
                dtCard.Add(new DataCard(item.data.chat, item.data.nut));
            }

            if (CheckSanh(dtCard)) {
                g.a = GroupCard.AA._Sanh8;
            } else {
                foreach (var item in cards) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                if (CheckDoiThong(dtCard)) {
                    g.a = GroupCard.AA._4DoiThong;
                } else {
                    print("dell");
                }
            }
        } else if (g.data.Count == 9) {
            g.a = GroupCard.AA._Sanh9;
        } else if (g.data.Count == 10) {
            List<DataCard> dtCard = new List<DataCard>();

            foreach (var item in cards) {
                dtCard.Add(new DataCard(item.data.chat, item.data.nut));
            }

            if (CheckSanh(dtCard)) {
                g.a = GroupCard.AA._Sanh10;
            } else {
                foreach (var item in cards) {
                    dtCard.Add(new DataCard(item.data.chat, item.data.nut));
                }
                if (CheckDoiThong(dtCard)) {
                    g.a = GroupCard.AA._5DoiThong;
                } else {
                    print("dell");
                }
            }
        } else if (g.data.Count == 11) {
            g.a = GroupCard.AA._Sanh11;
        } else if (g.data.Count == 12) {
            g.a = GroupCard.AA._Sanh12;
        } else if (g.data.Count == 13) {
            g.a = GroupCard.AA._Sanh13;
        }

        g.p = newP;

        groupCard = g;

        StartCoroutine(DanhCo());
    }

    IEnumerator DanhCo()
    {
        yield return new WaitForSeconds(.5f);
        NextPlayer();
    }

    public void NextPlayer()
    {
        int pHetBai = 0;
        if (p1.p.hetBai) {
            pHetBai++;
        }
        if (p2.p.hetBai) {
            pHetBai++;
        }
        if (p3.p.hetBai) {
            pHetBai++;
        }
        if (p4.p.hetBai) {
            pHetBai++;
        }

        if (pHetBai == 3) {
            ShowKetQua();
            return;
        }

        int pBoLuot = 0;
        if (!p1.p.coLuot || p1.p.hetBai) {
            pBoLuot++;
        }
        if (!p2.p.coLuot || p2.p.hetBai) {
            pBoLuot++;
        }
        if (!p3.p.coLuot || p3.p.hetBai) {
            pBoLuot++;
        }
        if (!p4.p.coLuot || p4.p.hetBai) {
            pBoLuot++;
        }

        if (pBoLuot == 3) {
            ResetLuot();
            if (groupCard.p.gameObject.name == "Player 1") {
                if (p1.p.hetBai) {
                    if (p2.p.hetBai) {
                        if (p3.p.hetBai) {
                            turnPlayer = TurnPlayer.P4;
                            p4.ToiLuotDanh();
                        } else {
                            turnPlayer = TurnPlayer.P3;
                            p3.ToiLuotDanh();
                        }
                    } else {
                        turnPlayer = TurnPlayer.P2;
                        p2.ToiLuotDanh();
                    }
                } else {
                    turnPlayer = TurnPlayer.P1;
                }
            } else if (groupCard.p.gameObject.name == "Player 2") {//LA BAI CUOI DC DANH RA BOI NGUOI CHOI DA HET BAI MA KHONG CO NGUOI AN SE LOI
                if (p2.p.hetBai) {
                    if (p3.p.hetBai) {
                        if (p4.p.hetBai) {
                            turnPlayer = TurnPlayer.P1;
                        } else {
                            turnPlayer = TurnPlayer.P4;
                            p4.ToiLuotDanh();
                        }
                    } else {
                        turnPlayer = TurnPlayer.P3;
                        p3.ToiLuotDanh();
                    }
                } else {
                    turnPlayer = TurnPlayer.P2;
                    p2.ToiLuotDanh();
                }
            } else if (groupCard.p.gameObject.name == "Player 3") {
                if (p3.p.hetBai) {
                    if (p4.p.hetBai) {
                        if (p1.p.hetBai) {
                            turnPlayer = TurnPlayer.P2;
                            p2.ToiLuotDanh();
                        } else {
                            turnPlayer = TurnPlayer.P1;
                        }
                    } else {
                        turnPlayer = TurnPlayer.P4;
                        p4.ToiLuotDanh();
                    }
                } else {
                    turnPlayer = TurnPlayer.P3;
                    p3.ToiLuotDanh();
                }
            } else if (groupCard.p.gameObject.name == "Player 4") {
                if (p4.p.hetBai) {
                    if (p1.p.hetBai) {
                        if (p2.p.hetBai) {
                            turnPlayer = TurnPlayer.P3;
                            p3.ToiLuotDanh();
                        } else {
                            turnPlayer = TurnPlayer.P2;
                            p2.ToiLuotDanh();
                        }
                    } else {
                        turnPlayer = TurnPlayer.P1;
                    }
                } else {
                    turnPlayer = TurnPlayer.P4;
                    p4.ToiLuotDanh();
                }
            }

            groupCard.p = null;
        } else {
            switch (turnPlayer) {
                case TurnPlayer.P1:
                if (!p2.p.hetBai && p2.p.coLuot) {
                    turnPlayer = TurnPlayer.P2;
                    p2.ToiLuotDanh();
                } else {
                    if (!p3.p.hetBai && p3.p.coLuot) {
                        turnPlayer = TurnPlayer.P3;
                        p3.ToiLuotDanh();
                    } else {
                        turnPlayer = TurnPlayer.P4;
                        p4.ToiLuotDanh();
                    }
                }
                break;
                case TurnPlayer.P2:
                if (!p3.p.hetBai && p3.p.coLuot) {
                    turnPlayer = TurnPlayer.P3;
                    p3.ToiLuotDanh();
                } else {
                    if (!p4.p.hetBai && p4.p.coLuot) {
                        turnPlayer = TurnPlayer.P4;
                        p4.ToiLuotDanh();
                    } else {
                        turnPlayer = TurnPlayer.P1;
                    }
                }
                break;
                case TurnPlayer.P3:
                if (!p3.p.hetBai && p4.p.coLuot) {
                    turnPlayer = TurnPlayer.P4;
                    p4.ToiLuotDanh();
                } else {
                    if (!p1.p.hetBai && p1.p.coLuot) {
                        turnPlayer = TurnPlayer.P1;
                    } else {
                        turnPlayer = TurnPlayer.P2;
                        p2.ToiLuotDanh();
                    }
                }
                break;
                case TurnPlayer.P4:
                if (!p1.p.hetBai && p1.p.coLuot) {
                    turnPlayer = TurnPlayer.P1;
                } else {
                    if (!p2.p.hetBai && p2.p.coLuot) {
                        turnPlayer = TurnPlayer.P2;
                        p2.ToiLuotDanh();
                    } else {
                        turnPlayer = TurnPlayer.P3;
                        p3.ToiLuotDanh();
                    }
                }
                break;
            }
        }
    }

    public void PlayerHetBai(int s)
    {
        if (indexP1 == 0) {
            indexP1 = s;
        } else if (indexP2 == 0) {
            indexP2 = s;
        } else if (indexP3 == 0) {
            indexP3 = s;

            if (!p1.p.hetBai) {
                indexP4 = 1;
            } else if (!p2.p.hetBai) {
                indexP4 = 2;
            } else if (!p3.p.hetBai) {
                indexP4 = 3;
            } else if (!p4.p.hetBai) {
                indexP4 = 4;
            }

        } else if (indexP4 == 0) {
            indexP4 = s;
        }
    }

    void ShowKetQua()
    {
        foreach (Transform item in GameObject.Find("Group Card").transform) {
            Destroy(item.gameObject);
        }

        sortI = 1;

        p1.p.coLuot = true;
        p1.p.hetBai = false;

        p2.p.coLuot = true;
        p2.p.hetBai = false;

        p3.p.coLuot = true;
        p3.p.hetBai = false;

        p4.p.coLuot = true;
        p4.p.hetBai = false;

        groupCard.p = null;

        foreach (var item in p1.card) {
            Destroy(item);
        }

        foreach (var item in p2.card) {
            Destroy(item);
        }

        foreach (var item in p3.card) {
            Destroy(item);
        }

        foreach (var item in p4.card) {
            Destroy(item);
        }

        stateGame = StateGame.KetQua;

        panelKetQua.SetActive(true);
        nameP1.text = "Player " + indexP1;
        nameP2.text = "Player " + indexP2;
        nameP3.text = "Player " + indexP3;
        nameP4.text = "Player " + indexP4;

        if (indexP1 == 1) {
            turnPlayer = TurnPlayer.P1;
        }
        if (indexP1 == 2) {
            turnPlayer = TurnPlayer.P2;
        }
        if (indexP1 == 3) {
            turnPlayer = TurnPlayer.P3;
        }
        if (indexP1 == 4) {
            turnPlayer = TurnPlayer.P4;
        }
    }
}
