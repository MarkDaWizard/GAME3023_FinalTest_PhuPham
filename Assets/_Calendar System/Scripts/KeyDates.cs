using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManager;

[CreateAssetMenu(menuName = "Time Date System/Key Dates")]
public class KeyDates : ScriptableObject
{
    public DateTime KeyDate;
    public Sprite thumbnail;
    public string Desc;

    public bool hasParticleFX;
    [Range(0,100)]
    public float particleFXChance;
    public int particleSystemID;


    public bool hasSoundFX;
    [Range(0, 100)]
    public float soundFXChance;
    public int audioSourceID;

}
