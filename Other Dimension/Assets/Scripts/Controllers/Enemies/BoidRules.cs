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
         * boid tends towards common goal (leader)
         * pass in goal position and boid
         * return goal position - boid position / 100
         */

        public void boidRule1(Boid boid, List<Rigidbody> neighbours) // cohesion
        {
            var average = new Vector3(0, 0, 0);
            var boidRb = boid.BoidRigidbody;

            //if (neighbours.Count == 0) return average;

            foreach (var neighbour in neighbours)
            {
                average += neighbour.position;
            }
            
            var direction = average;

            boidRb.MovePosition(boidRb.position + direction * boid.MovementSpeed * Time.deltaTime);
        }

        public void boidRule2(Boid boid, List<Rigidbody> neighbours) // separation
        {
            var boidRb = boid.BoidRigidbody;
            var separation = new Vector3(0, 0, 0);

            foreach (var neighbour in neighbours)
            {
                var distanceVector = boidRb.position - neighbour.position;
                if (Vector3.Distance(boidRb.position, neighbour.position) < boid.NeighbourSeparationDistance) separation -= distanceVector;
            }

            boidRb.MovePosition(boidRb.position += -separation * boid.MovementSpeed * Time.deltaTime);
        }

        public void boidRule3(Boid boid, List<Rigidbody> neighbours) // match velocity
        {
            var averageVelocity = new Vector3(0, 0, 0);
            var boidRb = boid.BoidRigidbody;

            foreach (var neighbour in neighbours)
            {
                averageVelocity += neighbour.velocity;
            }

            boidRb.MovePosition(boidRb.position += averageVelocity * boid.MovementSpeed * Time.deltaTime);
        }
        
        public void BoidRule4(Boid boid) // tend towards leader
        {
            var direction = boid.Leader.position - boid.BoidRigidbody.position;
            if (!boid.Leader) return;
            if (Vector3.Distance(boid.BoidRigidbody.position, boid.Leader.position) < boid.LeaderDistance) return;
            boid.BoidRigidbody.MovePosition(boid.BoidRigidbody.position += direction * boid.MovementSpeed * Time.deltaTime);
        }

        public void BoidRule5(Boid boid, List<Rigidbody> enemies) // tend away from enemy
        {
            var separation = new Vector3(0, 0, 0);
            var boidRb = boid.BoidRigidbody;

            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(boidRb.position, enemy.position) > boid.NeighbourRange) continue;
                separation = enemy.position - boidRb.position;
            }
            
            boidRb.MovePosition(boidRb.position + -separation * boid.MovementSpeed * Time.deltaTime);
        }

        public void BoidRule6(Boid boid, List<Rigidbody> enemies) // separation from enemy
        {
            var boidRb = boid.BoidRigidbody;
            var separation = new Vector3(0, 0, 0);

            foreach (var enemy in enemies)
            {
                var distanceVector = boidRb.position - enemy.position;
                if (Vector3.Distance(boidRb.position, enemy.position) < boid.EnemySeparationDistance) separation -= distanceVector;
            }

            boidRb.MovePosition(boidRb.position += -separation * boid.MovementSpeed * Time.deltaTime);
        }

    }
}
