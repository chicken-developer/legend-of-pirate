package Actors

import akka.actor.{Actor, ActorLogging}
import com.google.gson._ 
import Core.Game._ 
import Core.GameBehavior._
class GameMasterActor extends Actor with ActorLogging {
    
    val playersInGame = collection.mutable.LinkedHashMap[String, PlayerDataWithActor]()

    override def receive: Receive = {
        case JoinMatch(userName, actor) =>
            val newPlayer = PlayerData(userName,
                                        Option(
                                            InGameData(
                                                0,
                                                0,
                                                0,
                                                List(1,2,3,2,1,5),
                                                stillAlive = true)
                                        ))
            playersInGame += (userName -> PlayerDataWithActor(newPlayer, actor))
            println(s"Player $userName enter game success")
            NotifyGameUpdate()

        case LeftMatch(userName) =>
            playersInGame -= userName
            NotifyGameUpdate()

        case UpdateScore(player, newScore) =>
            log.info("Enter case update score")
            NotifyGameUpdate()    
        case _ => log.info("Enter Game master actor")
    }

    def NotifyGameUpdate(): Unit = {
        playersInGame.values.foreach(_.actor ! GameMasterChanged(playersInGame.values.map(_.profile)))
    }

}