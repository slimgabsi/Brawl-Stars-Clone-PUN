using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using System.Linq;

public class CustomMatchMakingLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject lobbyConnectBtn;
    [SerializeField] private GameObject lobbyPanel;


    [SerializeField] private TMPro.TMP_InputField playerNameInput;
    private string roomName;
    private int roomSize;
    private List<RoomInfo> roomListings;
    [SerializeField] private Transform roomContainer;
    [SerializeField] private GameObject roomListingPrefab;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectBtn.SetActive(true);
        roomListings = new List<RoomInfo>();

        if (PlayerPrefs.HasKey("NickName"))
        {
            if (PlayerPrefs.GetString("NickName")=="")
            {
                PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 10000);
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            }
           
        }
        else PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 10000);

        playerNameInput.text = PhotonNetwork.NickName;
        
    }
    public void PlayerNameUpdate(string nameInput)
    {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
        playerNameInput.text = nameInput;
    }

    public void JoinLobbyOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
        Debug.Log("joining Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int tempIndex;
        foreach (RoomInfo room in roomList)
        {
            if (roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }

            else tempIndex = -1;

            if(tempIndex!=-1)
            {
                roomList.RemoveAt(tempIndex);
                Destroy(roomContainer.GetChild(tempIndex).gameObject);
            }

            if(room.PlayerCount>0)
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void OnRoomNameChanged(string nameIN)
    {
        roomName = nameIN;
    } public void OnRoomSizeChanged(string  sizeIN)
    {
        roomSize = int.Parse(sizeIN);
    }

    public void CreateRoom()
    {
        Debug.Log("creating Room");
        RoomOptions roomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("faield to create room");

    }

    public void MatchMackingCancel()
    {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}

