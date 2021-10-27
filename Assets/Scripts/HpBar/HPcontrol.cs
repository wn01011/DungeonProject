using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPcontrol : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Init());
    }

    void Update()
    {
        HpBarController();
    }
    public IEnumerator Init()
    {
        while(GameObject.FindGameObjectWithTag("Monster")==null)
        {
            yield return new WaitForEndOfFrame();
        }
        monsterSpawn = true;
        yield return new WaitForSeconds(0.5f);
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        for(int i=0; i< monsters.Length; ++i)
        {
            t_objects.Add(monsters[i]);
        }

        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");
        for (int i = 0; i < heros.Length; ++i)
        {
            t_objects.Add(heros[i]);
        }

        t_objects.Add(GameObject.FindGameObjectWithTag("Boss"));
        HpBarMaker();
    }
    private void HpBarController()
    {
        for(int i = 0; i< monster_objectList.Count; i++)
        {
            hpBarList[i].transform.position 
                = m_cam.WorldToScreenPoint(
                    monster_objectList[i].position 
                    + Vector3.up* monster_objectList[i].GetComponentInChildren<Collider>().bounds.size.y);
        }
        if(monsterSpawn)
            HandleHP();
    }

    private void HpBarMaker()
    {
        m_cam = Camera.main;
        for (int i = 0; i < t_objects.Count; i++)
        {
            monster_objectList.Add(t_objects[i].transform);
            Slider t_hpbar = Instantiate(Hp_bar, t_objects[i].transform.position, Quaternion.identity, hpBars.transform);
            hpBarList.Add(t_hpbar);
        }
    }
    public void NewHpBarMaker(GameObject _newHero)
    {
        t_objects.Add(_newHero);
        monster_objectList.Add(_newHero.transform);
        Slider t_hpbar = Instantiate(Hp_bar, t_objects[t_objects.Count - 1].transform.position, Quaternion.identity, hpBars.transform);
        hpBarList.Add(t_hpbar);
    }
    private void HandleHP()
    {
        for(int i=0; i<monster_objectList.Count; ++i)
        {
            if(monster_objectList[i].GetComponent<Monster>())
            {
                Monster myMonster = monster_objectList[i].GetComponent<Monster>();
                hpBarList[i].value = Mathf.Lerp(hpBarList[i].value, myMonster.GetHp()/ myMonster.GetMaxHp(), Time.deltaTime*10);
            }
            else if(monster_objectList[i].GetComponent<Hero>())
            {
                Hero myHero = monster_objectList[i].GetComponent<Hero>();
                hpBarList[i].value = Mathf.Lerp(hpBarList[i].value, myHero.GetHp() / myHero.GetMaxHp(), Time.deltaTime * 10);
            }
        }
    }

    private IEnumerator ExchangeBar(int _barListNum, Monster _targetMonster)
    {
        GameObject oldMonster = t_objects[_barListNum];
        yield return new WaitForSeconds(0.1f);
        monster_objectList[_barListNum] = _targetMonster.transform;
        t_objects[_barListNum] = _targetMonster.gameObject;

        Destroy(oldMonster);
    }
    public void ExchangeBarCoroutine(int _barListNum, Monster _targetMonster)
    {
        StartCoroutine(ExchangeBar(_barListNum, _targetMonster));
    }


    [SerializeField] private Slider Hp_bar = null;
    [SerializeField] private GameObject hpBars = null;
    public List<Transform> monster_objectList = new List<Transform>();
    public List<Slider> hpBarList = new List<Slider>();
    public List<GameObject> t_objects = new List<GameObject>();

    private Camera m_cam = null;

    private bool monsterSpawn = false;
}
