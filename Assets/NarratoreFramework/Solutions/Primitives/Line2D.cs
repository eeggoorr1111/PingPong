using UnityEngine;
using Narratore.Helpers;

namespace Narratore.Primitives
{
    public struct Line2D
    {
        public Line2D(Vector2 point1, Vector2 point2, float allowableError = float.Epsilon)
        {
            AllowableError = allowableError;

            A = B = C = 0f;
            ToRebuild(point1, point2);
        }
        public Line2D(float a, float b, float c, float allowableError = float.Epsilon)
        {
            AllowableError = allowableError;

            A = a;
            B = b;
            C = c;
        }


        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }
        public float AllowableError { get; }
        public bool ParallelAxisX => A == 0;
        public bool ParallelAxisY => B == 0;


        public void ToRebuild(Vector2 point1, Vector2 point2)
        {
            A = point1.y - point2.y;
            B = point2.x - point1.x;
            C = point1.x * point2.y - point2.x * point1.y;

            if (A.Is0(AllowableError) && B.Is0(AllowableError))
                throw new System.Exception($"Коэфициенты прямой A и B не могут оба быть равны 0. Погрешность {AllowableError}");
        }
        /// <summary>
        /// !!!!! Если прямая парралельна оси абцисс или ординат, то будут выдаваться некорректные значения.
        /// Самый частый кейс, когда надо для дебага построить прямую. Поэтому, если оказывается, что прямая 
        /// парралельна оси Y используем метод <see cref="GetXByY"/>
        /// </summary>
        public float GetYByX(float x)
        {
            if (B.Is0(AllowableError))
                return 0;
            
            return (-A * x - C) / B;
        }
        /// <summary>
        /// !!!!! Если прямая парралельна оси абцисс или ординат, то будут выдаваться некорректные значения.
        /// Самый частый кейс, когда надо для дебага построить прямую. Поэтому, если оказывается, что прямая 
        /// парралельна оси X используем метод <see cref="GetYByX"/>
        /// </summary>
        public float GetXByY(float y)
        {
            if (A.Is0(AllowableError))
                return 0;

            return (-B * y - C) / A;
        }
        public Line2D GetPerpendicularFrom(Vector2 point)
        {
            return new Line2D(B, -A, -(B * point.x - A * point.y));
        }
        public Vector2 GetIntersect(Line2D line)
        {
            Vector2 intersect;

            if (IsParallelLine(line))
                return Vector2.zero;

            intersect.x = -(C * line.B - line.C * B) / (A * line.B - line.A * B);
            intersect.y = -(A * line.C - line.A * C) / (A * line.B - line.A * B);

            return intersect;
        }
        public Vector2 GetIntersectWithPerpendicularFrom(Vector2 point)
        {
            Line2D perpendicularLine = GetPerpendicularFrom(point);
            return GetIntersect(perpendicularLine);
        }
        public bool LiesOnMe(Vector2 point)
        {
            float rightPartFact = A * point.x + B * point.y + C;
            return (Mathf.Abs(rightPartFact) < AllowableError);
        }
        public bool IsParallelLine(Line2D line)
        {
            float res = A * line.B - line.A * B;
            if (res.Is0())
                return true;

            return false;
        }
        public float TransformC(Vector2 point)
        {
            float x;
            float y;
            float newC;

            if (ParallelAxisY)
            {
                y = 0;
                x = -C / A;
            }
            else
            {
                x = 0;
                y = -C / B;
            }

            x = x - point.x;
            y = y - point.y;
            newC = -(A * x + B * y);

            return newC;
        }
        public override string ToString()
        {
            return A + " " + B + " " + C;
        }
        public bool PointBetween(Vector2 check, Vector2 point1, Vector2 point2)
        {
            bool xBetween = false;
            bool yBetween = false;

            if (!LiesOnMe(check))
            {
                Debug.LogError("Проверяемая точка не лежит на данной прямой");
                return false;
            }

            if (!LiesOnMe(point1))
            {
                Debug.LogError("Точка 1(" + point1 + ") не лежит на данной прямой");
                return false;
            }

            if (!LiesOnMe(point2))
            {
                Debug.LogError("Точка 2(" + point2 + ") не лежит на данной прямой");
                return false;
            }

            if (check.x >= point1.x && check.x <= point2.x ||
                check.x >= point2.x && check.x <= point1.x)
                xBetween = true;

            if (check.y >= point1.y && check.y <= point2.y ||
                check.y >= point2.y && check.y <= point1.y)
                yBetween = true;

            return (xBetween && yBetween);
        }
    }
}
