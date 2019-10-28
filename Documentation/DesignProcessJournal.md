# **Week 1 - 9-6-19 - Starting the project**
This week, we all met and established our plans for the game. We designated official responsibilities and titles and we all started working.

## **Art:**
* Room layout model
* Rocking chair model
* CRT TV prototype model
* Full character sketch and turnaround
* Fully colored character art
![Grandma Concept Art](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype0.jpg)

### Room Layout
  Both environment artists worked on different ideas of what the layout of the room should be before merging their ideas into one cohesive room.
 ![Room Layout](https://github.com/parkerhams/grandma-game/blob/3ce0a283bb38b5345974e38ba9895a664a5d0853/Documentation/images/InteriorConcept.png)

## **Design:**
* Physical prototype
![Cardboard Prototype](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype1.jpg)
* Dramatic Question worksheet filled out
* Drafted mockups of State Machine for TV and VCR systems in correlation with physical mockups
![Cable References](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/physicalPrototype2.jpg) 

# **Week 2 - 9-13-19 - Core assets and prototyping**

## **Art:**
### Grandma Model
  The grandma base mesh was completed and UV unwrapped. Basic textures will be apllied and a Rig should be put on the model. By next week, a fully textured grandma posed in her rocking chair will be completed a ready to be put in game.
![Grandma Base Mesh](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/GrandmaBaseMesh.JPG)
![Grandma Base Mesh Frontal View](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/GrandmaBaseMeshFront.JPG)
![Grandma UV Map](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/GrandmaUV.png)

### Environment
  Cable heads we worked on, as well as the base tv model.
![Cable Heads](https://github.com/parkerhams/grandma-game/blob/db8ef396abc433d19d24eb62ec3d3c25cabbfe30/cables01.png)

## **Design:**
### CRT & VCR Prototyping
The core loop of the game focuses closely on player interaction with the CRT TV and VCR devices. This presents an interesting challenge because we're trying to both emulate the behavior of a real-world object but also simplify it for clumsy VR hands.

Our solution to this problem is to maintain the core functionality of the device between reality to the model so that it acts in a predictable and recognizeable manner, while also simplifying the scope and complexity of its actions. 

For example, the model CRT TV still requires power to function, has to be turned on before it can be adjusted, can switch between a number of channels, and otherwise acts the same as a true-to-life CRT TV. However, the scope of its actions is far simplified. The array of buttons available has been cut down to just three: power, channel up, and channel down. Similarly, only three channels are available to flip through. This combination of functional emulation and scope simplification results in a happy medium that can be understood by players with or without preexisting knowledge of how a CRT television functions.

Armed with this goal and an early model for the Television, we prototyped the device's behavioral system as well as its simplified buttons and sockets.

![CRT Prototype 1](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/CRT%20Prototype%201.JPG)
![CRT Prototype 2](https://github.com/parkerhams/grandma-game/blob/master/Documentation/images/CRT%20Prototype%202.JPG)

### Drafting Dialogue for Grandma
The dialogue document was split into three sections: Instructional Dialogue, Prompted Dialogue, and Unprompted Dialogue. The instructional dialogue is dialogue that tells the player to do something, like "Plug in the TV." The prompted dialogue is dialogue that is prompted by something the player does, like "Oh, you turned on the TV." The unprompted dialogue is dialogue that happens regardless of what the player is doing, like "Do you wanna make cookies later?" I wrote a bunch of dialogue barks in all of these sections, to be edited, deleted, or added to as we progress.

## **Programming:**
### Cable Behavior
There were three options available for implementing cables and their behavior: using pre-made Unity Store assets, using verlet integration, which is an exceedingly complex method for creating original cable functionality, or using capsule-shaped game objects connected in Unity with joints like character, hinge, or configurable joints. It was decided that due to the scope and timeframe of this project, verlet integration was not feasible. There is a very well-made asset on the Unity Store that provides great cable functionality, but using a premade asset wouldn't exactly demonstrate our skills very well, seeing as this is a class project. With this in mind, we decided to keep the premade asset in mind as a last resort and create cables out of capsules and joints. 

We made the cables by anchoring a sequence of capsules to each other with configurable joints. There are many variables involved in the behavior of joints, such as low and high twist swing limits, bounciness, contact distance, projection distance, mass scale, and break force. We tweaked the values manually to find the most convincing movement to mimic cables, but they still present several problems. When acted upon by multiple forces in separate directions (such as pulling the cable apart with two hands), the capsule nearest to the tension begins to swing wildly in every direction. Similarly, when the cable is plugged in, the neighboring capsule will often times jitter nervously, trying to reach a stabilization point for its anchor that it can't find because its anchor capsule is locked into position in the socket. These issues aren't severe enough to impact gameplay meaningfully, and the rest of the functionality fits the intended design, so we've decided to hold off on spending time on these issues until we reach the polish phase.

# **Week 3 - 9-20-19 - State machine, cables, and implementing other stuff**

## **Art:**

## **Design:**

## **Programming:**
### Configuring Cable Output
The goal for this week was configuring the behavior of the TV depending on what was plugged in and what wasn't. Since this logic depends on a huge number of variables, a plan was drawn up for how the code should operate. For more details, please visit the Week 3 entry of the [Technical Journal.](https://github.com/parkerhams/grandma-game/blob/master/Documentation/TechnicalJournal.md)

# **Week 4 - 9-27-19 - Modeling and coding everything, forever**

## **Art:**

## **Design:**

## **Programming:**
Alongside many general fixes such as getting the various sockets and cable plugs lined up, adding walls and lighting, and importing art asses, this week was focused on button functionality. They're a simple concept, but required plenty of work to feel right: as soon as the player's hand touches the button, it needs to provide feedback by pushing inwards and then pulling out, letting the player know that the button was pushed even if the CRT isn't plugged in yet. Additionally, the power button on the CRT and VCR function differently: once pushed in, they lock into place. They only push back out when the player presses the button again. Coupled with some changes to the internal logic of the CRT and VCR, this means that players will always know that power is "on" when the button is pushed in, rather than potentially swapping state meanings if the player pushes the button when the system isn't hooked up yet.

# **Week 5 - 10-4-19 - Getting ready for alpha!!** 

## **Art:**

## **Design:**

## **Programming:**
### VHS System
This week was about implementing the VCR and allowing it to accept and eject VHS, and to play the appropriate video when ready. The VCR was given a trigger collider in front of its cover piece; when an acceptable object (VHS) is received, it locks into position in front of the VCR and slowly inserts itself, ungrabbing from the player's hand. It resides inside the VCR until the player waves their hand in front of the cover, then ejects the same as it entered. Because of how it's designed, it can accept any object at all as long as we designate that object as such. This means we could, for example, let players stuff a banana into the VCR and have some sort of banana video play on the CRT. Make sense? No! Sound fun? Yes!

### Dialogue System
The dialogue system has proven to be a difficult beast to implement. Several methods were tried over an extended prototyping session by Parker and Dan. Using Unity event systems and having certain events trigger dialogue barks (ie "The TV is on!" when you plug in the TV) proved to be much more complicated than initially thought, and it was decided late in the night to just use UI elemnts like a canvas and text as a placeholder for the alpha build and that a functional dialogue system would have to wait for a time when a group of us could work on it together.

# **Week 6 - 10-11-19 - Playtesting, logging bugs, and making plans**

[Please see our playtesting document for our log!](https://github.com/parkerhams/grandma-game/new/master/Documentation)
## **Programming:**
After seeing players test the game, we came away with a lot of necessary changes to improve its usability:

- Added a highlight to grabbable objects when the player's hand is pointing at them, both so that the player knows they can grab from a distance, and so that the player knows exactly what's going to be grabbed when they push the trigger.
- Added an obstruction layer to the TV stand and CRT, which prevents objects from being distance grabbed through them. This prevents players from accidentally pulling out a cable from the other side of the TV stand.
- Removed player collision with most objects and positioned the player directly behind the TV stand. This allows players to get as close as they need to in order to plug in cables, and being positioned here lets them see the unplugged sockets on both devices as well as grandma herself, making it clear what they should do without having to explicitly tell them anything.
- Revised the cable tension behavior, causing any currently grabbed cable to instruct each capsule of the cable to constantly check the distance from its neighbors. If any capsule gets too far from its neighbors, the cable releases itself from the player's hands, preventing them from pulling the cable apart. In addition, the capsules attached to the plugs become kinematic when plugged in, preventing accidental unplugs from tugging on any other piece of the cable. Players can now instead instantly unplug a cable by directly grabbing that plug capsule.

# **Week 7 - 10-18-19 - Going ham on the dialogue system, and world-filling models**

## **Art:**
### Video Tapes and Minor Assets
With the game almost finished, we only had a few more models to make. These included the pretty essential VHS tapes and their sleeves. Since the tapes themselves will just be clones of each other with different data attached, we only needed one model that we could implement multiple times. We also started work on other assets to just fill in the world so it feels more homey and less like an empty cell. 

### Rigging Grandma
Thorne I don't know anything about rigging, please write some stuff lol

## **Design:**

## **Programming:**

We made the choice to implement the PixelCrushers dialogue system in order to properly support dialogue. 

# **Week 8 - 10-25-19 - Finally getting the dialogue system to work!!**

## **Art:**

Our artists have been working on world-filling and quality-of-life art this week. The floor lamp is almost done and the radio is being worked on, and we added two kid-style drawings to be framed and put on the wall. 

## **Design:**

## **Programming:**



