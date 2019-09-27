using System;
using System.Diagnostics;
using System.Speech.Synthesis;

/// <summary>
/// Speak the number of seconds in a minutes - seconds format.
/// </summary>
public class TimeAnnouncer {
    private Tts tts;
    /// <summary>
    /// Time between announcements in seconds.
    /// </summary>
    private double speechGap;
    /// <summary>
    /// The time (in seconds) for the most recent announcement.
    /// </summary>
    private int? lastSpokenTime = null;   
    private int? lastSpokenMinute = null;
    private const int MIN_SEC_BETWEEN_ANNOUNCEMENTS = 1;

    public TimeAnnouncer(double secBetweenAnnouncements = 5.0) {
        tts = new Tts();
        tts.gender = VoiceGender.Female;
        speechGap = secBetweenAnnouncements;
        if (speechGap < MIN_SEC_BETWEEN_ANNOUNCEMENTS) {
            Debug.Write($"Seconds between announcements must be >= {MIN_SEC_BETWEEN_ANNOUNCEMENTS}.");
            speechGap = MIN_SEC_BETWEEN_ANNOUNCEMENTS;
        }
    }

    /// <summary>
    /// Announce the time if the current time is secBetweenAnnouncements or greater from the last time announced. 
    /// To count down from 10 seconds, secBetweenAnnouncements = 1 and the sequence passed to AnnounceIfDelta would be 10, 9, 8...j
    /// To count up, secBetweenAnnouncements = 1 and the currentSec sequence would be 1, 2, ...
    /// AnnounceIfDelta will return after the announcement has been spoken.
    /// </summary>
    /// <param name="currentSec"> The current time. Time can be increasing or decreasing.</param>
    public void AnnounceIfTime(int currentSec) {
        if ((lastSpokenTime.HasValue == false) || ((Math.Abs(currentSec - lastSpokenTime.Value) / speechGap) >= 1.0)) {
            lastSpokenTime = currentSec;
            string announcement = FormatMinSec(currentSec);
            Debug.Write($"\nSpeaking {announcement}.");
            tts.Speak(announcement);
        }
    }

    /// <summary>
    /// Format the number of seconds into minutes and seconds.
    /// When the clock rolls over to a new minute, only the minute will be spoken,
    /// and it will be followed by the word "minutes".
    /// Otherwise only the seconds will be spoken without a following "seconds".
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private string FormatMinSec(int seconds) {
        int spokenMinutes = seconds / 60;
        int spokenSeconds = seconds % 60;
        string announcement = "";

        if (lastSpokenMinute.HasValue == false) {
            lastSpokenMinute = spokenMinutes;
        }

        if (Math.Abs(spokenMinutes-lastSpokenMinute.Value) > 0) {
            // More than a minute since last announced the minute
            lastSpokenMinute = spokenMinutes;
            announcement = $"{spokenMinutes} minute";
            if (spokenMinutes != 1) {
                announcement += "s";
            } 
        }
        else {
            announcement = $"{spokenSeconds}";
        }
        return announcement;
    }
}
