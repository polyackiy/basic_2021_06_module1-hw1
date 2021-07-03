using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
        character.target.GetComponent<Character>().Die();
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
        character.target.GetComponent<Character>().Die();
    }

    void HitEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
        character.target.GetComponent<Character>().Die();
    }
}
