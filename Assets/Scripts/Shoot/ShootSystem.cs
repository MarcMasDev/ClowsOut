using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    //public enum BulletType { ATTRACTOR = 0, LINKED, EXPLOSIVE, KNOCKBACK, FIRE, TELEPORT }
    //public BulletType m_CurrBulletType;

    public Bullet m_PrefabBullet;
    public float m_Speed=2;


    public LayerMask m_ColisionWithEffect, m_ColisionLayerMask;
    /// <summary>
    /// Create a bullet giving a direction and speed
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="bulletType"></param>
    public void BulletShoot(Vector3 direction, float speed)//, BulletType bulletType)
    {
        //Bullet l_Bullet = new Bullet(direction, speed);  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            BulletShoot(new Vector3(0, 0, 1), m_Speed);//, ShootSystem.BulletType.ATTRACTOR);
        }
    }


    public void OnCollisionWithOutEffect()
    { }

    public void OnCollisionWithEffect()
    { }

}
