using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkerEnemy : Enemy
{
    public int actions;
    public int damage;
    public float moveDuration = 0.3f;

    public override void Act()
    {
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        int remainingMovement = actions;
        while (remainingMovement > 0)
        {
            Vector2Int nextPosition = position + Vector2Int.left;
            Debug.Log("Enemy attempting to move to " +  nextPosition);
            if (IsValidPosition(nextPosition))
            {
                GridCell nextCell = GridManager.Instance.grid[nextPosition.x, nextPosition.y];
                if (nextCell.IsOccupiedByTower)
                {
                    // Deal damage to the tower
                    TowerObject tower = nextCell.towerObject.GetComponent<TowerObject>();
                    yield return StartCoroutine(DealDamage(tower, moveDuration));
                    remainingMovement--;
                }
                else
                {
                    // Move to the next cell with smooth movement
                    yield return StartCoroutine(SmoothMove(transform.position, new Vector3(nextPosition.x, transform.position.y, nextPosition.y), moveDuration));
                    position = nextPosition;
                    remainingMovement--;
                }
            }
            else
            {
                RunManager.Instance.UseLifeLine();
                break; 
            }
        }
    }

    private IEnumerator DealDamage(TowerObject tower, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        tower.TakeDamage(damage);
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

    protected bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < GridManager.Instance.width &&
               position.y >= 0 && position.y < GridManager.Instance.height;
    }
}
