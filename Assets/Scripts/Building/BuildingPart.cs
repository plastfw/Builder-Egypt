using System;
using UnityEngine;

public class BuildingPart : MonoBehaviour
{
    public Action<BuildingPart> BuildingPartIsEnabled;

    public void AddToBuilding() => BuildingPartIsEnabled?.Invoke(this);
}
