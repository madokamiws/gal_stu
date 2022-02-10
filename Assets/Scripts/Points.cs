using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    private HitpointGame hitpointGame;
    // Start is called before the first frame update
    void Start()
    {
        hitpointGame = transform.parent.GetComponent<HitpointGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void HitPoint()
    {
        hitpointGame.hitnum++;
        gameObject.SetActive(false);
    }
}
