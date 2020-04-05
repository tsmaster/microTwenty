// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class Ep1DungeonRatHoleMap : HexMap {
    public Ep1DungeonRatHoleMap (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "ep1d_rathole";
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
      RockWall(11, -3, -8);

      RockWall(-2, 9, -7);
      RockWall(-1, 8, -7);
      Plain(0, 7, -7);
      Plain(1, 6, -7);
      RockWall(2, 5, -7);
      RockWall(3, 4, -7);
      RockWall(4, 3, -7);
      RockWall(5, 2, -7);
      RockWall(6, 1, -7);
      RockWall(7, 0, -7);
      RockWall(8, -1, -7);
      RockWall(9, -2, -7);
      RockWall(10, -3, -7);

      RockWall(-2, 8, -6);
      Plain(-1, 7, -6);
      Plain(0, 6, -6);
      Plain(1, 5, -6);
      Plain(2, 4, -6);
      Plain(3, 3, -6);
      Plain(4, 2, -6);
      Plain(5, 1, -6);
      Plain(6, 0, -6);
      RockWall(7, -1, -6);
      RockWall(8, -2, -6);
      RockWall(9, -3, -6);
      RockWall(10, -4, -6);

      RockWall(-3, 8, -5);
      Plain(-2, 7, -5);
      Plain(-1, 6, -5);
      Plain(0, 5, -5);
      RockWall(1, 4, -5);
      RockWall(2, 3, -5);
      RockWall(3, 2, -5);
      RockWall(4, 1, -5);
      RockWall(5, 0, -5);
      Plain(6, -1, -5);
      RockWall(7, -2, -5);
      RockWall(8, -3, -5);
      RockWall(9, -4, -5);

      RockWall(-3, 7, -4);
      Plain(-2, 6, -4);
      Plain(-1, 5, -4);
      RockWall(0, 4, -4);
      RockWall(1, 3, -4);
      RockWall(2, 2, -4);
      RockWall(3, 1, -4);
      RockWall(4, 0, -4);
      RockWall(5, -1, -4);
      Plain(6, -2, -4);
      RockWall(7, -3, -4);
      RockWall(8, -4, -4);
      RockWall(9, -5, -4);

      RockWall(-4, 7, -3);
      RockWall(-3, 6, -3);
      RockWall(-2, 5, -3);
      RockWall(-1, 4, -3);
      Plain(0, 3, -3);
      Plain(1, 2, -3);
      Plain(2, 1, -3);
      Plain(3, 0, -3);
      Plain(4, -1, -3);
      RockWall(5, -2, -3);
      Plain(6, -3, -3);
      RockWall(7, -4, -3);
      RockWall(8, -5, -3);

      RockWall(-4, 6, -2);
      RockWall(-3, 5, -2);
      RockWall(-2, 4, -2);
      Plain(-1, 3, -2);
      RockWall(0, 2, -2);
      RockWall(1, 1, -2);
      RockWall(2, 0, -2);
      RockWall(3, -1, -2);
      Plain(4, -2, -2);
      RockWall(5, -3, -2);
      Plain(6, -4, -2);
      RockWall(7, -5, -2);
      RockWall(8, -6, -2);

      RockWall(-5, 6, -1);
      RockWall(-4, 5, -1);
      RockWall(-3, 4, -1);
      Plain(-2, 3, -1);
      RockWall(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Plain(2, -1, -1);
      RockWall(3, -2, -1);
      Plain(4, -3, -1);
      RockWall(5, -4, -1);
      Plain(6, -5, -1);
      RockWall(7, -6, -1);

      RockWall(-5, 5, 0);
      RockWall(-4, 4, 0);
      Plain(-3, 3, 0);
      RockWall(-2, 2, 0);
      Plain(-1, 1, 0);
      RockWall(0, 0, 0);
      RockWall(1, -1, 0);
      Plain(2, -2, 0);
      RockWall(3, -3, 0);
      Plain(4, -4, 0);
      RockWall(5, -5, 0);
      Plain(6, -6, 0);
      RockWall(7, -7, 0);

      RockWall(-6, 5, 1);
      RockWall(-5, 4, 1);
      Plain(-4, 3, 1);
      RockWall(-3, 2, 1);
      RockWall(-2, 1, 1);
      Plain(-1, 0, 1);
      RockWall(0, -1, 1);
      RockWall(1, -2, 1);
      Plain(2, -3, 1);
      RockWall(3, -4, 1);
      Plain(4, -5, 1);
      Plain(5, -6, 1);
      RockWall(6, -7, 1);

      RockWall(-6, 4, 2);
      Plain(-5, 3, 2);
      RockWall(-4, 2, 2);
      RockWall(-3, 1, 2);
      Plain(-2, 0, 2);
      RockWall(-1, -1, 2);
      Plain(0, -2, 2);
      RockWall(1, -3, 2);
      Plain(2, -4, 2);
      RockWall(3, -5, 2);
      RockWall(4, -6, 2);
      RockWall(5, -7, 2);
      RockWall(6, -8, 2);

      RockWall(-7, 4, 3);
      Plain(-6, 3, 3);
      RockWall(-5, 2, 3);
      RockWall(-4, 1, 3);
      Plain(-3, 0, 3);
      RockWall(-2, -1, 3);
      Plain(-1, -2, 3);
      RockWall(0, -3, 3);
      RockWall(1, -4, 3);
      Plain(2, -5, 3);
      RockWall(3, -6, 3);
      RockWall(4, -7, 3);
      RockWall(5, -8, 3);

      RockWall(-7, 3, 4);
      Plain(-6, 2, 4);
      RockWall(-5, 1, 4);
      Plain(-4, 0, 4);
      RockWall(-3, -1, 4);
      Plain(-2, -2, 4);
      RockWall(-1, -3, 4);
      RockWall(0, -4, 4);
      RockWall(1, -5, 4);
      Plain(2, -6, 4);
      RockWall(3, -7, 4);
      RockWall(4, -8, 4);
      RockWall(5, -9, 4);

      RockWall(-8, 3, 5);
      Plain(-7, 2, 5);
      RockWall(-6, 1, 5);
      Plain(-5, 0, 5);
      RockWall(-4, -1, 5);
      Plain(-3, -2, 5);
      RockWall(-2, -3, 5);
      RockWall(-1, -4, 5);
      RockWall(0, -5, 5);
      Plain(1, -6, 5);
      RockWall(2, -7, 5);
      RockWall(3, -8, 5);
      RockWall(4, -9, 5);

      RockWall(-8, 2, 6);
      Plain(-7, 1, 6);
      RockWall(-6, 0, 6);
      Plain(-5, -1, 6);
      RockWall(-4, -2, 6);
      Plain(-3, -3, 6);
      RockWall(-2, -4, 6);
      RockWall(-1, -5, 6);
      Plain(0, -6, 6);
      RockWall(1, -7, 6);
      RockWall(2, -8, 6);
      RockWall(3, -9, 6);
      RockWall(4, -10, 6);

      RockWall(-9, 2, 7);
      RockWall(-8, 1, 7);
      Plain(-7, 0, 7);
      RockWall(-6, -1, 7);
      Plain(-5, -2, 7);
      RockWall(-4, -3, 7);
      Plain(-3, -4, 7);
      RockWall(-2, -5, 7);
      RockWall(-1, -6, 7);
      Plain(0, -7, 7);
      RockWall(1, -8, 7);
      RockWall(2, -9, 7);
      RockWall(3, -10, 7);

      RockWall(-9, 1, 8);
      Plain(-8, 0, 8);
      RockWall(-7, -1, 8);
      RockWall(-6, -2, 8);
      Plain(-5, -3, 8);
      RockWall(-4, -4, 8);
      Plain(-3, -5, 8);
      RockWall(-2, -6, 8);
      RockWall(-1, -7, 8);
      Plain(0, -8, 8);
      RockWall(1, -9, 8);
      RockWall(2, -10, 8);
      RockWall(3, -11, 8);

      RockWall(-10, 1, 9);
      Plain(-9, 0, 9);
      RockWall(-8, -1, 9);
      RockWall(-7, -2, 9);
      Plain(-6, -3, 9);
      RockWall(-5, -4, 9);
      Plain(-4, -5, 9);
      RockWall(-3, -6, 9);
      RockWall(-2, -7, 9);
      Plain(-1, -8, 9);
      RockWall(0, -9, 9);
      RockWall(1, -10, 9);
      RockWall(2, -11, 9);

      RockWall(-10, 0, 10);
      Plain(-9, -1, 10);
      RockWall(-8, -2, 10);
      Plain(-7, -3, 10);
      RockWall(-6, -4, 10);
      Plain(-5, -5, 10);
      RockWall(-4, -6, 10);
      RockWall(-3, -7, 10);
      Plain(-2, -8, 10);
      RockWall(-1, -9, 10);
      RockWall(0, -10, 10);
      RockWall(1, -11, 10);
      RockWall(2, -12, 10);

      RockWall(-11, 0, 11);
      RockWall(-10, -1, 11);
      Plain(-9, -2, 11);
      Plain(-8, -3, 11);
      RockWall(-7, -4, 11);
      RockWall(-6, -5, 11);
      Plain(-5, -6, 11);
      Plain(-4, -7, 11);
      Plain(-3, -8, 11);
      RockWall(-2, -9, 11);
      RockWall(-1, -10, 11);
      RockWall(0, -11, 11);
      RockWall(1, -12, 11);

      RockWall(-11, -1, 12);
      RockWall(-10, -2, 12);
      RockWall(-9, -3, 12);
      RockWall(-8, -4, 12);
      RockWall(-7, -5, 12);
      RockWall(-6, -6, 12);
      RockWall(-5, -7, 12);
      RockWall(-4, -8, 12);
      RockWall(-3, -9, 12);
      RockWall(-2, -10, 12);
      RockWall(-1, -11, 12);
      RockWall(0, -12, 12);
      RockWall(1, -13, 12);

    }
  }
}

// This file was auto-generated

