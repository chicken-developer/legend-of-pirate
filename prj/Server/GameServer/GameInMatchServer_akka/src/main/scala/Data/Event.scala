package Data

import akka.actor.ActorRef
import Data.GameData._

case object GameEvent {
  trait GameMasterEvent

  case class GameMasterUpdate(playerID: UserID) extends GameMasterEvent
  case class JoinMatch(playerID: UserID) extends GameMasterEvent 
  case class LeftMatch(playerID: UserID) extends GameMasterEvent 

  case class MovementRequest(playerID: UserID, direction: String) extends GameMasterEvent 
  case class AttackRequest(playerID: UserID, target: String) extends GameMasterEvent 
  //TODO: Add more event in feature

}