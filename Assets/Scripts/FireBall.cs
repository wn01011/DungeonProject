using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBall : MonoBehaviour
{
    [SerializeField] Slider Mp_bar = null;

    public GameObject fireball = null;
    private Animator animator = null;

    private bool cast_on = false;

    private float maxMp = 100;
    private float curMp = 100;

    float mncMp;

    Vector3 mousePos, transPos;

    void Start()
    {
        mncMp = (float)curMp / (float)maxMp;
        animator = fireball.GetComponent<Animator>();
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);

        Cast();
        HandleMP();
        Regeneration();
    }

    private void Cast()
    {
        if (transPos.x > -8f && transPos.x <8f && transPos.y < 2f && transPos.y > -5f)
        {
            if (Input.GetMouseButtonDown(0) && cast_on == true)
            {
                if (curMp > 10)
                {
                    StartCoroutine("Explosion");
                    curMp -= 10;

                    if (curMp < 10)
                    {
                        cast_on = false;
                    }
                }
            }
        }
    }

    private void Regeneration()
    {
          if (curMp < 100)
          {
               curMp += Time.deltaTime * 2f;
          }
    }

    private void HandleMP()
    {
        mncMp = (float)curMp / (float)maxMp;
        Mp_bar.value = Mathf.Lerp(Mp_bar.value, mncMp, Time.deltaTime * 10);
    }


    public void Cast_on()
    {
        Debug.Log("Click");
        if (cast_on == true)
        {
            cast_on = false;
        }
        else
        {
            cast_on = true;
        }

    }

    private IEnumerator Explosion()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0f;
        GameObject myfireball = Instantiate(fireball, mPosition, Quaternion.identity);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(myfireball);
    }
}
