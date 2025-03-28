using UnityEngine;
public class InstantMove : MonoBehaviour
{
    public Vector3[] newPosition;
    public GameObject Light;
    public void Teleport0()
    {
        Light.transform.localPosition = newPosition[0];
    }
    public void Teleport1()
    {
        Light.transform.localPosition = newPosition[1];
    }
    public void Teleport2()
    {
        Light.transform.localPosition = newPosition[2];
    }
}
