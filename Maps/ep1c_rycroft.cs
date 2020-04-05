// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class Ep1CityRycroftMap : HexMap {
    public Ep1CityRycroftMap (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "ep1c_rycroft";
    }

    public override void LayoutMap() {
      WoodWall(-1, 9, -8);
      WoodWall(0, 8, -8);
      WoodWall(1, 7, -8);
      WoodWall(2, 6, -8);
      WoodWall(3, 5, -8);
      Plain(4, 4, -8);
      WoodWall(5, 3, -8);
      WoodWall(6, 2, -8);
      WoodWall(7, 1, -8);
      WoodWall(8, 0, -8);
      WoodWall(9, -1, -8);
      WoodWall(10, -2, -8);
      WoodWall(11, -3, -8);

      WoodWall(-2, 9, -7);
      Plain(-1, 8, -7);
      Plain(0, 7, -7);
      Plain(1, 6, -7);
      Plain(2, 5, -7);
      Plain(3, 4, -7);
      Plain(4, 3, -7);
      Plain(5, 2, -7);
      Plain(6, 1, -7);
      Plain(7, 0, -7);
      Plain(8, -1, -7);
      Plain(9, -2, -7);
      WoodWall(10, -3, -7);

      WoodWall(-2, 8, -6);
      Plain(-1, 7, -6);
      Plain(0, 6, -6);
      Plain(1, 5, -6);
      Plain(2, 4, -6);
      Plain(3, 3, -6);
      Plain(4, 2, -6);
      Plain(5, 1, -6);
      Plain(6, 0, -6);
      Plain(7, -1, -6);
      Plain(8, -2, -6);
      Plain(9, -3, -6);
      WoodWall(10, -4, -6);

      WoodWall(-3, 8, -5);
      Plain(-2, 7, -5);
      Plain(-1, 6, -5);
      Plain(0, 5, -5);
      Plain(1, 4, -5);
      Plain(2, 3, -5);
      Plain(3, 2, -5);
      Plain(4, 1, -5);
      Plain(5, 0, -5);
      Plain(6, -1, -5);
      Plain(7, -2, -5);
      Plain(8, -3, -5);
      WoodWall(9, -4, -5);

      WoodWall(-3, 7, -4);
      Plain(-2, 6, -4);
      Plain(-1, 5, -4);
      Plain(0, 4, -4);
      Plain(1, 3, -4);
      Plain(2, 2, -4);
      Plain(3, 1, -4);
      Plain(4, 0, -4);
      Plain(5, -1, -4);
      Plain(6, -2, -4);
      Plain(7, -3, -4);
      Plain(8, -4, -4);
      WoodWall(9, -5, -4);

      WoodWall(-4, 7, -3);
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
      WoodWall(8, -5, -3);

      WoodWall(-4, 6, -2);
      Plain(-3, 5, -2);
      Plain(-2, 4, -2);
      Plain(-1, 3, -2);
      Plain(0, 2, -2);
      Plain(1, 1, -2);
      Plain(2, 0, -2);
      Building(3, -1, -2);
      Building(4, -2, -2);
      Plain(5, -3, -2);
      Plain(6, -4, -2);
      Plain(7, -5, -2);
      WoodWall(8, -6, -2);

      WoodWall(-5, 6, -1);
      Plain(-4, 5, -1);
      Plain(-3, 4, -1);
      Plain(-2, 3, -1);
      Plain(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Building(2, -1, -1);
      Building(3, -2, -1);
      Building(4, -3, -1);
      Plain(5, -4, -1);
      Plain(6, -5, -1);
      WoodWall(7, -6, -1);

      WoodWall(-5, 5, 0);
      Plain(-4, 4, 0);
      Plain(-3, 3, 0);
      Plain(-2, 2, 0);
      Plain(-1, 1, 0);
      Plain(0, 0, 0);
      Plain(1, -1, 0);
      BuildingEntrance(2, -2, 0);
      Building(3, -3, 0);
      Plain(4, -4, 0);
      Plain(5, -5, 0);
      Plain(6, -6, 0);
      WoodWall(7, -7, 0);

      WoodWall(-6, 5, 1);
      Plain(-5, 4, 1);
      Plain(-4, 3, 1);
      Building(-3, 2, 1);
      Plain(-2, 1, 1);
      Plain(-1, 0, 1);
      Plain(0, -1, 1);
      Plain(1, -2, 1);
      Plain(2, -3, 1);
      Plain(3, -4, 1);
      Plain(4, -5, 1);
      Plain(5, -6, 1);
      WoodWall(6, -7, 1);

      WoodWall(-6, 4, 2);
      Plain(-5, 3, 2);
      Building(-4, 2, 2);
      BuildingEntrance(-3, 1, 2);
      Plain(-2, 0, 2);
      Plain(-1, -1, 2);
      Plain(0, -2, 2);
      Plain(1, -3, 2);
      Plain(2, -4, 2);
      Building(3, -5, 2);
      Plain(4, -6, 2);
      Plain(5, -7, 2);
      WoodWall(6, -8, 2);

      WoodWall(-7, 4, 3);
      Plain(-6, 3, 3);
      Plain(-5, 2, 3);
      Plain(-4, 1, 3);
      Plain(-3, 0, 3);
      Plain(-2, -1, 3);
      Plain(-1, -2, 3);
      Plain(0, -3, 3);
      Plain(1, -4, 3);
      BuildingEntrance(2, -5, 3);
      Building(3, -6, 3);
      Plain(4, -7, 3);
      WoodWall(5, -8, 3);

      WoodWall(-7, 3, 4);
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
      Plain(4, -8, 4);
      WoodWall(5, -9, 4);

      Plain(-8, 3, 5);
      Plain(-7, 2, 5);
      Plain(-6, 1, 5);
      Plain(-5, 0, 5);
      Plain(-4, -1, 5);
      Plain(-3, -2, 5);
      Plain(-2, -3, 5);
      Plain(-1, -4, 5);
      Plain(0, -5, 5);
      Plain(1, -6, 5);
      Plain(2, -7, 5);
      Plain(3, -8, 5);
      WoodWall(4, -9, 5);

      WoodWall(-8, 2, 6);
      Plain(-7, 1, 6);
      Plain(-6, 0, 6);
      Plain(-5, -1, 6);
      Plain(-4, -2, 6);
      Plain(-3, -3, 6);
      Plain(-2, -4, 6);
      Plain(-1, -5, 6);
      BuildingEntrance(0, -6, 6);
      Building(1, -7, 6);
      Plain(2, -8, 6);
      Plain(3, -9, 6);
      WoodWall(4, -10, 6);

      WoodWall(-9, 2, 7);
      Plain(-8, 1, 7);
      Building(-7, 0, 7);
      Building(-6, -1, 7);
      Plain(-5, -2, 7);
      Plain(-4, -3, 7);
      BuildingEntrance(-3, -4, 7);
      Plain(-2, -5, 7);
      Plain(-1, -6, 7);
      Building(0, -7, 7);
      Plain(1, -8, 7);
      Plain(2, -9, 7);
      WoodWall(3, -10, 7);

      WoodWall(-9, 1, 8);
      Plain(-8, 0, 8);
      Building(-7, -1, 8);
      Plain(-6, -2, 8);
      Plain(-5, -3, 8);
      Building(-4, -4, 8);
      Building(-3, -5, 8);
      Plain(-2, -6, 8);
      Plain(-1, -7, 8);
      Plain(0, -8, 8);
      Plain(1, -9, 8);
      Plain(2, -10, 8);
      WoodWall(3, -11, 8);

      WoodWall(-10, 1, 9);
      Plain(-9, 0, 9);
      Plain(-8, -1, 9);
      Plain(-7, -2, 9);
      BuildingEntrance(-6, -3, 9);
      Plain(-5, -4, 9);
      Plain(-4, -5, 9);
      Plain(-3, -6, 9);
      Plain(-2, -7, 9);
      Dock(-1, -8, 9);
      Water(0, -9, 9);
      Water(1, -10, 9);
      Water(2, -11, 9);

      WoodWall(-10, 0, 10);
      Plain(-9, -1, 10);
      Plain(-8, -2, 10);
      Building(-7, -3, 10);
      Building(-6, -4, 10);
      Plain(-5, -5, 10);
      Plain(-4, -6, 10);
      Plain(-3, -7, 10);
      Water(-2, -8, 10);
      Dock(-1, -9, 10);
      Water(0, -10, 10);
      Water(1, -11, 10);
      Water(2, -12, 10);

      WoodWall(-11, 0, 11);
      Plain(-10, -1, 11);
      Plain(-9, -2, 11);
      Plain(-8, -3, 11);
      Plain(-7, -4, 11);
      Plain(-6, -5, 11);
      Plain(-5, -6, 11);
      Plain(-4, -7, 11);
      Water(-3, -8, 11);
      Water(-2, -9, 11);
      Dock(-1, -10, 11);
      Water(0, -11, 11);
      Ship(0, -11, 11);
      Water(1, -12, 11);

      WoodWall(-11, -1, 12);
      WoodWall(-10, -2, 12);
      WoodWall(-9, -3, 12);
      WoodWall(-8, -4, 12);
      WoodWall(-7, -5, 12);
      WoodWall(-6, -6, 12);
      WoodWall(-5, -7, 12);
      Water(-4, -8, 12);
      Water(-3, -9, 12);
      Water(-2, -10, 12);
      Water(-1, -11, 12);
      Water(0, -12, 12);
      Water(1, -13, 12);

    }
  }
}

// This file was auto-generated

