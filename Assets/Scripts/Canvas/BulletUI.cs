using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootSystem;

public class BulletUI : MonoBehaviour
{
    [SerializeField] public string[] m_BulletTypesNames = new string[7];

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
}
