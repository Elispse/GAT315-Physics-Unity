using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenImageTarget : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Camera view;
    [SerializeField] float distance = 5;

    private void LateUpdate()
    {
        Vector3 screen = image.transform.position;
        screen.z = distance;

        Vector3 world = view.ScreenToWorldPoint(screen);
        transform.position = world;
    }
}