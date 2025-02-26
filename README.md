## Project Description
Since the development of modern racing games, considerable progress has been made in the intelligence of computer opponents against players, and there have been many attempts. The human-machine confrontation in the game is becoming more and more real and the gameplay is getting stronger. How can non-player-controlled cars avoid the other cars on the track, the track fence, reach the finish line as fast as possible, and even interfere with the player's car through driving skills? It is the most complex and core thing. The challenge is that NPC racing should be able to perform high-speed real-time calculations and predictions for the road conditions, and let the car react appropriately through analysis and decision making. We extracted this core processing and computational problem, mapping it to a simple racing game using Unity, applying certain analytical decision algorithms, and finally enabling the car to drive automatically on the game's track.


In terms of calculations and algorithms, there are many ways to implement such automatic driving methods, but in the past three or four years, the effect is quite good. The more popular method is to use the generation of confrontation network simulation learning developed by convolutional neural networks, with practical problems. Some other auxiliary means to deal with.


This project focuses on verifying the feasibility and practical effect of the GAN algorithm applied to autonomous driving. As an undergraduate graduation project, the so-called autopilot is achieved to the extent that the car can be automatically controlled by the computer, along the track, to avoid collisions. The walls on both sides of the track should also try to avoid colliding with other vehicles on the track by slowing down and changing lanes.


This project uses an imitation learning algorithm based on generating an anti-network to implement the autopilot function. In order to reflect better learning performance and accelerate learning, it also considers other auxiliary learning and provides some human driving. The data is pre-trained, which should result in better generalization performance with less data.
