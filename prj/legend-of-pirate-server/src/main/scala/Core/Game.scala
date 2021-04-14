package Core 
import akka.actor.ActorRef

object Game {
    trait GameData
        case class PlayerDataWithActor(profile: PlayerData, actor: ActorRef)
        case class PlayerData(userName: String, data: Option[InGameData]) extends GameData
        case class InGameData(score: Int, level: Int, stillAlive: Boolean) extends GameData

    trait GameEvent 
        case class EnterLobby(userName: String, actor: ActorRef) extends GameEvent
        case class ExitLobby(userName: String) extends GameEvent
        case class LobbyUpdate() extends GameEvent
        case class LobbyChanged(profile: Iterable[PlayerData]) extends GameEvent
        case class StartGameFromLobby() extends GameEvent
        case class CreateRoom(roomID: String) extends GameEvent

        case class EnterGameMaster(profile: Iterable[PlayerDataWithActor]) extends GameEvent
        case class GameUpdate(profile: PlayerData) extends GameEvent
        case class GameMasterChanged(profile: Iterable[PlayerData]) extends GameEvent
        case class JoinMatch(userName: String, actor: ActorRef) extends GameEvent
        case class LeftMatch(userName: String) extends GameEvent
        case class UpdateScore(profile: PlayerData, newScore: String) extends GameEvent
        case class UpdateLevel(profile: PlayerData, newLevel: String) extends GameEvent

        case class Fire(eggDir: Int ) extends GameEvent
        case object SwapEgg extends GameEvent
        case class SendTrashEgg() extends GameEvent
}

object GameLogic {
    
}

object GameBehavior {
    
}