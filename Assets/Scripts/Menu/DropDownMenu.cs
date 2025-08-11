using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    public RectTransform menuRect;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Toggle the visibility of the dropdown menu
            menuRect.gameObject.SetActive(true);
            Vector2 mousePosition = Input.mousePosition;
            menuRect.position = new Vector2(mousePosition.x, mousePosition.y);
        }

        if (Input.GetMouseButtonDown(0) && menuRect.gameObject.activeSelf)
        {
            bool clickedInside = RectTransformUtility.RectangleContainsScreenPoint(menuRect, Input.mousePosition, null);

            if (!clickedInside)
            {
                menuRect.gameObject.SetActive(false);
            }
        }


    }


}
