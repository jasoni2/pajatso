# TODO list

## Physics

- Re-visit ball movement physics
    - Right now we use default Unity physics logic, but tweaking the settings is an absolute pain and it is incredibly difficult to get a setup that feels good.
    - Disabling ball rotation causes the ball to maintain momentum during bounces much better, but easily causes infinite-bounces. Enabling it causes random speed-ups, speed-downs, and angle changes that feel unnatural and bad.
    - Collisions appear to happen VERY far in advance, causing the ball to appear to bounce up without even contacting the bumpers.
    - Might need to switch the ball to a kinematic Rigidbody where we control the velocity, speed, and can have better control over slight variations to keep every launch feeling good and avoid infinite bounces.

- We should add some sort of collision tracker. We'll need this for scores and etc. anyway.
    - Perhaps we can have a scriptable object for this? and we push each collision in there, so we can debug visualise it as well as count them properly.

## Core

- Input needs to be handled properly using Input Actions, not hardcoded to the old API's
- Implement scriptable objects for tracking game state
- Implement scoring
- Figure out gameplay loop flow when score is introduced
- Implement game event dispatcher for better communication between things
    - Alternatively, diff elements (UI) completely stateless but disabled/enabled by an element manager that listens to game events??

## Levels

- Do we want different levels?
- Right now the level build process is VERY manual: each bumper is hand placed, the camera distance is hand placed, etc... very error prone. Should figure out a better system.

## Metagame

- Scaffold player progression
- Scaffold scriptable objects for different types of balls
- Scaffold scriptable objects for bumpers and talents

## UI

- Scaffold gameplay UI