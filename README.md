# M3CH PLANET

## Getting Started
Preparing the Image Target
This game supports up to 4 people at maximum. Before starting the game, all players should have the following image targets at hand (found in the README_Assets folder):

### Battle Arena Image Target

![Battle Arena Image Target](README_Assets/battlearena.png)

This image target allows the application to load the battle arena for the players. Ensure that this image target is laid on a flat surface and that players can either walk completely around it or lean over on each side. A small table would suffice. 

### Wand Image Target

![Wand Image Target](README_Assets/wand.png)

This image target allows the player to select power ups. They should hold this in their dominant hand and the mobile phone in the other.

An example setup should look like the following:

![Ideal Setup Picture](README_Assets/ideal_setup.jpg)

### Hosting and Joining a Game
Upon launching the application, the user will be given the main menu screen. They will have the options of either creating a game, joining a game, or editing their profile.

[ screenshot of menu ]

### Edit Player Profile
After clicking “My Profile”, the user will be directed to a screen where they can change the name of their profile. The name entered here will be the same name that is shown above their virtual avatar when they are starting and playing in a game.

![Profile Screen](README_Assets/profile.jpg)

### Hosting a Game
After pressing “Host Game”,  the user will then be directed to the next screen in which they can enter the name of the game. After entering the desired name of their game, they can press the button “Create Game”. 

![Host Game Screen](README_Assets/createroom.jpg)

Afterwards, the user will be directed to the waiting room. Here, the user needs to point the camera at the battle arena image target to start tracking it. After doing so, a virtual avatar with the player’s specified profile name and the Not Ready status will drop down and land on the image target, as seen below.

![Waiting Room](README_Assets/waitingroom.jpg)
 
### Waiting Room 
Joining a Game
From the starting menu, the user can also press “Join Game” to be directed to the following screen:

![Join Room](README_Assets/joinroom.jpg)

Here, the user will see a list of the available games created by other users who may have the app loaded and have chosen to host a game. If there is an available game with space, they can tap on the game they’d like to join and be immediately directed to the waiting room (as seen above). 
Starting the Match
When a user first joins a waiting room and is waiting for the game to start, their status is indicated as Not Ready. They can press the “Toggle Ready” button to toggle their status as needed. When all players in the room are ready, the game automatically starts and the battle arena (i.e. the spherical planet) is loaded with a particular orientation so that the respective avatar of the user holding the phone is shown on top of the planet. In other words, each player in the game will see their own avatar at the top of the planet (egocentric perspective) because each planet will be oriented differently on each of their screens. The positions of each player, however, is tracked and updated in real-time such that players can explore the planet to see the position of other players relative to their own position. 

## Game Mechanics 

### Turn-Based Gameplay
Once the game has started, it will proceed in turn-based gameplay. When it is the player’s turn, they will see and be able to press three different buttons - “End Turn”, “Walk”, and “Attack”. When it is not the player’s turn, they will not be able to see these buttons but will instead see a button “ [ insert name ] “ that allows the player to toggle between their own perspective and the perspective of the player whose turn it is. In this case, the perspective of a player means that the planet is oriented such that the respective virtual avatar is residing at the top of the planet. 

### Walk
When the user holds the “Walk” button, the avatar will walk forward in the direction they are currently facing. To change the direction of the avatar, the user must physically move to the appropriate side of the planet and face towards that direction with their phone. For example, to walk to the user’s left, the user must walk to the right side of the planet and point their phone towards the left side of the planet. Then, the user must hold the “Walk” button in order for their avatar to move forward.

### Attack
When the user holds the “Attack” button, they are now able to throw grenades to do damage to other players. In this mode, they should then use the wand in their other hand as a bow-and-arrow to direct the magnitude and direction of their grenade throw. Here, the direction and magnitude of the grenade throw is mapped directly to the direction that the red line (i.e. the “arrow”) points to and its length. The red line originates from the player’s position, as shown below:

![Attack Interaction](README_Assets/arrow.jpg)

As an example, if the user moves the wand to their right, the red line will point towards the left. If the user pulls their wand back, the red line will lengthen to indicate that the strength of the throw is increased. 

Once the user releases the “Attack” button, the grenade is then immediately thrown and the user’s turn is automatically ended.  
### End Turn
When the user presses “End Turn”, their turn is immediately ended and it progresses to the turn of the next user. 

