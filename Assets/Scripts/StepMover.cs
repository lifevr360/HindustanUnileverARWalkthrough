using UnityEngine;
using System.Collections;

public class StepMover : MonoBehaviour
{
    public Transform objectToMove;    // the object sitting on the table
    public Transform[] checkpoints;   // 5 empty GameObjects = target after btn1..btn5
    public float speed = 0.3f;        // metres per second (tune for AR scale)

    private Coroutine moveRoutine;

    // Wire each button's OnClick to this, passing 0,1,2,3,4
    public void MoveToStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= checkpoints.Length) return;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveTo(checkpoints[stepIndex].position));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(objectToMove.position, target) > 0.001f)
        {
            objectToMove.position = Vector3.MoveTowards(
                objectToMove.position, target, speed * Time.deltaTime);
            yield return null;
        }
        objectToMove.position = target;
    }
}