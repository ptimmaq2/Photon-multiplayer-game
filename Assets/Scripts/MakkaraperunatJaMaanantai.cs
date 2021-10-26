using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MakkaraperunatJaMaanantai : MonoBehaviourPunCallbacks
{
    public float movementSpeed;
    public float rotateSpeed;

    public Camera myCam;

    public GameObject ammo;
    public GameObject ammoSpawn;

    public float health;
    public Text healthField;
    public string playerName;
    public Text nameField;

    // Start is called before the first frame update
    void Start()
    {

        object[] obj = photonView.InstantiationData;
        playerName = obj[0].ToString();
        nameField.text = playerName;

        if (photonView.IsMine)
        {
            myCam.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            float xMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            float zMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

            transform.Translate(xMovement, 0, zMovement);

            float mouseInput = Input.GetAxis("Mouse X");
            Vector3 lookHere = new Vector3(0, mouseInput * rotateSpeed * Time.deltaTime, 0);
            transform.Rotate(lookHere);

            if (Input.GetButtonDown("Fire1"))
            {
                photonView.RPC("Shoot", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void Shoot()
    {
        GameObject ammoInstance = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        ammoInstance.GetComponent<Rigidbody>().AddForce(ammoSpawn.transform.forward * 30, ForceMode.Impulse);
        Destroy(ammoInstance, 3);
    }

    [PunRPC]
    public void LoseHealth()
    {
        health -= 10;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ammo") && photonView.IsMine) //bufferoidaan muutokset healthiin, jotta ne åäivittyy kaikille myöhemmin tuleville pelaajille.
        {
            photonView.RPC("LoseHealth", RpcTarget.AllBufferedViaServer);
            Destroy(collision.gameObject);
        }
    }
}
