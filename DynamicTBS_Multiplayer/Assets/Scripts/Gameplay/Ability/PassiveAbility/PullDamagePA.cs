using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PullDamagePA : IPassiveAbility
{
    private static PatternType pullDamagePatternType = PatternType.Cross;

    private Character owner;

    public PullDamagePA(Character character)
    {
        owner = character;
    }

    public void Apply() 
    {
        List<Character> characters = CharacterHandler.GetAllLivingCharacters();

        foreach (Character character in characters)
        {
            var defaultIsDamageable = character.isDamageable;
            character.isDamageable = (damage) =>
            {
                if (character.GetPassiveAbility().GetType() == typeof(PullDamagePA))
                {
                    return defaultIsDamageable(damage);
                }

                if(owner.isDamageable(damage) && CharacterHandler.AlliedNeighbors(character, owner, pullDamagePatternType))
                {
                    return false;
                }

                return defaultIsDamageable(damage);
            };
        }

        CharacterEvents.OnCharacterTakesDamage += AbsorbDamage;
    }

    private void AbsorbDamage(Character character, int damage)
    {
        if (owner.IsDead())
        {
            CharacterEvents.OnCharacterTakesDamage -= AbsorbDamage;
            return;
        }

        if (character.GetPassiveAbility().GetType() == typeof(PullDamagePA))
        {
            return;
        }

        if(owner.isDamageable(damage) && CharacterHandler.AlliedNeighbors(character, owner, pullDamagePatternType))
        {
            owner.TakeDamage(damage);
        }
    }


    ~PullDamagePA()
    {
        CharacterEvents.OnCharacterTakesDamage -= AbsorbDamage;
    }
}