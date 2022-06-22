using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvadersM : MonoBehaviour
{
    public Invaders[] prefabs;

    public int rows = 5;

    public int columns = 11;


    private void Awake()
    {
        for(int row = 0; row < this.rows; row++)
        {
            for(int col = 0; col < this.columns; col++)
            {
               Invaders invader = Instantiate(this.prefabs[row], this.transform);
            }
        }
    }
}
