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


        [Header("Animation Trigger")]
        public Animator animator;
        public string animationTriggerName;

    }



    [Header("Sequence Steps")]
    public List<StepData> steps = new List<StepData>();


    [Header("Audio Source")]
    public AudioSource audioSource;


    private int currentStepIndex = 0;



    //=========================================================
    // ENABLE
    //=========================================================

    private void OnEnable()
    {
        ResetSequence();
        StartSequence();
    }



    //=========================================================
    // DISABLE
    //=========================================================

    private void OnDisable()
    {
        ResetSequence();
    }





    //=========================================================
    // RESET COMPLETE SEQUENCE
    //=========================================================

    private void ResetSequence()
    {

        StopAllCoroutines();


        // Stop audio
        if(audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
        }



        // Reset step index
        currentStepIndex = 0;



        foreach(StepData step in steps)
        {

            // Disable objects enabled by steps
            foreach(GameObject obj in step.objectsToEnable)
            {
                if(obj != null)
                {
                    obj.SetActive(false);
                }
            }



            // Reset animation
            ResetAnimator(step.animator, step.animationTriggerName);

        }


        Debug.Log("Sequence Reset Complete");

    }





    //=========================================================
    // RESET ANIMATOR
    //=========================================================

    private void ResetAnimator(Animator animator, string triggerName)
    {

        if(animator == null)
            return;



        if(!string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
        }


        //animator.Rebind();
        animator.Update(0f);

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





        // AUDIO CONTROL

        if(step.audioClip != null)
        {

            PlayAudio(step.audioClip, step.loopAudio);



            if(step.loopAudio)
            {

                TriggerAnimation(step);

                CompleteStep();

            }
            else
            {

                StartCoroutine(WaitForAudioFinish(step));

            }

        }
        else
        {

            TriggerAnimation(step);

            CompleteStep();

        }

    }





    //=========================================================
    // WAIT FOR AUDIO FINISH
    //=========================================================

    private IEnumerator WaitForAudioFinish(StepData step)
    {

        while(audioSource != null && audioSource.isPlaying)
        {
            yield return null;
        }


        TriggerAnimation(step);


        CompleteStep();

    }





    //=========================================================
    // TRIGGER ANIMATION
    //=========================================================

    private void TriggerAnimation(StepData step)
    {

        if(step.animator != null &&
           !string.IsNullOrEmpty(step.animationTriggerName))
        {

            Debug.Log(
                "PLAYING ANIMATION : " +
                step.animationTriggerName
            );


            step.animator.SetTrigger(step.animationTriggerName);

        }

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
    // AUDIO PLAY
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
    // PLAY SPECIFIC STEP
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
    // GET CURRENT STEP
    //=========================================================

    public int GetCurrentStep()
    {
        return currentStepIndex;
    }

}