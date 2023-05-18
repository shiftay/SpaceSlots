using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableBackground : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    const float MIN = 962, MAX = -962;

    void Awake()
    {
    
    }


    private Vector2 initialClick;

    public void OnPointerDown(PointerEventData eventData) {
        initialClick = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

        // Direction + Distance
        Vector2 heading = initialClick - eventData.position;

        float distance = heading.magnitude;
        Vector2 direction = heading / distance;


        Debug.Log ("distance: " + distance  + " | delta: " + eventData.delta + " | scroll delta" + eventData.scrollDelta);

        direction.y = 0;

        

        transform.localPosition += (Vector3)eventData.delta;
        transform.localPosition = ScalePosition(transform.localPosition);
        // transform.localPosition = ScalePosition(eventData.position);
        // throw new System.NotImplementedException();
    }

    public Vector3 ScalePosition(Vector3 scale) {
        Vector3 retVal = scale;

        retVal.y = 0;

        if(retVal.x > MIN) retVal.x = MIN;
        if(retVal.x < MAX) retVal.x = MAX;

        return retVal;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
