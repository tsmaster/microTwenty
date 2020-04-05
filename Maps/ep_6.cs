// This file was auto-generated

using MicroTwenty;

namespace MicroTwenty {
  class Episode6Map : HexMap {
    public Episode6Map (GameMgr gameMgr) : base(gameMgr) {
      LayoutMap();
    }

    public override string Name () {
      return "ep_6";
    }

    public override void LayoutMap() {
      Mountain(-1, 9, -8);
      Mountain(0, 8, -8);
      Mountain(1, 7, -8);
      Mountain(2, 6, -8);
      Mountain(3, 5, -8);
      Mountain(4, 4, -8);
      Mountain(5, 3, -8);
      WaterMount(6, 2, -8);
      WaterMount(7, 1, -8);
      Water(8, 0, -8);
      Water(9, -1, -8);
      Water(10, -2, -8);

      Mountain(-2, 9, -7);
      Plain(-1, 8, -7);
      Plain(0, 7, -7);
      Plain(1, 6, -7);
      Plain(2, 5, -7);
      Plain(3, 4, -7);
      Plain(4, 3, -7);
      Water(5, 2, -7);
      Water(6, 1, -7);
      Water(7, 0, -7);
      Water(8, -1, -7);
      Water(9, -2, -7);

      Mountain(-2, 8, -6);
      Plain(-1, 7, -6);
      Plain(0, 6, -6);
      Plain(1, 5, -6);
      Plain(2, 4, -6);
      Plain(3, 3, -6);
      Plain(4, 2, -6);
      Water(5, 1, -6);
      Water(6, 0, -6);
      Water(7, -1, -6);
      Water(8, -2, -6);
      Water(9, -3, -6);

      Mountain(-3, 8, -5);
      Plain(-2, 7, -5);
      Plain(-1, 6, -5);
      Plain(0, 5, -5);
      Plain(1, 4, -5);
      Plain(2, 3, -5);
      Plain(3, 2, -5);
      City(4, 1, -5);
      Water(5, 0, -5);
      Water(6, -1, -5);
      Water(7, -2, -5);
      Water(8, -3, -5);

      Mountain(-3, 7, -4);
      Plain(-2, 6, -4);
      Plain(-1, 5, -4);
      Plain(0, 4, -4);
      Plain(1, 3, -4);
      Plain(2, 2, -4);
      Plain(3, 1, -4);
      Plain(4, 0, -4);
      Plain(5, -1, -4);
      Water(6, -2, -4);
      Water(7, -3, -4);
      Water(8, -4, -4);

      Mountain(-4, 7, -3);
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
      Water(7, -4, -3);

      Mountain(-4, 6, -2);
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
      Mountain(7, -5, -2);

      Mountain(-5, 6, -1);
      Plain(-4, 5, -1);
      Plain(-3, 4, -1);
      Plain(-2, 3, -1);
      Plain(-1, 2, -1);
      Plain(0, 1, -1);
      Plain(1, 0, -1);
      Plain(2, -1, -1);
      Mountain(3, -2, -1);
      Plain(4, -3, -1);
      Plain(5, -4, -1);
      Mountain(6, -5, -1);

      Mountain(-5, 5, 0);
      Plain(-4, 4, 0);
      Plain(-3, 3, 0);
      Plain(-2, 2, 0);
      Plain(-1, 1, 0);
      Plain(0, 0, 0);
      Plain(1, -1, 0);
      Mountain(2, -2, 0);
      Plain(3, -3, 0);
      Plain(4, -4, 0);
      Plain(5, -5, 0);
      Mountain(6, -6, 0);

      Mountain(-6, 5, 1);
      Plain(-5, 4, 1);
      Plain(-4, 3, 1);
      Plain(-3, 2, 1);
      Plain(-2, 1, 1);
      Plain(-1, 0, 1);
      Plain(0, -1, 1);
      Plain(1, -2, 1);
      Mountain(2, -3, 1);
      Plain(3, -4, 1);
      Plain(4, -5, 1);
      Mountain(5, -6, 1);

      Mountain(-6, 4, 2);
      Plain(-5, 3, 2);
      Plain(-4, 2, 2);
      Plain(-3, 1, 2);
      Plain(-2, 0, 2);
      Plain(-1, -1, 2);
      Plain(0, -2, 2);
      Mountain(1, -3, 2);
      Plain(2, -4, 2);
      Plain(3, -5, 2);
      Plain(4, -6, 2);
      Mountain(5, -7, 2);

      Mountain(-7, 4, 3);
      Plain(-6, 3, 3);
      Plain(-5, 2, 3);
      Plain(-4, 1, 3);
      Plain(-3, 0, 3);
      Plain(-2, -1, 3);
      Plain(-1, -2, 3);
      Plain(0, -3, 3);
      Mountain(1, -4, 3);
      Plain(2, -5, 3);
      Plain(3, -6, 3);
      Mountain(4, -7, 3);

      Mountain(-7, 3, 4);
      Plain(-6, 2, 4);
      Plain(-5, 1, 4);
      Plain(-4, 0, 4);
      Plain(-3, -1, 4);
      Plain(-2, -2, 4);
      Plain(-1, -3, 4);
      Plain(0, -4, 4);
      Mountain(1, -5, 4);
      Plain(2, -6, 4);
      Plain(3, -7, 4);
      Mountain(4, -8, 4);

      Mountain(-8, 3, 5);
      Plain(-7, 2, 5);
      Plain(-6, 1, 5);
      City(-5, 0, 5);
      Plain(-4, -1, 5);
      Plain(-3, -2, 5);
      Plain(-2, -3, 5);
      Plain(-1, -4, 5);
      Mountain(0, -5, 5);
      Plain(1, -6, 5);
      Plain(2, -7, 5);
      Mountain(3, -8, 5);

      Mountain(-8, 2, 6);
      Plain(-7, 1, 6);
      Plain(-6, 0, 6);
      Plain(-5, -1, 6);
      Plain(-4, -2, 6);
      Plain(-3, -3, 6);
      Plain(-2, -4, 6);
      Plain(-1, -5, 6);
      Mountain(0, -6, 6);
      Plain(1, -7, 6);
      Plain(2, -8, 6);
      Mountain(3, -9, 6);

      Mountain(-9, 2, 7);
      Plain(-8, 1, 7);
      Plain(-7, 0, 7);
      Plain(-6, -1, 7);
      Plain(-5, -2, 7);
      Plain(-4, -3, 7);
      Plain(-3, -4, 7);
      Plain(-2, -5, 7);
      Plain(-1, -6, 7);
      Mountain(0, -7, 7);
      Plain(1, -8, 7);
      Mountain(2, -9, 7);

      Mountain(-9, 1, 8);
      Plain(-8, 0, 8);
      Plain(-7, -1, 8);
      Plain(-6, -2, 8);
      Plain(-5, -3, 8);
      Plain(-4, -4, 8);
      Plain(-3, -5, 8);
      Plain(-2, -6, 8);
      Mountain(-1, -7, 8);
      Plain(0, -8, 8);
      Plain(1, -9, 8);
      Plain(2, -10, 8);

      Mountain(-10, 1, 9);
      Mountain(-9, 0, 9);
      Mountain(-8, -1, 9);
      Mountain(-7, -2, 9);
      Mountain(-6, -3, 9);
      Mountain(-5, -4, 9);
      Mountain(-4, -5, 9);
      Mountain(-3, -6, 9);
      Mountain(-2, -7, 9);
      Mountain(-1, -8, 9);
      Mountain(0, -9, 9);
      Mountain(1, -10, 9);

    }
  }
}

// This file was auto-generated

