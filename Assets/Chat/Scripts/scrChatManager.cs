using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class scrChatManager : NetworkBehaviour
{


    //Logs should note the time zone, also should be based on the host.
    //Consider enabling logging for users as well.
    [System.Serializable]
    public struct Message
    {//this is mostly used to record logging.
        public string UID; //could be useless for the sake of logging
        public DateTime stamp;
        public string stampname; //Important cause userID differs from the name, example someone could be Bill892, but their name could be Carrie
        public string text;
        public string ip; //might not be useful 
        public bool skipHistory;

    }

    public struct Player
    {
        public string UID; //Maybe create this on first run of the program? Save it in playerprefs? Alternatively have the user create their own userID, this has the issue of having to worry about people with the same ID. Probably just make it impossible to user the same username. The game isn't large scale enough like an MMO for this to be a massive problem.
        public string name;
        public string ipv4;
        public string ipv6;
    }

    public class ListMessages {
        public List<Message> messages;
    }

    public TMP_InputField parser;
    public GameObject messageList;
    public GameObject messageUI;
    public Scrollbar hbar;

    private int historyMaxMessage = 20; //Default load 20 lines from the history.
    private Player localPlayer = new Player(); //The local player.
    private Player TestPlayer;
    private bool IsLoadingHistory = true;
    private int histCount = 0;
    private int histInd = 0;
    private List<Player> allPlayers; //player list handled on the server only.
    private List<Message> allMessages = new List<Message>(); //The list of all possible messages

    [SerializeField] private GameObject canvas = null;

    public readonly SyncList<Message> messages = new SyncList<Message>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        canvas.SetActive(true);

       

        messages.Callback += OnMessageUpdated;

        for(int index = 0; index < messages.Count; index++)
            OnMessageUpdated(SyncList<Message>.Operation.OP_ADD, index, new Message(), messages[index]);

       
        //loadHistory();

    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        canvas.SetActive(false);

    }


    void Start()
    {

        localPlayer.name = "Testperger";
        localPlayer.ipv4 = "127.0.0.1";
        localPlayer.UID = "UID12345";
        string test = "butttest";
        DateTime testTime = DateTime.Now;
        Debug.Log(DateTime.Now);
        Debug.Log(DateTime.Now.ToString("[hh:mm:ss] ") + test + ":");
        Debug.Log("Hours: " + testTime.Hour);
        Debug.Log("Minutes: " + testTime.Minute);
        Debug.Log("Seconds: " + testTime.Second);
        Debug.Log(Application.dataPath + "/data");

        if (base.isServer)
        {
            localPlayer.name = "TestpergerServer";

            loadHistory();
            //sendMessage("test");
            //sendMessage("Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.");
            //sendMessage("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            //sendMessage("What is Lorem Ipsum?");
            //sendMessage("A perfect example of how powerful the SNES soundchip actually was.");
            //sendMessage("They really pushed the SNES to its limits with this game’s OST, my god.");
            //sendMessage("Ok if you aren't listening to this on headphones that can do directional audio you should be. This track is full of sliding sound from left ear to right ear and back again that you don't get without them. I feel like I'm driving donuts around a guitarist in some kind of sweet car. Plain old stereo audio was barely even a thing for TVs at the time, but Tim Follin went hard with not only stereo, but full on 180 degree sliding gradients in his rando game soundtrack. On a SNES. ");
        }
    }



    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Return))
        {
            sendMessage();
        }


     
    }

    // Called when the a client is connected to the server
    public override void OnStartAuthority()
    {
        canvas.SetActive(true);

        // OnMessage += HandleNewMessage;
    }

    // Called when a client has exited the server
    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        //OnMessage -= HandleNewMessage;
    }


    [Command(requiresAuthority = false)]
    private void CmdSend(Message m)
    {
        messages.Add(m);
    }

    [ClientRpc]
    private void RpcSend(Message m)
    {
        CmdSend(m);
    }

    [Client]
    public void CSendMsg(Message m) {
        CmdSend(m);
    }

    void appendMessage()
    {
    }

    //(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem

    void OnMessageUpdated(SyncList<Message>.Operation op, int index, Message oldM, Message newM) {

        Debug.Log("bllah");

        //Maybe instead create a message field to skip history.


        switch (op)
        {
            case SyncList<Message>.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item
                if (base.isServer && !newM.skipHistory) {
                   
                    //System.IO.File.WriteAllText(Application.dataPath + "/data/logs.json", history);
                    //System.IO.File.Exists(Application.dataPath.Write
                    Debug.Log("Log saved!");

                    if (File.Exists(Application.dataPath + "/data/chatlog.json"))
                    {

                        //Debug.Log(Application.dataPath + "/data/maptest.json");
                        //Debug.Log(messages[0].ToString());
                        string json = File.ReadAllText(Application.dataPath + "/data/chatlog.json");
                        ListMessages ml = JsonUtility.FromJson<ListMessages>(json);
                        ml.messages.Add(newM);
                        json = JsonUtility.ToJson(ml);
                        File.WriteAllText(Application.dataPath + "/data/chatlog.json", json);

                    }
                    else
                    {//On first run
                        Debug.Log("TEst: " + newM.text);
                        ListMessages ml = new ListMessages();
                        ml.messages = new List<Message>();
                        ml.messages.Add(newM);
                        string json = JsonUtility.ToJson(ml);
                        Debug.Log(Application.dataPath + "/data/chatlog.json");
                        Directory.CreateDirectory(Application.dataPath + "/data/"); //Create file path
                        using (File.Create(Application.dataPath + "/data/chatlog.json")); // Create file
                        File.WriteAllText(Application.dataPath + "/data/chatlog.json", json);//Write to file
                        
                    }

                }
                GameObject nm = Instantiate(messageUI);

                //Needs a check for local player only

                nm.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = newM.stampname;
                nm.transform.GetChild(1).GetComponent<TMP_Text>().text = newM.text;
                nm.transform.SetParent(messageList.transform, false);
                histInd++;
                if (histCount == histInd) {
                    IsLoadingHistory = false;
                }
                
                break;
            case SyncList<Message>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncList<Message>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                break;
            case SyncList<Message>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                break;
            case SyncList<Message>.Operation.OP_CLEAR:
                // list got cleared
                break;
        }

    }

    void sendMessage() //Called when users submits a message 
    {
        Message newMessage = new Message();

        //GetMessageFromParser
        newMessage.text = parser.text;

        if (newMessage.text[0] == '/') {//Check for slash command, consider the idea of implementing errors into the logs
            newMessage.skipHistory = true;
            newMessage.text = "[ERROR]: Invalid Command";
               
        }

        //GetTimeStamp
        newMessage.stamp = DateTime.Now;

        //AppendUserName
        newMessage.stampname = newMessage.stamp.ToString("[hh:mm:ss] ") + localPlayer.name + ": "; //Takes the time stamp and the player name

        //Take string length, multiple by character width, take that value. Assign to the width of gameobject
        //Just need to figure out character width.

        


        //AddToMessageList
        if (base.isServer) { 
            messages.Add(newMessage);
        }
        if (!base.isServer) {
            CSendMsg(newMessage);
        }

        parser.text = "";
        


        //saveHistory();




    }

    void sendMessage(string m) //for testing only.
    {
        Message newMessage = new Message();

        //GetTimeStamp
        newMessage.stamp = DateTime.Now;

        //AppendUserName
        newMessage.stampname = newMessage.stamp.ToString("[hh:mm:ss] ") + localPlayer.name + ": "; //Takes the time stamp and the player name

        //GetMessageFromParser
        newMessage.text = m;

        //AddToMessageList
        messages.Add(newMessage);

        

    }

    void getMessage() //Called when message is submitted
    {
    }

    void saveHistory() //Called when message is submitted
    {
        //This should trigger on the host side only.
        //Write this raw for each, later write function to check if the history was loaded first.

    }

    void loadHistory() //Called when chat first boots up.
    {//pull the file 
        if (!File.Exists(Application.dataPath + "/data/chatlog.json")) { IsLoadingHistory = false; return; }

            string json = File.ReadAllText(Application.dataPath + "/data/chatlog.json");
        if (json == "{}" || String.IsNullOrEmpty(json)) { IsLoadingHistory = false; return; }
        ListMessages ml = JsonUtility.FromJson<ListMessages>(json);
        histCount = ml.messages.Count;
        foreach (Message m in ml.messages) {
           Message nm = m;
            nm.skipHistory = true;
            messages.Add(m);
        };

    }

    void createHistory() //Called when
    {
        //create the file in the desired location.
    }

    void onServerStart()
    {
        //load history, if history doesn't exist create the file.

    }

    void onPlayerJoin()
    {
        //Call When Player Joins
        //Assign this player to the local player list
        //Add player to the player list.
        //Update Player List for everyone.
        //Push System message that player has joined

    }

    void onPlayerLeave()
    {
        //update player list
        //push system mesage that player has left.
    }
    // **HOST ONLY SLASH ACTIONS** 

    void kickPlayer(string uid)
    {
        // /kick uid


    }

    void banPlayer(string uid)
    { //Maybe try fetching IP based on UID to ban/kick players. 
        // /ban uid
    }

    void pingPlayer(string uid)
    { //make a sound at a player to wake them up.
        // /ping uid
    }

    void clearChat()
    { // clears the chat for all players.
        // /clear
    }

    void assignRole()
    { //This can honestly wait, but eventually the host should be able to check off thins for the users to be able to do.
      //not sure how this should be handled yet

    }

    // **BOTH**

    void console()
    {
        // /console
    }

    void whisper(string senderUID, string receiverUID)
    {

    }


}
