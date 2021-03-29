package Data
import akka.actor.ActorRef
object GameData {
    trait GameMasterData
    case class UserID(id: String)
    
    //GameMaster Data
    case class GameMasterProfileWithActor(profile: GameMasterProfile, actor: ActorRef)
    case class GameMasterProfile(profileID: UserID, gameData: InGameData) extends GameMasterData 
    case class GameMasterProfileWithInformation(profileID: UserID, information: Information, gameData: InGameData) extends GameMasterData
    
        case class Information(profileID: UserID, additionalInfo: AdditionalInfo) extends GameMasterData
            case class AdditionalInfo(userName: String,  password: String, realName: String, age: Int, phoneNumber: String, email: String) extends GameMasterData

        case class InGameData(profileID: UserID, stats: PlayerStats) extends GameMasterData
            case class PlayerStats(HP: Int, MP: Int, skills: List[Skill], weapons: List[Weapon]) extends GameMasterData 
            case class Skill(skillName: String, manaCost: Int, damage: Int) extends GameMasterData 
            case class Weapon(name: String, goldNeedToBuy: Int, expireData: String) extends GameMasterData  
}