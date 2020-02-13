using System.Collections.Generic;
using Controllers;
using Interface;
using Terrain;

namespace PathFinding
{
    public class ObjectAvoidance : IAvoider
    {
        public List<Controller> Objects = new List<Controller>();
        public AreaRestriction Container;
        public void UpdateSpaces(Controller controller) => Objects.Add(controller);
    }
}
