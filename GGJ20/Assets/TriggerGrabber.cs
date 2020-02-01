using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TriggerGrabber : MonoBehaviour
{
    bool itemGrabbed = false;
    public Vector3 translationOffset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;
    private ConstraintSource constraintSource;

    private void OnTriggerEnter(Collider other)
    {
        if (itemGrabbed) return;

        if (other.CompareTag("Item"))
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            ParentConstraint p = other.GetComponent<ParentConstraint>();
            if (p == null) return;
            constraintSource.sourceTransform = this.transform;
            constraintSource.weight = 1.0f;
            p.AddSource(constraintSource);
            p.SetTranslationOffset(0, translationOffset);
            p.SetRotationOffset(0, rotationOffset);
            p.constraintActive = true;
            //other.transform.SetParent(this.transform);
            //other.transform.localPosition = Vector3.zero;
            //other.transform.localRotation = Quaternion.identity;
            itemGrabbed = true;
        }
    }

}
