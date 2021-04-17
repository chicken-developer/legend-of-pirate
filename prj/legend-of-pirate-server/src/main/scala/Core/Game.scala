package Core 
import akka.actor.ActorRef

object Game {
    trait GameData
        case class PlayerDataWithActor(profile: PlayerData, actor: ActorRef)
        case class PlayerData(userName: String, data: Option[InGameData]) extends GameData
        case class InGameData(score: Int, level: Int, stillAlive: Boolean) extends GameData

    trait GameEvent
        case class EnterGameMaster(profile: Iterable[PlayerDataWithActor]) extends GameEvent
        case class GameUpdate(profile: PlayerData, newData: String) extends GameEvent
        case class GameMasterChanged(profile: Iterable[PlayerData]) extends GameEvent
        case class JoinMatch(userName: String, actor: ActorRef) extends GameEvent
        case class LeftMatch(userName: String) extends GameEvent

}

