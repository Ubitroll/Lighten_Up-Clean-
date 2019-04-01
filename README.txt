- Note that box colliders are used.
- Colliders, scripts and tags attached to flamable objects are attached to cubes in the models.
- Objects have weird origin positions - rotating makes the rotation like on a large circle, fire starts somewhere else (far away from the actual object).
- Changed how the steam effect works. Not a coroutine anymore. Instantiated and it destroys itself after 1.2s (particle duration option)
- Ammunition for water gun works properly now.
- Cleaned the code.

- Added house health bar and attached a script to empty game object "HouseHealthObject"
- Added ThrowObjectScript and Rigidbody to objects that are small - currently to armchair, coffeetable, Chair, Chair (1), Chair (2), Chair (3), TV
- Changed input buttons to support controllers. 
	HUMAN:  RB and LB - switch through weapons, 
		B - action (pick up objects, throw objects, shoot)
		X - refill weapon or leave picked up object 
		Y - reload gun
- The player can only pick up an item if he has no weapon selected
- Added tags Human and Candle for the players
- Disabled candle movement when it's burning objects quicker
- Animation types changed from Generic to Legacy
- Added animations

- Added fire particle effect to the candle
- Updated fire particle effect - now it will cover the whole object
- Due to some kind of 'weird' issues with the fire particle effect it MUST be assigned in the inspector!
- The steam particle effect needs to be assigned in the inspector as well
- Major fixes with raycasting
- Extinguish and fire bar are now treated as game objects and not images

- ThrowObjectScript has been updated - there's no longer need to assign the object holder, player and player cam in the inspector