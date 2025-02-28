﻿using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public static PathFollower Instance { get; private set; }

        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 10;

        private float _distanceTravelled;

        private void Awake()
        {
            Instance = this;
        }

        void Start() 
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravelled, endOfPathInstruction);

                transform.eulerAngles += new Vector3(0, 0, 90);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            _distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void SpeedUp()
        {
            speed += 3;
        }
    }
}