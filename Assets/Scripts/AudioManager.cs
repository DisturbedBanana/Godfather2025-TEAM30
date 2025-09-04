using UnityEngine;
using UnityEngine.Audio;

// Gestionnaire de musique et d'effets sonores
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist; // Liste des musiques � jouer
    public AudioSource audioSource; // Source audio utilis�e pour lire les musiques
    private int musicIndex = 0; // Index de la musique actuellement jou�e dans la playlist

    public AudioMixerGroup soundEffectMixer; // Groupe de mixage pour les effets sonores

    public static AudioManager instance; // Instance unique pour le singleton AudioManager

    private void Awake()
    {
        // V�rifie s'il existe d�j� une instance d'AudioManager
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de AudioManager dans la sc�ne");
            return;
        }

        // D�finit cette instance comme la seule instance d'AudioManager
        instance = this;
    }

    // D�marre la musique d�s le lancement du jeu
    void Start()
    {
        audioSource.clip = playlist[0]; // Charge la premi�re musique de la playlist
        audioSource.Play(); // Joue la musique
    }

    // V�rifie en continu si une musique est termin�e pour lancer la suivante
    void Update()
    {
        if (!audioSource.isPlaying) // Si aucune musique n'est en cours de lecture
        {
            PlayNextSong(); // Passe � la musique suivante
        }
    }

    // Passe � la musique suivante dans la playlist
    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % playlist.Length; // Incr�mente l'index et revient au d�but si n�cessaire
        audioSource.clip = playlist[musicIndex]; // Charge la musique correspondante
        audioSource.Play(); // Joue la musique
    }

    // Joue un effet sonore � une position sp�cifique
    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        // Cr�e un GameObject temporaire pour l'effet sonore
        GameObject tempGO = new GameObject("TempAudio");

        // D�finit la position du GameObject � l'endroit o� le son doit �tre jou�
        tempGO.transform.position = pos;

        // Ajoute une source audio au GameObject temporaire
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();

        // Associe le clip audio � la source audio
        audioSource.clip = clip;

        // Connecte la source audio au groupe de mixage des effets sonores
        audioSource.outputAudioMixerGroup = soundEffectMixer;

        // Joue le son
        audioSource.Play();

        // D�truit le GameObject temporaire apr�s la fin de la lecture du clip
        Destroy(tempGO, clip.length);

        return audioSource; // Retourne la source audio pour un contr�le optionnel
    }
}
