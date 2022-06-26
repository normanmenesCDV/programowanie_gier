using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvadersM : MonoBehaviour
{
    public Invaders[] prefabs;

    public int rows = 5;

    public int columns = 11;

    public AnimationCurve speed;

    public Projectile missilePrefab;

    public float missileAttackRate = 1.0f;

    public int amoutKilled { get; private set; }

    public int amountAlive => this.totalInvaders - this.amoutKilled;

    public int totalInvaders => this.rows * this.columns;

    public float percentKilled => (float)this.amoutKilled / (float)this.totalInvaders;

    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
        for(int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for(int col = 0; col < this.columns; col++)
            {
                Invaders invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;

                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

         Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
         Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

         foreach(Transform invaders in this.transform)
         {
            if(!invaders.gameObject.activeInHierarchy)
            {
                continue;
            }

            if(_direction == Vector3.right && invaders.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
            }
            else if(_direction == Vector3.left && invaders.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
         }
    }


    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach(Transform invaders in this.transform)
         {
            if(!invaders.gameObject.activeInHierarchy)
            {
                continue;
            }

            if(Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.missilePrefab, invaders.position, Quaternion.identity);
                break;
            }
         }
    }

    private void InvaderKilled()
    {
        this.amoutKilled++;

        if(this.amoutKilled >= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
