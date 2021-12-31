using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Positions
    {
        public Vector3 Top { get; set; }
        public Vector3 Bottom { get; set; }
        public Vector3 Left { get; set; }
        public Vector3 Right { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

}
}
