using UnityEngine;

public class PuzzleTarget : MonoBehaviour
{
    private bool touched = false;

    public bool IsTouched()
    {
        return touched;
    }

    public void MarkTouched()
    {
        touched = true;
    }

    public void ResetTouched()
    {
        touched = false;
    }
}