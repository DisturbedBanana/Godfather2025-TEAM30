using UnityEngine;
using UnityEngine.Audio;

// Gestionnaire de musique et d'effets sonores
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist; // Liste des musiques à jouer
    public AudioSource audioSource; // Source audio utilisée pour lire les musiques
    private int musicIndex = 0; // Index de la musique actuellement jouée dans la playlist

    public AudioMixerGroup soundEffectMixer; // Groupe de mixage pour les effets sonores

    public static AudioManager instance; // Instance unique pour le singleton AudioManager

    private void Awake()
    {
        // Vérifie s'il existe déjà une instance d'AudioManager
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de AudioManager dans la scène");
            return;
        }

        // Définit cette instance comme la seule instance d'AudioManager
        instance = this;
    }

    // Démarre la musique dès le lancement du jeu
    void Start()
    {
        audioSource.clip = playlist[0]; // Charge la première musique de la playlist
        audioSource.Play(); // Joue la musique
    }

    // Vérifie en continu si une musique est terminée pour lancer la suivante
    void Update()
    {
        if (!audioSource.isPlaying) // Si aucune musique n'est en cours de lecture
        {
            PlayNextSong(); // Passe à la musique suivante
        }
    }

    // Passe à la musique suivante dans la playlist
    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % playlist.Length; // Incrémente l'index et revient au début si nécessaire
        audioSource.clip = playlist[musicIndex]; // Charge la musique correspondante
        audioSource.Play(); // Joue la musique
    }

    // Joue un effet sonore à une position spécifique
    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        // Crée un GameObject temporaire pour l'effet sonore
        GameObject tempGO = new GameObject("TempAudio");

        // Définit la position du GameObject à l'endroit où le son doit être joué
        tempGO.transform.position = pos;

        // Ajoute une source audio au GameObject temporaire
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();

        // Associe le clip audio à la source audio
        audioSource.clip = clip;

        // Connecte la source audio au groupe de mixage des effets sonores
        audioSource.outputAudioMixerGroup = soundEffectMixer;

        // Joue le son
        audioSource.Play();

        // Détruit le GameObject temporaire après la fin de la lecture du clip
        Destroy(tempGO, clip.length);

        return audioSource; // Retourne la source audio pour un contrôle optionnel
    }
}
