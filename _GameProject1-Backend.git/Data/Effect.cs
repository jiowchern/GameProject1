using System;

namespace Regulus.Project.GameProject1.Data
{
    
    
    public struct Effect
    {
    
        public EFFECT_TYPE Type;
    
        public float Value;
    }

    public enum EFFECT_TYPE
    {
        ATTACK_ADD, 
        ILLUMINATE_ADD, 
        LIFE, 
        BLOCK, 
        SMASH, 
        PUNCH,
        SHIFT_DIRECTION, 
        SHIFT_SPEED, 
        MOVE_FORWARD,
        MOVE_BACKWARD,
        MOVE_TURNLEFT,
        MOVE_TURNRIGHT,
        MOVE_RUNFORWARD,
        DISARM,
        AID,
        SKILL_AXE1,
        SKILL_SWORD1,
        SKILL_PIKE1,
        SKILL_CLAYMORE1,
        SKILL_MELEE1,

        SKILL_DUALSWORD1,

        SKILL_SWORDSHIELD1,
    }
}