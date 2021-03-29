package Actors


import akka.actor.{Actor, ActorLogging}

class GameMasterActor extends Actor with ActorLogging{
 override def receive: Receive = {
  case _ => log.info("Enter Game master actor")
 }
}