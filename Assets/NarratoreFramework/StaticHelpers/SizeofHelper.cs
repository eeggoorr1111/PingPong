using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narratore.Helpers
{
    public static class SizeofHelper
    {
        /// <summary>
        /// ���� ��������, ��� ��� ���������� �� ������ ��������� ����� ����, ��������� � ������
        /// �������� �����. � ��� ������� ����� ������������. ���� ������������ �� ���� ������ � ������
        /// ��������� ������� ����� ���������. ��� ������� ����� �� ����������. ��� �� �� �� ����, ��
        /// ��������� ��� ������������� � ��������������� ������ ���� ������ ��� ������ ����������
        /// ��� �������� ����.
        /// </summary>
        private static readonly Dictionary<Type, int> _sizeof = new Dictionary<Type, int>()
        {
            { typeof(bool),     1},
            { typeof(byte),     1},
            { typeof(short),    2},
            { typeof(ushort),   2},
            { typeof(int),      4},
            { typeof(uint),     4},
            { typeof(long),     8},
            { typeof(ulong),    8},

            { typeof(float),    4},
            { typeof(double),   8},
            { typeof(decimal),  16},

            { typeof(char),     2},

            { typeof(Vector2),      8},
            { typeof(Vector2Int),   8},
            { typeof(Vector3),      12},
            { typeof(Vector3Int),   12},
            { typeof(Vector4),      16}
        };

        public static int Sizeof<T>(this T item) where T : struct
        {
            return Sizeof(typeof(T));
        }
        public static int Sizeof<T>(this IReadOnlyCollection<T> collection, bool includeLengthAsInt = false) where T : struct
        {
            if (collection.IsEmpty())
                return 0;

            int sizeofLength = includeLengthAsInt ? sizeof(int) : 0;

            return Sizeof(typeof(T)) * collection.Count + sizeofLength;
        }


        private static int Sizeof(this Type type)
        {
            if (_sizeof.ContainsKey(type))
                return _sizeof[type];

            throw new Exception($"������� �������� ������ ���� {type.Name}, ��� �������� �� ����� ������");
        }
    }
}
