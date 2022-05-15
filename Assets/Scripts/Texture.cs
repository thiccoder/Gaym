using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Texture : IEquatable<Texture>
{
    public abstract bool Equals(Texture other);
    public bool Active;
    public Vector2Int Resolution;
}
