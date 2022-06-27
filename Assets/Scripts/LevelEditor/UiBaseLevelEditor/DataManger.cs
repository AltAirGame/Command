using UnityEngine;
using UnityEngine.Serialization;


namespace MHamidi
{
//notification Channel    
public class DataManger:MonoBehaviour
{


        
    private const string playerDataName = "Player";
    private const string gameDataName = "Game";
    
    public static DataManger current;

    public PlayerData playerdata;
    public GameData gameData;

    private void OnEnable()
    {
        LoadPlayerData();
        var gameDataLoadResponse = Repository.current.LoadGameData(gameDataName);
        if (string.IsNullOrWhiteSpace(gameDataLoadResponse.error.message))
        {
            
        }
        else
        {
            //Handel Error 
            //Show Modal
        }
    }

    private void LoadPlayerData()
    {
        var playerDatLoadResponse = Repository.current.LoadPlayerData(playerDataName);
        if (string.IsNullOrWhiteSpace(playerDatLoadResponse.error.message))
        {
            playerdata = playerDatLoadResponse.data;
        }
        else
        {
            //handel error 
            //Show Modal
        }
    }

    private void OnDisable()
    {
        //Save Data
    }
}
}
