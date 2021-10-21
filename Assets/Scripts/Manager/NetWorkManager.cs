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
        print("접속 완료");
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
        print("방에 참가완료");
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
            print("닉네임을 입력하고 연결이 완료될떄까지 기다리세요");
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
        print("서버접속완료");
    }
    public override void OnCreatedRoom() => print("방만들기완료");
    public override void OnJoinedLobby() => print("로비접속완료");
    public override void OnDisconnected(DisconnectCause cause) => print("연결 끊김");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기 실패");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가 실패");
    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가 실패");

    //[ContextMenu("정보")]
    //private void Info()
    //{
    //    if(PhotonNetwork.InRoom)
    //    {
    //        print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
    //        print("현재 방 인원수: " + PhotonNetwork.CurrentRoom.PlayerCount);
    //        print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

    //        string playerStr = "방에 있는 플레이어 목록 : ";
    //        for(int i=0; i< PhotonNetwork.PlayerList.Length; ++i)
    //        {
    //            playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
    //        }
    //        print(playerStr);
    //    }
    //    else
    //    {
    //        print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
    //        print("방 개수 : " + PhotonNetwork.CountOfRooms);
    //        print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
    //        print("로비에 있는지? : " + PhotonNetwork.InLobby);
    //        print("연결됐는지? : " + PhotonNetwork.IsConnected);
    //    }
    //}
}
