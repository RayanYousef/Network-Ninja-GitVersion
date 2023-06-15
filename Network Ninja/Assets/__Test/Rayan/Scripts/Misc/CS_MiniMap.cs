using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_MiniMap : MonoBehaviour
{
    // World player is used to get the position of the player in the world
    [Header("Player Position & Rotation In the World")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform playerCamera;
    [SerializeField] RectTransform playerIconInMiniMap;

    // Mini map guide is used to get the size of the mini map in the world in order to
    // map it on the canvas mini map E.A. to get the ratio and map it to mini map
    [Header("Mini Map Guides In World/Canvas")]
    [SerializeField] SpriteRenderer miniMapGuideInWorld;
    [SerializeField] SpriteRenderer miniMapGuideInCanvas;

    [Header("Pos Ratio to the Total Length of X and Y")]
    [SerializeField]float ratioInX;
    [SerializeField] float ratioInZ;

    [Header("Mini Map World Variables")]
    // The variables below are the boundaries of the spirite in the world.
    [SerializeField] float minXW;
    [SerializeField] float maxXW, minZW,maxZW;
    [SerializeField] float xTotalLengthW,zTotalLengthW;

    [Header("Mini Map Canvas Variables")]
    // The variables below are the boundaries of the spirite in the world.
    [SerializeField] float minXC;
    [SerializeField] float maxXC, minYC, maxYC;
    [SerializeField] float xTotalLengthC, yTotalLengthC;


    void Start()
    {
        WorldVariables();
        CanvasVariables();
    }

    private void FixedUpdate()
    {
        PlayerPosInMiniMap();
        PlayerRotInMiniMap();
    }

    private void WorldVariables()
    {
        minXW = miniMapGuideInWorld.bounds.center.x - miniMapGuideInWorld.bounds.extents.x;
        maxXW = miniMapGuideInWorld.bounds.center.x + miniMapGuideInWorld.bounds.extents.x;

        minZW = miniMapGuideInWorld.bounds.center.z - miniMapGuideInWorld.bounds.extents.z;
        maxZW = miniMapGuideInWorld.bounds.center.z + miniMapGuideInWorld.bounds.extents.z;

        xTotalLengthW = miniMapGuideInWorld.bounds.size.x;
        zTotalLengthW = miniMapGuideInWorld.bounds.size.z;
    }
    private void CanvasVariables()
    {
        minXC = miniMapGuideInCanvas.bounds.center.x - miniMapGuideInCanvas.bounds.extents.x;
        maxXC = miniMapGuideInCanvas.bounds.center.x + miniMapGuideInCanvas.bounds.extents.x;

        minYC = miniMapGuideInCanvas.bounds.center.y - miniMapGuideInCanvas.bounds.extents.y;
        maxYC = miniMapGuideInCanvas.bounds.center.y + miniMapGuideInCanvas.bounds.extents.y;

        xTotalLengthC = miniMapGuideInCanvas.bounds.size.x;
        yTotalLengthC = miniMapGuideInCanvas.bounds.size.y;
    }
    private void PlayerPosInMiniMap()
    {
         ratioInX = Mathf.Abs((playerTransform.position.x- minXW)/ xTotalLengthW);
         ratioInZ = Mathf.Abs((playerTransform.position.z - maxZW) / zTotalLengthW);

        float xPos = minXC + (ratioInX * xTotalLengthC);
        float YPos = maxYC - (ratioInZ * yTotalLengthC);
        playerIconInMiniMap.position = new Vector3(xPos,YPos);
    }

    private void PlayerRotInMiniMap()
    {
        float RotationAroundY = playerCamera.eulerAngles.y;
        playerIconInMiniMap.rotation = Quaternion.Euler(0,0,-RotationAroundY);
    }
}
