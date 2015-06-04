using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

// Target Data
// timeAlive
// hitPrecision
// wasHit
// Velocity
// Contrast
// Scale
// Color/Colour (r,g,b)
// targetData ID
// target Manager ID
/// <summary>
/// The Schema for storing data, may be possible to abstract it later.
/// </summary>,
public class ObjectData
{

    /// <summary>
    /// The Primary key of the Target
    /// </summary>
    [PrimaryKey]
    public string objectID { get; set; }
    /// <summary>
    /// The time the target spent alive.
    /// </summary>
    [NotNull]
    public float timeAlive { get; set; }
    /// <summary>
    /// The FK for the manager of this Target
    /// </summary>
    [NotNull]
    public string managerID { get; set; }
    /// <summary>
    /// The distance to from the touch point to the exact center of the object.
    /// </summary>
    public float hitPrecision { get; set; }
    /// <summary>
    /// True if the object was tapped, false if the object timmed out.
    /// </summary>
    public bool wasHit { get; set; }
    /// <summary>
    /// The actual speed the target was moving, in Game units / second.
    /// </summary>
    [NotNull]
    public float velocity { get; set; }
    /// <summary>
    /// The alpha value of the sprite, rangeing between 0 and 1 ( 1 being completely opaque ).
    /// </summary>
    public float opacity { get; set; }
    /// <summary>
    /// The size of the object in terms of the x/y scale attribute
    /// </summary>
    public float scale { get; set; }
    /// <summary>
    /// the value of red color, rangeing between 0 and 1 ( 1 being completely saturated ).
    /// </summary>
    public float red { get; set; }
    /// <summary>
    /// the value of green color, rangeing between 0 and 1 ( 1 being completely saturated ).
    /// </summary>
    public float green { get; set; }
    /// <summary>
    /// the value of blue color, rangeing between 0 and 1 ( 1 being completely saturated ).
    /// </summary>
    public float blue { get; set; }


    
    const string targetTable = "Target";

    /// <summary>
    /// Generate an SQL insert statement for the Class
    /// This is not needed for inserting into the local database
    /// </summary>
    /// <param name="ManID">The FK id for the Target Manager class</param>
    /// <returns>An SQL statement in the form of a string</returns>
    public string generateInsert()
    {
        // Generate first segment
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "(objectID, managerID, timeAlive, hitPrecision, wasHit, velocity, opacity, red, green, blue, scale) ";
        insert += "Values";
        insert += string.Format("( '{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});",
            objectID, managerID, timeAlive, hitPrecision, wasHit, velocity
            , opacity, red, green, blue, scale);

        return insert;
    }

}
// Target Manager Data
// Total Targets
// Total Hits
// Total Misses
// Near Misses
// Target Manager ID
// GameSession ID

public class ManagerData
{
    [PrimaryKey]
    public string managerID { get; set; }
    [NotNull]
    public string mechanicID { get; set; }
    [NotNull]
    public int totalTargets { get; set; }
    [NotNull]
    public int hits { get; set; }
    [NotNull]
    public int misses { get; set; }
    public int unsuccessfulHits { get; set; }
    public int nearMisses { get; set; }

    const string targetTable = "Manager";
    IEnumerable Targets {get; set;}


    /// <summary>
    /// Generate an SQL insert statement for the Class
    /// This is not needed for inserting into the local database
    /// </summary>
    /// <param name="ManID">The FK id for the Game Manager class</param>
    /// <returns>An SQL statement in the form of a string</returns>
    public string generateInsert()
    {
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "(managerID, mechanicID, totalTargets, hits, unsuccessfulHits, misses, nearMisses) ";
        insert += "Values";
        insert += string.Format("( '{0}', '{1}', {2}, {3}, {4}, {5}, {6});",
            managerID, mechanicID, totalTargets, hits, unsuccessfulHits, misses, nearMisses);

        return insert;
    }
}

public class MechanicData
{
    // stores some basic information as well as the difficulty 
    //settings used at the time of the level
    [PrimaryKey]
    public string mechanicID { get; set; }
    [NotNull]
    public string gameInstanceID { get; set; }

    public int maxOnScreen { get; set; }

    public float targetScale { get; set; }

    public float targetOpacity { get; set; }

    public float minTargetSpeed { get; set; }
    public float maxTargetSpeed { get; set; }

    public float targetTimeout { get; set; }

    public float targetSpawnInterval { get; set; }

    public float backgroundOpacity { get; set; }
    public float backgroundSpeed { get; set; }

    public float secondaryOpacity { get; set; }
    public float secondaryScale { get; set; }

    public int targetsToWin { get; set; }
    public string mechanicType { get; set; }
    
    const string targetTable = "Mechanic";
    
    IEnumerable Managers;
    
    /// <summary>
    /// Generate an SQL insert statement for the Class
    /// This is not needed for inserting into the local database
    /// </summary>
    /// <param name="ManID">The FK id for the Game Manager class</param>
    /// <returns>An SQL statement in the form of a string</returns>
    public string generateInsert()
    {
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "(mechanicID, gameInstanceID, maxOnScreen, targetScale, targetOpacity, minTargetSpeed";
        insert += ", maxTargetSpeed, targetTimeout, targetSpawnInterval,";
        insert += "backgroundOpacity, backgroundSpeed, targetToWin, mechanicType) ";
        insert += " Values";
        insert += string.Format("('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, '{12}');",
            mechanicID, gameInstanceID, maxOnScreen, targetScale, targetOpacity, minTargetSpeed,
                maxTargetSpeed, targetTimeout, targetSpawnInterval, backgroundOpacity, backgroundSpeed, targetsToWin, mechanicType);
        return insert;
    }
}

public class MovingTargetsSettingsData
{

    [PrimaryKey]
    public int levelID { get; set; }

    public int maxOnScreen { get; set; }

    public float targetScale { get; set; }

    public float targetOpacity { get; set; }

    public float minTargetSpeed { get; set; }
    public float maxTargetSpeed { get; set; }

    public float targetTimeout { get; set; }

    public float targetSpawnInterval { get; set; }

    public float backgroundOpacity { get; set; }
    public float backgroundSpeed { get; set; }

    public int targetsToWin { get; set; }

    const string targetTable = "MovingTargetsSettings";

    public string generateInsert()
    {
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "(levelID, maxOnScreen, targetScale, targetOpacity, minTargetSpeed, maxTargetSpeed, targetTimeout, ";
        insert += "targetSpawnInterval, backgrtoundOpacity, backgroundSpeed, targetsToWin )";
        insert += "Values";
        insert += string.Format("( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10} );",
            levelID, maxOnScreen, targetScale, targetOpacity, minTargetSpeed,
                maxTargetSpeed, targetTimeout, targetSpawnInterval, backgroundOpacity, 
                backgroundSpeed, targetsToWin);
        return insert;
    }
}

public class GameInstance
{
    // Stores the user and the time of the game
    [PrimaryKey]
    public string gameInstanceID { get; set; }
    public string userID { get; set; }
    public string gameType { get; set; }

    public System.DateTime completionDate { get; set; }
    public System.DateTime startDate { get; set; }

    
    const string targetTable = "GameInstance";
    // when queried, contains all mechanics from the game, like movingtargetsman

    public string generateInsert()
    {
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "\n(gameInstanceID, userID, completionDate, startDate, gameType )";
        insert += " \nVALUES";
        insert += string.Format("\n( '{0}', '{1}', '{2}', '{3}', '{4}');",
            gameInstanceID, userID, completionDate.ToString("yyyy-MM-dd HH:mm:ss"), startDate.ToString("yyyy-MM-dd HH:mm:ss"), gameType);
        return insert;
    }
    
}

public class User
{
    // stores the user, their slot, and their current level(s)
    [PrimaryKey]
    public string userID { get; set; }
    [NotNull]
    public int currentLevel { get; set; }

    public int slotNumber { get; set; }

    public System.DateTime creationDate { get; set; }

    const string targetTable = "User";

    public string generateInsert()
    {
        var insert = "INSERT into ";
        insert += targetTable;
        insert += "(vuserID, currentLevel, slotNumber, creationDate )";
        insert += "Values";
        insert += string.Format("( '{0}', {1}, {2}, '{3}');",
            userID, currentLevel, slotNumber, creationDate);
        return insert;
    }
}

public class Sync
{
    [PrimaryKey, AutoIncrement]
    public int syncID { get; set; }
    [NotNull]
    public System.DateTime syncDate { get; set; }

}