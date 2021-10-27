using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
{
    public struct MonsterUpgradeCost
    {
        public int boneCost;
        public int tearCost;
        public int soulgemCost;
        public MonsterUpgradeCost(int _boneCost, int _tearCost, int _soulgemCost)
        {
            boneCost = _boneCost;
            tearCost = _tearCost;
            soulgemCost = _soulgemCost;
        }
    }

    private void Start()
    {
        #region List.Add

        btnList.Add(unitBtn);
        btnList.Add(buildBtn);
        btnList.Add(itemBtn);
        btnList.Add(optionBtn);
        btnList.Add(mailBtn);

        panelList.Add(unitBtnPanel);
        panelList.Add(buildBtnPanel);
        panelList.Add(itemBtnPanel);
        panelList.Add(optionBtnPanel);
        panelList.Add(mailBtnPanel);

        #endregion

        #region upgradeCost 불러오기

        #region skeleton

        skeletonUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        skeletonUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonBones", 100);
        skeletonUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonTears", 100);
        skeletonUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonSoulGems", 100);
        
        #endregion

        #region zombie

        zombieUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        zombieUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieBones", 100);
        zombieUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieTears", 100);
        zombieUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieSoulGems", 100);
        
        #endregion

        #region goblin

        goblinUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        goblinUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinBones", 100);
        goblinUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinTears", 100);
        goblinUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinSoulGems", 100);

        #endregion

        #region flyingEye

        flyingEyeUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        flyingEyeUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeBones", 100);
        flyingEyeUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeTears", 100);
        flyingEyeUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeSoulGems", 100);

        #endregion

        #region darkWizard

        darkWizardUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        darkWizardUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardBones", 100);
        darkWizardUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardTears", 100);
        darkWizardUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardSoulGems", 100);

        #endregion

        #region deathBringer

        deathBringerUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        deathBringerUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerBones", 100);
        deathBringerUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerTears", 100);
        deathBringerUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerSoulGems", 100);

        #endregion

        #region boss

        bossUpgradeCost = new MonsterUpgradeCost(0, 0, 0);
        bossUpgradeCost.boneCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossBones", 100);
        bossUpgradeCost.tearCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossTears", 100);
        bossUpgradeCost.soulgemCost = PlayerPrefs.GetInt(PhotonNetwork.LocalPlayer.NickName + "BossSoulGems", 100);

        #endregion

        magicRoomUpgradeCost = 100;
        skullHarvestRoomUpgradeCost = 100;

        #endregion

        #region upgradeCost Text

        upgradeText = contents.GetComponentsInChildren<Text>();
        for(int i=0; i < (upgradeText.Length) * 0.5f; ++i)
        {
            upgradeText[2 * i + 1].text = "100";
        }
        roomUpgradeText = buildBtnPanel.GetComponentsInChildren<Text>();

        #endregion

        MakeCursor();
        WaveTextUpdate();
        StartCoroutine(GoodsTextUpdate());
        StartCoroutine(roomRayCastColCheckCoroutine());
        StartCoroutine(StatsTextUpdate());
    }

    private void Update()
    {
        MoveCursor();
    }

    private IEnumerator roomRayCastColCheckCoroutine()
    {
        yield return new WaitUntil(() => roomRayCastCol.GetComponentsInChildren<BoxCollider>() != null);
        
        BoxCollider[] roomCols = roomRayCastCol.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < roomCols.Length; ++i)
        {
            roomColArray.Add(roomCols[i].gameObject);
        }
    }

    #region Button OnClick Methods

    #region UIBtns

    //btnList[idx], panelList[idx] match
    private void ButtonClick(Button _btnType)
    {
        int panelIdx = btnList.IndexOf(_btnType);
        GameObject panel = panelList[panelIdx];
        isBtnClick = true;
        

        if(panel.activeSelf==false)
        {
            for(int i=0; i<panelList.Count; ++i)
            {
                GameObject otherPanel = panelList[i];
                otherPanel.SetActive(false);
            }
        }
        panel.SetActive(true);
    }
    public void unitBtnClick()
    {
        RoomColArraySetActive(false);
        ButtonClick(unitBtn);
        SoundManager.soundManager.SFXplayer("unitBtn", panelclip);
    }
    public void bulidBtnClick()
    {
        RoomColArraySetActive(false);
        ButtonClick(buildBtn);
        SoundManager.soundManager.SFXplayer("buildBtn", panelclip);
    }
    public void itemBtnClick()
    {
        RoomColArraySetActive(false);
        ButtonClick(itemBtn);
    }
    public void optionBtnClick()
    {
        RoomColArraySetActive(false);
        ButtonClick(optionBtn);
    }
    public void mailBtnClick()
    {
        RoomColArraySetActive(false);
        ButtonClick(mailBtn);
    }
    public void ExitBtnClick()
    {
        RoomColArraySetActive(true);
        PhotonNetwork.Disconnect();
        Debug.Log("연결해제!");
        SceneManager.LoadScene("Title");
    }
    public void SetMonsterPanelExitBtn()
    {
        RoomColArraySetActive(true);
        exitBtnPanel.SetActive(false);
        SoundManager.soundManager.SFXplayer("SetMonsterPanel", panelclip);
    }

    public void DungeonMaintenanceBtn()
    {
        isMaintenance = false;
        maintenanceBtn.gameObject.SetActive(false);
    }

    public void UnitAndBuildExitBtn(GameObject _panel)
    {
        _panel.SetActive(false);
        RoomColArraySetActive(true);
    }
    private void RoomColArraySetActive(bool _bool)
    {
        foreach(GameObject roomCol in roomColArray)
        {
            roomCol.SetActive(_bool);
        }
    }

    #region GameSpdBtns

    public void x1SpeedBtn()
    {
        Time.timeScale = 1f;
    }

    public void x2SpeedBtn()
    {
        Time.timeScale = 2f;
    }

    public void x3SpeedBtn()
    {
        Time.timeScale = 3f;
    }

    #endregion

    #endregion

    #region MonsterUnit UpgradeBtn

    #region Skeleton upgradeBtn
    public void Skeleton_atkUp()
    {
        if (gameManager.goods.bone >= skeletonUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= skeletonUpgradeCost.boneCost;
            Skeleton.staticDmg += 1f;
            skeletonUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonBones", skeletonUpgradeCost.boneCost);
            upgradeText[1].text = skeletonUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(skeletonUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void Skeleton_defUp()
    {
        if (gameManager.goods.tear >= skeletonUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= skeletonUpgradeCost.tearCost;
            Skeleton.staticDef += 1f;
            skeletonUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonTears", skeletonUpgradeCost.tearCost);
            upgradeText[3].text = skeletonUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(skeletonUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void Skeleton_hpUp()
    {
        Skeleton[] skeletons = FindObjectsOfType<Skeleton>();
        if (gameManager.goods.soulgem >= skeletonUpgradeCost.soulgemCost)
        {
            gameManager.goods.soulgem -= skeletonUpgradeCost.soulgemCost;
            Skeleton.static_maxHp += 10f;
            foreach(Skeleton targetSkeleton in skeletons)
            {
                targetSkeleton.SetStaticHp();
            }
            skeletonUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "SkeletonSoulGems", skeletonUpgradeCost.soulgemCost);
            upgradeText[5].text = skeletonUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(skeletonUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region Zombie upgradeBtn
    public void Zombie_atkUp()
    {
        if (gameManager.goods.bone >= zombieUpgradeCost.boneCost)
        {
            
            gameManager.goods.bone -= zombieUpgradeCost.boneCost;
            Zombie.staticDmg += 1f;
            zombieUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieBones", zombieUpgradeCost.boneCost);
            upgradeText[7].text = zombieUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(zombieUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void Zombie_defUp()
    {
        if (gameManager.goods.tear >= zombieUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= zombieUpgradeCost.tearCost;
            Zombie.staticDef += 1f;
            zombieUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieTears", zombieUpgradeCost.tearCost);
            upgradeText[9].text = zombieUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(zombieUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void Zombie_hpUp()
    {
        if (gameManager.goods.soulgem >= zombieUpgradeCost.soulgemCost)
        {
            Zombie[] zombies = FindObjectsOfType<Zombie>();

            gameManager.goods.soulgem -= zombieUpgradeCost.soulgemCost;
            Zombie.static_maxHp += 10f;
            foreach(Zombie targetZom in zombies)
            {
                targetZom.SetStaticHp();
            }
            zombieUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "ZombieSoulGems", zombieUpgradeCost.soulgemCost);
            upgradeText[11].text = zombieUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(zombieUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region Goblin upgradeBtn
    public void Goblin_atkUp()
    {
        if (gameManager.goods.bone >= goblinUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= goblinUpgradeCost.boneCost;
            Goblin.staticDmg += 1f;
            goblinUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinBones", goblinUpgradeCost.boneCost);
            upgradeText[13].text = goblinUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(goblinUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void Goblin_defUp()
    {
        if (gameManager.goods.tear >= goblinUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= goblinUpgradeCost.tearCost;
            Goblin.staticDef += 1f;
            goblinUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinTears", goblinUpgradeCost.tearCost);
            upgradeText[15].text = goblinUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(goblinUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void Goblin_hpUp()
    {
        if (gameManager.goods.soulgem >= goblinUpgradeCost.soulgemCost)
        {
            Goblin[] goblins = FindObjectsOfType<Goblin>();

            gameManager.goods.soulgem -= goblinUpgradeCost.soulgemCost;
            Goblin.static_maxHp += 10f;
            foreach(Goblin targetGo in goblins)
            {
                targetGo.SetStaticHp();
            }
            goblinUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "GoblinSoulGems", goblinUpgradeCost.soulgemCost);
            upgradeText[17].text = goblinUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(goblinUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region FlyingEye upgradeBtn
    public void FlyingEye_atkUp()
    {
        if (gameManager.goods.bone >= flyingEyeUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= flyingEyeUpgradeCost.boneCost;
            FlyingEye.staticDmg += 1f;
            flyingEyeUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeBones", flyingEyeUpgradeCost.boneCost);
            upgradeText[19].text = flyingEyeUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(flyingEyeUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void FlyingEye_defUp()
    {
        if (gameManager.goods.tear >= flyingEyeUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= flyingEyeUpgradeCost.tearCost;
            FlyingEye.staticDef += 1f;
            flyingEyeUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeTears", flyingEyeUpgradeCost.tearCost);
            upgradeText[21].text = flyingEyeUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(flyingEyeUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void FlyingEye_hpUp()
    {
        if (gameManager.goods.soulgem >= flyingEyeUpgradeCost.soulgemCost)
        {
            FlyingEye[] flyingEyes = FindObjectsOfType<FlyingEye>();

            gameManager.goods.soulgem -= flyingEyeUpgradeCost.soulgemCost;
            FlyingEye.static_maxHp += 10f;
            foreach(FlyingEye targetEye in flyingEyes)
            {
                targetEye.SetStaticHp();
            }
            flyingEyeUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "FlyingEyeSoulGems", flyingEyeUpgradeCost.soulgemCost);
            upgradeText[23].text = flyingEyeUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(flyingEyeUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region DarkWizard upgradeBtn
    public void DarkWizard_atkUp()
    {
        if (gameManager.goods.bone >= darkWizardUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= darkWizardUpgradeCost.boneCost;
            DarkWizard.staticDmg += 1f;
            darkWizardUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardBones", darkWizardUpgradeCost.boneCost);
            upgradeText[25].text = darkWizardUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(darkWizardUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void DarkWizard_defUp()
    {
        if (gameManager.goods.tear >= darkWizardUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= darkWizardUpgradeCost.tearCost;
            DarkWizard.staticDef += 1f;
            darkWizardUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardTears", darkWizardUpgradeCost.tearCost);
            upgradeText[27].text = darkWizardUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(darkWizardUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void DarkWizard_hpUp()
    {
        if (gameManager.goods.soulgem >= darkWizardUpgradeCost.soulgemCost)
        {
            DarkWizard[] darkWizards = FindObjectsOfType<DarkWizard>();

            gameManager.goods.soulgem -= darkWizardUpgradeCost.soulgemCost;
            DarkWizard.static_maxHp += 10f;
            foreach(DarkWizard targetWizard in darkWizards)
            {
                targetWizard.SetStaticHp();
            }
            darkWizardUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DarkWizardSoulGems", darkWizardUpgradeCost.soulgemCost);
            upgradeText[29].text = darkWizardUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(darkWizardUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region DeathBringer upgradeBtn
    public void DeathBringer_atkUp()
    {
        if (gameManager.goods.bone >= deathBringerUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= deathBringerUpgradeCost.boneCost;
            DeathBringer.staticDmg += 1f;
            deathBringerUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerBones", deathBringerUpgradeCost.boneCost);
            upgradeText[31].text = deathBringerUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(deathBringerUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void DeathBringer_defUp()
    {
        if (gameManager.goods.tear >= deathBringerUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= deathBringerUpgradeCost.tearCost;
            DeathBringer.staticDef += 1f;
            deathBringerUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerTears", deathBringerUpgradeCost.tearCost);
            upgradeText[33].text = deathBringerUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(deathBringerUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void DeathBringer_hpUp()
    {
        if (gameManager.goods.soulgem >= deathBringerUpgradeCost.soulgemCost)
        {
            DeathBringer[] deathBringers = FindObjectsOfType<DeathBringer>();

            gameManager.goods.soulgem -= deathBringerUpgradeCost.soulgemCost;
            DeathBringer.static_maxHp += 10f;
            foreach(DeathBringer targetBringer in deathBringers)
            {
                targetBringer.SetStaticHp();
            }

            deathBringerUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "DeathBringerSoulGems", deathBringerUpgradeCost.soulgemCost);
            upgradeText[35].text = darkWizardUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(deathBringerUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #region Boss upgradeBtn
    public void Boss_atkUp()
    {
        if (gameManager.goods.bone >= bossUpgradeCost.boneCost)
        {
            gameManager.goods.bone -= bossUpgradeCost.boneCost;
            Boss.staticDmg += 1f;
            bossUpgradeCost.boneCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "BossBones", bossUpgradeCost.boneCost);
            upgradeText[37].text = bossUpgradeCost.boneCost.ToString();
        }
        else
        {
            Debug.Log(bossUpgradeCost.boneCost - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
    }

    public void Boss_defUp()
    {
        if (gameManager.goods.tear >= bossUpgradeCost.tearCost)
        {
            gameManager.goods.tear -= bossUpgradeCost.tearCost;
            Boss.staticDef += 1f;
            bossUpgradeCost.tearCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "BossTears", bossUpgradeCost.tearCost);
            upgradeText[39].text = bossUpgradeCost.tearCost.ToString();
        }
        else
        {
            Debug.Log(bossUpgradeCost.tearCost - gameManager.goods.tear + " Tear 이 모자랍니다.");
            return;
        }
    }

    public void Boss_hpUp()
    {
        if (gameManager.goods.soulgem >= bossUpgradeCost.soulgemCost)
        {
            Boss boss = FindObjectOfType<Boss>();

            gameManager.goods.soulgem -= bossUpgradeCost.soulgemCost;
            Boss.static_maxHp += 10f;
            boss.SetStaticHp();
            bossUpgradeCost.soulgemCost += 100;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "BossSoulGems", bossUpgradeCost.soulgemCost);
            upgradeText[41].text = bossUpgradeCost.soulgemCost.ToString();
        }
        else
        {
            Debug.Log(bossUpgradeCost.soulgemCost - gameManager.goods.soulgem + " SoulGem 이 모자랍니다.");
            return;
        }
    }
    #endregion

    #endregion

    #region Room UpgradeBtn

    public void MagicRoomUpgradeBtn()
    {
        FireBall fireBall = canvas.GetComponent<FireBall>();
        
        if(gameManager.goods.tear >= magicRoomUpgradeCost)
        {
            gameManager.goods.tear -= magicRoomUpgradeCost;
            fireBall.mpRegenerateAdjust += 0.2f;
            magicRoomUpgradeCost += 50;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "magicRoomUpgradeCost", magicRoomUpgradeCost);
            PlayerPrefs.SetFloat(PhotonNetwork.LocalPlayer.NickName + "mpRegenerateAdjust", fireBall.mpRegenerateAdjust);
            roomUpgradeText[3].text = magicRoomUpgradeCost.ToString();
        }
        else
        {
            Debug.Log(magicRoomUpgradeCost - gameManager.goods.tear + "Tear 이 모자랍니다.");
        }
    }

    public void SkullHarvestRoomUpgradeBtn()
    {
        if(gameManager.goods.soulgem >= skullHarvestRoomUpgradeCost)
        {
            gameManager.goods.soulgem -= skullHarvestRoomUpgradeCost;
            gameManager.goodsCollectAdjust += 10;
            skullHarvestRoomUpgradeCost += 50;
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "skullHarvestRoomUpgardeCost", skullHarvestRoomUpgradeCost);
            PlayerPrefs.SetInt(PhotonNetwork.LocalPlayer.NickName + "goodsCollectAdjust", gameManager.goodsCollectAdjust);
            roomUpgradeText[6].text = skullHarvestRoomUpgradeCost.ToString();
        }

    }

    #endregion

    #endregion

    #region Texts

    private IEnumerator GoodsTextUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            boneText.text = gameManager.goods.bone.ToString();
            tearText.text = gameManager.goods.tear.ToString();
            soulgemText.text = gameManager.goods.soulgem.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
    public void WaveTextUpdate()
    {
        waveText.text = GameManager.wave + " wave(" + spawnManager.curHeroCount + " / " + spawnManager.maxHeroCount + ")";
    }
    private IEnumerator StatsTextUpdate()
    {
        yield return waitForSeconds;
        while (true)
        {
            atkText[0].text = "공격력" + "\n" + Skeleton.staticDmg.ToString();
            defText[0].text = "방어력" + "\n" + Skeleton.staticDef.ToString();
            hpText[0].text = "체력" + "\n" + Skeleton.static_maxHp.ToString();

            atkText[1].text = "공격력" + "\n" + Zombie.staticDmg.ToString();
            defText[1].text = "방어력" + "\n" + Zombie.staticDef.ToString();
            hpText[1].text = "체력" + "\n" + Zombie.static_maxHp.ToString();

            atkText[2].text = "공격력" + "\n" + Goblin.staticDmg.ToString();
            defText[2].text = "방어력" + "\n" + Goblin.staticDef.ToString();
            hpText[2].text = "체력" + "\n" + Goblin.static_maxHp.ToString();

            atkText[3].text = "공격력" + "\n" + FlyingEye.staticDmg.ToString();
            defText[3].text = "방어력" + "\n" + FlyingEye.staticDef.ToString();
            hpText[3].text = "체력" + "\n" + FlyingEye.static_maxHp.ToString();

            atkText[4].text = "공격력" + "\n" + DarkWizard.staticDmg.ToString();
            defText[4].text = "방어력" + "\n" + DarkWizard.staticDef.ToString();
            hpText[4].text = "체력" + "\n" + DarkWizard.static_maxHp.ToString();

            atkText[5].text = "공격력" + "\n" + DeathBringer.staticDmg.ToString();
            defText[5].text = "방어력" + "\n" + DeathBringer.staticDef.ToString();
            hpText[5].text = "체력" + "\n" + DeathBringer.static_maxHp.ToString();

            atkText[6].text = "공격력" + "\n" + Boss.staticDmg.ToString();
            defText[6].text = "방어력" + "\n" + Boss.staticDef.ToString();
            hpText[6].text = "체력" + "\n" + Boss.static_maxHp.ToString();

            isBtnClick = false;

            yield return waitForSeconds;
        }
    }

    #endregion

    #region Cursor

    private void MakeCursor()
    {
        cursor = new GameObject("Cursor");
        cursorSprite = Resources.Load<Sprite>("Sprites/cursor/cursor(1)");
        
        cursor.AddComponent<CanvasRenderer>();
        cursor.AddComponent<Image>();
        cursor.GetComponent<Image>().sprite = cursorSprite;
        cursor.GetComponent<Image>().raycastTarget = false;
        cursor.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
        cursor.transform.SetParent(canvas.transform);
    }
    private void MoveCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray))
        {
            cursor.GetComponent<Image>().rectTransform.position = Input.mousePosition + new Vector3(5f,-10f);
        }
    }

    #endregion

    #region variables

    #region variables
    protected MonsterUpgradeCost zombieUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost skeletonUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost zombieUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost goblinUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost flyingEyeUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost darkWizardUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost deathBringerUpgradeCost = new MonsterUpgradeCost();
    public MonsterUpgradeCost bossUpgradeCost = new MonsterUpgradeCost();

    public GameObject cursor = null;
    private Sprite cursorSprite = null;
    private bool isBtnClick = false;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    private List<Button> btnList = new List<Button>();
    private List<GameObject> panelList = new List<GameObject>();
    private Text[] upgradeText = null;

    protected int magicRoomUpgradeCost = 0;
    protected int skullHarvestRoomUpgradeCost = 0;

    private bool isBtnClick = false;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    private List<Button> btnList = new List<Button>();
    private List<GameObject> panelList = new List<GameObject>();
    private Text[] upgradeText = null;
    private Text[] roomUpgradeText = null;

    [Header("ETC")]
    public bool isMaintenance = false;
    [SerializeField] private GameObject roomRayCastCol = null;
    [SerializeField] private List<GameObject> roomColArray = null;
    [SerializeField] private Text waveText = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private GameObject contents = null;

    [Header("Manager")]
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private SpawnManager spawnManager = null;

    [Header("Button")]
    public Button maintenanceBtn = null;
    [SerializeField] private Button unitBtn = null;
    [SerializeField] private Button buildBtn = null;
    [SerializeField] private Button itemBtn = null;
    [SerializeField] private Button optionBtn = null;
    [SerializeField] private Button mailBtn = null;

    [Header("ButtonPanel")]
    [SerializeField] private GameObject unitBtnPanel = null;
    [SerializeField] private GameObject buildBtnPanel = null;
    [SerializeField] private GameObject itemBtnPanel = null;
    [SerializeField] private GameObject optionBtnPanel = null;
    [SerializeField] private GameObject mailBtnPanel = null;
    [SerializeField] private GameObject exitBtnPanel = null;

    [Header("Goods")]
    [SerializeField] private Text boneText = null;
    [SerializeField] private Text tearText = null;
    [SerializeField] private Text soulgemText = null;

    [Header("MonsterStats")]
    [SerializeField] private Text[] atkText = null;
    [SerializeField] private Text[] defText = null;
    [SerializeField] private Text[] hpText = null;
    
    #endregion
    [SerializeField] private AudioClip panelclip = null;
    #endregion
}
