using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColorSet : MonoBehaviour
{
    private Image im;
    private Player_BulletManager pBullet;
    [Tooltip("[0-Attractor, 1-Teleport, 2-Mark, 3-Sticky, 4-Ice, 5-Energy] order reference.")]
    [SerializeField] private Color[] bulletColors;
    private string currentBulletName = "";
    private string[] bullets = new string[] { "ATTRACTOR", "TELEPORT", "MARK", "STICKY", "ICE", "ENERGY" };
    void Start()
    {
        im = GetComponent<Image>();
        pBullet = GameManager.GetManager().GetPlayer().GetComponent<Player_BulletManager>();
    }
    void Update()
    {
        if (!currentBulletName.Equals(pBullet.m_CurrentBullet.ToString()) && Time.timeScale != 0)
        {
            currentBulletName = pBullet.m_CurrentBullet.ToString();
            ChangeBullet();
        }
    }
    private void ChangeBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (currentBulletName.Equals(bullets[i]))
            {
                im.color = bulletColors[i];
                return;
            }
        }
    }
}
