using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Lista kaikista huoneista
    private List<RoomInfo> roomsList;
    private const string roomName = "AntinHuone";

    public GUIStyle myStyle;
    public string playerNameInput;

    // Start is called before the first frame update
    void Start()
    {
        myStyle.normal.textColor = Color.red;
        myStyle.fontSize = 20;

        roomsList = new List<RoomInfo>();
        // Yhdistet‰‰n pilveen/serveriin
        PhotonNetwork.ConnectUsingSettings();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.IsConnectedAndReady.ToString(), myStyle);
        GUILayout.Label(PhotonNetwork.InLobby.ToString(), myStyle); // True jos ollaan lobbyssa. False jos ollaan huoneessa tai muualla. 

        // Jos ollaan lobbyssa, eli EI olla viel‰ miss‰‰n huoneessa, annetaan mahdollisuus tehd‰ huone
        // Listataan myˆs olemassaolevat huoneet ja annetaan mahdollisuus liitty‰ niihin. 
        if (PhotonNetwork.InRoom == false)
        {
            playerNameInput = GUI.TextField(new Rect(20, 80, 200, 30), playerNameInput, 25);

            // Nappula huoneen luontia varten. 
            if (GUI.Button(new Rect(100, 100, 200, 100), "Create Room"))
            {
                PhotonNetwork.CreateRoom(roomName + System.Guid.NewGuid().ToString("N"));
            }

            // Listataan kaikki olemassaolevat huoneet. Listataan vain jos huoneita on nemm‰n kuin 0
            if (roomsList.Count > 0)
            {
                // Tehd‰‰n jokaista huonetta varten nappula
                int i = 0;
                foreach (RoomInfo room in roomsList)
                {
                    if (GUI.Button(new Rect(100, 250 + 110 * i, 200, 100), "Join: " + room.Name))
                    {
                        PhotonNetwork.JoinRoom(room.Name);

                    }
                    i++;
                }
            }
        }
    }

    // P‰ivitet‰‰n paikallinen huonelistaus. Funktio ajetaan automaattisesti, kun serverill‰ tapahtuu mit‰ tahansa muutoksia huonelistaukseen. 
    // Serverin huonelistaus tulee parametrina funktiolle automaagisesti. 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomsList = roomList;
    }


    public override void OnConnectedToMaster()
    {
        // t‰m‰ ajetaan kun ollaan saatu yhteys serveriin.
        // Liityt‰‰n lobbyyn
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedRoom()
    {

        string[] pldata = new string[1];
        pldata[0] = playerNameInput;
        GameObject.FindGameObjectWithTag("BirdEyeCamera").SetActive(false);
        // base.OnJoinedRoom();
        PhotonNetwork.Instantiate("PlayerBox", new Vector3 (0, 0.5f, 0), Quaternion.identity, 0, pldata);


    }

}
