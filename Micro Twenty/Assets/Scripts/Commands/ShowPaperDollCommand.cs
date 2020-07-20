using System;
namespace MicroTwenty
{
    public class ShowPaperDollCommand:Command
    {
        private GameMgr _gameMgr;
        private Character _character;

        public ShowPaperDollCommand (Character c, GameMgr gameMgr) : base(string.Format("Show Paper Doll for {0}", c.Name), gameMgr)
        {
            _gameMgr = gameMgr;
            _character = c;
        }

        public override void Update (float deltaSeconds)
        {
            _gameMgr.ShowPaperDoll (_character);
            isDone = true;
        }
    }
}
