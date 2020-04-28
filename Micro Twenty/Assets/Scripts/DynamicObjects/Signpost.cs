using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicroTwenty
{
    public class Signpost : DynamicObject
    {
        private GameMgr _gameMgr;
        private HexCoord _hexCoord;
        private string _name;
        private List<string> _msg;

        public Signpost (GameMgr gameMgr, HexCoord hexCoord, string name, List<string> message) : base (gameMgr, hexCoord, DynamicObjectType.SIGN, false)
        {
            _gameMgr = gameMgr;
            _hexCoord = hexCoord;
            _name = name;
            _msg = message;
        }

        public override void OnMoveOver ()
        {
            // show our message

            Debug.Log ("Signpost says: " + _msg);

            _gameMgr.AddCommand (new DialogCommand (_gameMgr, _name, _msg, _gameMgr.GetMapManager().GetSignBitmap()));
        }

    }
}
