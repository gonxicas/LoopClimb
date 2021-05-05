// using System;
// using UnityEngine;
//
// public class MaskController : MonoBehaviour
// {
//     private GameObject _mask;
//     private float _deactivateTime;
//
//     private void Awake()
//     {
//         _mask = GetComponentInChildren<SpriteMask>().gameObject;
//         _deactivateTime = 5f;
//     }
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (!other.CompareTag("Player")) return;
//            
//         DeactivateMask();
//         CancelInvoke();
//             
//         
//     }
//
//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (!other.CompareTag("Player")) return;
//         
//         CancelInvoke();
//         Invoke(nameof(ActivateMask),_deactivateTime);
//
//     }
//
//     public void ActivateMask()
//     {
//         _mask.SetActive(true);
//         
//     }
//
//     public void DeactivateMask() => _mask.SetActive(false);
// }
