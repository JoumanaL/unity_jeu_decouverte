
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Variables/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int CurrentLifePoint = 0;
    public int maxLifePoint = 3;
}