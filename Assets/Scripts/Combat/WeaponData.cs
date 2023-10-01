using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
   public List<Health> Enemies;
   public int Damage = 10;
   

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Player")) return;

      if (other.TryGetComponent(out Health health))
      {
         Enemies.Add(health);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.CompareTag("Player")) return;

      if (other.TryGetComponent(out Health health))
      {
         Enemies.Remove(health);
      }
   }

   public void ClearEnemies()
   {
      Enemies.Clear();
   }
   
}
