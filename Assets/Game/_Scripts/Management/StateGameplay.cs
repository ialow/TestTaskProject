using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum StateGameplay
    {
        Init = 0,
        MainLoop = 1,
        Pause = 2,
        Victory = 3,
        Defeat = 4,
    
        ProjectDescription = 999,
        Restart = 1000,
    }
}
