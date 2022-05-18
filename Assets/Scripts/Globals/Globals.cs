using System;
using UnityEngine;

namespace Globals
{
    [Flags]
    public enum OrderTargetType : byte 
    {
        None = 0,
        Object = 1,
        Location = 2
    }
    public enum VisionType : byte 
    {
        None = 0,
        Revealed,
        Visible
    }    
}
