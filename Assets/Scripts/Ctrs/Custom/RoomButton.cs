using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class RoomButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI roomNameTxt ;
    [SerializeField] private TextMeshProUGUI roomSizeTxt ;

    private string roomName;
    private int roomSize;
    private int playerCount;
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public  void SetRoom(string nameInput,int sizeInput,int countInput)
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        roomNameTxt.text = nameInput;
        roomSizeTxt.text =countInput +"/"+ sizeInput;
    }
}
