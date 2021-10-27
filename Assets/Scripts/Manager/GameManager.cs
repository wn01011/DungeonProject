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
        #region Get PlayerPreference values
        
        goods = new Goods(0,0,0);
        goods.bone = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Bones", defaultResource);
        goods.tear = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Tears", defaultResource);
        goods.soulgem = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Soulgem", defaultResource);

        Skeleton.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonDmg", 1f);
        Skeleton.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonDef", 0f);
        Skeleton.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonHp", 10f);

        Zombie.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDmg", 1f);
        Zombie.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDef", 1f);
        Zombie.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieHp", 20f);

        FlyingEye.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDmg", 10f);
        FlyingEye.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDef", 0f);
        FlyingEye.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeHp", 20f);

        Goblin.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDmg", 3f);
        Goblin.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDef", 0f);
        Goblin.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinHp", 15f);

        DarkWizard.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDmg", 5f);
        DarkWizard.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDef", 1f);
        DarkWizard.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardHp", 40f);

        DeathBringer.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDmg", 8f);
        DeathBringer.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDef", 5f);
        DeathBringer.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerHp", 30f);

        Boss.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDmg", 2);
        Boss.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDef", 0);
        Boss.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossHp", 50);

        #endregion

        StartCoroutine(bossFinder());
        StartCoroutine(WaveCheck());
        heroes = FindObjectsOfType<Hero>();
    }

    //접속 종료시점에 PlayerPreferences에 포톤에 들어올때 입력했던 닉네임 + 재화이름 진행 상태등을 키로 설정하고 저장해둠
    //한 컴퓨터 내에선 데이터 저장이 가능함
    public override void OnDisconnected(DisconnectCause cause) 
    {
        #region SetPlayerPreference values

        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Bones", goods.bone);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Tears", goods.tear);
        PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Soulgem", goods.soulgem);
        
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonDmg", Skeleton.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonDef", Skeleton.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "SkeletonHp", Skeleton.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDmg", Zombie.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDef", Zombie.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieHp", Zombie.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDmg", FlyingEye.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDef", FlyingEye.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeHp", FlyingEye.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDmg", Goblin.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDef", Goblin.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinHp", Goblin.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDmg", DarkWizard.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDef", DarkWizard.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardHp", DarkWizard.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDmg", DeathBringer.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDef", DeathBringer.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerHp", DeathBringer.static_maxHp);

        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDmg", Boss.staticDmg);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDef", Boss.staticDef);
        PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "BossHp", Boss.static_maxHp);

        #endregion

        print("연결 끊김"); 
    }

    //보스 생성까지 기다려줌.
    private IEnumerator bossFinder()
    {
        while(!boss)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            yield return new WaitForFixedUpdate();
        }
        bossPos = boss.transform.position;
    }

    //wave가 올라갈지 아니면 다시 시작할지 결정//Hero들이 모두 죽었는지 count를 세서 판별
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
        //10웨이브클리어하면 restart에서 클리어판정
        if(wave > 10)
        {
            Debug.Log("GameOver!");
            GameOver();
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
    [SerializeField] private int defaultResource = 1000;
    private Hero[] heroes = null;
    private GameObject boss = null;
    private Vector3 bossPos = Vector3.zero;
    
    public static int wave = 1;
    public bool waveUp = false;
    public Goods goods = new Goods();
}
