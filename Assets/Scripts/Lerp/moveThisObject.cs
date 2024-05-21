using UnityEngine;

public class moveThisObject : MonoBehaviour
{
    public Vector3 newPosition;

    public void moveObject()
    {
        transform.position = newPosition;
    }
}
