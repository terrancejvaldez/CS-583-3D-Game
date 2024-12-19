# CS-583-3D-Game 
Game Name: 3 Point Shootout
https://drive.google.com/file/d/1rmwXzp0ik2DjDTbcSfOgCvEMVHhkoA0K/view?usp=sharing

Group Members: Daniel Ha, Michael Zalameda, Jett Morgan, Alan Yin, Ethan Fox, Terrance Valdez

Target: Design a game where you control a player performing a 3 point contest. With a system that will emulate real life basketball physics and keeps track of how many 3 pointers you make in a time frame, showing the results in the end. We potentially might try to add more mechanics / gamemodes but for now limiting the scope to easily feasible.

Daniel Ha: Implement a fixed player shooting stance for each rack. Set rack positions at the right corner, right wing, top of the key, left wing, and left corner. Design or source racks to hold 5 basketballs. Added camera bobbing to simulate walking/running. Imported commercial boards and stage lights with the help of post process effects to emulate bright glow.

Michael Zalameda: Animate the player’s shooting motions. Smooth transitions when moving to the next rack. Add idle and celebratory animations for immersion.

Jett Morgan: Track and display scores in real-time based on 1-point normal balls and 2-point money balls. Add a countdown timer visible to the player. Display instructions and results at the start and end of the game.

Alan Yin: Create 2 new game modes that are based on the original game. One as a free shooting basketball game where there is not a timer and only count your points. The other as only one ball and create an enemy which chase the ball and after it has been within a certain radius of the ball for a short time, you lose. Create a pause menu and a main menu which player can use to choose the game they want to go to or exit game, and use the pause menu in case they want to go back to main menu.

Ethan Fox Goals: Implement controls for shooting, including aiming and shot power adjustment. Add realistic physics-based basketball trajectories. Ensure smooth transition from one rack to the next after completing 5 shots. Generally been working towards getting the game to be in a "prototype version", mainly working on core functions such as ball physics (bled into ball handling), colliders with the hoops, and very basic audio mechanics for playtesting. 

Terrance Valdez: Model or source a realistic basketball court with 3-point line dimensions. Optimize lighting and camera angles for a professional view of the action. Program automatic movement to the next rack after completing 5 shots.
      

