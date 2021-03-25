import Service.GameServer
import akka.actor.ActorSystem
import akka.http.scaladsl.client.RequestBuilding.WithTransformation
import akka.http.scaladsl.model.ws.{BinaryMessage, TextMessage}
import akka.http.scaladsl.testkit.{ScalatestRouteTest, WSProbe}
import akka.http.scaladsl.testkit.WSTestRequestBuilding.WS
import akka.stream.Materializer
import akka.stream.scaladsl.{Flow, Keep, Sink, Source}
import akka.stream.testkit.scaladsl.{TestSink, TestSource}
import akka.testkit.{TestKit, TestProbe}
import org.scalatest.BeforeAndAfterAll
import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpecLike

import scala.concurrent.Await
import scala.concurrent.duration._
import scala.util.{Failure, Success}
class ServerTestSpec extends AnyWordSpecLike
    with Matchers
    with ScalatestRouteTest
    with BeforeAndAfterAll {

    override def afterAll(): Unit = {
      TestKit.shutdownActorSystem(system)
    }

//    def assertWebSocket(playerHashKey: String) (assertions: WSProbe => Unit) : Unit = {
//      val gameServer = new GameServer()
//      val wsClient = WSProbe()
//      WS(s"/?hashKey=$playerHashKey", wsClient.flow) ~> gameServer.websocketRoute ~>
//        check(assertions(wsClient))
//    }
//
//
//    "Game server" should {
//
//      "Create empty game server first" in {
//        new GameServer()
//      }
//
//      "Connect to game server first" in {
//       assertWebSocket("john") { wsClient =>
//         isWebSocketUpgrade shouldEqual true }
//      }
//
//      "Response with same send message" in {
//        assertWebSocket("john") { wsClient =>
//            wsClient.sendMessage("hello")
//            wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//          }
//      }
//
//      "Register for player" in {
//        assertWebSocket("john") { wsClient =>
//            wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//          }
//      }
//
//      "Response with name player correctly" in {
//        assertWebSocket("john") { wsClient =>
//            wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//        }
//      }
//
//      "Register for multiplayer" in {
//        val gameServer = new GameServer()
//        val johnClient = WSProbe()
//        val andrewClient = WSProbe()
//        WS("/?playerName=john", johnClient.flow) ~> gameServer.websocketRoute ~>
//          check {
//            johnClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//          }
//        WS("/?playerName=andrew", andrewClient.flow) ~> gameServer.websocketRoute ~>
//          check {
//            andrewClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}},{\"name\":\"andrew\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//          }
//      }
//
//      "Register player and move it up" in {
//        assertWebSocket("john") { wsClient =>
//         wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//
//        }
//      }
//      "Response correct player location" in {
//          assertWebSocket("john") { wsClient =>
//            wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//            wsClient.sendMessage("up")
//            wsClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":1,\"z_Pos\":0}}]")
//          }
//        }
//      "Response correct multi player location" in {
//        val gameServer = new GameServer()
//        val johnClient = WSProbe()
//        val andrewClient = WSProbe()
//        WS("/?playerName=john", johnClient.flow) ~> gameServer.websocketRoute ~>
//          check {
//            johnClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//            johnClient.sendMessage("up")
//            johnClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":1,\"z_Pos\":0}}]")
//          }
//        WS("/?playerName=andrew", andrewClient.flow) ~> gameServer.websocketRoute ~>
//          check {
//            andrewClient.expectMessage("[{\"name\":\"john\",\"position\":{\"x_Pos\":0,\"y_Pos\":1,\"z_Pos\":0}},{\"name\":\"andrew\",\"position\":{\"x_Pos\":0,\"y_Pos\":0,\"z_Pos\":0}}]")
//          }
//      }
//
//
//
//    }


}


