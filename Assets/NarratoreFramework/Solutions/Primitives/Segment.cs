using UnityEngine;

namespace Narratore.Primitives
{
    public struct Segment
    {
        public Segment(Vector2 point1, Vector2 point2, float allowableError = float.Epsilon)
        {
            _point1 = point1;
            _point2 = point2;
            _line = new Line2D(_point1, _point2, allowableError);
        }


        public Vector2 Point1
        {
            get { return _point1; }
            set
            {
                _point1 = value;
                _line.ToRebuild(_point1, _point2);
            }
        }
        public Vector2 Point2
        {
            get { return _point2; }
            set
            {
                _point2 = value;
                _line.ToRebuild(_point1, _point2);
            }
        }
        public float AllowableError => _line.AllowableError;
        public bool Degenerate => Length < AllowableError;
        public float DeltaX => _point2.x - _point1.x;
        public float DeltaY => _point2.y - _point1.y;
        public float Length => (Point2 - Point1).magnitude;


        private readonly Line2D _line;
        private Vector2 _point1;
        private Vector2 _point2;

       
        public void Set(Vector2 point1, Vector2 point2)
        {
            _point1 = point1;
            _point2 = point2;

            _line.ToRebuild(_point1, _point2);
        }
        /*public RelativePos GetRelativePos(Vector2 point)
        {
            Vector2 vectorSegment = Point2 - Point1;
            Vector2 vectorToPoint = point - Point1;
            float pseudoDot = vectorSegment.PseudoDot(vectorToPoint);

            if (Mathf.Abs(pseudoDot) <= _allowableError)
                return RelativePos.On;
            else if (pseudoDot > 0)
                return RelativePos.Left;
            else
                return RelativePos.Right;
        }*/
        public bool Intersect(Segment segment)
        {
            Vector2 intersect;
            Line2D otherLine = segment._line;

            if (Degenerate || segment.Degenerate)
                return false;

            if (_line.IsParallelLine(otherLine))
                return false;

            intersect = _line.GetIntersect(otherLine);

            if (LiesOnMe(intersect) && segment.LiesOnMe(intersect))
                return true;

            return false;
        }
        public bool Intersect(Segment section, out Vector2 intersect)
        {
            Line2D otherLine = section._line;

            intersect = new Vector2();
            if (Degenerate || section.Degenerate)
                return false;

            if (_line.IsParallelLine(otherLine))
                return false;

            intersect = _line.GetIntersect(otherLine);

            if (LiesOnMe(intersect) && section.LiesOnMe(intersect))
                return true;

            return false;
        }
        public bool LiesOnMe(Vector2 point)
        {
            bool xBetween = false;
            bool yBetween = false;

            if (_line.LiesOnMe(point))
            {
                Vector2 pointNormalizedBySegment = new Vector2()
                {
                    x = (point.x - _point1.x) / DeltaX,
                    y = (point.y - _point1.y) / DeltaY
                };
                
                if (IsPointOnMeByOneAxis(pointNormalizedBySegment.x, DeltaX))
                    xBetween = true;

                if (IsPointOnMeByOneAxis(pointNormalizedBySegment.y, DeltaY))
                    yBetween = true;
            }

            return (xBetween || yBetween);
        }
        public Vector2 GetIntersectWithPerpendicularFromPoint(Vector2 point, out bool existsPoint)
        {
            Vector2 intersect = _line.GetIntersectWithPerpendicularFrom(point);
            existsPoint = LiesOnMe(intersect);

            return intersect;
        }


        private bool IsPointOnMeByOneAxis(float normalizedPointByAxis, float sizeSegmentByAxis)
        {
            if (float.IsNaN(sizeSegmentByAxis) || Mathf.Abs(sizeSegmentByAxis) < AllowableError)
                return false;

            float bottomLimit = AllowableError / 2;
            float topLimit = 1 - AllowableError / 2;

            return normalizedPointByAxis > bottomLimit &&
                   normalizedPointByAxis < topLimit;
        }
    }
}
    
