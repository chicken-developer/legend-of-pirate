package Service

import Data.GameData._
import Data.GameEvent._
import Data.DefaultObject._

case object GameData {
    val playersInGameInMatch = collection.mutable.LinkedHashMap[String, GameMasterProfileWithActor]()
}


case object Database {

    def CheckConnection(): Boolean = {
        //TODO: Implement CheckDatabaseConnection()
        true
    }
    def PlayerFindMatch(playerID: UserID): GameMasterProfileWithInformation = {
        def GetFullProfileFromDatabaseByID(id: UserID): Option[GameMasterProfileWithInformation] = {
            None
        }

        GetFullProfileFromDatabaseByID(playerID) match {
            case Some(gameMasterFullProfile) => gameMasterFullProfile
            case _ => templateProfileForGameMasterWithInfo
        }
    }

    def PlayerJoinedMatch(playerID: UserID): GameMasterProfile = {
        def GetProfileFromDatabaseByID(id: UserID): Option[GameMasterProfile] = {
            None
        }

        GetProfileFromDatabaseByID(playerID) match {
            case Some(gameMasterProfile) => gameMasterProfile
            case _ => templateProfileForGameMaster
        }
    }

    def ReconnectToGameInMatch(playerID: UserID): Option[GameMasterProfile] = {
        None
    }

}
