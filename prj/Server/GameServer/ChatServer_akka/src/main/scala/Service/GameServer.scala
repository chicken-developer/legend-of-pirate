package Service

import Actors.GameMasterActor
import Data._
import Data.GameData._
import Data.GameEvent._
import akka.actor.{ActorRef, ActorSystem, Props}
import akka.http.scaladsl.model.ws.{Message, TextMessage}
import akka.http.scaladsl.server.Route
import akka.stream.{FlowShape, Materializer, OverflowStrategy}
import akka.stream.scaladsl.{Flow, GraphDSL, Merge, Sink, Source}
import akka.http.scaladsl.server.Directives._

class GameServer(implicit val system: ActorSystem, implicit val materializer: Materializer) {

    val GameMasterHandleActor: ActorRef = system.actorOf(Props[GameMasterActor], "GameMasterHandleActor")
    val gameMasterProfileSource: Source[GameMasterEvent, ActorRef] = Source.actorRef[GameMasterEvent](5,OverflowStrategy.fail)

    def gameInMatchFlow(fullProfile: GameMasterProfileWithInformation): Flow[Message, Message, Any] =
        Flow.fromGraph(GraphDSL.create(gameMasterProfileSource) { implicit builder => gameInMatchProfileShape =>
            import GraphDSL.Implicits._
            val materialization = builder.materializedValue.map(profileActorRef => JoinMatch(fullProfile.profileID))
            val merge = builder.add(Merge[GameMasterEvent](2))
            val gameInMatchProfileSink = Sink.actorRef[GameMasterEvent](GameMasterHandleActor, LeftMatch(fullProfile.profileID))

            val MessageToGameInMatchEventConverter = builder.add(Flow[Message].map {
                case TextMessage.Strict("move") =>
                    println("Have movement request from " + fullProfile.profileID)
                    MovementRequest(fullProfile.profileID, "move")
                case TextMessage.Strict("attack") =>
                    println("Have attack request from " + fullProfile.profileID)
                    AttackRequest(fullProfile.profileID, "attack")
                //TODO: Priority 3: Implement game in match request
            })

            val GameInMatchEventBackToMessageConverter = builder.add(Flow[GameMasterEvent].map{
                case MovementRequest(playerID, direction) =>
                    TextMessage("NewPlayerPosition.toJSon.Serialization")
                case AttackRequest(playerID, target) =>
                    TextMessage("NewPlayerStats.toJSon.Serialization and target") 
            })
            materialization ~> merge ~> gameInMatchProfileSink
            MessageToGameInMatchEventConverter ~> merge 
            gameInMatchProfileShape ~> GameInMatchEventBackToMessageConverter
            FlowShape(MessageToGameInMatchEventConverter.in, GameInMatchEventBackToMessageConverter.out)
        })
        
    val GameMasterFullProfile: GameMasterProfileWithInformation = Database.PlayerFindMatch(UserID("soisan"))
    
     val GameMasterRoute: Route = {
        get {
            handleWebSocketMessages(gameInMatchFlow(GameMasterFullProfile))
        }
    }

    val FinalRoute: Route = GameMasterRoute
}