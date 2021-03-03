using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    private MazeRenderer mazeRenderer;

    [SerializeField] private GameObject pathPrefab = null;

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

        mazeRenderer = GetComponentInParent<MazeRenderer>();

        //Organize path under one gameobject
        GameObject path = new GameObject("Path");
        path.transform.parent = mazeRenderer.transform;

        //wait for maze to be fully drawn
        yield return new WaitUntil(() => mazeRenderer.mazeDrawn); 

        aiPath.canSearch = false;

        AstarPath.active.Scan(); //Calculate grid

        yield return new WaitUntil(() => (!AstarPath.active.isScanning && !AstarPath.active.IsAnyGraphUpdateInProgress && !AstarPath.active.IsAnyGraphUpdateQueued)); //When grid is calculated

        //Calculate path to key
        dest.target = mazeRenderer.key;
        aiPath.canSearch = true;

        //when path is calculated
        yield return new WaitUntil(() => aiPath.hasPath);
        yield return new WaitUntil(() => seeker.GetCurrentPath().IsDone() && !aiPath.pathPending);

        aiPath.canSearch = false;
        pathLength += seeker.GetCurrentPath().vectorPath.Count * nodeSize; //Add to pathLength

        //Add path vectors as gameobjects as path transform children
        foreach(Vector3 vector in seeker.GetCurrentPath().vectorPath)
		{
            Instantiate(pathPrefab, vector, Quaternion.identity, path.transform);
		}

        transform.position = mazeRenderer.key.position; //Move AI to key position
        //Calculate path from key to finish
        dest.target = mazeRenderer.finish;
        aiPath.canSearch = true;
        
        yield return new WaitForSecondsRealtime(aiPath.repathRate);//Wait for ai to pick next path

        //when path is calculated
        yield return new WaitUntil(() => aiPath.hasPath);
        yield return new WaitUntil(() => seeker.GetCurrentPath().IsDone() && !aiPath.pathPending);

        aiPath.canSearch = false;
        pathLength += seeker.GetCurrentPath().vectorPath.Count * nodeSize; //Add to pathLength

        //Add path vectors as gameobjects as path transform children
        foreach (Vector3 vector in seeker.GetCurrentPath().vectorPath)
        {
            Instantiate(pathPrefab, vector, Quaternion.identity, path.transform);
        }

        path.SetActive(false); //TEMP Hide path

        pathCalculated = true; //Finished calculation
    }
}
