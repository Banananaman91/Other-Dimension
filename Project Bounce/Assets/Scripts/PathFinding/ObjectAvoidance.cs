﻿using System.Collections.Generic;
using Controllers;
using Interface;

namespace PathFinding
{
    public class ObjectAvoidance : IAvoider
    {
        public List<Controller> Objects = new List<Controller>();
        public void UpdateSpaces(Controller controller) => Objects.Add(controller);
    }
}
