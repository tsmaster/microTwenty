# microTwenty
A content-minimal, but feature-rich Computer Role Playing Game in the world of The Twenty

WIP "playable" at http://bigdicegames.com/TheTwenty/MicroTwenty/index.html

Current features
- walking around an "overland" map of 6 separate "episode" regions
- transport from episode to episode
- entering in to cities
- hex grid movement, controlled by WEADZX
- beginnings of a combat mode, with colored sprites "jumping" from tile to tile
- units move in initiative order
- made combat movement smarter by not moving into occupied tiles (including both other sprites as well as static tiles that block movement)
- added "pass" action that does nothing and is immediately complete
- added attack actions
- displaying Hit Points above sprites
- combat ends and return to overland map when one (or fewer) teams survive

Near term TODO
- add A* pathfinding for longer moves
- refactor CombatMgr and MapMgr to have a common base class
- clarify what a CombatUnit is vs a CombatantSprite
- add multiple tile sheets, including
  - one terrain sheet per episode
  - one sprite sheet for player characters
  - at least one sprite sheet for monsters (one per episode?)
  - city tiles?
  - dungeon tiles?

Longer Term TODO
- stores in town
  - pub
  - garth's weapon shop
  - armor
  - Le Healerie
  - magic items
- Portfolio Search w/ MCTS for combat AI