//  val gameLobbyActorHandler: ActorRef = system.actorOf(Props[GameLobbyActor], "GameLobbyHandleActor")
//     val gameLobbyProfileSource: Source[GameEvent, ActorRef] = Source.actorRef[GameEvent](50,OverflowStrategy.fail)
//     def gamePrepareFlow(userName: String): Flow[Message, Message, Any] = 
//        Flow.fromGraph(GraphDSL.create(gameLobbyProfileSource) { implicit builder => profileShape =>
//             import GraphDSL.Implicits._
//             val materialization = builder.materializedValue.map(profileActorRef => EnterLooby(userName,profileActorRef))
//             val merge = builder.add(Merge[GameEvent](2))
//             val gameLobbyProfileSink = Sink.actorRef[GameEvent](gameLobbyActorHandler, ExitLooby(userName))

//             //This will tell request to actor, and actor update and push back an event
//             val MessageToGameLobbyEventConverter = builder.add(Flow[Message].map {
//                 case TextMessage.Strict(lobbyRequest) =>
//                     println("Have new lobby request")
//                     LobbyUpdate()
//                 case TextMessage.Strict(roomID) =>
//                     println(s"Creating $roomID room")
//                     CreateRoom(roomID)
//                 case TextMessage.Strict("start") => 
//                     StartGameFromLobby()
//             })

//             //This handle back event from actor, and send text message to client
//             val GameLobbyEventBackToMessageConverter = builder.add(Flow[GameEvent].map{
//                 case LobbyChanged(userNames) =>
//                     TextMessage("Lobby Update player01,50 | player02,45")
//                 case EnterGameMaster(players) =>
//                     TextMessage("Start game player01,50 | player02,45")

//             })

//             materialization ~> merge ~> gameLobbyProfileSink
//             MessageToGameLobbyEventConverter ~> merge 
//             profileShape ~> GameLobbyEventBackToMessageConverter
//             FlowShape(MessageToGameLobbyEventConverter.in, GameLobbyEventBackToMessageConverter.out)
//         })