// package Relearn

// val profile: PlayerProfile = DatabaseHandle.PlayerLogin("username", "password")
//   val playerInLobby: Player = DatabaseHandle.PlayerJoinedLobby(profile)
//   val playerInMatch: Player = DatabaseHandle.PlayerJoinedMatch(playerInLobby)
//   val playerActorSource: Source[TGameEvent, ActorRef] = Source.actorRef[TGameEvent](5,OverflowStrategy.fail)

//   val gameBaseActor: ActorRef = system.actorOf(Props(new GameBaseActor()))
//   val lobbyActor: ActorRef = system.actorOf(Props(new LobbyAreaActor()))
//   val matchActor: ActorRef = system.actorOf(Props(new GameAreaActor()))

//   def handleGameFlow(playerData: PlayerProfile): Flow[Message, Message, Any] = Flow.fromGraph(GraphDSL.create(playerActorSource) { implicit builder => playerActorShape =>
//     import GraphDSL.Implicits._
//     val materialization = builder.materializedValue.map(playerActorRef => PlayerEnterGame(playerData, playerActorRef))
//     val merge = builder.add(Merge[TPlayerEvent](2))

//     val inputMessageConvertToGameEvents = builder.add(Flow[Message].map{
//       case TextMessage.Strict(request) =>
//         println("Receive new message in handleGameFlow: " + request)
//         PlayerUpdateStates(playerData, request)
//       case _ =>
//         println("Receive nothing in handleGameFlow ")
//         PlayerUpdateStates(playerData, "nothing")

//     })

//     val gameEventsConvertToOutputMessage = builder.add(Flow[TPlayerEvent].map {
//      ???
//     })
//     val gameBaseActorSink =  Sink.actorRef[TPlayerEvent](gameBaseActor,PlayerExitGame(playerData))

//     materialization ~> merge ~> gameBaseActorSink
//     inputMessageConvertToGameEvents ~> merge
//     playerActorShape ~> gameEventsConvertToOutputMessage
//     FlowShape(inputMessageConvertToGameEvents.in, gameEventsConvertToOutputMessage.out)
//   })

//   def handleLobbyFlow(inLobbyPlayer: Player): Flow[Message, Message, Any] = Flow.fromGraph(GraphDSL.create(playerActorSource) { implicit builder => playerActorShape =>
//     import GraphDSL.Implicits._
//     val materialization = builder.materializedValue.map(playerActorRef => PlayerJoinedLobby(inLobbyPlayer, playerActorRef))
//     val merge = builder.add(Merge[TGameEvent](2))

//     val inputMessageConvertToGameEvents = builder.add(Flow[Message].map{
//       case TextMessage.Strict(request) =>
//         println("Receive new message in handleLobbyFlow: " + request)
//         PlayerUpdate(inLobbyPlayer, request)
//       case _ =>
//         println("Receive nothing in handleLobbyFlow ")
//         PlayerUpdate(inLobbyPlayer, "request")

//     })

//     val gameEventsConvertToOutputMessage = builder.add(Flow[TPlayerEvent].map {
//       ???
//     })
//     val lobbyActorSink =  Sink.actorRef[TGameEvent](lobbyActor,PlayerLeftLobby(inLobbyPlayer))

//     materialization ~> merge ~> lobbyActorSink
//     inputMessageConvertToGameEvents ~> merge
//     playerActorShape ~> gameEventsConvertToOutputMessage
//     FlowShape(inputMessageConvertToGameEvents.in, gameEventsConvertToOutputMessage.out)
//   })

//   def handleMatchFlow(inLobbyPlayer: Player): Flow[Message, Message, Any] = Flow.fromGraph(GraphDSL.create(playerActorSource) { implicit builder => playerActorShape =>
//     import GraphDSL.Implicits._
//     val materialization = builder.materializedValue.map(playerActorRef => PlayerJoinedMatch(inLobbyPlayer, playerActorRef))
//     val merge = builder.add(Merge[TGameEvent](2))

//     val inputMessageConvertToGameEvents = builder.add(Flow[Message].map{
//       case TextMessage.Strict(request) =>
//         println("Receive new message in handleMatchFlow: " + request)
//         PlayerUpdate(inLobbyPlayer, request)

//       case _ =>
//         println("Receive nothing in handleMatchFlow ")
//         PlayerUpdate(inLobbyPlayer, "request")
//     })

//     val gameEventsConvertToOutputMessage = builder.add(Flow[TGameEvent].map {
//       case PlayerUpdatesProfile(players) =>
//         TextMessage("Hello")
//     })
//     val matchActorSink =  Sink.actorRef[TGameEvent](matchActor,PlayerLeftMatch(inLobbyPlayer))

//     materialization ~> merge ~> matchActorSink
//     inputMessageConvertToGameEvents ~> merge
//     playerActorShape ~> gameEventsConvertToOutputMessage
//     FlowShape(inputMessageConvertToGameEvents.in, gameEventsConvertToOutputMessage.out)
//   })

//   val GameRoute: Route = {
//     get {
//       handleWebSocketMessages(handleGameFlow(profile))
//     }
//   }

//   val LobbyRoute: Route = {
//     get {
//       handleWebSocketMessages(handleLobbyFlow(playerInLobby))
//     }
//   }

//   val MatchRoute: Route = {
//     get {
//       handleWebSocketMessages(handleMatchFlow(playerInMatch))
//     }
//   }

//   val finalRoute = GameRoute ~ LobbyRoute ~ MatchRoute

//   package DataAndAction

// import akka.actor.ActorRef

// case object GameAction {

//   import GameData._

//   //In Lobby
//   case class PlayerUpdate(player: Player, newState: String) extends TGameEvent
//   case class LobbyUpdate(players: Iterable[Player])
//   case class PlayerChangeTeam(player: Player) extends TGameEvent
//   case class PlayerJoinedLobby(player: Player, actorRef: ActorRef) extends TGameEvent
//   case class PlayerLeftLobby(player: Player) extends TGameEvent
//   case class RootPlayerStartMatch(players: Iterable[Player]) extends TGameEvent
//   case object PlayerCancelMatch extends TGameEvent

//   //In Match
//   case class MatchUpdate(players: Iterable[Player]) extends TGameEvent
//   case class PlayerJoinedMatch(player: Player, actorRef: ActorRef) extends TGameEvent
//   case class PlayerLeftMatch(player: Player) extends TGameEvent
//   case class PlayerMovement(player: Player, direction: String) extends TGameEvent
//   case class PlayerAttack(player: Player, enemyName: String) extends TGameEvent

// }

// package DataAndAction

// import akka.actor.ActorRef

// case object GameData {

//   case class Player(username: String, stats: PlayerInformation, isInFirstTeam: Boolean) extends TGameData
//   case class PlayerInformation(HP: Int, MP: Int, position: Position,currentGold: Int, weapon: Weapon) extends TGameData
//   case class Weapon(weaponName: String, numberOfBullets: Int, damage: Int, goldNeedToBuy: Int) extends TGameData

//   case class Position(x_Pos: Int, y_Pos: Int, z_Pos: Int) extends TGameData {
//     def + (other: Position): Position = {
//       Position(x_Pos + other.x_Pos, y_Pos + other.y_Pos, z_Pos + other.z_Pos)
//     }
//   }
//   case class PlayerWithActor(player: Player, actorRef: ActorRef)
// }

// package Actors

// import DataAndAction.GameAction.{MatchUpdate, PlayerJoinedMatch, PlayerLeftMatch, PlayerMovement}
// import DataAndAction.GameData.{Player, PlayerInformation, PlayerWithActor, Position}
// import akka.actor.Actor

// class GameAreaActor extends Actor{

//   val allPlayersInMatch = collection.mutable.LinkedHashMap[String, PlayerWithActor]()

//   def MatchUpdateRequest(): Unit = {
//     allPlayersInMatch.values.foreach(_.actorRef ! MatchUpdate(allPlayersInMatch.values.map(_.player)))
//   }

//   def PlayerMovementNotification(): Unit = {
//     ???
//   }

//   override def receive: Receive = {
//     case PlayerJoinedMatch(player, actorRef) =>
//       println("Player join a match")
//       val newPlayer = Player(player.username, player.stats, player.isInFirstTeam)
//       allPlayersInMatch += (newPlayer.username -> PlayerWithActor(newPlayer, actorRef))
//       MatchUpdateRequest()

//     case PlayerLeftMatch(player) =>
//       allPlayersInMatch -= player.username //TODO: Need change code if player reconnect
//       MatchUpdateRequest()

//     case PlayerMovement(player, moveAction) =>
//       val offset = moveAction match { //TODO: Need config position with Unity3d client
//         case "forward" => Position(0,1,0)
//         case "back" => Position(0,-1,0)
//         case "left" => Position(1,0,0)
//         case "right" => Position(-1,0,0)
//         case "jump" => Position(0,0,1)

//       }
//       val oldPlayerWithActor = allPlayersInMatch(player.username)
//       val oldPlayer = oldPlayerWithActor.player
//       val oldInfo = oldPlayer.stats

//       val newPlayerInfo = PlayerInformation(oldInfo.HP, oldInfo.MP, oldInfo.position + offset, oldInfo.currentGold, oldInfo.weapon)
//       val actor = oldPlayerWithActor.actorRef
//       allPlayersInMatch(player.username) = PlayerWithActor(Player(player.username,newPlayerInfo, player.isInFirstTeam), actor)
//       MatchUpdateRequest()

//   }
// }