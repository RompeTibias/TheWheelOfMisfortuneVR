using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class RarityWeights
{
    [SerializeField] private float common;
    [SerializeField] private float rare;
    [SerializeField] private float epic;
    [SerializeField] private float legendary;

    public float GetRarityWeights(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return common;
            case Rarity.Rare:
                return rare;
            case Rarity.Epic:
                return epic;
            case Rarity.Legendary:
                return legendary;
            default:
                return 0;
        }
    }
    
    #if UNITY_EDITOR
    public void Set( float comun, float rara, float epica, float legendaria)
    {
        common = comun;
        rare = rara;
        epic = epica;
        legendary = legendaria;
    }
    #endif
}
[CreateAssetMenu(fileName = "RarityTable", menuName = "ScriptableObjects/RarityTable")]
public class RarityTable : ScriptableObject
{
    [SerializeField] private RarityWeights weightsRoundOne;
    [SerializeField] private RarityWeights weightsRoundMax;
    [Min(2)]
    [SerializeField] private int roundMax;
    [Min(0.1f)]
    [SerializeField] private float curve;
    
    RarityWeights rarityWeights;

    [ContextMenu("Probar pesos")]
    public void ProbarPesos()
    {
        for (int i = 1; i <= 6; i++)
        {
            Debug.Log(GetWeightForRound(Rarity.Common, i));
            Debug.Log(GetWeightForRound(Rarity.Rare, i));
            Debug.Log(GetWeightForRound(Rarity.Epic, i));
            Debug.Log(GetWeightForRound(Rarity.Legendary, i));
        }
    }
    
    public float GetWeightForRound(Rarity rarity, int round)
    {
        float roundOne;
        float roundMaxime;
        float realRound;
       
        float t = Mathf.Clamp01((float)(round - 1) / (roundMax - 1));
        t = Mathf.Pow(t, curve);
        
        roundOne =  weightsRoundOne.GetRarityWeights(rarity);
        
        roundMaxime =  weightsRoundMax.GetRarityWeights(rarity);
        
       float interpolate =  Mathf.Lerp(roundOne,roundMaxime,t);
       
       return interpolate;
    }

    public void ApplyImport(
        float common1, float rare1, float epic1, float legendary1,
        float commonMax, float rareMax, float epicMax, float legendaryMax,
        int newRoundMax, float newCurve
        )
    {
        weightsRoundOne.Set(common1,rare1,epic1,legendary1);
        weightsRoundMax.Set(commonMax,rareMax,epicMax,legendaryMax);
        roundMax = newRoundMax;
        curve = newCurve;
    }
}

