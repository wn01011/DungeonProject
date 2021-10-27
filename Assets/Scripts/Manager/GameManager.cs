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

        uiManager.skeletonUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonBones", 100);
        uiManager.skeletonUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonTears", 100);
        uiManager.skeletonUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonSoulGem", 100);

        Zombie.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDmg", 2f);
        Zombie.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieDef", 1f);
        Zombie.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "ZombieHp", 20f);

        uiManager.zombieUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieBones", 100);
        uiManager.zombieUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieTears", 100);
        uiManager.zombieUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieSoulGem", 100);

        FlyingEye.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDmg", 5f);
        FlyingEye.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeDef", 2f);
        FlyingEye.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeHp", 40f);

        uiManager.flyingEyeUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeBones", 100);
        uiManager.flyingEyeUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeTears", 100);
        uiManager.flyingEyeUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeSoulGem", 100);

        Goblin.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDmg", 3f);
        Goblin.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinDef", 2f);
        Goblin.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "GoblinHp", 30f);

        uiManager.goblinUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinBones", 100);
        uiManager.goblinUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinTears", 100);
        uiManager.goblinUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinSoulGem", 100);

        DarkWizard.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDmg", 6f);
        DarkWizard.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardDef", 3f);
        DarkWizard.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DarkWizardHp", 50f);

        uiManager.darkWizardUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardBones", 100);
        uiManager.darkWizardUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardTears", 100);
        uiManager.darkWizardUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardSoulGem", 100);

        DeathBringer.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDmg", 8f);
        DeathBringer.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerDef", 5f);
        DeathBringer.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "DeathBringerHp", 60f);

        uiManager.deathBringerUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerBones", 100);
        uiManager.deathBringerUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerTears", 100);
        uiManager.deathBringerUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerSoulGem", 100);

        Boss.staticDmg = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDmg", 5);
        Boss.staticDef = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossDef", 0);
        Boss.static_maxHp = PlayerPrefs.GetFloat(PhotonNetwork.LocalPlayer.NickName + "BossHp", 50);

        uiManager.bossUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossBones", 100);
        uiManager.bossUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossTears", 100);
        uiManager.bossUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossSoulGem", 100);

        for(int i=0; i<spawnManager.spawn.Length - 1; ++i)
        {
            spawnMappingArray[i] = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Spawn[" + i.ToString() + "]", 1);
        }
        spawnMappingArray[18] = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "Spawn[" + (spawnManager.spawn.Length - 1).ToString() + "]", 7);

        spawnManager.SpawnMapping();

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

        for(int i=0; i< spawnManager.spawn.Length; ++i)
        {
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "Spawn[" + i.ToString() + "]", (int)spawnManager.spawn[i]);
        }

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

    public void ResourceBtn()
    {
        goods.bone += 10000;
        goods.tear += 10000;
        goods.soulgem += 10000;
    }

    //유지 보수 시간
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
    public int goodsCollectAdjust = 10;
    public int[] spawnMappingArray = new int[19];
}
