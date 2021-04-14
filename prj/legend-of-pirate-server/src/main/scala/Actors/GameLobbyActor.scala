package Actors

import akka.actor.{Actor, ActorLogging}
import Core.Game._ 

class GameLobbyActor extends Actor with ActorLogging {
    val playersInLobby = collection.mutable.LinkedHashMap[String, PlayerDataWithActor]()

    override def receive: Receive = {
       case EnterLobby(userName, actor) =>
            val newPlayer = PlayerData(userName, None)
            playersInLobby += (newPlayer.userName -> PlayerDataWithActor(newPlayer, actor))
            log.info(s"Player $userName enter lobby")
            NotifyLobbyUpdate()

        case ExitLobby(userName) =>
            playersInLobby -= userName
            log.info(s"Player $userName exit lobby")
            NotifyLobbyUpdate()

        case CreateRoom(roomID) => 
            //TODO: Implement code here
            log.info(s"Create $roomID room")
            NotifyLobbyUpdate()
            
        case StartGameFromLobby()  => 
            log.info("Start from lobby")
            NotifyEnterGame()

        case _ => 
            log.info("Enter Game lobby actor")
    }

    def NotifyLobbyUpdate(): Unit = {
        playersInLobby.values.foreach(_.actor ! LobbyChanged(playersInLobby.values.map(_.profile)))
    }

    def NotifyEnterGame(): Unit = {
        playersInLobby.values.foreach(_.actor ! EnterGameMaster(playersInLobby.values))
    }

}