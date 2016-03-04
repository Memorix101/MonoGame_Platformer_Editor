using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace RogueLikePlatfromer
{
    public static class ControlInput
    {
        public static KeyboardState keyState;
        public static KeyboardState lastkeyState;

        public static void Update(KeyboardState state)
        {
            lastkeyState = keyState;
            keyState = state;
        }

        public static bool IsKeyPressed(Keys keys)
        {
            if (keyState.IsKeyDown(keys) && lastkeyState.IsKeyUp(keys))
            {
                return true;
            }

            return false;
        }
    }
}
