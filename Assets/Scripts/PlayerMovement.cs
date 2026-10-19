using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;
    
    private int currentLane = 1;
    private Vector2 touchStartPos;
    private readonly float swipeThreshold = 50f;

    void Update()
    {
        HandleInput();

        Vector3 targetPosition = new((currentLane - 1) * laneDistance, transform.position.y, 0f);
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) touchStartPos = touch.position;
            else if (touch.phase == TouchPhase.Ended)
            {
                float xDiff = touch.position.x - touchStartPos.x;
                float yDiff = touch.position.y - touchStartPos.y;

                if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff) && Mathf.Abs(xDiff) > swipeThreshold)
                {
                    if (xDiff > 0) currentLane = Mathf.Min(2, currentLane + 1);
                    else currentLane = Mathf.Max(0, currentLane - 1);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) currentLane = Mathf.Min(2, currentLane + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) currentLane = Mathf.Max(0, currentLane - 1);
    }
}