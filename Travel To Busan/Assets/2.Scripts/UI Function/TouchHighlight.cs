using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchHighlight : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    
    private Image img;
    private Color basic;
    private PointerEventData eventData;

    private void Awake()
    {
        eventData = new PointerEventData(EventSystem.current);
        img = GetComponent<Image>();
        basic = new Color(img.color.r, img.color.g, img.color.b);
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            List<RaycastResult> set = new List<RaycastResult>();
            Touch touch = Input.GetTouch(i);
            eventData.position = touch.position;
            raycaster.Raycast(eventData, set);
            if (set.Count == 0)
                continue;

            GameObject obj = set[0].gameObject; 

            if (obj.Equals(this.gameObject))
            {
                img.color = basic * 0.8f;
                return;
            }
        }
        img.color = basic;
    }
}
