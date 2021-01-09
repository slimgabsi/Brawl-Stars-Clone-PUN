using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;
    [SerializeField]private int multiplayerSceneIndex;
    [SerializeField]private int menuSceneIndex;
    [SerializeField]private int minPlayersToStart;
    private int playerCount;
    private int roomSize;
    [SerializeField] private TextMeshProUGUI playerCountTXT;
    [SerializeField] private TextMeshProUGUI timeToStartTXT;
    private bool readyToCountDown;
    private bool readyTosStart;
    private bool startingGame;
    private float timertoStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    [SerializeField]private float maxWaitTime;
    [SerializeField]private float maxFullGameWaitTime;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timertoStartGame = maxWaitTime;
        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountTXT.text = playerCount.ToString() + " : " + roomSize;

        if (playerCount == roomSize)
        {
            readyTosStart = true;
        }
        else if(playerCount>=minPlayersToStart) {

            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyTosStart = false;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timertoStartGame);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timertoStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn<fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }
    private void Update()
    {
        WaitingForMorePLayers();
    }
    void WaitingForMorePLayers()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }
        if (readyTosStart)
        {
            fullGameTimer -= Time.deltaTime;
            timertoStartGame = fullGameTimer;
        }
        else if (readyToCountDown) {
            notFullGameTimer -= Time.deltaTime;
            timertoStartGame = notFullGameTimer;
        }
        string tempTimer = string.Format("{0:00}", timertoStartGame);
        timeToStartTXT.text = tempTimer;
        if (timertoStartGame <= 0)
        {
            if (startingGame) return;
            StartGame();  
        }
    }

    void ResetTimer()
    {
        timertoStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxWaitTime;
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
