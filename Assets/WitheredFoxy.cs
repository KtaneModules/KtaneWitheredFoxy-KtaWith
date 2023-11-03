using System.Collections;
using UnityEngine;

public class WitheredFoxy : MonoBehaviour {
       
    public KMNeedyModule needy;
    public KMBombInfo bomb;
    public KMAudio audio;
    
    public Material[] m_foxy;
    public MeshRenderer screen;
    
    private KeyCode[] ControlKeys = {KeyCode.LeftControl, KeyCode.RightControl};

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved = true;
    
    private int flashes = 0;
    private bool lightOn = false;
    private bool foxyOn = false;
    
    private KMAudio.KMAudioRef ambref;

    private void Awake () {
        moduleId = moduleIdCounter++;
        needy.OnNeedyActivation += OnNeedyActivation;
        needy.OnTimerExpired += OnTimerExpired;
        
        needy.OnActivate += delegate { audio.PlaySoundAtTransform("Start", transform); };
    }

    private void OnNeedyActivation () {
        moduleSolved = false;
        flashes = 0;
        foxyOn = true;
        
        ambref = audio.PlaySoundAtTransformWithRef("Ambience", transform);
    }

    private void OnTimerExpired () {
        Reset();
        audio.PlaySoundAtTransform("Jumpscare", transform);
        needy.HandleStrike();
        Flash();
    }
    
    private void Reset() {
        ambref.StopSound();
        flashes = 0;
        foxyOn = false;
        moduleSolved = true;
    }
    
    private void Update () {
        if (Input.GetKeyDown(ControlKeys[0]))
            Flash();
    }
    
    private void Flash () {
        screen.sharedMaterial = m_foxy[1];
        if (!lightOn) {
            flashes++;
            if (flashes > 19 && foxyOn) {
                Reset();
                needy.HandlePass();
                audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
            }
            screen.sharedMaterial = m_foxy[2];
            if (foxyOn) screen.sharedMaterial = m_foxy[0];
        }
        lightOn = !lightOn;
    }

    // Twitch Plays
#pragma warning disable 414
    private static readonly string TwitchHelpMessage = @"!{0} flash [Hello! Hello! Do !# flash to flash Withered Foxy]";
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command) {
        if (command.Trim().ToUpperInvariant() != "FLASH")
            yield return "sendtochaterror The flashlight battery died, there’s nothing in the hallway.";
        yield return null;
        for (int i = 0; i < 40; i++) {
            Flash();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}