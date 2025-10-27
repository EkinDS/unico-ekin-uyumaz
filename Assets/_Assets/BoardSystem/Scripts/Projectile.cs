using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Assets.BoardSystem
{
   public class Projectile : MonoBehaviour
   {
      private Tween scaleTween;

   
      public void GetLaunched(Enemy targetEnemy, float damage)
      {
         gameObject.SetActive(true);

         StartCoroutine(GetLaunchedOverTime(targetEnemy, damage));
      }
   
   
      private IEnumerator GetLaunchedOverTime(Enemy targetEnemy, float damage)
      {
         float duration = 0.5F;
         Vector3 startPosition = transform.parent.position + Vector3.up * 0.5f;
         Vector3 targetPosition = targetEnemy.transform.position + new Vector3(0F, 0F, -duration * targetEnemy.GetSpeed());
      
         float timer = 0f;

         while (timer < duration)
         {
            float t = timer / duration;

            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);

            currentPos.y += 4f * t * (1F - t);

            transform.position = currentPos;
            transform.LookAt(targetPosition);

            timer += Time.deltaTime;
            yield return null;
         }

         if (targetEnemy)
         {
            targetEnemy.GetDamaged(damage);
         }

         transform.gameObject.SetActive(false);
      }
   

      private void OnDestroy()
      {
         scaleTween.Kill();

         StopAllCoroutines();
      }
   }
}
