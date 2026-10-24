## State machines

Game states: GameManager.cs - playing, paused, gameover
Character states: PlayerController.cs - idle, moving, hurt

How it changes:
playing -> paused when escape is pressed, paused -> playing when escape is pressed again, playing -> gameover when lives reach zero, idle -> moving when a, d, or arrow keys are pressed, moving -> idle when movement stops, idle/moving -> hurt when a life is lost, hurt -> idle after 0.5 seconds

Event that changes a state:
LivesChanged makes PlayerController change to the hurt state

## Help I used
copilot:
helped me script and debug the players state machines in PlayerController

video:
https://www.youtube.com/watch?v=-jkT4oFi1vk