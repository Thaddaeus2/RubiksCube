using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace RubiksCube
{
    public static class KeyBindings
    {
        public enum KeyMap
        {
            PanLeft = KEY_A,
            PanRight = KEY_D,
            PanUp = KEY_W,
            PanDown = KEY_S,
            PanIn = KEY_UP,
            PanOut = KEY_DOWN,
            ResetCam = KEY_SPACE,
            Randomize = KEY_R,
            FMove = KEY_KP_5,
            RMove = KEY_KP_6,
            UMove = KEY_KP_8,
            LMove = KEY_KP_4,
            BMove = KEY_KP_9,
            DMove = KEY_KP_2,
            MMove = KEY_KP_7,
            EMove = KEY_KP_3,
            SMove = KEY_KP_1,
            Scramble= KEY_ENTER,
            Test = KEY_T,
            AltBind1 = KEY_LEFT_ALT,
            AltBind2 = KEY_LEFT_SHIFT,
            QuickSwap1 = KEY_ONE,
            QuickSwap2 = KEY_TWO,
            QuickSwap3 = KEY_THREE,
            QuickSwap4 = KEY_FOUR,
            QuickSwap5 = KEY_FIVE,
            QuickSwap6 = KEY_SIX,
            QuickSwap7 = KEY_SEVEN,
            QuickSwap8 = KEY_EIGHT,
            QuickSwap9 = KEY_NINE,
        };

        public static bool PanLeft() => IsKeyPressed((KeyboardKey) KeyMap.PanLeft);
        public static bool PanRight() => IsKeyPressed((KeyboardKey) KeyMap.PanRight);
        public static bool PanUp() => IsKeyPressed((KeyboardKey) KeyMap.PanUp);
        public static bool PanDown() => IsKeyPressed((KeyboardKey) KeyMap.PanDown);
        public static bool PanIn() => IsKeyDown((KeyboardKey) KeyMap.PanIn);
        public static bool PanOut() => IsKeyDown((KeyboardKey) KeyMap.PanOut);
        public static bool ResetCam() => IsKeyPressed((KeyboardKey) KeyMap.ResetCam);
        public static bool Randomize() => IsKeyPressed((KeyboardKey) KeyMap.Randomize);
        public static bool Test() => IsKeyPressed((KeyboardKey) KeyMap.Test);
        public static bool FMove() => IsKeyPressed((KeyboardKey) KeyMap.FMove);
        public static bool RMove() => IsKeyPressed((KeyboardKey) KeyMap.RMove);
        public static bool UMove() => IsKeyPressed((KeyboardKey) KeyMap.UMove);
        public static bool LMove() => IsKeyPressed((KeyboardKey) KeyMap.LMove);
        public static bool BMove() => IsKeyPressed((KeyboardKey) KeyMap.BMove);
        public static bool DMove() => IsKeyPressed((KeyboardKey) KeyMap.DMove);
        public static bool MMove() => IsKeyPressed((KeyboardKey) KeyMap.MMove);
        public static bool EMove() => IsKeyPressed((KeyboardKey) KeyMap.EMove);
        public static bool SMove() => IsKeyPressed((KeyboardKey) KeyMap.SMove);
        public static bool Scramble() => IsKeyPressed((KeyboardKey) KeyMap.Scramble);
        public static bool AltBind() => (IsKeyDown((KeyboardKey) KeyMap.AltBind1) || IsKeyDown((KeyboardKey) KeyMap.AltBind2));
        public static bool QuickSwap1() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap1) && AltBind();
        public static bool QuickSwap2() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap2) && AltBind();
        public static bool QuickSwap3() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap3) && AltBind();
        public static bool QuickSwap4() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap4) && AltBind();
        public static bool QuickSwap5() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap5) && AltBind();
        public static bool QuickSwap6() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap6) && AltBind();
        public static bool QuickSwap7() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap7) && AltBind();
        public static bool QuickSwap8() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap8) && AltBind();
        public static bool QuickSwap9() => IsKeyPressed((KeyboardKey) KeyMap.QuickSwap9) && AltBind();
    }
}
