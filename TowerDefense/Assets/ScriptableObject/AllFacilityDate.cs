using UnityEngine;

[CreateAssetMenu(fileName = "AllFacilityDate", menuName = "ScriptableObject/All Facility Date")]
public class AllFacilityDate : ScriptableObject
{
    [Tooltip("Prefab, �R�X�g, �P�����ɖ߂��Ă���R�X�g, ���x���A�b�v�R�X�g")]
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
