using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TypingEffect : MonoBehaviourPunCallbacks
{
    public AudioClip clip = null;

    Vector2 textTargetPos = new Vector2(0f, 90f);

    public Text moveText = null;
    public Text ending_tx = null;
    private string[] m_text = 
        {"세계 각지에서 용사라고 자칭하던 인간들은 모두 던전에 삼켜졌다.\n\n" ,
        "때리고 부수고 온갖 악행을 일삼던 용사 무리들이 마왕에 의해 사라지고\n\n" +
        "사람들은 마왕을 숭배하기 시작했다.\n\n" ,
        "고귀한 마왕의 이름은 \n\n" };
        //"_nickName";

    void Start()
    {
        moveText.text = PhotonNetwork.NickName;
        StartCoroutine(Typing());
        StartCoroutine(TextMover());
    }

    private IEnumerator TextMover()
    {
        yield return new WaitForSeconds(15f);
        while (moveText.rectTransform.anchoredPosition != textTargetPos)
        {
            moveText.rectTransform.anchoredPosition
                = Vector2.MoveTowards(
                    moveText.rectTransform.anchoredPosition,
                    textTargetPos,
                    Time.deltaTime * 200f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Typing()
    {
        for(int i = 0; i<= m_text[0].Length; i++)
        {
            ending_tx.text = m_text[0].Substring(0, i);
            SoundManager.soundManager.SFXplayer("Typing", clip);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i <= m_text[1].Length; i++)
        {
            ending_tx.text = m_text[0] + m_text[1].Substring(0, i);
            SoundManager.soundManager.SFXplayer("Typing", clip);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i <= m_text[2].Length; i++)
        {
            ending_tx.text = m_text[0] + m_text[1] +m_text[2].Substring(0, i);
            SoundManager.soundManager.SFXplayer("Typing", clip);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
