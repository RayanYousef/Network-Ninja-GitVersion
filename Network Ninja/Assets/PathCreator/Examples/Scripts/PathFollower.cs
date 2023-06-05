using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        private PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        public PathCreator PathCreator
        {
            get => pathCreator;
            set
            {
                pathCreator = value;
                if(pathCreator != null)
                {
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }
            }
        }

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //{
                if (pathCreator != null)
                {
                    MovePlayerOnPath();
                }
            //}
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if(other.GetComponentInParent<PathCreator>() != null)
        //    {
        //        pathCreator = other.GetComponentInParent<PathCreator>();
        //    }
        //}

        public void GetCurrentPath(PathCreator currentPath)
        {
            pathCreator = currentPath;
        }

        public void EmptyPath()
        {
            pathCreator = null;
        }
        private void MovePlayerOnPath()
        {

                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

        }
        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}