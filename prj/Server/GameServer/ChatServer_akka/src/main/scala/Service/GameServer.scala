package Service

import Actors.GameMasterActor
import Service.GameData.GameProfile
import akka.actor.{ActorRef, ActorSystem, Props}
import akka.http.scaladsl.model.ws.{Message, TextMessage}
import akka.http.scaladsl.server.Route
import akka.stream.{FlowShape, Materializer, OverflowStrategy}
import akka.stream.scaladsl.{Flow, GraphDSL, Merge, Sink, Source}
import akka.http.scaladsl.server.Directives._

object GameData {
    trait GameMasterData
    case class GameProfileWithActor(profile: GameProfile, actor: ActorRef)
    case class GameProfile(userName: String,highestScore: String, currentLevel: String,currentScore: Int) extends GameMasterData
}
object GameEvent {
    trait GameMasterEvent
    case class GameUpdate(profile: GameProfile) extends GameMasterEvent
    case class JoinMatch(profile: GameProfile) extends GameMasterEvent
    case class LeftMatch(profile: GameProfile) extends GameMasterEvent

    case class UpdateScore(profile: GameProfile, newScore: String) extends GameMasterEvent
    case class UpdateLevel(profile: GameProfile, newLevel: String) extends GameMasterEvent
    //TODO: Add more event in feature
}

class GameServer(implicit val system: ActorSystem, implicit val materializer: Materializer) {
    import GameData._
    import GameEvent._
    val GameMasterHandleActor: ActorRef = system.actorOf(Props[GameMasterActor], "GameMasterHandleActor")
    val gameMasterProfileSource: Source[GameMasterEvent, ActorRef] = Source.actorRef[GameMasterEvent](5,OverflowStrategy.fail)

    def gameInMatchFlow(profile: GameProfile): Flow[Message, Message, Any] =
        Flow.fromGraph(GraphDSL.create(gameMasterProfileSource) { implicit builder => gameInMatchProfileShape =>
            import GraphDSL.Implicits._
            val materialization = builder.materializedValue.map(profileActorRef => JoinMatch(profile))
            val merge = builder.add(Merge[GameMasterEvent](2))
            val gameInMatchProfileSink = Sink.actorRef[GameMasterEvent](GameMasterHandleActor, LeftMatch(profile))

            //This will tell request to actor, and actor update and push back an event
            val MessageToGameInMatchEventConverter = builder.add(Flow[Message].map {
                case TextMessage.Strict(score) =>
                    println("Have update score request from " + profile.toString)
                    UpdateScore(profile, score)
                case TextMessage.Strict(level) =>
                    println("Have update level request from " + profile.toString)
                    UpdateLevel(profile, level)
                //TODO: Priority 3: Implement game in match request
            })

            //This handle back event from actor, and send text message to client
            val GameInMatchEventBackToMessageConverter = builder.add(Flow[GameMasterEvent].map{
                case UpdateScore(profile, newScore) =>
                    TextMessage("player01,50 | player02,45")
                case UpdateLevel(profile, newLevel) =>
                    TextMessage("player01, 4 | player02, 3")
            })
            materialization ~> merge ~> gameInMatchProfileSink
            MessageToGameInMatchEventConverter ~> merge 
            gameInMatchProfileShape ~> GameInMatchEventBackToMessageConverter
            FlowShape(MessageToGameInMatchEventConverter.in, GameInMatchEventBackToMessageConverter.out)
        })

     val GameMasterRoute: Route = {
        get {
            handleWebSocketMessages(gameInMatchFlow())
        }
    }

    val FinalRoute: Route = GameMasterRoute
}