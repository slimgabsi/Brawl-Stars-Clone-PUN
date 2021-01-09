using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject delayStartBtn;
    [SerializeField] private GameObject delayCancelBtn;
    [SerializeField] private int roomSize;

    void Awake()
    {
        delayStartBtn.SetActive(false); delayCancelBtn.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartBtn.SetActive(true); 
    }
  
    public void DelayStart()
    {
        delayStartBtn.SetActive(false);
        delayCancelBtn.SetActive(true);
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

    public void DelayCancel()
    {
        delayCancelBtn.SetActive(false);
        delayStartBtn.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
