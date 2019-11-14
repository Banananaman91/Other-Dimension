using System.Collections;
using System.Collections.Generic;
using Controllers;
using Interface;
using UnityEngine;

public class ObjectAvoidance : IAvoider
{
    public List<Controller> Objects => new List<Controller>();
    public void UpdateSpaces(Controller controller) => Objects.Add(controller);
}
