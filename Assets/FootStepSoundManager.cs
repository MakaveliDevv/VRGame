namespace UnityEngine.XR.Interaction.Toolkit
{
    public class FootstepSoundManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] footstepSounds; // Array to hold footstep sound clips
        private AudioSource audioSource; // Audio source to play the sound

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing. Please add one to the GameObject.");
            }
        }

        public void PlayRandomFootstepSound()
        {
            if (footstepSounds.Length == 0 || audioSource == null)
                return;

            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }
}
