using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Sound
{
    public class AudioManager : MonoBehaviour
    {
        //The two main audio sources
        public AudioSource SoundEffectAudioSource;
        public AudioSource MusicAudioSource;
        


        //Music
        public AudioClip MenuMusic;
        public AudioClip GameMusicAmbient;
        public AudioClip GameMusicFight;
        public AudioClip GameMusicDanger;
        public AudioClip WinMusic;
        public AudioClip LossMusic;



        // Use this for initialization
        private void Start ()
        {
           
        }
	
        // Update is called once per frame
        private void Update()
        {
            
            PlayMusic();
        }

        /// <summary>
        /// Determine which music track to play
        /// </summary>
        private void PlayMusic()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    switch (BattleManager.Instance.ReturnState())
                    {
                        case BattleState.Battle:
                        
                            if (BattleManager.Instance.ReturnDangerLevel() == DangerLevel.Tension)
                                MusicAudioSource.clip = GameMusicAmbient;
                            else if (BattleManager.Instance.ReturnDangerLevel() == DangerLevel.Fight)
                                MusicAudioSource.clip = GameMusicFight;
                            else if (BattleManager.Instance.ReturnDangerLevel() == DangerLevel.Danger)
                                MusicAudioSource.clip = GameMusicDanger;

                            break;
                        case BattleState.Win:
                            MusicAudioSource.clip = WinMusic;
                            break;
                        case BattleState.Loss:
                            MusicAudioSource.clip = LossMusic;
                            break;
                        case BattleState.ResolutionPlayerOne:
                            MusicAudioSource.clip = WinMusic;
                            break;
                        case BattleState.ResolutionPlayerTwo:
                            MusicAudioSource.clip = WinMusic;
                            break;
                    }
                    break;
                case "Menu":
                    MusicAudioSource.clip = MenuMusic;
                    break;
                case "MapCreatorScene":
                    MusicAudioSource.clip = MenuMusic;
                    break;
            }

            if (MusicAudioSource.isPlaying) return;
            MusicAudioSource.loop = true;
            MusicAudioSource.Play();
        }

        /// <summary>
        /// Set the volume of the music
        /// </summary>
        /// <param name="value"></param>
        public void ChangeMusicVolume(float value)
        {
            MusicAudioSource.volume = value;
        }


        /// <summary>
        /// Set the volume of the sound effects
        /// </summary>
        /// <param name="value"></param>
        public void ChangeSoundVolume(float value)
        {
            SoundEffectAudioSource.volume = value;
        }

        /// <summary>
        /// Set the volume of the sound effects
        /// </summary>
        public float ReturnSoundVolume()
        {
            return SoundEffectAudioSource.volume;
        }

    }
}
