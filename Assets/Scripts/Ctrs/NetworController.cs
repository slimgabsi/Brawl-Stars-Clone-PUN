using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworController : MonoBehaviourPunCallbacks  
{

    public static NetworController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//connects to photon master servers 
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("*****connected to "+ PhotonNetwork.CloudRegion+" server*****");
    }
  
}
