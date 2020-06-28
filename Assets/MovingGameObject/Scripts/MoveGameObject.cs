using UnityEngine;

namespace CedarWoodSoftware
{
	public class MoveGameObject : MonoBehaviour
	{
        #region Variables
        [Header("Object To Move And Movement Speed")]
        [SerializeField] GameObject gameObject;
        [SerializeField] float moveSpeed;

        [Header("Waypoints For Object To Follow")]
        [SerializeField] Transform[] waypoints;
        [SerializeField] int firstWaypoint;

        [Header("Movement Options")]
        [SerializeField] bool useReverse = false;
        [SerializeField] bool useFlip = false;
        [SerializeField] bool facingRight = false;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Animator animator;

        Transform nextWaypoint;
        bool reverse;
        #endregion

        #region Unity Base Methods
        void Start()
		{
            nextWaypoint = waypoints[firstWaypoint];
        }

		void Update()
		{
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWaypoint.position, Time.deltaTime * moveSpeed);

            if (reverse)
            {
                Reverse();
            }
            else
            {
                Forward();
            }
        }
        #endregion

        #region User Methods
        void Forward()
        {
            if (gameObject.transform.position == nextWaypoint.position)
            {
                firstWaypoint++;

                if (useFlip) Flip();

                if (firstWaypoint == waypoints.Length)
                {
                    if (useReverse)
                    {
                        reverse = true;
                        firstWaypoint = waypoints.Length - 1;
                    }
                    else
                    {
                        firstWaypoint = 0;
                    }
                }
                nextWaypoint = waypoints[firstWaypoint];
            }
        }

        void Reverse()
        {
            if (gameObject.transform.position == nextWaypoint.position)
            {
                firstWaypoint--;

                if (firstWaypoint == 0)
                {
                    reverse = false;
                    firstWaypoint = 0;
                }
                nextWaypoint = waypoints[firstWaypoint];
            }
        }

        void Flip()
        {
            facingRight = !facingRight;

            if (facingRight)
            {
                spriteRenderer.flipX = true;
                animator.SetBool("FacingRight", true);
            }
            else
            {
                spriteRenderer.flipX = false;
                animator.SetBool("FacingRight", false);
            }
        }
        #endregion
    }
}