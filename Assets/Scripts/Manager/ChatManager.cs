using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text nickNameText = null;
    [SerializeField] private Toggle chatToggle = null;
    [SerializeField] private GameObject chatPanel = null;
    private new PhotonView photonView = null;
    public InputField chatInput = null;
    public Text[] chatText = null;
    public Text ListText = null;
    public Text RoomInfoText = null;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        nickNameText.text = PhotonNetwork.NickName;
        //nickNameText.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        for(int i=0; i < chatText.Length; ++i)
        {
            chatText[i].text = "";
        }
        RoomRenewal();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && chatInput.text != "")
        {
            Send();
        }
        if(chatToggle.isOn)
        {
            chatPanel.SetActive(true);
        }
        else if (!chatToggle.isOn)
        {
            chatPanel.SetActive(false);
        }

        
    }
    
    public override void OnJoinedRoom()
    {
        RoomRenewal();
        chatInput.text = "";
        for (int i = 0; i < chatText.Length; ++i)
            chatText[i].text = "";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }
    private void RoomRenewal()
    {
        ListText.text = "";
        for(int i=0;i<PhotonNetwork.PlayerList.Length; ++i)
        {
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");    
        }
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "��/ " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    }

    public void Send()
    {
        photonView.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] //RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�.
    private void ChatRPC(string message)
    {
        bool isInput = false;
        for(int i=0; i<chatText.Length; ++i)
        {
            if(chatText[i].text =="")
            {
                isInput = true;
                chatText[i].text = message;
                break;
            }
        }
        if(!isInput)//������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < chatText.Length; ++i)
                chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = message;
        }

    }
}
