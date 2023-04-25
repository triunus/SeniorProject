using System.Collections;
using System.Collections.Generic;
using System;

namespace MVP.InGameSkills.EnumData
{
    // 2 4 1 2
    enum TypeData
    {
        Projectile,
        AreaEffect
    }
    enum AttributeData
    {
        Black = 1,
        Ice = 2,
        Ice_Black = 3,
        Fire = 4,
        Fire_Black = 5,
        Fire_Ice = 6,
        Fire_Ice_Black = 7,
        Wind = 8,
        Wind_Black = 9,
        Wind_Ice = 10,
        Wind_Ice_Black = 11,
        Wind_Fire = 12,
        Wind_Fire_Black = 13,
        Wind_Fire_Ice = 14,
        Wind_Fire_Ice_Black = 15
    }

    enum StartPositionData
    {
        Player = 1,
        NearPlayer = 2,
        Enemy = 3,
        MultipleEnemies = 4,
        Random = 5,
    }
    enum DestinationPositionData
    {
        Non = 0,            // 목표 지점이 생성 지점임 ( 안움직임 )
        Player = 1,         // 
        NearPlayer = 2,
        Enemy = 3,
        MultipleEnemies = 4,
        Random = 5          // 랜덤
    }
    enum MovementTypeData
    {
        // 2의 제곱승을 제외하고는 실행되지 않는다.
        // 서버에서 합성된 스킬이 int 형식으로 넘어올때, 이를 bit OR 연산하여 사용되는 연산의 종류를 인지하기 위해서 이다.
        // 즉, 3은 straight와 circle 2개의 운동이 수행된다.
        Non = 0,
        Straight = 1,       // -> 0
        Circle = 2,         // -> 1
        Circle_Straight = 3,// -> 아마도 회오리로 돌 거임.
        Wave = 4,            // -> 2
        Wave_Straight = 5,
        Wave_Circle = 6,
        Wave_Circle_Straight = 7
    }
    enum StartPositionReposition
    {
        Non = 0,
        PlayerSatelite = 1
    }
    enum DestinationPosistionReposition
    {
        Non = 0,
        EnemyGuided = 1
    }

    enum AttackEffectData
    {
        Non = 0,
        Penetrate = 1,
        Persistent = 2,
        Persistent_Penetrate = 3,
        Slow = 4,
        Slow_Penetrate = 5,
        Slow_Persistent = 6,
        Slow_Persistent_Penetrate = 7,
        Stop = 8,
        Stop_Penetrate = 9,
        Stop_Persistent = 10,
        Stop_Persistent_Penetrate = 11,
        Stop_Slow = 12,
        Stop_Slow_Penetrate = 13,
        Stop_Slow_Persistent = 14,
        Stop_Slow_Persistent_Penetrate = 15,
    }

    enum DestroyTypeData
    {
        Count = 0,
        Time = 1
    }
    enum DestroyEffectData
    {
        Non = 0,
        Explosion = 1,
        Shock = 2,
        Shock_Explosion = 3
    }

    enum BulletSizeData
    {
        Small = 1,
        Middle = 2,
        Big = 3,
        Huge = 4
    }
}