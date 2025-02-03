using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(CommandStateBehaviour))]
public class CommandStateBehaviourEditor : Editor
{
    private AnimationClip _previewClip;
    private float _previewTime;
    private bool _isPreviewing;
    private SerializedProperty _eventsListProperty;
    private List<SerializedProperty> _eventNames = new();
    private List<SerializedProperty> _eventTriggerTimes = new();
    private List<float> _previousEventTriggerTimes = new();

    public override void OnInspectorGUI()
    {
        InitSerializedProperties();
        DrawDefaultInspector();

        CommandStateBehaviour commandStateBehaviour = (CommandStateBehaviour)target;

        if (Validate(commandStateBehaviour, out string errorMessage))
        {
            {
                if (_isPreviewing)
                {
                    if (GUILayout.Button("Stop Preview"))
                    {
                        EnforceTPose();
                        AnimationMode.StopAnimationMode();
                        _isPreviewing = false;
                    }
                    else
                    {
                        PreviewAnimationClip(commandStateBehaviour);
                    }
                }
                else if (GUILayout.Button("Preview"))
                {
                    _isPreviewing = true;
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    private void PreviewAnimationClip(CommandStateBehaviour commandStateBehaviour)
    {
        if (!_previewClip)
        {
            return;
        }

        for (int i = 0; i < _eventTriggerTimes.Count; i++)
        {
            if (_eventTriggerTimes[i].floatValue != _previousEventTriggerTimes[i])
            {
                _previewTime = _eventTriggerTimes[i].floatValue * _previewClip.length;
                _previousEventTriggerTimes[i] = _eventTriggerTimes[i].floatValue;
            }
        }

        AnimationMode.StartAnimationMode();
        AnimationMode.SampleAnimationClip(Selection.activeGameObject, _previewClip, _previewTime);
        // AnimationMode.StopAnimationMode();
    }

    private bool Validate(CommandStateBehaviour commandStateBehaviour, out string errorMessage)
    {
        AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
        if (!animatorController) return false;
        ChildAnimatorState matchingState = animatorController.layers.SelectMany(layer => layer.stateMachine.states)
            .FirstOrDefault(state => state.state.behaviours.Contains(commandStateBehaviour));

        _previewClip = matchingState.state?.motion as AnimationClip;
        if (!_previewClip)
        {
            errorMessage = "No valid AnimationClip found for the current state.";
            return false;
        }

        return true;
    }

    [MenuItem("GameObject/Enforce T-Pose", false, 0)]
    static void EnforceTPose()
    {
        GameObject selected = Selection.activeGameObject;
        if (!selected || !selected.TryGetComponent<Animator>(out Animator animator) || !animator.avatar)
        {
            return;
        }

        SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

        foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (bone == HumanBodyBones.LastBone) continue;

            Transform boneTransform = animator.GetBoneTransform(bone);
            if (!boneTransform) continue;

            SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
            if (skeletonBone.name == null) continue;

            if (bone == HumanBodyBones.Hips)
            {
                boneTransform.localPosition = skeletonBone.position;
            }

            boneTransform.localRotation = skeletonBone.rotation;
        }

        AnimationMode.StopAnimationMode();

        Debug.Log("T-Pose Enforced successfully");
    }

    private AnimatorController GetValidAnimatorController(out string errorMessage)
    {
        GameObject targetGameObject = Selection.activeGameObject;
        if (!targetGameObject)
        {
            errorMessage = "Select a GameObject with an Animator to preview";
            return null;
        }

        Animator animator = targetGameObject.GetComponent<Animator>();
        if (!animator)
        {
            errorMessage = "The selected GameObject does not have an Animator component";
            return null;
        }

        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (!animatorController)
        {
            errorMessage = "The selected Animator controller does not have a valid AnimatorController";
            return null;
        }

        errorMessage = "";
        return animatorController;
    }


    private void InitSerializedProperties()
    {
        _eventsListProperty = serializedObject.FindProperty("_events");

        _eventNames.Clear();
        _eventTriggerTimes.Clear();

        for (int i = 0; i < _eventsListProperty.arraySize; i++)
        {
            SerializedProperty timedAnimationEventProperty = _eventsListProperty.GetArrayElementAtIndex(i);
            SerializedProperty triggerTimeProperty = timedAnimationEventProperty.FindPropertyRelative("TriggerTime");
            SerializedProperty eventNameProperty = timedAnimationEventProperty.FindPropertyRelative("Name");

            _eventNames.Add(eventNameProperty);
            _eventTriggerTimes.Add(triggerTimeProperty);
            _previousEventTriggerTimes.Add(triggerTimeProperty.floatValue);
        }
    }
}