﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeatmapObjectContainerCollection : MonoBehaviour
{
    public AudioTimeSyncController AudioTimeSyncController;
    public List<BeatmapObjectContainer> LoadedContainers = new List<BeatmapObjectContainer>();
    public BeatmapObjectCallbackController SpawnCallbackController;
    public BeatmapObjectCallbackController DespawnCallbackController;
    public Transform GridTransform;

    private void OnEnable()
    {
        BeatmapObjectContainer.FlaggedForDeletionEvent += CreateActionThenDelete;
        SubscribeToCallbacks();
    }

    private void CreateActionThenDelete(BeatmapObjectContainer obj)
    {
        BeatmapActionContainer.AddAction(new BeatmapObjectDeletionAction(obj));
        DeleteObject(obj);
    }

    public void DeleteObject(BeatmapObjectContainer obj)
    {
        if (LoadedContainers.Contains(obj))
        {
            LoadedContainers.Remove(obj);
            Destroy(obj.gameObject);
            SelectionController.RefreshMap();
        }
    }

    private void OnDisable()
    {
        BeatmapObjectContainer.FlaggedForDeletionEvent -= CreateActionThenDelete;
        UnsubscribeToCallbacks();
    }

    internal abstract void SubscribeToCallbacks();
    internal abstract void UnsubscribeToCallbacks();
    public abstract void SortObjects();
    public abstract BeatmapObjectContainer SpawnObject(BeatmapObject obj);
}