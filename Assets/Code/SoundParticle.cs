using UnityEngine;
using UnityEngine.Audio;

public class SoundParticle : MonoBehaviour {
    private static readonly float epsilon = .1f;
    [SerializeField] private AudioMixerGroup mixerGroup = null;
    
    public void OneShot(AudioClip clip) {
        var go = new GameObject("SoundParticle");
        var source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();
        Destroy(go, clip.length + epsilon);
    }
}