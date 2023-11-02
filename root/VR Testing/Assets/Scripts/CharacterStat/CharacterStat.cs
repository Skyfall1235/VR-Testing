/* File: CharacterStat
 * Version: 1.1
 * Author: Wyatt Murray
 * Created: 6/21/23
 * Last Updated: 10/27/23
 * --------------------------------------------------------------
 * Script Summary:
 * This script is a singular file which, when instantiated stores a singular float value, and provides methods to add modifiers to the value, 
 * in any or all of the following forms: 
 * - float addition
 * - Float Percent Addition
 * - float Percent Multiplication
 * 
 * Upon the addition of any modifiers, the stats final value will be changed and can be accessed to display current information.
 * --------------------------------------------------------------
 * Current Accessible Methods:
 * - AddModifier
 * - RemoveAllModifiers
 * - GetFlatAddMultiplyValues
 * - CalculateTotalValue
 * --------------------------------------------------------------
 * Ownership and Usage:
 * - This code comes as a singular file titled "CharacterStat"
 * - The singular file titled "CharacterStat" is created and owned by Wyatt Murray.
 * - You are granted a non-exclusive, perpetual license to use and
 *   modify this code for personal and commercial purposes.
 * - Distribution of this code, in part or in whole, is not allowed.
 * - Attribution is required for any commercial products that
 *   incorporate this code.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Burst;

public class CharacterStat
{
    #region Variables
    /// <summary>
    /// The base value of the stat.
    /// </summary>
    private float m_baseValue;
    /// <summary>
    /// [PUBLIC] The base value of the stat.
    /// </summary>
    public float BaseValue
    {
        get { return m_baseValue; } 
        set { m_baseValue = value; }
    }

    /// <summary>
    /// The cached value of the stat, taking into account all modifiers.
    /// </summary>
    private float m_finalValue;
    /// <summary>
    /// [PUBLIC] The cached value of the stat, taking into account all modifiers.
    /// </summary>
    public float FinalValue
    {
        get { return m_finalValue; }
    }

    /// <summary>
    /// A list of modifiers that affect the stat.
    /// </summary>
    private List<StatModifier> m_statModifiers = new List<StatModifier>();
    /// <summary>
    /// [PUBLIC] A list of modifiers that affect the stat.
    /// </summary>
    public List<StatModifier> StatModifiers
    {
        get { return m_statModifiers; }
        set { m_statModifiers = value; }
    }
    
    /// <summary>
    /// [PUBLIC] A UnityEvent that is invoked when a new modifier is added to the stat.
    /// </summary>
    public UnityEvent m_OnAddModifierEvent;
    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the CharacterStat class.
    /// </summary>
    /// <param name="baseValue">The base value of the stat.</param>
    public CharacterStat(float baseValue)
    {
        // Initialize the base value.
        m_baseValue = baseValue;

        // Create a new empty list of stat modifiers.
        m_statModifiers = new List<StatModifier>();

        // Create a new UnityEvent to be invoked when a stat modifier is added.
        m_OnAddModifierEvent = new UnityEvent();
    }

    /// <summary>
    /// Initializes a new instance of the CharacterStat class with the given base value and UnityEvent.
    /// </summary>
    /// <param name="baseValue">The base value of the stat.</param>
    /// <param name="onAddModifierEvent">The UnityEvent to be invoked when a stat modifier is added.</param>
    /// <exception cref="ArgumentNullException">Thrown if the onAddModifierEvent parameter is null.</exception>
    public CharacterStat(float baseValue, UnityEvent onAddModifierEvent)
    {
        // Initialize the base value.
        m_baseValue = baseValue;

        // Create a new empty list of stat modifiers.
        m_statModifiers = new List<StatModifier>();

        // If the onAddModifierEvent parameter is null, throw an exception.
        if (onAddModifierEvent == null)
        {
            throw new ArgumentNullException(nameof(onAddModifierEvent));
        }

        // Set the OnAddModifierEvent property.
        m_OnAddModifierEvent = onAddModifierEvent;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds a new modifier to the character's stats.
    /// </summary>
    /// <param name="newModifier">The new modifier to add.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the modifier type is not valid.</exception>
    [BurstCompile]
    public virtual void AddModifier(StatModifier newModifier)
    {
        // Confirm the method parameters are properly defined
        if (!Enum.IsDefined(typeof(StatModType), newModifier.modType))
        {
            throw new ArgumentOutOfRangeException("newModifier.modType");
        }

        // Add the modifier to the list
        m_statModifiers.Add(newModifier);
        CalculateTotalValue();
        m_OnAddModifierEvent.Invoke();
    }

    /// <summary>
    /// Removes All modifiers from the stat
    /// </summary>
    [BurstCompile]
    public virtual void RemoveAllModifiers()
    {
        m_statModifiers.Clear();
        CalculateTotalValue();
    }

    /// <summary>
    /// Calculates the total flat, percent add, and percent multiply values for the character's stats.
    /// </summary>
    /// <returns> A Vector3 containing the total flat, percent add, and percent multiply values of the tracked stat.</returns>
    [BurstCompile]
    public virtual Vector3 GetFlatAddMultiplyValues()
    {
        // Initialize the total values.
        float totalFlat = 0;
        float totalPercentAdd = 0;
        float totalPercentMultiply = 1;

        // Iterate over all of the character's stat modifiers.
        foreach (StatModifier mod in m_statModifiers)
        {
            // Switch on the modifier type.
            switch (mod.modType)
            {
                // Add flat modifiers to the total flat value.
                case StatModType.Flat:
                    totalFlat += mod.value;
                    break;

                // Add percent add modifiers to the total percent add value.
                case StatModType.PercentAdd:
                    totalPercentAdd += mod.value;
                    break;

                // Multiply percent multiply modifiers into the total percent multiply value.
                case StatModType.PercentMultiply:
                    totalPercentMultiply *= 1 + mod.value;
                    break;
            }
        }

        // Return the total flat, percent add, and percent multiply values as a `Vector3`.
        return new Vector3(totalFlat, totalPercentAdd, totalPercentMultiply);
    }

    /// <summary>
    /// Calculates the total value of the stat, taking into account all modifiers.
    /// </summary>
    [BurstCompile]
    public void CalculateTotalValue()
    {
        // Start with the base value of the stat.
        float finalValue = m_baseValue;
        // Keep track of the sum of all percent-additive modifiers.
        float sumPercentAdd = 0;

        // Iterate over all modifiers.
        for (int i = 0; i < m_statModifiers.Count; i++)
        {
            StatModifier mod = m_statModifiers[i];

            // Switch on the modifier type.
            switch (m_statModifiers[i].modType)
            {
                // Flat modifiers are added directly to the final value.
                case StatModType.Flat: 
                    finalValue += mod.value;
                    break;

            // Percent-additive modifiers are added to the final value, but only after all other modifiers have been applied.
                case StatModType.PercentAdd:
                    sumPercentAdd += mod.value;

                    // If this is the last percent-additive modifier, or if the next modifier is not a percent-additive modifier,
                    // apply the percent-additive modifiers to the final value and reset the sum.
                    if (i + 1 < m_statModifiers.Count || m_statModifiers[i + 1].modType != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                    break;

                // Percent-multiplicative modifiers are multiplied into the final value.
                case StatModType.PercentMultiply:
                    finalValue *= 1 + mod.value;
                    break;
            }
        }
        
        // Return the final value of the stat.
        finalValue = (float)Math.Round(finalValue, 4);
    }
    #endregion
}

#region Data Organization
/// <summary>
/// Represents a modifier to a character's stat.
/// </summary>
public struct StatModifier
{
    /// <summary>
    /// The name of the stat
    /// </summary>
    public readonly string name;

    /// <summary>
    /// The value of the modifier.
    /// </summary>
    public readonly float value;

    /// <summary>
    /// The type of the modifier.
    /// </summary>
    public readonly StatModType modType;

    /// <summary>
    /// Initializes a new instance of the StatModifier struct.
    /// </summary>
    /// <param name="name">The name of the modifier</param>
    /// <param name="value">The value of the modifier.</param>
    /// <param name="modifierType">The type of the modifier.</param>
    public StatModifier(string name, float value, StatModType modifierType)
    {
        this.name = name;
        this.value = value;
        this.modType = modifierType;
    }
}

/// <summary>
/// An enumeration of the different types of stat modifiers.
/// </summary>
public enum StatModType
{
    /// <summary>
    /// A flat modifier adds a fixed value to the stat.
    /// </summary>
    Flat,

    /// <summary>
    /// A percent-additive modifier adds a percentage of the stat's base value to the stat.
    /// </summary>
    PercentAdd,

    /// <summary>
    /// A percent-multiplicative modifier multiplies the stat's value by a percentage.
    /// </summary>
    PercentMultiply
}
#endregion

