using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class NarrativeMsg : DynamicObject
    {
        private GameMgr _gameMgr;
        private HexCoord _hexCoord;
        private string _name;
        private List<string> _msg;
        private Texture2D _icon;

        public NarrativeMsg (GameMgr gameMgr, HexCoord hexCoord, string name, List<string> message) : base (gameMgr, hexCoord, DynamicObjectType.NARRATIVEMSG, false)
        {
            _gameMgr = gameMgr;
            _hexCoord = hexCoord;
            _name = name;
            _msg = message;
            _icon = null;
        }

        public override void OnMoveOver ()
        {
            // show our message

            Debug.Log ("Narrative msg says: " + _msg);

            _gameMgr.AddCommand (new DialogCommand (_gameMgr, _name, _msg, _icon));
        }

    }
}
