using System;
namespace MicroTwenty
{
    public abstract class Command
    {
        protected GameMgr gameMgr;
        protected bool isDone;

        public Command (string name, GameMgr gameMgr)
        {
            this.gameMgr = gameMgr;
            this.isDone = false;
        }

        public virtual bool IsDone ()
        {
            return isDone;
        }

        public abstract void Update (float deltaSeconds);
    }
}
