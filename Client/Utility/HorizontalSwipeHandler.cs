using Microsoft.AspNetCore.Components.Web;

namespace Couple.Client.Utility;

public class HorizontalSwipeHandler
{
    private const double SwipeThreshold = 0.8;
    private readonly Action _swipeLeft;
    private readonly Action _swipeRight;
    private (TouchPoint ReferencePoint, DateTime StartTime) _startPoint;

    public HorizontalSwipeHandler(Action swipeLeft, Action swipeRight)
    {
        _swipeLeft = swipeLeft;
        _swipeRight = swipeRight;
    }

    public void HandleTouchStart(TouchEventArgs t)
    {
        _startPoint.ReferencePoint = t.TargetTouches[0];
        _startPoint.StartTime = DateTime.Now;
    }

    public void HandleTouchEnd(TouchEventArgs t)
    {
        var endReferencePoint = t.ChangedTouches[0];

        var diffX = _startPoint.ReferencePoint.ClientX - endReferencePoint.ClientX;
        var diffTime = DateTime.Now - _startPoint.StartTime;
        var velocityX = Math.Abs(diffX / diffTime.Milliseconds);

        if (velocityX < SwipeThreshold)
        {
            return;
        }

        if (diffX < 0)
        {
            _swipeRight.Invoke();
        }
        else
        {
            _swipeLeft.Invoke();
        }
    }
}
