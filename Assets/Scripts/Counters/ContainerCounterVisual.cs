using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string OpenClose = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabObject += ContainerCounter_OnPlayerGrabObject;
    }

    private void ContainerCounter_OnPlayerGrabObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OpenClose);
    }
}
