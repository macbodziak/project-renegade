using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;
using System;

public class WalkableAreaExample : MonoBehaviour
{
    Actor selectedActor;
    bool inputBlocked = false;
    List<GameObject> areaMarkers;
    WalkableArea area;

    [SerializeField] GameObject pathFlagPrefab;
    [SerializeField] NavGrid grid;
    [SerializeField] int budget = 50;


    public void Start()
    {
        areaMarkers = new();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (inputBlocked)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearAreaPreview();
            selectedActor = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            LayerMask layerMask = LayerMask.GetMask(new string[] { "Actor", "Grid" });
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {

                Actor clickedActor = hit.collider.gameObject.GetComponent<Actor>();
                if (clickedActor != null)
                {
                    selectedActor = clickedActor;
                    area = Pathfinder.FindWalkableArea(grid, selectedActor.NodeIndex, budget);
                    ClearAreaPreview();
                    PreviewArea(area);
                    return;
                }

                if (selectedActor == null)
                {
                    return;
                }

                int clickedIndex;
                clickedIndex = grid.IndexAt(hit.point);

                if (area.ContainsNode(clickedIndex))
                {
                    inputBlocked = true;
                    ClearAreaPreview();
                    Path path = area.GetPathFromNodeIndex(clickedIndex);
                    Pathfinder.DebugDrawPath(path, Color.red, 5f);
                    selectedActor.MovementFinishedEvent += OnActorFinishedMovement;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    selectedActor.MoveAlongPath(path);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
    }


    private void PreviewArea(WalkableArea _area)
    {
        if (_area == null)
        {
            return;
        }
        areaMarkers = new List<GameObject>(_area.Count());

        foreach (var el in _area.GetWalkableAreaElements())
        {
            GameObject flag = Instantiate(pathFlagPrefab, el.worldPosition, Quaternion.identity);
            areaMarkers.Add(flag);
        }
    }

    private void ClearAreaPreview()
    {
        foreach (var el in areaMarkers)
        {
            Destroy(el.gameObject);
        }

        areaMarkers.Clear();
    }

    private void OnActorFinishedMovement(object sender, ActorFinishedMovementEventArgs e)
    {
        Actor actor = sender as Actor;
        Debug.Log(actor.gameObject.name + " has finished movement at " + grid.GridCoordinatesAt(e.GoalIndex));
        inputBlocked = false;
        area = Pathfinder.FindWalkableArea(grid, selectedActor.NodeIndex, budget);
        PreviewArea(area);
        selectedActor.MovementFinishedEvent -= OnActorFinishedMovement;
    }

}
