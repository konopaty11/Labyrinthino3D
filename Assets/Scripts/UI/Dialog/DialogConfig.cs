using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogConfig", menuName = "SO/DialogConfig")]
public class DialogConfig : ScriptableObject
{
    public List<LogSerializable> logs = new();
}

[Serializable]
public class LogSerializable
{
    public Author author;
    public string name;
    public string message;
}

public enum Author
{
    None,
    Player,
    Robot
}