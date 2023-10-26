using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitheredFoxy : MonoBehaviour {
	   
	public KMNeedyModule needy;
	public KMBombInfo bomb;
	public KMAudio audio;
	
	public Material[] m_foxy;
	public MeshRenderer screen;
	
	private KeyCode[] ControlKeys = {KeyCode.LeftControl, KeyCode.RightControl};

	static int moduleIdCounter = 1;
	int moduleId;
	private bool moduleSolved;
	
	private int flashes = 0;
	private bool lightOn = false;
	private bool foxyOn = false;
	
	private KMAudio.KMAudioRef ambref;

	void Awake () {
		moduleId = moduleIdCounter++;
		needy.OnNeedyActivation += OnNeedyActivation;
		needy.OnTimerExpired += OnTimerExpired;
		
		needy.OnActivate += delegate { audio.PlaySoundAtTransform("Start", transform); };
	}
	
	void Start () {
		moduleSolved = true;
	}

	void OnNeedyActivation () {
		moduleSolved = false;
		flashes = 0;
		foxyOn = true;
		
		ambref = audio.PlaySoundAtTransformWithRef("Ambience", transform);
	}

	void OnTimerExpired () {
		Reset();
		audio.PlaySoundAtTransform("Jumpscare", transform);
		needy.HandleStrike();
		Flash();
	}
	
	void Reset() {
		ambref.StopSound();
		flashes = 0;
		foxyOn = false;
		moduleSolved = true;
	}
	
	void Update () {
		for (int i = 0; i < 1; i++) {
                if (Input.GetKeyDown(ControlKeys[i])) {
                    Flash();
                }
            }
	}
	
	void Flash () {
		screen.sharedMaterial = m_foxy[1];
		if (!lightOn) {
			flashes++;
			if (flashes > 19 && foxyOn) {
				Reset();
				needy.HandlePass();
			}
			screen.sharedMaterial = m_foxy[2];
			if (foxyOn) screen.sharedMaterial = m_foxy[0];
		}
		lightOn = !lightOn;
	}
}
