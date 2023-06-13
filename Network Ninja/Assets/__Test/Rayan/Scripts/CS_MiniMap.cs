using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CS_MiniMap : MonoBehaviour
{
    [SerializeField] Transform worldMap,canvasMap;
    [SerializeField] Transform ObjectToMove;
    [SerializeField] SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer= GetComponent<SpriteRenderer>();
        if (ObjectToMove != null )  
        MoveObjecttoSpriteCorner();

     
    }

    public void MoveObjecttoSpriteCorner()
    {
        float newXPos= _spriteRenderer.bounds.center.x+ _spriteRenderer.bounds.extents.x;
        float newZPos = _spriteRenderer.bounds.center.z + _spriteRenderer.bounds.extents.z;

        ObjectToMove.position = new Vector3(newXPos, _spriteRenderer.bounds.center.y , newZPos);
    }
}
