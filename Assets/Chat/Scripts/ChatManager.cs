using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{

    //Logs should note the time zone, also should be based on the host.
    //Consider enabling logging for users as well.
    [System.Serializable]
    public struct Message {//this is mostly used to record logging.
        public string UID; //could be useless for the sake of logging
        public DateTime stamp;
        public string name; //Important cause userID differs from the name, example someone could be Bill892, but their name could be Carrie
        public string message;
        public string ip; //might not be useful 

    }

    public struct Player {
        public string UID; //Maybe create this on first run of the program? Save it in playerprefs? Alternatively have the user create their own userID, this has the issue of having to worry about people with the same ID. Probably just make it impossible to user the same username. The game isn't large scale enough like an MMO for this to be a massive problem.
        public string name;
        public string ipv4;
        public string ipv6;
    }

    private int historyMaxMessage = 20; //Default load 20 lines from the history.
    private Player localPlayer; //The local player.
    private List<Player> allPlayers; //player list handled on the server only.


    void sendMessage(){

    }

    void getMessage(){
    }

    void saveHistory(){
    
    }

    void loadHistory(){//pull the file 
    
    }

    void createHistory()
    {
        //create the file in the desired location.
    }

    void onServerStart()
    {
        //load history, if history doesn't exist create the file.

    }

    void onPlayerJoin() {
        //Call When Player Joins
        //Assign this player to the local player list
        //Add player to the player list.
        //Update Player List for everyone.
        //Push System message that player has joined
       
    }

    void onPlayerLeave() { 
        //update player list
        //push system mesage that player has left.
    }
    // **HOST ONLY SLASH ACTIONS** 
    
    void kickPlayer(string uid) {
        // /kick uid
              
    
    }

    void banPlayer(string uid) { //Maybe try fetching IP based on UID to ban/kick players. 
        // /ban uid
    }

    void pingPlayer(string uid) { //make a sound at a player to wake them up.
        // /ping uid
    }

    void clearChat() { // clears the chat for all players.
        // /clear
    }

    void assignRole() { //This can honestly wait, but eventually the host should be able to check off thins for the users to be able to do.
        //not sure how this should be handled yet
    
    }

    // **BOTH**

    void console() { 
        // /console
    }

    void whisper(string senderUID, string receiverUID) { 

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
