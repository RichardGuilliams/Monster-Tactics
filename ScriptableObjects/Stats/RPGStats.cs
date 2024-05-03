using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "RPG Stats")]
public class Stats : ScriptableObject
{
    public float speed;
    public int health;
    public int atk;
    public int def;
    public int exp;
    public int level;
    public int wis;
    public int weight;
    public int stamina;
    public int torpidity;
    public int tame;
    public int critChance;
    public int critAtk;
    public int critRate;
    public int luck;    
    public Types type1;
    public Types type2;
}
public enum Types{
    None,
    Serpent,
    Amphibian,
    Bird,
    Bug,
    Beast,
    Undead,
    Holy,
    Unholy,
    Fae,
    Aquatic,
    Fire,
    Water,
    Earth,
    Air,
    Electric,
    Ice
}