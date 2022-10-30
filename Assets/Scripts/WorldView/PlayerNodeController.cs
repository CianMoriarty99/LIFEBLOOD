using UnityEngine;

public class PlayerNodeController : MonoBehaviour
{
    public Transform moveToward;
    public bool moving;
    public float searchRange, distanceToPlayer;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                distanceToPlayer = Vector3.Distance(hit.transform.position, transform.position);
                if (distanceToPlayer < searchRange)
                {
                    moving = true;
                    gm.InitialiseEnemyDeck(hit.transform.gameObject.GetComponent<NodeProperties>().difficulty);
                    moveToward = hit.transform;
                }

            }
        }

        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveToward.position, 0.01f);
        }

        if (moveToward && transform.position == moveToward.position)
        {
            moving = false;
            moveToward = null;
            gm.ChangeScene(Scene.InGame);
        }
    }
}
