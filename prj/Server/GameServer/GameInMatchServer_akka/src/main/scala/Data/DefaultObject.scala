package Data

import Data.GameData._
import Data.GameEvent._

case object DefaultObject {
  val templateProfileForGameMaster: GameMasterProfile = GameMasterProfile(profileID, inGameData)
  val templateProfileForGameMasterWithInfo: GameMasterProfileWithInformation = GameMasterProfileWithInformation(profileID, info, inGameData)

  val profileID = UserID("demoprofile01")
  val userName: String = "hackerno1"
  val password: String = "123123"

  val info: Information = Information(profileID, additionalInfo)
  val additionalInfo = AdditionalInfo(userName, password, "Nguyen Van An", 22,"0765263722", "quynhdemo@gmail.com")


  val inGameData: InGameData = InGameData(profileID, playerStats)
    val playerStats: PlayerStats = PlayerStats(1000, 600, skills, weapons)
      val skills: List[Skill] = List(Skill("kamehameha",100,450), Skill("kaioken",500,850), Skill("ahihi",10,200))
      val weapons: List[Weapon] = List(Weapon("shortGun",2000,"09072021"), Weapon("machineGun",8000, "none"), Weapon("SnipGun", 12000, "18022021"))
}
