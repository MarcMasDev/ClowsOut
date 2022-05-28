using UnityEngine;
using static ShootSystemManager;

public class BulletUI : MonoBehaviour
{
    [HideInInspector]
    public Sprite[] m_BulletTypesSprites = new Sprite[7];
    [SerializeField]
    private string[] m_BulletTypesNames = new string[7];

    public string BulletTypeToName(int bulletType)
    {
        switch (bulletType)
        {
            case (int)BulletType.NORMAL:
                return m_BulletTypesNames[0];
            case (int)BulletType.ATTRACTOR:
                return m_BulletTypesNames[1];
            case (int)BulletType.TELEPORT:
                return m_BulletTypesNames[2];
            case (int)BulletType.MARK:
                return m_BulletTypesNames[3];
            case (int)BulletType.STICKY:
                return m_BulletTypesNames[4];
            case (int)BulletType.ICE:
                return m_BulletTypesNames[5];
            case (int)BulletType.ENERGY:
                return m_BulletTypesNames[6];
            default:
                return null;
        }
    }
    public Sprite BulletTypeToSprite(int bulletType)
    {
        switch (bulletType)
        {
            case (int)BulletType.NORMAL:
                return m_BulletTypesSprites[0];
            case (int)BulletType.ATTRACTOR:
                return m_BulletTypesSprites[1];
            case (int)BulletType.TELEPORT:
                return m_BulletTypesSprites[2];
            case (int)BulletType.MARK:
                return m_BulletTypesSprites[3];
            case (int)BulletType.STICKY:
                return m_BulletTypesSprites[4];
            case (int)BulletType.ICE:
                return m_BulletTypesSprites[5];
            case (int)BulletType.ENERGY:
                return m_BulletTypesSprites[6];
            default:
                return null;
        }
    }
}
