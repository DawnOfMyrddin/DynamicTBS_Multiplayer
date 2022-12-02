using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : MonoBehaviour
{
    public static GameObject FindGameObjectByPosition(List<GameObject> destinations, Vector3 position)
    {
        return FindGameObjectByRay(destinations, DefaultRay(position));
    }

    public static Ray DefaultRay(Vector3 position)
    {
        position.z = 0;
        return new Ray(position, Vector3.forward);
    }

    public static GameObject FindGameObjectByRay(List<GameObject> destinations, Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (hits != null && hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                GameObject gameObject = hit.transform.gameObject;
                if (destinations.Contains(gameObject))
                    return gameObject;
            }
        }

        return null;
    }
}