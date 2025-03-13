using UnityEngine;

public class SelectionTower : MonoBehaviour
{
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(TouchRay, out RaycastHit hit))
        {
            transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                enabled = false;
                GetComponent<Tower>().enabled = true;
            }
        }
    }
    
}
