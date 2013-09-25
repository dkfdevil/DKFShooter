/// <summary>
/// This script isnt attached to any gameobject but it is used
/// by the PlayerDatabase script in building the playerlist
/// </summary>

public class PlayerDataClass {
	
	//Variables Start
	
	public int networkPlayer;
	public string playerName;
	public int playerScore;
	public string playerTeam;
	
	//Variables End
	
	public PlayerDataClass Constructor ()
	{
		PlayerDataClass capture = new PlayerDataClass();
		
		capture.networkPlayer = networkPlayer;
		capture.playerName = playerName;
		capture.playerScore = playerScore;
		capture.playerTeam = playerTeam;
		
		return capture;
	}
}