using UnityEngine;

public class scene : MonoBehaviour
{
    public Transform cube;
    public Transform destination;
    public float distance = 1.0f;

    [Range(0, 1)] public float lerpSpeed;

    public float moveValue;
    public bool moveTarget = false;

    private void FixedUpdate()
    {
        if (moveTarget)
        {
            cube.position = Vector3.Lerp(cube.position, destination.position, Time.deltaTime * lerpSpeed);
        }
    }

    public void moveCube()
    {
        moveTarget = true;
    }

}
