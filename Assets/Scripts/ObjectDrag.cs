using UnityEngine;

public class ObjectDrag : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 scanPos;


    private float sensitivity;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;


    void Start()
    {
        sensitivity = 0.4f;
        rotation = Vector3.zero;
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            screenPoint = Camera.main.WorldToScreenPoint(scanPos);
            GetComponent<MoleculeIdenti>().ChangeToSlectedColor();
            offset = scanPos;
            isRotating = false;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            GetComponent<MoleculeIdenti>().ChangeToSlectedColor();
            isRotating = true;
            mouseReference = Input.mousePosition;
        }
    }


    private void Update()
    {
        RotateObject();
    }


    private void RotateObject()
    {
        if (isRotating)
        {
            mouseOffset = (Input.mousePosition - mouseReference);

            rotation.y = -(mouseOffset.x + mouseOffset.y) * sensitivity;

            transform.Rotate(rotation);

            mouseReference = Input.mousePosition;
        }

        if(isRotating && Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
    }


    public void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
