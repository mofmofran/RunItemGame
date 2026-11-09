using UnityEngine;

public enum UpgradeType
{
    LifeUp,//ライフ増加
    PierceBullet,//貫通弾
    BulletUp,//弾数増加
}

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;//アップグレード名
    [TextArea(2, 5)]
    public string description;//説明文
    public UpgradeType upgradeType;//種類
    public int value;//効果値
    
}
