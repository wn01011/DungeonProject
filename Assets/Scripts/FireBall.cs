using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FireBall : MonoBehaviour, IPointerDownHandler
{
    public AudioClip clip;

    void Start()
    {
        mncMp = (float)curMp / (float)maxMp;
        animator = fireball.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMP();
        Regeneration();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject curTarget = eventData.pointerCurrentRaycast.gameObject;
        int fightZoneNum = 0;

        if(cast_on && curTarget.CompareTag("FightZone"))
        {
            for(int i=0; i < fightZones.Length; ++i)
            {
                if(fightZones[i] == curTarget.GetComponent<BoxCollider>())
                {
                    fightZoneNum = i;
                    break;
                }
            }

            Hero[] curTargetHeros = spawnManager.rooms[fightZoneNum].GetComponentsInChildren<Hero>();
            if(curMp > 10)
            {
                StartCoroutine("Explosion");
                curMp -= 10;

                if(curMp < 10)
                {
                    cast_on = false;
                    uiManager.cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/cursor/cursor(1)");
                }

                for(int i=0; i< curTargetHeros.Length; ++i)
                {
                    curTargetHeros[i].Hurt(2f, 0f);
                }
            }
        }
    }

    private void Regeneration()
    {
          if (curMp < 100)
          {
               curMp += Time.deltaTime * mpRegenerateAdjust;
          }
    }

    public void MP_absorb()
    {
        curMp += 2f;
    }
    public void MP_absorb()
    {
        curMp += 2f;
    }
    private void HandleMP()
    {
        mncMp = (float)curMp / (float)maxMp;
        Mp_bar.value = Mathf.Lerp(Mp_bar.value, mncMp, Time.deltaTime * 10);
    }

    public void Cast_on()
    {
        if (cast_on == true)
        {
            cast_on = false;
            uiManager.cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/cursor/cursor(1)");
        }
        else
        {
            cast_on = true;
            uiManager.cursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/cursor/cursor(14)");
        }

    }

    private IEnumerator Explosion()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0f;
        GameObject myfireball = Instantiate(fireball, mPosition, Quaternion.identity);
        SoundManager.soundManager.SFXplayer("Fireball", clip);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(myfireball);
    }

    #region variables

    [SerializeField] Slider Mp_bar = null;
    [SerializeField] UIManager uiManager = null;
    [SerializeField] SpawnManager spawnManager = null;
    [SerializeField] BoxCollider[] fightZones = null;

    public GameObject fireball = null;
    public float mpRegenerateAdjust = 2f;
    private Animator animator = null;

    private bool cast_on = false;

    private float maxMp = 100;
    private float curMp = 100;

    float mncMp;

    #endregion
}
