using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPositionAndRotation : MonoBehaviour
{
   public Transform original;
   public bool absolute = false;
   public bool transferPosition = false;

   private void Update()
   {
      if (!absolute)
      {
         Quaternion r = original.localRotation;
            transform.localRotation = original.localRotation;
         if (transferPosition)
            transform.localPosition = original.localPosition;
      }
      else
      {
         transform.rotation = original.rotation;
         if (transferPosition)
            transform.position = original.position;
      }
   }   
}
