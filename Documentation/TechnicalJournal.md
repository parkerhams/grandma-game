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

