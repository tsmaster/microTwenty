// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class CombatFoyerMap : HexMap {
    public CombatFoyerMap (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "combat_foyer";
    }

    public override void LayoutMap() {
      RockWall(-1, 9, -8);
      RockWall(0, 8, -8);
      RockWall(1, 7, -8);
      RockWall(2, 6, -8);
      RockWall(3, 5, -8);
      RockWall(4, 4, -8);
      RockWall(5, 3, -8);
      RockWall(6, 2, -8);
      RockWall(7, 1, -8);
      RockWall(8, 0, -8);
      RockWall(9, -1, -8);
      RockWall(10, -2, -8);

      RockWall(-2, 9, -7);
      Plain(-1, 8, -7);
      Plain(0, 7, -7);
      Plain(1, 6, -7);
      Plain(2, 5, -7);
      RockWall(3, 4, -7);
      Plain(4, 3, -7);
      RockWall(5, 2, -7);
      Plain(6, 1, -7);
      Plain(7, 0, -7);
      Plain(8, -1, -7);
      Plain(9, -2, -7);
      RockWall(10, -3, -7);

      RockWall(-2, 8, -6);
      Plain(-1, 7, -6);
      Plain(0, 6, -6);
      RockWall(1, 5, -6);
      RockWall(2, 4, -6);
      Plain(3, 3, -6);
      Plain(4, 2, -6);
      RockWall(5, 1, -6);
      RockWall(6, 0, -6);
      Plain(7, -1, -6);
      Plain(8, -2, -6);
      RockWall(9, -3, -6);

      RockWall(-3, 8, -5);
      Plain(-2, 7, -5);
      RockWall(-1, 6, -5);
      RockWall(0, 5, -5);
      Plain(1, 4, -5);
      Plain(2, 3, -5);
      Plain(3, 2, -5);
      Plain(4, 1, -5);
      Plain(5, 0, -5);
      RockWall(6, -1, -5);
      RockWall(7, -2, -5);
      Plain(8, -3, -5);
      RockWall(9, -4, -5);

      RockWall(-3, 7, -4);
      Plain(-2, 6, -4);
      Plain(-1, 5, -4);
      Plain(0, 4, -4);
      RockWall(1, 3, -4);
      Plain(2, 2, -4);
      Plain(3, 1, -4);
      RockWall(4, 0, -4);
      Plain(5, -1, -4);
      Plain(6, -2, -4);
      Plain(7, -3, -4);
      RockWall(8, -4, -4);

      RockWall(-4, 7, -3);
      Plain(-3, 6, -3);
      Plain(-2, 5, -3);
      Plain(-1, 4, -3);
      Plain(0, 3, -3);
      Plain(1, 2, -3);
      Plain(2, 1, -3);
      Plain(3, 0, -3);
      Plain(4, -1, -3);
      Plain(5, -2, -3);
      Plain(6, -3, -3);
      Plain(7, -4, -3);
      RockWall(8, -5, -3);

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

      RockWall(-5, 6, -1);
      Plain(-4, 5, -1);
      Plain(-3, 4, -1);
      Plain(-2, 3, -1);
      RockWall(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Plain(2, -1, -1);
      RockWall(3, -2, -1);
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
      Plain(5, -5, 0);
      RockWall(6, -6, 0);

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
      Plain(5, -6, 1);
      RockWall(6, -7, 1);

      RockWall(-6, 4, 2);
      Plain(-5, 3, 2);
      Plain(-4, 2, 2);
      Plain(-3, 1, 2);
      RockWall(-2, 0, 2);
      Plain(-1, -1, 2);
      Plain(0, -2, 2);
      RockWall(1, -3, 2);
      Plain(2, -4, 2);
      Plain(3, -5, 2);
      Plain(4, -6, 2);
      RockWall(5, -7, 2);

      RockWall(-7, 4, 3);
      Plain(-6, 3, 3);
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

      RockWall(-8, 3, 5);
      RockWall(-7, 2, 5);
      Plain(-6, 1, 5);
      RockWall(-5, 0, 5);
      RockWall(-4, -1, 5);
      Plain(-3, -2, 5);
      Plain(-2, -3, 5);
      Plain(-1, -4, 5);
      RockWall(0, -5, 5);
      RockWall(1, -6, 5);
      Plain(2, -7, 5);
      RockWall(3, -8, 5);
      RockWall(4, -9, 5);

      RockWall(-8, 2, 6);
      Plain(-7, 1, 6);
      Plain(-6, 0, 6);
      Plain(-5, -1, 6);
      RockWall(-4, -2, 6);
      Plain(-3, -3, 6);
      Plain(-2, -4, 6);
      RockWall(-1, -5, 6);
      Plain(0, -6, 6);
      Plain(1, -7, 6);
      Plain(2, -8, 6);
      RockWall(3, -9, 6);

      RockWall(-9, 2, 7);
      Plain(-8, 1, 7);
      Plain(-7, 0, 7);
      Plain(-6, -1, 7);
      RockWall(-5, -2, 7);
      Plain(-4, -3, 7);
      Plain(-3, -4, 7);
      Plain(-2, -5, 7);
      RockWall(-1, -6, 7);
      Plain(0, -7, 7);
      Plain(1, -8, 7);
      Plain(2, -9, 7);
      RockWall(3, -10, 7);

      RockWall(-9, 1, 8);
      Plain(-8, 0, 8);
      Plain(-7, -1, 8);
      Plain(-6, -2, 8);
      RockWall(-5, -3, 8);
      Plain(-4, -4, 8);
      Plain(-3, -5, 8);
      RockWall(-2, -6, 8);
      Plain(-1, -7, 8);
      Plain(0, -8, 8);
      Plain(1, -9, 8);
      RockWall(2, -10, 8);

      RockWall(-10, 1, 9);
      RockWall(-9, 0, 9);
      RockWall(-8, -1, 9);
      RockWall(-7, -2, 9);
      RockWall(-6, -3, 9);
      RockWall(-5, -4, 9);
      RockWall(-4, -5, 9);
      RockWall(-3, -6, 9);
      RockWall(-2, -7, 9);
      RockWall(-1, -8, 9);
      RockWall(0, -9, 9);
      RockWall(1, -10, 9);
      RockWall(2, -11, 9);

    }
  }
}

// This file was auto-generated

