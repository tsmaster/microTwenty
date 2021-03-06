// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class LabyrinthMap : HexMap {
    public LabyrinthMap (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "labyrinth";
    }

    public override void LayoutMap() {
      RockWall(-6, 16, -10);
      RockWall(-5, 15, -10);
      RockWall(-4, 14, -10);
      RockWall(-3, 13, -10);
      RockWall(-2, 12, -10);
      RockWall(-1, 11, -10);
      RockWall(0, 10, -10);
      RockWall(1, 9, -10);
      RockWall(2, 8, -10);
      RockWall(3, 7, -10);
      RockWall(4, 6, -10);
      RockWall(5, 5, -10);
      RockWall(6, 4, -10);
      RockWall(7, 3, -10);
      RockWall(8, 2, -10);
      RockWall(9, 1, -10);
      RockWall(10, 0, -10);
      RockWall(11, -1, -10);
      RockWall(12, -2, -10);
      RockWall(13, -3, -10);
      RockWall(14, -4, -10);
      RockWall(15, -5, -10);
      RockWall(16, -6, -10);

      RockWall(-6, 15, -9);
      RockWall(-5, 14, -9);
      RockWall(-4, 13, -9);
      RockWall(-3, 12, -9);
      RockWall(-2, 11, -9);
      RockWall(-1, 10, -9);
      RockWall(0, 9, -9);
      RockWall(1, 8, -9);
      RockWall(2, 7, -9);
      RockWall(3, 6, -9);
      RockWall(4, 5, -9);
      RockWall(5, 4, -9);
      RockWall(6, 3, -9);
      RockWall(7, 2, -9);
      RockWall(8, 1, -9);
      RockWall(9, 0, -9);
      RockWall(10, -1, -9);
      RockWall(11, -2, -9);
      RockWall(12, -3, -9);
      RockWall(13, -4, -9);
      RockWall(14, -5, -9);
      RockWall(15, -6, -9);

      RockWall(-7, 15, -8);
      RockWall(-6, 14, -8);
      RockWall(-5, 13, -8);
      RockWall(-4, 12, -8);
      RockWall(-3, 11, -8);
      Portal(-2, 10, -8);
      RockWall(-1, 9, -8);
      Plain(0, 8, -8);
      Plain(1, 7, -8);
      Plain(2, 6, -8);
      RockWall(3, 5, -8);
      Plain(4, 4, -8);
      RockWall(5, 3, -8);
      Plain(6, 2, -8);
      RockWall(7, 1, -8);
      Plain(8, 0, -8);
      Plain(9, -1, -8);
      Plain(10, -2, -8);
      RockWall(11, -3, -8);
      RockWall(12, -4, -8);
      RockWall(13, -5, -8);
      RockWall(14, -6, -8);
      RockWall(15, -7, -8);

      RockWall(-7, 14, -7);
      RockWall(-6, 13, -7);
      RockWall(-5, 12, -7);
      RockWall(-4, 11, -7);
      RockWall(-3, 10, -7);
      Plain(-2, 9, -7);
      RockWall(-1, 8, -7);
      RockWall(0, 7, -7);
      Plain(1, 6, -7);
      RockWall(2, 5, -7);
      RockWall(3, 4, -7);
      Plain(4, 3, -7);
      RockWall(5, 2, -7);
      Plain(6, 1, -7);
      Plain(7, 0, -7);
      RockWall(8, -1, -7);
      RockWall(9, -2, -7);
      RockWall(10, -3, -7);
      RockWall(11, -4, -7);
      RockWall(12, -5, -7);
      RockWall(13, -6, -7);
      RockWall(14, -7, -7);

      RockWall(-8, 14, -6);
      RockWall(-7, 13, -6);
      RockWall(-6, 12, -6);
      RockWall(-5, 11, -6);
      RockWall(-4, 10, -6);
      RockWall(-3, 9, -6);
      Plain(-2, 8, -6);
      Plain(-1, 7, -6);
      Plain(0, 6, -6);
      RockWall(1, 5, -6);
      Plain(2, 4, -6);
      RockWall(3, 3, -6);
      Plain(4, 2, -6);
      RockWall(5, 1, -6);
      Plain(6, 0, -6);
      RockWall(7, -1, -6);
      Plain(8, -2, -6);
      RockWall(9, -3, -6);
      RockWall(10, -4, -6);
      RockWall(11, -5, -6);
      RockWall(12, -6, -6);
      RockWall(13, -7, -6);
      RockWall(14, -8, -6);

      RockWall(-8, 13, -5);
      RockWall(-7, 12, -5);
      RockWall(-6, 11, -5);
      RockWall(-5, 10, -5);
      RockWall(-4, 9, -5);
      RockWall(-3, 8, -5);
      Plain(-2, 7, -5);
      RockWall(-1, 6, -5);
      RockWall(0, 5, -5);
      RockWall(1, 4, -5);
      Plain(2, 3, -5);
      RockWall(3, 2, -5);
      Plain(4, 1, -5);
      RockWall(5, 0, -5);
      Plain(6, -1, -5);
      Plain(7, -2, -5);
      RockWall(8, -3, -5);
      RockWall(9, -4, -5);
      RockWall(10, -5, -5);
      RockWall(11, -6, -5);
      RockWall(12, -7, -5);
      RockWall(13, -8, -5);

      RockWall(-9, 13, -4);
      RockWall(-8, 12, -4);
      RockWall(-7, 11, -4);
      RockWall(-6, 10, -4);
      RockWall(-5, 9, -4);
      Plain(-4, 8, -4);
      RockWall(-3, 7, -4);
      Plain(-2, 6, -4);
      RockWall(-1, 5, -4);
      Plain(0, 4, -4);
      RockWall(1, 3, -4);
      Plain(2, 2, -4);
      RockWall(3, 1, -4);
      Plain(4, 0, -4);
      Plain(5, -1, -4);
      Plain(6, -2, -4);
      Plain(7, -3, -4);
      Plain(8, -4, -4);
      RockWall(9, -5, -4);
      RockWall(10, -6, -4);
      RockWall(11, -7, -4);
      RockWall(12, -8, -4);
      RockWall(13, -9, -4);

      RockWall(-9, 12, -3);
      RockWall(-8, 11, -3);
      RockWall(-7, 10, -3);
      RockWall(-6, 9, -3);
      RockWall(-5, 8, -3);
      Plain(-4, 7, -3);
      RockWall(-3, 6, -3);
      Plain(-2, 5, -3);
      RockWall(-1, 4, -3);
      Plain(0, 3, -3);
      RockWall(1, 2, -3);
      Plain(2, 1, -3);
      Plain(3, 0, -3);
      RockWall(4, -1, -3);
      RockWall(5, -2, -3);
      RockWall(6, -3, -3);
      RockWall(7, -4, -3);
      RockWall(8, -5, -3);
      RockWall(9, -6, -3);
      RockWall(10, -7, -3);
      RockWall(11, -8, -3);
      RockWall(12, -9, -3);

      RockWall(-10, 12, -2);
      RockWall(-9, 11, -2);
      RockWall(-8, 10, -2);
      RockWall(-7, 9, -2);
      RockWall(-6, 8, -2);
      RockWall(-5, 7, -2);
      Plain(-4, 6, -2);
      Plain(-3, 5, -2);
      Plain(-2, 4, -2);
      Plain(-1, 3, -2);
      Plain(0, 2, -2);
      RockWall(1, 1, -2);
      Plain(2, 0, -2);
      Plain(3, -1, -2);
      Plain(4, -2, -2);
      RockWall(5, -3, -2);
      Plain(6, -4, -2);
      RockWall(7, -5, -2);
      RockWall(8, -6, -2);
      RockWall(9, -7, -2);
      RockWall(10, -8, -2);
      RockWall(11, -9, -2);
      RockWall(12, -10, -2);

      RockWall(-10, 11, -1);
      RockWall(-9, 10, -1);
      RockWall(-8, 9, -1);
      RockWall(-7, 8, -1);
      RockWall(-6, 7, -1);
      RockWall(-5, 6, -1);
      RockWall(-4, 5, -1);
      RockWall(-3, 4, -1);
      RockWall(-2, 3, -1);
      RockWall(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Plain(2, -1, -1);
      RockWall(3, -2, -1);
      RockWall(4, -3, -1);
      Plain(5, -4, -1);
      RockWall(6, -5, -1);
      RockWall(7, -6, -1);
      RockWall(8, -7, -1);
      RockWall(9, -8, -1);
      RockWall(10, -9, -1);
      RockWall(11, -10, -1);

      RockWall(-11, 11, 0);
      RockWall(-10, 10, 0);
      RockWall(-9, 9, 0);
      RockWall(-8, 8, 0);
      RockWall(-7, 7, 0);
      Plain(-6, 6, 0);
      Plain(-5, 5, 0);
      Plain(-4, 4, 0);
      Plain(-3, 3, 0);
      Plain(-2, 2, 0);
      RockWall(-1, 1, 0);
      Plain(0, 0, 0);
      RockWall(1, -1, 0);
      Plain(2, -2, 0);
      RockWall(3, -3, 0);
      Plain(4, -4, 0);
      Plain(5, -5, 0);
      Plain(6, -6, 0);
      RockWall(7, -7, 0);
      RockWall(8, -8, 0);
      RockWall(9, -9, 0);
      RockWall(10, -10, 0);
      RockWall(11, -11, 0);

      RockWall(-11, 10, 1);
      RockWall(-10, 9, 1);
      RockWall(-9, 8, 1);
      RockWall(-8, 7, 1);
      RockWall(-7, 6, 1);
      RockWall(-6, 5, 1);
      RockWall(-5, 4, 1);
      Plain(-4, 3, 1);
      RockWall(-3, 2, 1);
      Plain(-2, 1, 1);
      RockWall(-1, 0, 1);
      Plain(0, -1, 1);
      RockWall(1, -2, 1);
      Plain(2, -3, 1);
      RockWall(3, -4, 1);
      RockWall(4, -5, 1);
      RockWall(5, -6, 1);
      RockWall(6, -7, 1);
      RockWall(7, -8, 1);
      RockWall(8, -9, 1);
      RockWall(9, -10, 1);
      RockWall(10, -11, 1);

      RockWall(-12, 10, 2);
      RockWall(-11, 9, 2);
      RockWall(-10, 8, 2);
      RockWall(-9, 7, 2);
      RockWall(-8, 6, 2);
      RockWall(-7, 5, 2);
      Plain(-6, 4, 2);
      Plain(-5, 3, 2);
      Plain(-4, 2, 2);
      RockWall(-3, 1, 2);
      Plain(-2, 0, 2);
      Plain(-1, -1, 2);
      Plain(0, -2, 2);
      RockWall(1, -3, 2);
      Plain(2, -4, 2);
      Plain(3, -5, 2);
      Plain(4, -6, 2);
      RockWall(5, -7, 2);
      RockWall(6, -8, 2);
      RockWall(7, -9, 2);
      RockWall(8, -10, 2);
      RockWall(9, -11, 2);
      RockWall(10, -12, 2);

      RockWall(-12, 9, 3);
      RockWall(-11, 8, 3);
      RockWall(-10, 7, 3);
      RockWall(-9, 6, 3);
      RockWall(-8, 5, 3);
      RockWall(-7, 4, 3);
      RockWall(-6, 3, 3);
      Plain(-5, 2, 3);
      Plain(-4, 1, 3);
      RockWall(-3, 0, 3);
      RockWall(-2, -1, 3);
      Plain(-1, -2, 3);
      Plain(0, -3, 3);
      RockWall(1, -4, 3);
      RockWall(2, -5, 3);
      Plain(3, -6, 3);
      Plain(4, -7, 3);
      RockWall(5, -8, 3);
      RockWall(6, -9, 3);
      RockWall(7, -10, 3);
      RockWall(8, -11, 3);
      RockWall(9, -12, 3);

      RockWall(-13, 9, 4);
      RockWall(-12, 8, 4);
      RockWall(-11, 7, 4);
      RockWall(-10, 6, 4);
      RockWall(-9, 5, 4);
      Plain(-8, 4, 4);
      Plain(-7, 3, 4);
      Plain(-6, 2, 4);
      RockWall(-5, 1, 4);
      Plain(-4, 0, 4);
      RockWall(-3, -1, 4);
      Plain(-2, -2, 4);
      RockWall(-1, -3, 4);
      Plain(0, -4, 4);
      RockWall(1, -5, 4);
      Plain(2, -6, 4);
      RockWall(3, -7, 4);
      Plain(4, -8, 4);
      RockWall(5, -9, 4);
      RockWall(6, -10, 4);
      RockWall(7, -11, 4);
      RockWall(8, -12, 4);
      RockWall(9, -13, 4);

      RockWall(-13, 8, 5);
      RockWall(-12, 7, 5);
      RockWall(-11, 6, 5);
      RockWall(-10, 5, 5);
      RockWall(-9, 4, 5);
      Plain(-8, 3, 5);
      RockWall(-7, 2, 5);
      Plain(-6, 1, 5);
      RockWall(-5, 0, 5);
      Plain(-4, -1, 5);
      RockWall(-3, -2, 5);
      RockWall(-2, -3, 5);
      RockWall(-1, -4, 5);
      RockWall(0, -5, 5);
      RockWall(1, -6, 5);
      RockWall(2, -7, 5);
      RockWall(3, -8, 5);
      RockWall(4, -9, 5);
      RockWall(5, -10, 5);
      RockWall(6, -11, 5);
      RockWall(7, -12, 5);
      RockWall(8, -13, 5);

      RockWall(-14, 8, 6);
      RockWall(-13, 7, 6);
      RockWall(-12, 6, 6);
      RockWall(-11, 5, 6);
      RockWall(-10, 4, 6);
      RockWall(-9, 3, 6);
      Plain(-8, 2, 6);
      RockWall(-7, 1, 6);
      Plain(-6, 0, 6);
      RockWall(-5, -1, 6);
      Plain(-4, -2, 6);
      Plain(-3, -3, 6);
      Plain(-2, -4, 6);
      Plain(-1, -5, 6);
      Plain(0, -6, 6);
      Plain(1, -7, 6);
      Plain(2, -8, 6);
      RockWall(3, -9, 6);
      RockWall(4, -10, 6);
      RockWall(5, -11, 6);
      RockWall(6, -12, 6);
      RockWall(7, -13, 6);
      RockWall(8, -14, 6);

      RockWall(-14, 7, 7);
      RockWall(-13, 6, 7);
      RockWall(-12, 5, 7);
      RockWall(-11, 4, 7);
      RockWall(-10, 3, 7);
      RockWall(-9, 2, 7);
      RockWall(-8, 1, 7);
      Plain(-7, 0, 7);
      RockWall(-6, -1, 7);
      Plain(-5, -2, 7);
      RockWall(-4, -3, 7);
      Plain(-3, -4, 7);
      RockWall(-2, -5, 7);
      RockWall(-1, -6, 7);
      RockWall(0, -7, 7);
      RockWall(1, -8, 7);
      Plain(2, -9, 7);
      RockWall(3, -10, 7);
      RockWall(4, -11, 7);
      RockWall(5, -12, 7);
      RockWall(6, -13, 7);
      RockWall(7, -14, 7);

      RockWall(-15, 7, 8);
      RockWall(-14, 6, 8);
      RockWall(-13, 5, 8);
      RockWall(-12, 4, 8);
      RockWall(-11, 3, 8);
      Plain(-10, 2, 8);
      Plain(-9, 1, 8);
      Plain(-8, 0, 8);
      RockWall(-7, -1, 8);
      Plain(-6, -2, 8);
      RockWall(-5, -3, 8);
      Plain(-4, -4, 8);
      Plain(-3, -5, 8);
      Plain(-2, -6, 8);
      Plain(-1, -7, 8);
      Plain(0, -8, 8);
      RockWall(1, -9, 8);
      Portal(2, -10, 8);
      RockWall(3, -11, 8);
      RockWall(4, -12, 8);
      RockWall(5, -13, 8);
      RockWall(6, -14, 8);
      RockWall(7, -15, 8);

      RockWall(-15, 6, 9);
      RockWall(-14, 5, 9);
      RockWall(-13, 4, 9);
      RockWall(-12, 3, 9);
      RockWall(-11, 2, 9);
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
      RockWall(3, -12, 9);
      RockWall(4, -13, 9);
      RockWall(5, -14, 9);
      RockWall(6, -15, 9);

      RockWall(-16, 6, 10);
      RockWall(-15, 5, 10);
      RockWall(-14, 4, 10);
      RockWall(-13, 3, 10);
      RockWall(-12, 2, 10);
      RockWall(-11, 1, 10);
      RockWall(-10, 0, 10);
      RockWall(-9, -1, 10);
      RockWall(-8, -2, 10);
      RockWall(-7, -3, 10);
      RockWall(-6, -4, 10);
      RockWall(-5, -5, 10);
      RockWall(-4, -6, 10);
      RockWall(-3, -7, 10);
      RockWall(-2, -8, 10);
      RockWall(-1, -9, 10);
      RockWall(0, -10, 10);
      RockWall(1, -11, 10);
      RockWall(2, -12, 10);
      RockWall(3, -13, 10);
      RockWall(4, -14, 10);
      RockWall(5, -15, 10);
      RockWall(6, -16, 10);

    }
  }
}

// This file was auto-generated

