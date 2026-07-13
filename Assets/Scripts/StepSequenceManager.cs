using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSequenceManager : MonoBehaviour
{

    [System.Serializable]
    public class StepData
    {

        [Header("Step Information")]
        public string stepName;


        [Header("Audio")]
        public AudioClip audioClip;
        public bool loopAudio = false;


        [Header("Objects To Enable")]
        public List<GameObject> objectsToEnable = new List<GameObject>();


        [Header("Objects To Disable")]
        public List<GameObject> objectsToDisable = new List<GameObject>();


        [Header("Animation Trigger")]
        public Animator animator;
        public string animationTriggerName;

    }



    [Header("Sequence Steps")]
    public List<StepData> steps = new List<StepData>();



    [Header("Audio Source")]
    public AudioSource audioSource;



    private int currentStepIndex = 0;


    void Start()
    {
        StartSequence();
    }

    //=========================================================
    // START SEQUENCE
    //=========================================================

    public void StartSequence()
    {
        currentStepIndex = 0;

        PlayCurrentStep();
    }





    //=========================================================
    // PLAY CURRENT STEP
    //=========================================================

    private void PlayCurrentStep()
    {

        if(currentStepIndex >= steps.Count)
        {
            Debug.Log("ALL STEPS COMPLETED");
            return;
        }


        StepData step = steps[currentStepIndex];


        ExecuteStep(step);

    }





    //=========================================================
    // EXECUTE STEP
    //=========================================================

    private void ExecuteStep(StepData step)
    {

        Debug.Log(
            "STEP STARTED : " +
            (currentStepIndex + 1) +
            " - " +
            step.stepName
        );



        // ENABLE OBJECTS

        foreach(GameObject obj in step.objectsToEnable)
        {
            if(obj != null)
            {
                obj.SetActive(true);
            }
        }




        // DISABLE OBJECTS

        foreach(GameObject obj in step.objectsToDisable)
        {
            if(obj != null)
            {
                obj.SetActive(false);
            }
        }




        // TRIGGER ANIMATION

        if(step.animator != null &&
           !string.IsNullOrEmpty(step.animationTriggerName))
        {

            step.animator.SetTrigger(step.animationTriggerName);

        }





        // AUDIO CONTROL

        if(step.audioClip != null)
        {

            PlayAudio(step.audioClip, step.loopAudio);



            if(step.loopAudio)
            {
                // Loop audio does not block next step

                CompleteStep();

            }
            else
            {
                // Wait for audio completion

                StartCoroutine(WaitForAudioFinish());

            }

        }
        else
        {

            // No audio assigned

            CompleteStep();

        }

    }





    //=========================================================
    // WAIT FOR NON LOOP AUDIO
    //=========================================================

    private IEnumerator WaitForAudioFinish()
    {

        while(audioSource != null && audioSource.isPlaying)
        {
            yield return null;
        }


        CompleteStep();

    }





    //=========================================================
    // COMPLETE CURRENT STEP
    //=========================================================

    private void CompleteStep()
    {

        StepData completedStep = steps[currentStepIndex];


        Debug.Log(
            "STEP ENDED : " +
            (currentStepIndex + 1) +
            " - " +
            completedStep.stepName
        );



        currentStepIndex++;



        if(currentStepIndex < steps.Count)
        {
            PlayCurrentStep();
        }
        else
        {
            Debug.Log("ALL STEPS COMPLETED");
        }

    }





    //=========================================================
    // PUBLIC AUDIO FUNCTION
    // Can be called from any other script
    //=========================================================

    public void PlayAudio(AudioClip clip, bool loop)
    {

        if(audioSource == null)
        {
            Debug.LogWarning("Audio Source is not assigned");
            return;
        }



        audioSource.Stop();


        audioSource.clip = clip;
        audioSource.loop = loop;


        audioSource.Play();

    }





    //=========================================================
    // STOP AUDIO
    //=========================================================

    public void StopAudio()
    {

        if(audioSource != null)
        {
            audioSource.Stop();
        }

    }





    //=========================================================
    // PLAY SPECIFIC STEP MANUALLY
    //=========================================================

    public void PlayStep(int stepIndex)
    {

        if(stepIndex < 0 || stepIndex >= steps.Count)
        {
            Debug.LogWarning("Invalid Step Index");
            return;
        }



        StopAllCoroutines();


        currentStepIndex = stepIndex;


        PlayCurrentStep();

    }





    //=========================================================
    // NEXT STEP MANUALLY
    //=========================================================

    public void NextStep()
    {

        StopAllCoroutines();


        CompleteStep();

    }





    //=========================================================
    // CURRENT STEP INDEX
    //=========================================================

    public int GetCurrentStep()
    {
        return currentStepIndex;
    }

}