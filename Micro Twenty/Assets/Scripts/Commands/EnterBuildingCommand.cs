using System;

namespace MicroTwenty
{
    public class EnterBuildingCommand : Command
    {
        private string _name;
        private BuildingTrigger.BuildingType _buildingType;

        public EnterBuildingCommand (string name, GameMgr gameMgr, BuildingTrigger.BuildingType buildingType) : base (name, gameMgr)
        {
            _name = name;
            _buildingType = buildingType;
        }

        public override void Update (float deltaSeconds)
        {
            gameMgr.EnterBuilding (_name, _buildingType);
            this.isDone = true;
        }
    }
}
