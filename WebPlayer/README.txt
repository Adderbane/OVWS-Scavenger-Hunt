My Project is a simple chase the sphere game, using standard FPS controls (hold shift to move fast).
I used a similar method to the PlayerSyncPosition to move the sphere around.  The method to collide with the sphere is
called on the client, which checks that it collided with a player object, then sends a command to the server.  The command
takes care of moving the sphere to a new valid position and updates the syncvar.
I fully integrated the scrolling chatbox into the project, so users may communicate fully.  Appending the username
to the message at the client stage of the process fixed the problem I had earlier (I was appending it after the message went to the
server, so naturally it grabbed the host's username).
By increasing the lerp rates significantly, I ensure that the apparent positions of the player are accurate reflections of where they are on the server.
With the low rates before, the program took far too long to lerp through the positions, resulting in huge lag as it waited to get through the intermediate positions.
I wanted to sync and display the name of the player that last collided with the sphere, and also give them a permanent run speed boost as a reward.
However, I wasn't able to make this work, and the relevant code is commented out since it was crashing the game.
I was able to display text above each player, but didn't have enough time to figure out how to set it to their username or change their model's color.
