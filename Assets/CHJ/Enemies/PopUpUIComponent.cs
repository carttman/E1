using UnityEngine;

public class PopUpUIComponent : MonoBehaviour
{
    private Camera MyCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, 0.01f);
        //transform.position = Vector3.Lerp(transform.position, transform.parent.position, 1f);
        transform.position += Vector3.up * (0.01f);
        
        Vector3 pos = MyCamera.transform.position - transform.position;
        //Vector3 pos = MyCamera.transform.position - MyCamera.WorldToScreenPoint(transform.position);
        Vector3 newVec = new Vector3(pos.x, 0, pos.z);
        //transform.rotation = Quaternion.LookRotation(-newVec);
    }
}
