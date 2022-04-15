using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public GameObject frmMain;
    public GameObject frmCrud;
    public GameObject frmJoin;
    public TMP_InputField inpUsername;
    public Button btnNew;
    public Button btnLoad;
    public Button btnJoin;
    public Button btnExit;


    //Host Inputs
    public TMP_InputField inpChatName;
    public TMP_InputField inpHostIP;
    //Needs a check mark for IPv6 or IPv4
    public TMP_InputField inpMaxPlayer;
    public TMP_InputField inpHostPassword;
    //Host Buttons
    public Button btnHost;
    public Button btnHostBack;

    //Join Inputs
    public TMP_InputField inpClientIP;
    public TMP_InputField inpClientPass;
    //Join Buttons
    public Button btnClientJoin;
    public Button btnClientBack;



    // Start is called before the first frame update
    void Start()
    {
        //main menu
        Debug.Log(PlayerPrefs.GetString("username"));
        btnNew.onClick.AddListener(delegate { toHost(); });
        btnLoad.onClick.AddListener(delegate { toHost(); });
        btnJoin.onClick.AddListener(delegate { toJoin(); });
        btnExit.onClick.AddListener(delegate { Exit(); });
        //host menu
        btnHost.onClick.AddListener(delegate { startHost(); });
        btnHostBack.onClick.AddListener(delegate { toMain(); });
        //client menu
        btnClientJoin.onClick.AddListener(delegate { joinChat(); });
        btnClientBack.onClick.AddListener(delegate { toMain(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void toHost(string chatName = "", string ip = "127.0.0.1", int maxPlayer = 8, string Password = "") { //For the host, the IP should be disabled for the input, and then should be able to show IPv6 or IPv4
        frmMain.SetActive(false);
        frmCrud.SetActive(true);
        frmJoin.SetActive(false);

        inpChatName.text = chatName;
        inpHostIP.text = ip;
        inpMaxPlayer.text = maxPlayer+"";
        inpHostPassword.text = Password ;



    }

    void toHost()
    { //For the host, the IP should be disabled for the input, and then should be able to show IPv6 or IPv4
        frmMain.SetActive(false);
        frmCrud.SetActive(true);
        frmJoin.SetActive(false);




    }

    void toJoin(string username = "") {
        frmMain.SetActive(false);
        frmCrud.SetActive(false);
        frmJoin.SetActive(true);
    }

    void toMain(){
        frmMain.SetActive(true);
        frmCrud.SetActive(false);
        frmJoin.SetActive(false);
    }

    void startHost() {
        //SceneManager.LoadScene()
        PlayerPrefs.SetString("username", inpUsername.text);
        PlayerPrefs.SetString("chatname", inpChatName.text);
        PlayerPrefs.SetString("hostIP", inpHostIP.text);
        PlayerPrefs.SetString("hostPassword", inpHostPassword.text);

        SceneManager.LoadScene("chat");

    }

    void joinChat() {
        PlayerPrefs.SetString("username", inpUsername.text);
        PlayerPrefs.SetString("clientPassword", inpClientPass.text);
        SceneManager.LoadScene("chat");

    }

    void Exit() {
        PlayerPrefs.SetString("username", inpUsername.text);
        Application.Quit();
    }
}
