using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public struct Goods
    {
        public int bone;
        public int tear;
        public int soulgem;
        public Goods(int _bone, int _tear, int _soulgem)
        {
            this.bone = _bone;
            this.tear = _tear;
            this.soulgem = _soulgem;
        }
    }
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(goods.bone);
    //        stream.SendNext(goods.tear);
    //        stream.SendNext(goods.crystal);
    //    }
    //    else
    //    {
    //        goods.bone = (int)stream.ReceiveNext();
    //        goods.tear = (int)stream.ReceiveNext();
    //        goods.crystal = (int)stream.ReceiveNext();
    //    }
    //}

    private void Start()
    {
        goods = new Goods(0,0,0);
        goods.bone = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Bones", 0);
        goods.tear = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Tears", 0);
        goods.soulgem = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Soulgem", 0);
        StartCoroutine(bossFinder());
        StartCoroutine(WaveCheck());
        heroes = FindObjectsOfType<Hero>();
    }
    public override void OnDisconnected(DisconnectCause cause) 
    {
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Bones", goods.bone);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Tears", goods.tear);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Soulgem", goods.soulgem);
        print("¿¬°á ²÷±è"); 
    }

    private IEnumerator bossFinder()
    {
        while(!boss)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            yield return new WaitForFixedUpdate();
        }
        bossPos = boss.transform.position;
    }
    public IEnumerator WaveCheck()
    {
        while (!boss.GetComponent<Boss>().bossDie)
        {
            heroes = FindObjectsOfType<Hero>();
            yield return new WaitForSeconds(1.0f);
            int dieCount = 0;
            for (int i = 0; i < heroes.Length; ++i)
            {
                if (!heroes[i].isDie)
                {
                    break;
                }
                else if (heroes[i].isDie)
                {
                    ++dieCount;
                    continue;
                }
            }
            if (heroes.Length == dieCount)
            {
                waveUp = true;
                ++wave;
                Debug.Log("WaveClear!\nwave : " + wave);
                ReStart();
            }
        }
        Debug.Log("Boss Die!");
        ReStart();
    }
    public void ReStart()
    {
        if(wave > 10)
        {
            GameOver();
            Debug.Log("GameOver!");
            return;
        }
        spawnManager.Restart();
        foreach(Monster monster in spawnManager.GetComponentsInChildren<Monster>())
        {
            monster.isDie = false;
        }
        boss.GetComponent<Boss>().bossDie = false;
        boss.transform.position = bossPos;
        StartCoroutine(WaveCheck());
    }
    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    [SerializeField]private SpawnManager spawnManager = null;
    private Hero[] heroes = null;
    private GameObject boss = null;
    private Vector3 bossPos = Vector3.zero;
    public static int wave = 1;
    public bool waveUp = false;
    public Goods goods = new Goods();
}
