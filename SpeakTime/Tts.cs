using System.Speech.Synthesis;
using System.Diagnostics;

/// <summary>
///  Provides Text to Speech using the .Net Speech.Synthesis module.
///  See following URL for more info.
///  https://www.codeproject.com/Articles/28725/A-Very-Easy-Introduction-to-Microsoft-NET-Speech-S
/// </summary>
public class Tts
{
    /// <summary>
    /// The gender that will be use for the voice 
    /// </summary>
    public VoiceGender gender { get; set; } = VoiceGender.Male;

    /// <summary>
    /// Use .NET text to speech to speak the string. 
    /// This routine will return after the string has been spoken.
    /// </summary>
    /// <param name="announcement">The string to speak.</param>
    public void Speak(string announcement) {
        // System.Speech.Synthesis.SpeechSynthesizer has a memory leak, so Dispose it after each use.
        using (var synth = new SpeechSynthesizer()) {
            synth.SelectVoiceByHints(gender);
            synth.Speak(announcement);
        }
    }
}
