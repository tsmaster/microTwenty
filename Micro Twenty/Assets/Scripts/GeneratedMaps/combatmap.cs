// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class CombatMap : HexMap {
    public CombatMap (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "combat";
    }

    public override void LayoutMap() {
      Plain(-1, 9, -8);
      Plain(0, 8, -8);
      Plain(1, 7, -8);
      RockWall(2, 6, -8);
      RockWall(3, 5, -8);
      RockWall(4, 4, -8);
      RockWall(5, 3, -8);
      RockWall(6, 2, -8);
      RockWall(7, 1, -8);
      Plain(8, 0, -8);
      Plain(9, -1, -8);
      Plain(10, -2, -8);
      Plain(11, -3, -8);

      Plain(-2, 9, -7);
      Plain(-1, 8, -7);
      Plain(0, 7, -7);
      RockWall(1, 6, -7);
      Plain(2, 5, -7);
      Plain(3, 4, -7);
      Plain(4, 3, -7);
      Plain(5, 2, -7);
      Plain(6, 1, -7);
      RockWall(7, 0, -7);
      Plain(8, -1, -7);
      Plain(9, -2, -7);
      Plain(10, -3, -7);

      Plain(-2, 8, -6);
      Plain(-1, 7, -6);
      RockWall(0, 6, -6);
      Plain(1, 5, -6);
      RockWall(2, 4, -6);
      Plain(3, 3, -6);
      Plain(4, 2, -6);
      Plain(5, 1, -6);
      Plain(6, 0, -6);
      RockWall(7, -1, -6);
      Plain(8, -2, -6);
      Plain(9, -3, -6);
      Plain(10, -4, -6);

      Plain(-3, 8, -5);
      Plain(-2, 7, -5);
      RockWall(-1, 6, -5);
      Plain(0, 5, -5);
      Plain(1, 4, -5);
      Plain(2, 3, -5);
      Plain(3, 2, -5);
      Plain(4, 1, -5);
      Plain(5, 0, -5);
      Plain(6, -1, -5);
      RockWall(7, -2, -5);
      Plain(8, -3, -5);
      Plain(9, -4, -5);

      Plain(-3, 7, -4);
      RockWall(-2, 6, -4);
      Plain(-1, 5, -4);
      Plain(0, 4, -4);
      Plain(1, 3, -4);
      Plain(2, 2, -4);
      Plain(3, 1, -4);
      Plain(4, 0, -4);
      Plain(5, -1, -4);
      Plain(6, -2, -4);
      RockWall(7, -3, -4);
      Plain(8, -4, -4);
      Plain(9, -5, -4);

      Plain(-4, 7, -3);
      RockWall(-3, 6, -3);
      Plain(-2, 5, -3);
      Plain(-1, 4, -3);
      Plain(0, 3, -3);
      Plain(1, 2, -3);
      Plain(2, 1, -3);
      Plain(3, 0, -3);
      Plain(4, -1, -3);
      Plain(5, -2, -3);
      Plain(6, -3, -3);
      RockWall(7, -4, -3);
      Plain(8, -5, -3);

      RockWall(-4, 6, -2);
      Plain(-3, 5, -2);
      Plain(-2, 4, -2);
      Plain(-1, 3, -2);
      Plain(0, 2, -2);
      Plain(1, 1, -2);
      Plain(2, 0, -2);
      Plain(3, -1, -2);
      Plain(4, -2, -2);
      Plain(5, -3, -2);
      Plain(6, -4, -2);
      RockWall(7, -5, -2);
      Plain(8, -6, -2);

      Plain(-5, 6, -1);
      RockWall(-4, 5, -1);
      Plain(-3, 4, -1);
      Plain(-2, 3, -1);
      Plain(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Plain(2, -1, -1);
      Plain(3, -2, -1);
      Plain(4, -3, -1);
      Plain(5, -4, -1);
      Plain(6, -5, -1);
      RockWall(7, -6, -1);

      RockWall(-5, 5, 0);
      Plain(-4, 4, 0);
      Plain(-3, 3, 0);
      Plain(-2, 2, 0);
      Plain(-1, 1, 0);
      Plain(0, 0, 0);
      Plain(1, -1, 0);
      Plain(2, -2, 0);
      Plain(3, -3, 0);
      Plain(4, -4, 0);
      RockWall(5, -5, 0);
      Plain(6, -6, 0);
      RockWall(7, -7, 0);

      RockWall(-6, 5, 1);
      Plain(-5, 4, 1);
      Plain(-4, 3, 1);
      Plain(-3, 2, 1);
      Plain(-2, 1, 1);
      Plain(-1, 0, 1);
      Plain(0, -1, 1);
      Plain(1, -2, 1);
      Plain(2, -3, 1);
      Plain(3, -4, 1);
      Plain(4, -5, 1);
      RockWall(5, -6, 1);
      Plain(6, -7, 1);
      RockWall(7, -8, 1);

      RockWall(-6, 4, 2);
      Plain(-5, 3, 2);
      Plain(-4, 2, 2);
      Plain(-3, 1, 2);
      Plain(-2, 0, 2);
      Plain(-1, -1, 2);
      Plain(0, -2, 2);
      Plain(1, -3, 2);
      Plain(2, -4, 2);
      Plain(3, -5, 2);
      RockWall(4, -6, 2);
      Plain(5, -7, 2);
      RockWall(6, -8, 2);

      Plain(-7, 4, 3);
      RockWall(-6, 3, 3);
      Plain(-5, 2, 3);
      Plain(-4, 1, 3);
      Plain(-3, 0, 3);
      Plain(-2, -1, 3);
      Plain(-1, -2, 3);
      Plain(0, -3, 3);
      Plain(1, -4, 3);
      Plain(2, -5, 3);
      Plain(3, -6, 3);
      Plain(4, -7, 3);
      RockWall(5, -8, 3);

      RockWall(-7, 3, 4);
      Plain(-6, 2, 4);
      Plain(-5, 1, 4);
      Plain(-4, 0, 4);
      Plain(-3, -1, 4);
      Plain(-2, -2, 4);
      Plain(-1, -3, 4);
      Plain(0, -4, 4);
      Plain(1, -5, 4);
      Plain(2, -6, 4);
      Plain(3, -7, 4);
      RockWall(4, -8, 4);
      Plain(5, -9, 4);

      Plain(-8, 3, 5);
      RockWall(-7, 2, 5);
      Plain(-6, 1, 5);
      Plain(-5, 0, 5);
      Plain(-4, -1, 5);
      Plain(-3, -2, 5);
      Plain(-2, -3, 5);
      Plain(-1, -4, 5);
      Plain(0, -5, 5);
      Plain(1, -6, 5);
      Plain(2, -7, 5);
      RockWall(3, -8, 5);
      Plain(4, -9, 5);

      Plain(-8, 2, 6);
      RockWall(-7, 1, 6);
      Plain(-6, 0, 6);
      Plain(-5, -1, 6);
      RockWall(-4, -2, 6);
      Plain(-3, -3, 6);
      Plain(-2, -4, 6);
      Plain(-1, -5, 6);
      Plain(0, -6, 6);
      Plain(1, -7, 6);
      RockWall(2, -8, 6);
      Plain(3, -9, 6);
      Plain(4, -10, 6);

      Plain(-9, 2, 7);
      Plain(-8, 1, 7);
      RockWall(-7, 0, 7);
      Plain(-6, -1, 7);
      RockWall(-5, -2, 7);
      Plain(-4, -3, 7);
      Plain(-3, -4, 7);
      Plain(-2, -5, 7);
      Plain(-1, -6, 7);
      Plain(0, -7, 7);
      RockWall(1, -8, 7);
      Plain(2, -9, 7);
      Plain(3, -10, 7);

      Plain(-9, 1, 8);
      Plain(-8, 0, 8);
      RockWall(-7, -1, 8);
      Plain(-6, -2, 8);
      Plain(-5, -3, 8);
      Plain(-4, -4, 8);
      Plain(-3, -5, 8);
      Plain(-2, -6, 8);
      Plain(-1, -7, 8);
      RockWall(0, -8, 8);
      Plain(1, -9, 8);
      Plain(2, -10, 8);
      Plain(3, -11, 8);

      Plain(-10, 1, 9);
      Plain(-9, 0, 9);
      Plain(-8, -1, 9);
      RockWall(-7, -2, 9);
      RockWall(-6, -3, 9);
      RockWall(-5, -4, 9);
      RockWall(-4, -5, 9);
      RockWall(-3, -6, 9);
      RockWall(-2, -7, 9);
      RockWall(-1, -8, 9);
      Plain(0, -9, 9);
      Plain(1, -10, 9);
      Plain(2, -11, 9);

    }
  }
}

// This file was auto-generated

