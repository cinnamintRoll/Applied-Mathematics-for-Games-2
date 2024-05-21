using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Cube;
    public float speed;

    void Start()
    {

    }

    void Update()
    {
        Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, Cube.transform.position, speed);
    }
}
