package Service

import Actors.GameMasterActor
import akka.actor.{ActorRef, ActorSystem, Props}
import akka.http.scaladsl.model.ws.{Message, TextMessage}
import akka.http.scaladsl.server.Route
import akka.stream.{FlowShape, Materializer, OverflowStrategy}
import akka.stream.scaladsl.{Flow, GraphDSL, Merge, Sink, Source}
import akka.http.scaladsl.server.Directives._
import akka.http.scaladsl.Http
import akka.http.scaladsl.model.{ContentTypes, HttpEntity, StatusCodes}
import akka.stream.ActorMaterializer
import Core.Game._ 
import Actors._
class GameServer(implicit val system: ActorSystem, implicit val materializer: Materializer) {

    val gameMasterHandleActor: ActorRef = system.actorOf(Props[GameMasterActor], "GameMasterHandleActor")
    val gameMasterProfileSource: Source[GameEvent, ActorRef] = Source.actorRef[GameEvent](50,OverflowStrategy.fail)

    def gameInMatchFlow(profile: PlayerData): Flow[Message, Message, Any] =
        Flow.fromGraph(GraphDSL.create(gameMasterProfileSource) { implicit builder => profileShape =>
            import GraphDSL.Implicits._
            val materialization = builder.materializedValue.map(profileActorRef => JoinMatch(profile.userName, profileActorRef))
            val merge = builder.add(Merge[GameEvent](2))
            val gameInMatchProfileSink = Sink.actorRef[GameEvent](gameMasterHandleActor, LeftMatch(profile.userName))

            //This will tell request to actor, and actor update and push back an event
            val MessageToGameInMatchEventConverter = builder.add(Flow[Message].map {
                case TextMessage.Strict(newScore) =>
                    println("Have update score request from " + profile.toString)
                    UpdateScore(profile, newScore)
                case TextMessage.Strict(newLevel) =>
                    println("Have update level request from " + profile.toString)
                    UpdateScore(profile, newLevel)
                //TODO: Priority 3: Implement game in match request
            })
            //This handle back event from actor, and send text message to client
            val GameInMatchEventBackToMessageConverter = builder.add(Flow[GameEvent].map{
                case GameMasterChanged(profile) =>
                    TextMessage("Update score player01, 4 | player02, 3")
                
            })
            materialization ~> merge ~> gameInMatchProfileSink
            MessageToGameInMatchEventConverter ~> merge 
            profileShape ~> GameInMatchEventBackToMessageConverter
            FlowShape(MessageToGameInMatchEventConverter.in, GameInMatchEventBackToMessageConverter.out)
        })

    val content =
        """
        |<html>
        | <head></head>
        | <body>
        |   This is an HTML page served by Akka HTTP!
        | </body>
        |</html>
        """

    val GameFinalRoute = 
    //  (get & parameter("playerName")){ playerName =>
    //     handleWebSocketMessages(gameInMatchFlow(PlayerData(playerName, None)))
    //  } ~   
     get {
        complete(
            HttpEntity(
            ContentTypes.`text/html(UTF-8)`,
            content
            )
        )
    }
     
}
