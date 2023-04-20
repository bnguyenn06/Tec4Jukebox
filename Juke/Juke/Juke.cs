using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Juke
{
    public partial class Juke : Form
    {
        // Create a new instance of the MediaPlayer class
        MediaPlayer player = new MediaPlayer();
        private bool isPlaying = false;

        private static string directoryPath = @"C:\Users\larry\source\repos\Juke\Juke\All"; // Replace with your directory path
        private string[] audioFiles = Directory.GetFiles(directoryPath, "*.mp3"); // Get all MP3 files in the directory

        int currentAudioIndex = 0; // Keep track of the current audio file being played



        // Play the current audio file
        void PlayCurrentAudio()
        {
            if (currentAudioIndex < audioFiles.Length)
            {
                player.Open(new Uri(audioFiles[currentAudioIndex])); // Open the audio file
                player.Play(); // Play the audio file
                               // Subscribe to the MediaEnded event of the MediaPlayer instance to automatically play the next song
                player.MediaEnded += (sender, e) => PlayNextAudio();
                string fileName = Path.GetFileName(audioFiles[currentAudioIndex]); // Get the file name without the path
                lblCurrentSong.Text = "Current Song: " + fileName;
              
            }
        }

        public  void PlayNextAudio()
        {
            if (currentAudioIndex < audioFiles.Length - 1) // Make sure we are not at the end of the list
            {
                currentAudioIndex++; // Move to the next audio file
                PlayCurrentAudio(); // Play the new audio file
            }
        }

        public Juke()
        {
            InitializeComponent();

            // Retrieve the last saved volume level from the application settings
            int savedVolume = Properties.Settings.Default.VolumeLevel;

            // Set the volume slider's value to the last saved volume level
            volumeBar.Value = savedVolume;

            lblVolume.Text = $"{savedVolume}%";

            // Set up event handler for the volume slider
            volumeBar.ValueChanged += (sender, e) => UpdateVolumeLevel(volumeBar.Value);

        }


        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                player.Pause();
                //mediaPlayer.Pause();
                isPlaying = false;
                btnPlay.Text= "Play";
                
            }
            else
            {
                PlayCurrentAudio();
                //mediaPlayer.Play();
                isPlaying = true;
                btnPlay.Text = "Pause";

            }
        }
        // Update the volume level of the MediaPlayer instance
        public void UpdateVolumeLevel(int volume)
        {
            player.Volume = volume / 100.0;
            lblVolume.Text = $"{volume}%";

            // Save the new volume level to the application settings
            Properties.Settings.Default.VolumeLevel = volume;
            Properties.Settings.Default.Save();
        }
        

       
    }
}
