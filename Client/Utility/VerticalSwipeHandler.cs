﻿using Microsoft.AspNetCore.Components.Web;
using System;

namespace Couple.Client.Utility
{
    public class VerticalSwipeHandler
    {
        private const double SwipeThreshold = 0.8;
        private (TouchPoint ReferencePoint, DateTime StartTime) _startPoint;
        private readonly Action _swipeUp;
        private readonly Action _swipeDown;

        public VerticalSwipeHandler(Action swipeUp, Action swipeDown)
        {
            _swipeUp = swipeUp;
            _swipeDown = swipeDown;
        }

        public void HandleTouchStart(TouchEventArgs t)
        {
            _startPoint.ReferencePoint = t.TargetTouches[0];
            _startPoint.StartTime = DateTime.Now;
        }

        public void HandleTouchEnd(TouchEventArgs t)
        {
            var endReferencePoint = t.ChangedTouches[0];

            var diffY = _startPoint.ReferencePoint.ClientY - endReferencePoint.ClientY;
            var diffTime = DateTime.Now - _startPoint.StartTime;
            var velocityY = Math.Abs(diffY / diffTime.Milliseconds);

            if (velocityY < SwipeThreshold)
            {
                return;
            }

            if (diffY < 0)
            {
                _swipeDown.Invoke();
            }
            else
            {
                _swipeUp.Invoke();
            }
        }
    }
}