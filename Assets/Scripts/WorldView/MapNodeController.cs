using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeController : MonoBehaviour
{
    public GameObject informationIcon;
    SpriteRenderer sprR;
    public GameObject playerNode;
    public float searchRange, distanceToPlayer;
    public bool travelPlayer;
    public GameManager gm;
    NodeProperties np;
    public TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        sprR = GetComponent<SpriteRenderer>();
        np = GetComponent<NodeProperties>();
        tmp.text = "Difficulty: " + np.difficulty.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(playerNode.transform.position, transform.position);
        if (distanceToPlayer > searchRange)
        {
            sprR.color = Color.gray;
        } else
        {
            sprR.color = Color.white;
        }
    }

    private void OnMouseEnter()
    {
        informationIcon.SetActive(true);
    }

    private void OnMouseExit()
    {
        informationIcon.SetActive(false);
    }

}
