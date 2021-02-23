using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    [SerializeField] private float nodeSize = 0.5f; // A* grid graph node size

    public float pathLength = 0; //Path from player spawn position to finish with checkpoint at key postion

    [HideInInspector] public bool pathCalculated = false; //Indicator that AI finished calculating path

    private Seeker seeker;
    private AIPath aiPath;
    private AIDestinationSetter dest;

    private void Start()
	{
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        dest = GetComponent<AIDestinationSetter>();
    }

	public IEnumerator CalculatePathLength()
	{
        pathCalculated = false; //Started calculation

        aiPath.canSearch = false;
        
        AstarPath.active.Scan(); //Calculate grid

        yield return new WaitUntil(() => (!AstarPath.active.isScanning && !AstarPath.active.IsAnyGraphUpdateInProgress && !AstarPath.active.IsAnyGraphUpdateQueued)); //When grid is calculated

        //Calculate path to key
        dest.target = GetComponentInParent<MazeRenderer>().key;
        aiPath.canSearch = true;

        //when path is calculated
        yield return new WaitUntil(() => aiPath.hasPath);
        yield return new WaitUntil(() => seeker.GetCurrentPath().IsDone() && !aiPath.pathPending);

        aiPath.canSearch = false;
        pathLength += seeker.GetCurrentPath().vectorPath.Count * nodeSize; //Add to pathLength

        transform.position = GetComponentInParent<MazeRenderer>().key.position; //Move AI to key position
        //Calculate path from key to finish
        dest.target = GetComponentInParent<MazeRenderer>().finish;
        aiPath.canSearch = true;
        
        yield return new WaitForSecondsRealtime(aiPath.repathRate);//Wait for ai to pick next path
        //when path is calculated
        yield return new WaitUntil(() => aiPath.hasPath);
        yield return new WaitUntil(() => seeker.GetCurrentPath().IsDone() && !aiPath.pathPending);

        aiPath.canSearch = false;
        pathLength += seeker.GetCurrentPath().vectorPath.Count * nodeSize; //Add to pathLength

        pathCalculated = true; //Finished calculation
    }
}
