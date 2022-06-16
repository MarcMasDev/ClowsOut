using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private bool m_IsBullet;
    [SerializeField]private float yOffset = 0.5f;
    [SerializeField]private float ySpeed= 3f;
    [SerializeField]private float scaleSpeed= 2f;
    [SerializeField] private string m_text;
    [SerializeField] private Sprite m_bulletSprite;
    [SerializeField] LayerMask m_CollisionLayerMask;
    private float i = 0;
    [SerializeField] float m_MaxDistToFloor = 0.5f;
    [SerializeField] float m_currentDist;
    public bool checkFloor = true;
    private void Awake()
    {
        transform.position -= new Vector3(0, yOffset/3, 0);
        transform.localScale = Vector3.zero;
    }
    private void Update()
    {
        if (i < yOffset)
        {
            if (checkFloor)
            {
                if (CheckDistFloor() < m_MaxDistToFloor)
                {
                    transform.position += new Vector3(0, Time.deltaTime * ySpeed, 0);
                    i += Time.deltaTime;
                    transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
                }
            }
            else
            {
                transform.position += new Vector3(0, Time.deltaTime * ySpeed, 0);
                i += Time.deltaTime;
                transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
            }

        }

    }
    public float CheckDistFloor()
    {
        RaycastHit l_hits = new RaycastHit();

        Physics.Raycast(transform.position, Vector3.down, out l_hits, 20f, m_CollisionLayerMask);
        if (l_hits.collider != null)
        {
            print("dist " + l_hits.distance);
            m_currentDist = l_hits.distance;
            return l_hits.distance;
        }
        else
        {
            print("distnul");
            m_currentDist = 0;
            return 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (!m_IsBullet)
        {
            if (other.CompareTag("Player"))
            {
                m_IsBullet = true;
                GameManager.GetManager().SetCurrentRoomIndex(GameManager.GetManager().GetCurrentRoomIndex() + 1);
              //  GameManager.GetManager().GetCanvasManager().ShowInfoBullet(m_text, m_bulletSprite);
                Destroy(gameObject);
            }
        }
    }
}
