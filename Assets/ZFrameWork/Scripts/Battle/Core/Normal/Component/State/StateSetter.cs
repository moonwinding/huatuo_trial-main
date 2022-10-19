using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Creat/NewState")]
public class StateSetter : ScriptableObject
{
    public CampType CampType;
    //public string Model;
    //public string HpBar;
    public float modelHeight;
    public AssetReference modelAssetReference;
    public AssetReference hpBarAssetReference;

    public static StateSetter LoadDataFromFile(string Path){
        var textAsset = AssetManager.Instance.LoadAsset<StateSetter>(Path);
        return textAsset;
    }
    public StateInfo GetStateInfo(Vector3 pBornPos)
    {
        var stateInfo = new StateInfo(pBornPos, CampType) { Model = this.modelAssetReference.AssetGUID, 
            modelHeight  = this.modelHeight};
        if (hpBarAssetReference != null){
            stateInfo.HpBar = hpBarAssetReference.AssetGUID;
        }
        else {
            stateInfo.HpBar = null;
        }
        return stateInfo;
    }
}
