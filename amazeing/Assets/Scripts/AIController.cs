using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    private MazeRenderer mazeRenderer;

    [Header("AI")]
    [SerializeField] private float nodeSize = 0.5f; // A* grid graph node size


    [Header("Path")]
    [SerializeField] private GameObject pathPrefab = null;

    [HideInInspector] public float pathLength = 0; //Path from player spawn position to finish with checkpoint at key postion

    [SerializeField] private float pathPointVisibilityTime = 1.0f;
    [SerializeField] private float pathPointVisibilityOffset = 1.0f;
    [SerializeField] private float pathPointMaxScale = 0.25f;

    [HideInInspector] public bool pathCalculated = false; //Indicator that AI finished calculating path


    [Header("Objects")]
    private Seeker seeker;
    private AIPath aiPath;
    private AIDestinationSetter dest;
    private GameObject path;


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
        path = new GameObject("Path");
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
        InstanitatePathPoint();

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
        InstanitatePathPoint();

        pathCalculated = true; //Finished calculation
    }

    private void InstanitatePathPoint()
	{
        foreach (Vector3 vector in seeker.GetCurrentPath().vectorPath)
        {
            GameObject pathPoint = Instantiate(pathPrefab, vector, Quaternion.identity, path.transform);
            pathPoint.transform.localScale = Vector3.zero;
        }
    }


    public void ShowPath()
	{
        for(int i=0; i<path.transform.childCount; i++)
		{
            Transform pathPoint = path.transform.GetChild(i);
            StartCoroutine(ShowPathPartCoroutine(pathPoint, ((i * pathPointVisibilityOffset) / pathLength)));
        }
    }
    private IEnumerator ShowPathPartCoroutine(Transform pathPoint, float timeToWait)
    {
        float t = 0f;

        float startScale = 0;
        float endScale = pathPointMaxScale;

        float newScale;

        //Wait before showing
        yield return new WaitForSeconds(timeToWait);

        //PopIn
        while (t < 1)
        {
            t += Time.deltaTime / (pathPointVisibilityTime / 2);

            newScale = Mathf.Lerp(startScale, endScale, t);

            pathPoint.localScale = new Vector3(newScale, newScale, newScale);

            yield return null;
        }

        t = 0f;
        startScale = pathPoint.localScale.x;
        endScale = 0;

        //PopOut
        while (t < 1)
        {
            t += Time.deltaTime / (pathPointVisibilityTime / 2);

            newScale = Mathf.Lerp(startScale, endScale, t);

            pathPoint.localScale = new Vector3(newScale, newScale, newScale);

            yield return null;
        }
    }
}
