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

    //접속 종료시점에 PlayerPreferences에 포톤에 들어올때 입력했던 닉네임 + 재화이름을 키로 설정하고 재화를 저장해둠
    //한 컴퓨터 내에선 데이터 저장이 가능함
    public override void OnDisconnected(DisconnectCause cause) 
    {
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Bones", goods.bone);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Tears", goods.tear);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Soulgem", goods.soulgem);
        print("연결 끊김"); 
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

    #region ReStart

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

        //유지 보수 시간 직전에 웨이브text를 업데이트 해줌
        uiManager.WaveTextUpdate();
        StartCoroutine(ReStartMaintenance());

        StartCoroutine(WaveCheck());
    }

    private IEnumerator ReStartMaintenance()
    {
        uiManager.isMaintenance = true;
        uiManager.maintenanceBtn.gameObject.SetActive(true);
        
        yield return new WaitUntil(() => !uiManager.isMaintenance);
        
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
   
    #endregion

    [SerializeField] private SpawnManager spawnManager = null;
    [SerializeField] private UIManager uiManager = null;
    private Hero[] heroes = null;
    private GameObject boss = null;
    private Vector3 bossPos = Vector3.zero;
    public static int wave = 1;
    public bool waveUp = false;
    public Goods goods = new Goods();
}
