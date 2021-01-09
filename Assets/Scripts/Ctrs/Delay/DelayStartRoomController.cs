using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class DelayStartRoomController : MonoBehaviourPunCallbacks
{

    [SerializeField] private int waitingRoomSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined room");
        StartGame();

    }
    private void StartGame()
    {
         if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("starting game");
            SceneManager.LoadScene(waitingRoomSceneIndex); // load into waiting room
        }
    }
}
