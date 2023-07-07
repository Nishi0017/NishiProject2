using UnityEngine;

[CreateAssetMenu(fileName = "AllFacilityDate", menuName = "ScriptableObject/All Facility Date")]
public class AllFacilityDate : ScriptableObject
{
    [Tooltip("Prefab, コスト, 撤去時に戻ってくるコスト, レベルアップコスト")]
    public FacilityDates[] facilityDates;

    [System.Serializable]
    public class FacilityDates
    {
        public GameObject facilityPrefab;
        public int putCost;
        public int deleteCost;
        public int levelUpCost;
    }
}
