using System;
using UnityEngine;

namespace MicroTwenty
{
    public class BuildingTrigger : DynamicObject
    {
        public enum BuildingType
        {
            NONE,
            HEALING,
            PUB,
            MAGIC,
            WEAPONS,
            ARMOR,
            EQUIPMENT,
            GUILD,
            ADVANCEMENT,

            // not really buildings, but similar UI
            PARTY,
            PAPERDOLL,
        };

        private string _name;
        private BuildingType _buildingType;

        public BuildingTrigger (GameMgr gameMgr, HexCoord hexCoord, string name, BuildingType buildingType) : base (gameMgr, hexCoord, DynamicObjectType.TRIGGER, false)
        {
            this._name = name;
            this._buildingType = buildingType;
        }

        public override void OnMoveOver ()
        {
            Debug.LogFormat ("I've been moved over! {0}", _name);
            gameMgr.AddCommand (new EnterBuildingCommand (_name, gameMgr, _buildingType));
        }
    }
}
