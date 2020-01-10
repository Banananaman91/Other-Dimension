using System.Collections.Generic;
using Controllers.Enemies.Flying;
using UnityEngine;

namespace Controllers.Enemies
{
    public class BoidRules
    {
        /*
         *
         * boid rule 1
         * Boids try to fly towards centre of mass of neighbouring boids (cohesion)
         * new average vector
         * if there are no neighbours
         * return average
         * for each neighbour
         * avg += neighbour position
         * return average / neighbour count
         *
         *
         * boid rule 2
         * boids try to keep a small distance away from other objects (including other boids) (separation)
         * new separation vector
         * for each neighbour boid
         * distance = current position - neighbour position
         * if distance < separation distance
         * separation -= distance
         *
         * return seperation
         *
         *
         * boid rule 3
         * boids try to match the velocity of nearby boids
         * new vector velocity
         * for each neighbour
         * velocity += neighbour velocity
         * return velocity
         *
         * boid rule 4
         * boid tends towards common goal
         * pass in goal position and boid
         * return goal position - boid position / 100
         */

        public Vector3 boidRule1(List<Rigidbody> neighbours)
        {
            var average = new Vector3(0, 0, 0);

            if (neighbours.Count == 0) return average;

            foreach (var neighbour in neighbours)
            {
                average += neighbour.position;
            }

            return average / neighbours.Count;
        }

        public Vector3 boidRule2(FlyingBoid boid, List<Rigidbody> neighbours)
        {
            var boidRb = boid.BoidRigidbody;
            var separation = new Vector3(0, 0, 0);

            foreach (var neighbour in neighbours)
            {
                var distanceVector = boidRb.position - neighbour.position;
                if (Vector3.Distance(boidRb.position, neighbour.position) < boid.SeparationDistance) separation -= distanceVector;
            }

            return -separation;
        }

        public Vector3 boidRule3(List<Rigidbody> neighbours)
        {
            var averageVelocity = new Vector3(0, 0, 0);

            foreach (var neighbour in neighbours)
            {
                averageVelocity += neighbour.velocity;
            }

            return averageVelocity;
        }
    }
}
