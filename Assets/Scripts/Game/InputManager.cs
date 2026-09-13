// InputManager.cs - Handles both touch and keyboard input

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float SWIPE_THRESHOLD = 50f;
    private Vector2 touchStartPos;
    private bool touchStarted = false;

    public bool GetMoveLeftInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return true;

        return CheckSwipeLeft();
    }

    public bool GetMoveRightInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return true;

        return CheckSwipeRight();
    }

    public bool GetJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;

        return CheckSwipeUp();
    }

    public bool GetSlideInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            return true;

        return CheckSwipeDown();
    }

    public bool GetPauseInput()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private bool CheckSwipeLeft()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchStarted = true;
            }
            else if (touch.phase == TouchPhase.Ended && touchStarted)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;
                if (swipeDelta.x < -SWIPE_THRESHOLD && Mathf.Abs(swipeDelta.y) < SWIPE_THRESHOLD)
                {
                    touchStarted = false;
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckSwipeRight()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchStarted = true;
            }
            else if (touch.phase == TouchPhase.Ended && touchStarted)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;
                if (swipeDelta.x > SWIPE_THRESHOLD && Mathf.Abs(swipeDelta.y) < SWIPE_THRESHOLD)
                {
                    touchStarted = false;
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckSwipeUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchStarted = true;
            }
            else if (touch.phase == TouchPhase.Ended && touchStarted)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;
                if (swipeDelta.y > SWIPE_THRESHOLD && Mathf.Abs(swipeDelta.x) < SWIPE_THRESHOLD)
                {
                    touchStarted = false;
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckSwipeDown()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchStarted = true;
            }
            else if (touch.phase == TouchPhase.Ended && touchStarted)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;
                if (swipeDelta.y < -SWIPE_THRESHOLD && Mathf.Abs(swipeDelta.x) < SWIPE_THRESHOLD)
                {
                    touchStarted = false;
                    return true;
                }
            }
        }
        return false;
    }
}
