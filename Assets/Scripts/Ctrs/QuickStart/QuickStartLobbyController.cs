using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject quickStartBtn;
    [SerializeField] private GameObject quickCancelBtn;
    [SerializeField] private int roomSize;

    void Awake()
    {
        quickStartBtn.SetActive(false); quickStartBtn.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartBtn.SetActive(true); 
    }
  
    public void QuickStart()
    {
        quickStartBtn.SetActive(false);
        quickCancelBtn.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("quick start");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("failed to join room");
        CreateRoom();
    }
    public void CreateRoom() 
    {
        Debug.Log("creating room now ");
        int randomRoomNumber = Random.Range(1, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("failed to create room .. trying again");
        CreateRoom();
    }

    public void QuickCancel()
    {
        quickCancelBtn.SetActive(false);
        quickStartBtn.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
