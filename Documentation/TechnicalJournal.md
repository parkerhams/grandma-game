# Week 1
* Used Oculus Plugins to create a custom version of OVR Player Controller
* Deployed builds remotely from PC to Oculus
* Began prototyping cable system by creating a CableSpawner that loads character joint cables at runtime
* Drafted mockups of State Machine for TV and VCR systems in correlation with physical mockups

### Cable Prototyping
Parker, 9-13-19:
Alongside Cole and Justin, our team's lead Designer and Programmer, respectively, we began prototyping Cable Behaviors. We thought that having cable physics integrated into our project, especially in terms of gripping in VR, would be a pain and cause glitching. 
A potential soluton that Cole suggested was that we use Coliision Layers in Unity, so that the cables don't push the player around the scene or "launch them into oblivion," as we saw when trying out character joints that anchored cables together that push the player back with great force if they're separated or stretched.
This was the first foray into the game's mechanics to be spearheaded by Justin, whose approach to functionality and THEN form allowed to prototype quickly together on the Oculus Quest.
Here, we created a simple CableSpawner script (later reworked and changed) to spawn cables in real time. This allowed to see the cables fall onto the ground and, while a minor feature, was really useful in allowing to see how character joints were going to behave. Seeing their physics made it easier for Justin and I to make tweaks tpo their behavior going forward. 

# Week 2
### Cable Behavior
I anticipated cable behavior being the most difficult part of the process, and looked through various possible solutions. Among them are:
- verlet integration-- "a numerical method used to integrate Newton's equations of motion" -- necessary for writing convincingly behaving cables without the use of pre-existing Unity functionality
- Game objects connected in tandem using Unity joints such as character joints, hinge joints, or configurable joints
- pre-made assets available from the Unity Store

Creating cables using verlet integration would be an exceedingly difficult task that would require extensive mathematical knowledge. Given that this is a 7 week project, we knew we needed to manage a reasonable scope, so this solution was not viable. There is a very well made asset available that provides great cable functionality, but using it wouldn't demonstrate our own capabilities, so we decided to just keep that in our back pocket if nothing else pans out. So we got to work on using joints to make cables.
We made the cables by anchoring a sequence of capsules to each other with configurable joints. There are many variables involved in the behavior of joints, such as low and high twist swing limits, bounciness, contact distance, projection distance, mass scale, and break force. We tweaked the values manually to find the most convincing movement to mimic cables, but they still present several problems. When acted upon by multiple forces in separate directions (such as pulling the cable apart with two hands), the capsule nearest to the tension begins to swing wildly in every direction. Similarly, when the cable is plugged in, the neighboring capsule will often times jitter nervously, trying to reach a stabilization point for its anchor that it can't find because its anchor capsule is locked into position in the socket.
These issues aren't severe enough to impact gameplay meaningfully, and the rest of the functionality fits the intended design, so I've decided to hold off on spending time on these issues until we reach the polish phase.

Another issue that is yet to be tackled is the visual of the cable. As it stands, it's a clear enough representation of a cable, but the individual capsules are readily apparent and are less than ideal. Potential solutions included a line or tube renderer stretching across each capsule and mimicking their positions in real time.

# Week 3
### Game Features: Cables, State Machine, Implementation

The focus this week was getting the logic behind what the CRT should emit (video and audio) based on which cables are properly connected. For example, if the power is plugged in and an RCA cable is connected to the VCR's video socket and the CRT's video socket, then the CRT would display the video but no audio. Since there were so many permutations for how the logic should play out, I wrote out a plan for how the code should operate:
```
Both plugs on each end of cable have PlugBehavior script 
Sockets have SocketBehavior script 
Cables have CableBehavior script 
CRT has CRTBehavior script 

When a plug enters a socket, it pulls info from the SocketBehavior to figure out what it is (input on CRT or signal on VCR).
It sends that info to its cable's CableBehavior.
If CableBehavior learns it has an input from one plug and signal from the other plug, it tells the SocketBehavior on the input socket
what kind of signal is being provided. 
CRTBehavior listens to these input sockets to decide if any actions need to occur based on what signal they're getting and what socket
they are. 

 

This means that each plug only talks to its parent cable and the socket it's in, the parent cable only receives info from its plugs and
tells the input socket about the signal provided, and the CRT only knows the status of the input sockets (and the CRT's buttons). Each
time an input socket's signal status changes (gained or lost a signal), it tells the CRT to run through its possible outcomes and pick
the correct one. 

Other notes: 
The power plug is special, and is already plugged into the Power Input and can't be unplugged from it. Its only interaction is plugging the other side into Power Signal (wall socket). 
Power button is either on or off. 
Channel button cycles the CRT through 3 modes: Input, Channel 1, Channel 2. 

Example: 
CRT is told that the power button is pressed in. 
CRT is told that the channel button is on Input Mode. 
CRT is told that Power Input is receiving Power Signal. 
CRT is told that Left Audio Input is receiving Left Audio Signal. 
CRT is told that Right Audio Input is receiving Video Signal. 
CRT is told that Video Input is receiving nothing. 
With this information, CRT runs through its possible outcomes and decides to play cryptic audio. This is because although left audio is plugged in correctly, video signal is plugged into right audio socket, which provides the "cryptic audio" outcome and is a higher priority outcome. 

Possible "error" cases where things are plugged in incorrectly, but the CRT provides a result: 
- If you plug a Video signal into an Audio Input, the television plays cryptic audio 
- if you plug an Audio signal into a Video Input, the video plays static  
- if you switch the left and right audio signals, the audio plays BACKWARDS  
- if video is connected but not audio, the video is muted
- if the audio is connected but no video, the sound plays without visuals 

```

This plan helped me actually design the logic in more ways than one; in addition to being able to reference it and keep a consistent design (and share that design with team members), I also ended up rewriting pieces of the plan several times before I ever began implementing it. This is because being able to see the process mapped out in the plan helped me recognize flaws in the logic and rectify them before I ever had to spend time creating them. As a result, once I actually designed the system in code, it ran exactly as intended on the first attempt!
