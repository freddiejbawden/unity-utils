using System;
using UnityEngine;


public enum GameState
{
    FISHING,
    BOAT
}

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    /* Format 
     * 
     * public event Action<T> onEventTrigger;
     * public void EventTrigger(T t)
     * {
     *      if (onEventTrigger != null)
     *      {
     *          onEventTrigger(t)
     *      }
     * }
     */
    public event Action<int> onLoadLevel;
    public void LoadLevel(int levelIdx)
    {
        if (onLoadLevel != null)
        {
            onLoadLevel(levelIdx);
        }
    }

    public event Action onLoadNextLevel;
    public void LoadNextLevel()
    {
        if (onLoadNextLevel != null)
        {
            onLoadNextLevel();
        }
    }


}
