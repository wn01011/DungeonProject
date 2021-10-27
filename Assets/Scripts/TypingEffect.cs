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
        {"���� �������� ����� ��Ī�ϴ� �ΰ����� ��� ������ ��������.\n\n" ,
        "������ �μ��� �°� ������ �ϻ�� ��� �������� ���տ� ���� �������\n\n" +
        "������� ������ �����ϱ� �����ߴ�.\n\n" ,
        "����� ������ �̸��� \n\n" };
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
