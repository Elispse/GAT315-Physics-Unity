using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UVScroll : MonoBehaviour
{
    // The direction is in degrees and ranges from 0 to 360
    [SerializeField, Range(0, 360)] float direction;
    [SerializeField, Range(0, 2)] float speed;

    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // Calculate the UV offset direction based on the given angle
        // The angle is converted from degrees to radians for trigonometric functions
        Vector2 uv = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad));

        // Update the texture offset of the material to create the scrolling effect
        // The offset is calculated by multiplying the UV direction by time and speed
        meshRenderer.material.mainTextureOffset = Time.time * uv * speed;
    }
}