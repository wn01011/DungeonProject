using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    public Text statusText = null;
    public InputField nickNameInput = null;
    private string roomName = null;
    public bool ConnectOn = false;
    [SerializeField] private GameObject networkPanel = null;

    private void Update() => statusText.text = PhotonNetwork.NetworkClientState.ToString();

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnected()
    {
        print("���� �Ϸ�");
        ConnectOn = true;
    }
    public void StartBtn()
    {
        if(ConnectOn && nickNameInput.text != "")
        {
            JoinOrCreateRoom();
            PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        }
        else
        {
            print("Enter nickname and JoinOrCreateRoom First");
        }
    }
    public override void OnJoinedRoom()
    {
        print("�濡 �����Ϸ�");
        SceneManager.LoadScene("Main");
    }
    public void DisConnect()
    {
        PhotonNetwork.Disconnect();
        ConnectOn = false;
        networkPanel.SetActive(false);
    }
    public void JoinLobby()
    {
        if(ConnectOn)
            PhotonNetwork.JoinLobby();
    }
    public void JoinOrCreateRoom() 
    {
        if (ConnectOn && nickNameInput.text != "")
        {
            roomName = "Prince's Room";
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, null);
        }
        else
        {
            print("�г����� �Է��ϰ� ������ �Ϸ�ɋ����� ��ٸ�����");
        }
    }
    public void CreateRoom()
    {
        if (ConnectOn)
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, null);
        }
    }
    public void JoinRoom() 
    {
        if (ConnectOn)
            PhotonNetwork.JoinRoom(roomName);
    }
    public void JoinRandomRoom()
    {
        if (ConnectOn)
            PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom() 
    {
        if (ConnectOn)
            PhotonNetwork.LeaveRoom();
    }
    public override void OnConnectedToMaster()
    {
        print("�������ӿϷ�");
    }
    public override void OnCreatedRoom() => print("�游���Ϸ�");
    public override void OnJoinedLobby() => print("�κ����ӿϷ�");
    public override void OnDisconnected(DisconnectCause cause) => print("���� ����");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游��� ����");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("������ ����");
    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣������ ����");

    //[ContextMenu("����")]
    //private void Info()
    //{
    //    if(PhotonNetwork.InRoom)
    //    {
    //        print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
    //        print("���� �� �ο���: " + PhotonNetwork.CurrentRoom.PlayerCount);
    //        print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

    //        string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
    //        for(int i=0; i< PhotonNetwork.PlayerList.Length; ++i)
    //        {
    //            playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
    //        }
    //        print(playerStr);
    //    }
    //    else
    //    {
    //        print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
    //        print("�� ���� : " + PhotonNetwork.CountOfRooms);
    //        print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
    //        print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
    //        print("����ƴ���? : " + PhotonNetwork.IsConnected);
    //    }
    //}
}
