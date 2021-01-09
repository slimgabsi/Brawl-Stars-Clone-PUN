using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void CreatePlayer()
    {
        Debug.Log("Creating Player");

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PhotonPlayer"), LevelController.instance.GetRandomSpawnPos(), Quaternion.identity);

        
    }
}
