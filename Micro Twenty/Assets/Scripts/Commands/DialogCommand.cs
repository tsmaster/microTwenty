using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class DialogCommand : Command
    {
        private string _title;
        private List<string> _message;
        private DialogBox _dialogBox;

        public DialogCommand (GameMgr gameMgr, string title, List<string> message, Texture2D icon) : base (title, gameMgr)
        {
            _title = title;
            _message = message;

            _dialogBox = new DialogBox (gameMgr.GetMapManager (), title, message, icon);
        }

        public override void Update (float deltaSeconds)
        {
            if (!isDone) {
                _dialogBox.Draw ();

                _dialogBox.Update (deltaSeconds, out isDone);
            }
        }
    }

}
