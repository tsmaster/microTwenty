using System;
using UnityEngine;

namespace MicroTwenty
{
    public abstract class SelectionMode
    {
        public abstract bool IsLocationSelectable (HexCoord hc);

        public abstract SpriteId GetCursorSpriteIdForLoc (HexCoord hc, out Color spriteColor);

        public Action<HexCoord> OnLocationSelected;

        public Action OnCancel;

        public HexCoord CursorPos { get; set; }
    }
}
