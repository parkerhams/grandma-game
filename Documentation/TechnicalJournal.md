# **Week 1 - 9-6-19 - Starting the project**
This week, we all met and established our plans for the game. We designated official responsibilities and titles and we all started working.

## **Art:**
* Room layout model
* Rocking chair model
* CRT TV prototype model
* Full character sketch and turnaround
* Fully colored character art
![Grandma Concept Art](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype0.jpg)
## **Design:**
* Physical prototype
![Cardboard Prototype](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype1.jpg)
* Dramatic Question worksheet filled out

## **Programming:**
* Used Oculus Plugins to create a custom version of OVR Player Controller
* Deployed builds remotely from PC to Oculus
* Began prototyping cable system by creating a CableSpawner that loads character joint cables at runtime
* Drafted mockups of State Machine for TV and VCR systems in correlation with physical mockups
![Cable References](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype2.jpg) 

# **Week 2 - 9-13-19**

## **Art:**

## **Design:**
* Wrote draft of Grandma dialogue

## **Programming:**
### Cable Behavior
I anticipated cable behavior being the most difficult part of the process, and looked through various possible solutions. Among them are:
- verlet integration-- "a numerical method used to integrate Newton's equations of motion" -- necessary for writing convincingly behaving cables without the use of pre-existing Unity functionality
- Game objects connected in tandem using Unity joints such as character joints, hinge joints, or configurable joints
- pre-made assets available from the Unity Store

Creating cables using verlet integration would be an exceedingly difficult task that would require extensive mathematical knowledge. Given that this is a 7 week project, we knew we needed to manage a reasonable scope, so this solution was not viable. There is a very well made asset available that provides great cable functionality, but using it wouldn't demonstrate our own capabilities, so we decided to just keep that in our back pocket if nothing else pans out. So we got to work on using joints to make cables.
We made the cables by anchoring a sequence of capsules to each other with configurable joints. There are many variables involved in the behavior of joints, such as low and high twist swing limits, bounciness, contact distance, projection distance, mass scale, and break force. We tweaked the values manually to find the most convincing movement to mimic cables, but they still present several problems. When acted upon by multiple forces in separate directions (such as pulling the cable apart with two hands), the capsule nearest to the tension begins to swing wildly in every direction. Similarly, when the cable is plugged in, the neighboring capsule will often times jitter nervously, trying to reach a stabilization point for its anchor that it can't find because its anchor capsule is locked into position in the socket.
These issues aren't severe enough to impact gameplay meaningfully, and the rest of the functionality fits the intended design, so I've decided to hold off on spending time on these issues until we reach the polish phase.
Another issue that is yet to be tackled is the visual of the cable. As it stands, it's a clear enough representation of a cable, but the individual capsules are readily apparent and are less than ideal. Potential solutions included a line or tube renderer stretching across each capsule and mimicking their positions in real time.

# **Week 3 - 9-20-19**

## **Art:**

## **Design:**
* Wrote draft of Grandma dialogue

## **Programming:**
