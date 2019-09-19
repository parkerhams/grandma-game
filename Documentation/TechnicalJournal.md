# Programming
### Cable Behavior
I anticipated cable behavior being the most difficult part of the process, and looked through various possible solutions. Among them are:
- verlet integration-- "a numerical method used to integrate Newton's equations of motion" -- necessary for writing convincingly behaving cables without the use of pre-existing Unity functionality
- Game objects connected in tandem using Unity joints such as character joints, hinge joints, or configurable joints
- pre-made assets available from the Unity Store

Creating cables using verlet integration would be an exceedingly difficult task that would require extensive mathematical knowledge, so that was out. There is a very well made asset available that provides great cable functionality, but it wouldn't be much of a portfolio piece if we used it, so we decided to just keep that in our back pocket if nothing else pans out. So we got to work on using joints to make cables.
We made the cables by anchoring a sequence of capsules to each other with configurable joints. There are many variables involved in the behavior of joints, such as low and high twist swing limits, bounciness, contact distance, projection distance, mass scale, and break force. We tweaked the values manually to find the most convincing movement to mimic cables, but they still present several problems. When acted upon by multiple forces in separate directions (such as pulling the cable apart with two hands), the capsule nearest to the tension begins to swing wildly in every direction. Similarly, when the cable is plugged in, the neighboring capsule will often times jitter nervously, trying to reach a stabilization point for its anchor that it can't find because its anchor capsule is locked into position in the socket.
These issues aren't severe enough to impact gameplay meaningfully, and the rest of the functionality fits the intended design, so I've decided to hold off on spending time on these issues until we reach the polish phase.

# Art

# Design
