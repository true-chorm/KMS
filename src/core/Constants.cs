using System.Collections.Generic;
using System.IO;

namespace KMS.src.core
{
    static class Constants
    {

        internal static class TypeNumber
        {
            internal const ushort INVALID = 0;

            internal const byte BACKSPACE = 0x8;
            internal const byte TAB = 0x9;
            internal const byte ENTER = 0xd;
            internal const byte PAUSE_BREAK = 0x13;
            internal const byte CAPS_LOCK = 0x14;
            internal const byte ESC = 0x1B;
            internal const byte SPACE_BAR = 0x20;
            internal const byte PAGE_UP = 0x21;
            internal const byte PAGE_DOWN = 0x22;
            internal const byte END = 0x23;
            internal const byte HOME = 0x24;
            internal const byte LEFT = 0x25;
            internal const byte UP = 0x26;
            internal const byte RIGHT = 0x27;
            internal const byte DOWN = 0x28;
            internal const byte PRTSC = 0x2c;
            internal const byte INSERT = 0x2d;
            internal const byte DELETE = 0x2e;
            internal const byte NUM0 = 0x30;
            internal const byte NUM1 = 0x31;
            internal const byte NUM2 = 0x32;
            internal const byte NUM3 = 0x33;
            internal const byte NUM4 = 0x34;
            internal const byte NUM5 = 0x35;
            internal const byte NUM6 = 0x36;
            internal const byte NUM7 = 0x37;
            internal const byte NUM8 = 0x38;
            internal const byte NUM9 = 0x39;
            internal const byte A = 0x41;
            internal const byte B = 0x42;
            internal const byte C = 0x43;
            internal const byte D = 0x44;
            internal const byte E = 0x45;
            internal const byte F = 0x46;
            internal const byte G = 0x47;
            internal const byte H = 0x48;
            internal const byte I = 0x49;
            internal const byte J = 0x4A;
            internal const byte K = 0x4B;
            internal const byte L = 0x4C;
            internal const byte M = 0x4D;
            internal const byte N = 0x4E;
            internal const byte O = 0x4F;
            internal const byte P = 0x50;
            internal const byte Q = 0x51;
            internal const byte R = 0x52;
            internal const byte S = 0x53;
            internal const byte T = 0x54;
            internal const byte U = 0x55;
            internal const byte V = 0x56;
            internal const byte W = 0x57;
            internal const byte X = 0x58;
            internal const byte Y = 0x59;
            internal const byte Z = 0x5A;
            internal const byte LEFT_WIN = 0x5B;
            internal const byte RIGHT_WIN = 0x5C;
            internal const byte NUMPAD0 = 0x60;
            internal const byte NUMPAD1 = 0x61;
            internal const byte NUMPAD2 = 0x62;
            internal const byte NUMPAD3 = 0x63;
            internal const byte NUMPAD4 = 0x64;
            internal const byte NUMPAD5 = 0x65;
            internal const byte NUMPAD6 = 0x66;
            internal const byte NUMPAD7 = 0x67;
            internal const byte NUMPAD8 = 0x68;
            internal const byte NUMPAD9 = 0x69;
            internal const byte NUMPAD_MULTIPLY = 0x6a;
            internal const byte NUMPAD_ADD = 0x6b;
            internal const byte NUMPAD_ENTER = 0x6c;
            internal const byte NUMPAD_MINUS = 0x6d;
            internal const byte NUMPAD_DOT = 0x6e;
            internal const byte NUMPAD_DIVISION = 0x6f;
            internal const byte F1 = 0x70;
            internal const byte F2 = 0x71;
            internal const byte F3 = 0x72;
            internal const byte F4 = 0x73;
            internal const byte F5 = 0x74;
            internal const byte F6 = 0x75;
            internal const byte F7 = 0x76;
            internal const byte F8 = 0x77;
            internal const byte F9 = 0x78;
            internal const byte F10 = 0x79;
            internal const byte F11 = 0x7A;
            internal const byte F12 = 0x7B;
            internal const byte NUMLOCK = 0x90;
            internal const byte SCRLOCK = 0x91;
            internal const byte LEFT_SHIFT = 0xA0;
            internal const byte RIGHT_SHIFT = 0xA1;
            internal const byte LEFT_CTRL = 0xA2;
            internal const byte RIGHT_CTRL = 0xA3;
            internal const byte LEFT_ALT = 0xA4;
            internal const byte RIGHT_ALT = 0xA5;
            internal const byte PUNCTUATION_L1 = 0xba;   //;
            internal const byte PUNCTUATION_02 = 0xbb;  //=
            internal const byte PUNCTUATION_M1 = 0xbc;   //,
            internal const byte PUNCTUATION_01 = 0xbd;  //-
            internal const byte PUNCTUATION_M2 = 0xbe;  //.
            internal const byte PUNCTUATION_M3 = 0xbf;   // /
            internal const byte PUNCTUATION_11 = 0xc0;   //`
            internal const byte PUNCTUATION_P1 = 0xdb;   //[
            internal const byte PUNCTUATION_P3 = 0xdc;   // \符号键
            internal const byte PUNCTUATION_P2 = 0xdd;   //]
            internal const byte PUNCTUATION_L2 = 0xde;   //'

            internal const ushort KEYBOARD_TOTAL = 0x100;
            internal const ushort KEYBOARD_COMBOL_TOTAL = 0x101;
            internal const ushort MOUSE_TOTAL = 0x102;

            internal const ushort MOUSE_LEFT_BTN = 0x200;
            internal const ushort MOUSE_RIGHT_BTN = 0x201;
            internal const ushort MOUSE_WHEEL_FORWARD = 0x202;
            internal const ushort MOUSE_WHEEL_BACKWARD = 0x203;
            internal const ushort MOUSE_WHEEL_CLICK = 0x204;
            internal const ushort MOUSE_SIDE_FORWARD = 0x205;
            internal const ushort MOUSE_SIDE_BACKWARD = 0x206;

            internal const ushort KB_SK_TOP1 = 0x300;
            internal const ushort KB_SK_TOP2 = 0x301;
            internal const ushort KB_SK_TOP3 = 0x302;
            internal const ushort KB_SK_TOP4 = 0x303;
            internal const ushort KB_SK_TOP5 = 0x304;
            internal const ushort KB_CK_TOP1 = 0x305;
            internal const ushort KB_CK_TOP2 = 0x306;
            internal const ushort KB_CK_TOP3 = 0x307;
            internal const ushort MS_LBTN_AREA_TOP1 = 0x308;
            internal const ushort MS_LBTN_AREA_TOP2 = 0x309;
            internal const ushort MS_LBTN_AREA_TOP3 = 0x30a;
            internal const ushort MS_LBTN_AREA_TOP4 = 0x30b;
            internal const ushort MS_LBTN_AREA_TOP5 = 0x30c;

            internal const ushort MOST_OPERATION_HOUR = 0x400;
        }

        internal static class HookEvent
        {
            internal const byte KEYBOARD_EVENT = 1;
            internal const byte MOUSE_EVENT = 2;
        }

        internal static class KeyEvent
        {
            internal const short WM_KEYDOWN = 0x100;
            internal const short WM_KEYUP = 0x101;
            internal const short WM_SYSKEYDOWN = 0x0104;
            internal const short WM_SYSKEYUP = 0x105;
        }

        internal static readonly Dictionary<byte, Key> Keyboard = new Dictionary<byte, Key>
        {
            {TypeNumber.BACKSPACE,            new Key(TypeNumber.BACKSPACE,              "Backspace",                "Backspace")},
            {TypeNumber.TAB,                  new Key(TypeNumber.TAB,                    "Tab",                      "Tab")},
            {TypeNumber.ENTER,                new Key(TypeNumber.ENTER,                  "Enter",                    "回车键")},
            {TypeNumber.PAUSE_BREAK,          new Key(TypeNumber.PAUSE_BREAK,            "Pause/Break",              "Pause/Break")},
            {TypeNumber.CAPS_LOCK,            new Key(TypeNumber.CAPS_LOCK,              "Caps Lock",                "Caps Lock")},
            {TypeNumber.ESC,                  new Key(TypeNumber.ESC,                    "Esc",                      "Esc")},
            {TypeNumber.SPACE_BAR,            new Key(TypeNumber.SPACE_BAR,              "Space bar",                "空格键")},
            {TypeNumber.PAGE_UP,              new Key(TypeNumber.PAGE_UP,                "Page up",                  "Page up")},
            {TypeNumber.PAGE_DOWN,            new Key(TypeNumber.PAGE_DOWN,              "Page down",                "Page down")},
            {TypeNumber.END,                  new Key(TypeNumber.END,                    "End",                      "End")},
            {TypeNumber.HOME,                 new Key(TypeNumber.HOME,                   "Home",                     "Home")},
            {TypeNumber.LEFT,                 new Key(TypeNumber.LEFT,                   "Left",                     "左方向键")},
            {TypeNumber.UP,                   new Key(TypeNumber.UP,                     "Up",                       "上方向键")},
            {TypeNumber.RIGHT,                new Key(TypeNumber.RIGHT,                  "Right",                    "右方向键")},
            {TypeNumber.DOWN,                 new Key(TypeNumber.DOWN,                   "Down",                     "下方向键")},
            {TypeNumber.PRTSC,                new Key(TypeNumber.PRTSC,                  "PrtSc",                    "PrtSc")},
            {TypeNumber.INSERT,               new Key(TypeNumber.INSERT,                 "Insert",                   "Ins")},
            {TypeNumber.DELETE,               new Key(TypeNumber.DELETE,                 "Delete",                   "Del")},
            {TypeNumber.NUM0,                 new Key(TypeNumber.NUM0,                   "0",                        "0")},
            {TypeNumber.NUM1,                 new Key(TypeNumber.NUM1,                   "1",                        "1")},
            {TypeNumber.NUM2,                 new Key(TypeNumber.NUM2,                   "2",                        "2")},
            {TypeNumber.NUM3,                 new Key(TypeNumber.NUM3,                   "3",                        "3")},
            {TypeNumber.NUM4,                 new Key(TypeNumber.NUM4,                   "4",                        "4")},
            {TypeNumber.NUM5,                 new Key(TypeNumber.NUM5,                   "5",                        "5")},
            {TypeNumber.NUM6,                 new Key(TypeNumber.NUM6,                   "6",                        "6")},
            {TypeNumber.NUM7,                 new Key(TypeNumber.NUM7,                   "7",                        "7")},
            {TypeNumber.NUM8,                 new Key(TypeNumber.NUM8,                   "8",                        "8")},
            {TypeNumber.NUM9,                 new Key(TypeNumber.NUM9,                   "9",                        "9")},
            {TypeNumber.A,                    new Key(TypeNumber.A,                      "A",                        "A")},
            {TypeNumber.B,                    new Key(TypeNumber.B,                      "B",                        "B")},
            {TypeNumber.C,                    new Key(TypeNumber.C,                      "C",                        "C")},
            {TypeNumber.D,                    new Key(TypeNumber.D,                      "D",                        "D")},
            {TypeNumber.E,                    new Key(TypeNumber.E,                      "E",                        "E")},
            {TypeNumber.F,                    new Key(TypeNumber.F,                      "F",                        "F")},
            {TypeNumber.G,                    new Key(TypeNumber.G,                      "G",                        "G")},
            {TypeNumber.H,                    new Key(TypeNumber.H,                      "H",                        "H")},
            {TypeNumber.I,                    new Key(TypeNumber.I,                      "I",                        "I")},
            {TypeNumber.J,                    new Key(TypeNumber.J,                      "J",                        "J")},
            {TypeNumber.K,                    new Key(TypeNumber.K,                      "K",                        "K")},
            {TypeNumber.L,                    new Key(TypeNumber.L,                      "L",                        "L")},
            {TypeNumber.M,                    new Key(TypeNumber.M,                      "M",                        "M")},
            {TypeNumber.N,                    new Key(TypeNumber.N,                      "N",                        "N")},
            {TypeNumber.O,                    new Key(TypeNumber.O,                      "O",                        "O")},
            {TypeNumber.P,                    new Key(TypeNumber.P,                      "P",                        "P")},
            {TypeNumber.Q,                    new Key(TypeNumber.Q,                      "Q",                        "Q")},
            {TypeNumber.R,                    new Key(TypeNumber.R,                      "R",                        "R")},
            {TypeNumber.S,                    new Key(TypeNumber.S,                      "S",                        "S")},
            {TypeNumber.T,                    new Key(TypeNumber.T,                      "T",                        "T")},
            {TypeNumber.U,                    new Key(TypeNumber.U,                      "U",                        "U")},
            {TypeNumber.V,                    new Key(TypeNumber.V,                      "V",                        "V")},
            {TypeNumber.W,                    new Key(TypeNumber.W,                      "W",                        "W")},
            {TypeNumber.X,                    new Key(TypeNumber.X,                      "X",                        "X")},
            {TypeNumber.Y,                    new Key(TypeNumber.Y,                      "Y",                        "Y")},
            {TypeNumber.Z,                    new Key(TypeNumber.Z,                      "Z",                        "Z")},
            {TypeNumber.LEFT_WIN,             new Key(TypeNumber.LEFT_WIN,               "Left win",                 "左Win")},
            {TypeNumber.RIGHT_WIN,            new Key(TypeNumber.RIGHT_WIN,              "Right win",                "右Win")},
            {TypeNumber.NUMPAD0,              new Key(TypeNumber.NUMPAD0,                "Num0",                     "小键盘0")},
            {TypeNumber.NUMPAD1,              new Key(TypeNumber.NUMPAD1,                "Num1",                     "小键盘1")},
            {TypeNumber.NUMPAD2,              new Key(TypeNumber.NUMPAD2,                "Num2",                     "小键盘2")},
            {TypeNumber.NUMPAD3,              new Key(TypeNumber.NUMPAD3,                "Num3",                     "小键盘3")},
            {TypeNumber.NUMPAD4,              new Key(TypeNumber.NUMPAD4,                "Num4",                     "小键盘4")},
            {TypeNumber.NUMPAD5,              new Key(TypeNumber.NUMPAD5,                "Num5",                     "小键盘5")},
            {TypeNumber.NUMPAD6,              new Key(TypeNumber.NUMPAD6,                "Num6",                     "小键盘6")},
            {TypeNumber.NUMPAD7,              new Key(TypeNumber.NUMPAD7,                "Num7",                     "小键盘7")},
            {TypeNumber.NUMPAD8,              new Key(TypeNumber.NUMPAD8,                "Num8",                     "小键盘8")},
            {TypeNumber.NUMPAD9,              new Key(TypeNumber.NUMPAD9,                "Num9",                     "小键盘9")},
            {TypeNumber.NUMPAD_MULTIPLY,      new Key(TypeNumber.NUMPAD_MULTIPLY,        "Num_*",                    "小键盘*")},
            {TypeNumber.NUMPAD_ADD,           new Key(TypeNumber.NUMPAD_ADD,             "Num_+",                    "小键盘+")},
            {TypeNumber.NUMPAD_ENTER,         new Key(TypeNumber.NUMPAD_ENTER,           "Num_Enter",                "小键盘Enter")},
            {TypeNumber.NUMPAD_MINUS,         new Key(TypeNumber.NUMPAD_MINUS,           "Num_-",                    "小键盘-")},
            {TypeNumber.NUMPAD_DOT,           new Key(TypeNumber.NUMPAD_DOT,             "Num_.",                    "小键盘.")},
            {TypeNumber.NUMPAD_DIVISION,      new Key(TypeNumber.NUMPAD_DIVISION,        "Num_/",                    "小键盘/")},
            {TypeNumber.F1,                   new Key(TypeNumber.F1,                     "F1",                       "F1")},
            {TypeNumber.F2,                   new Key(TypeNumber.F2,                     "F2",                       "F2")},
            {TypeNumber.F3,                   new Key(TypeNumber.F3,                     "F3",                       "F3")},
            {TypeNumber.F4,                   new Key(TypeNumber.F4,                     "F4",                       "F4")},
            {TypeNumber.F5,                   new Key(TypeNumber.F5,                     "F5",                       "F5")},
            {TypeNumber.F6,                   new Key(TypeNumber.F6,                     "F6",                       "F6")},
            {TypeNumber.F7,                   new Key(TypeNumber.F7,                     "F7",                       "F7")},
            {TypeNumber.F8,                   new Key(TypeNumber.F8,                     "F8",                       "F8")},
            {TypeNumber.F9,                   new Key(TypeNumber.F9,                     "F9",                       "F9")},
            {TypeNumber.F10,                  new Key(TypeNumber.F10,                    "F10",                      "F10")},
            {TypeNumber.F11,                  new Key(TypeNumber.F11,                    "F11",                      "F11")},
            {TypeNumber.F12,                  new Key(TypeNumber.F12,                    "F12",                      "F12")},
            {TypeNumber.NUMLOCK,              new Key(TypeNumber.NUMLOCK,                "Num Lock",                 "Num Lock")},
            {TypeNumber.SCRLOCK,              new Key(TypeNumber.SCRLOCK,                "Scroll Lock",              "Scroll Lock")},
            {TypeNumber.LEFT_SHIFT,           new Key(TypeNumber.LEFT_SHIFT,             "Left Shift",               "左Shift")},
            {TypeNumber.RIGHT_SHIFT,          new Key(TypeNumber.RIGHT_SHIFT,            "Right Shift",              "右Shift")},
            {TypeNumber.LEFT_CTRL,            new Key(TypeNumber.LEFT_CTRL,              "Left Ctrl",                "左Ctrl")},
            {TypeNumber.RIGHT_CTRL,           new Key(TypeNumber.RIGHT_CTRL,             "Right Ctrl",               "右Ctrl")},
            {TypeNumber.LEFT_ALT,             new Key(TypeNumber.LEFT_ALT,               "Left Alt",                 "左Alt")},
            {TypeNumber.RIGHT_ALT,            new Key(TypeNumber.RIGHT_ALT,              "Right Alt",                "右Alt")},
            {TypeNumber.PUNCTUATION_L1,       new Key(TypeNumber.PUNCTUATION_L1,         ";",                        ";")},
            {TypeNumber.PUNCTUATION_02,       new Key(TypeNumber.PUNCTUATION_02,         "=",                        "=")},
            {TypeNumber.PUNCTUATION_M1,       new Key(TypeNumber.PUNCTUATION_M1,         ",",                        ",")},
            {TypeNumber.PUNCTUATION_01,       new Key(TypeNumber.PUNCTUATION_01,         "-",                        "-")},
            {TypeNumber.PUNCTUATION_M2,       new Key(TypeNumber.PUNCTUATION_M2,         ".",                        ".")},
            {TypeNumber.PUNCTUATION_M3,       new Key(TypeNumber.PUNCTUATION_M3,         "/",                        "/")},
            {TypeNumber.PUNCTUATION_11,       new Key(TypeNumber.PUNCTUATION_11,         "`",                        "`")},
            {TypeNumber.PUNCTUATION_P1,       new Key(TypeNumber.PUNCTUATION_P1,         "[",                        "[")},
            {TypeNumber.PUNCTUATION_P3,       new Key(TypeNumber.PUNCTUATION_P3,         "\\",                       "\\")},
            {TypeNumber.PUNCTUATION_P2,       new Key(TypeNumber.PUNCTUATION_P2,         "]",                        "]")},
            {TypeNumber.PUNCTUATION_L2,       new Key(TypeNumber.PUNCTUATION_L2,         "'",                        "'")}
        };

        internal static class MouseEvent
        {
            internal const int WM_MOUSEMOVE = 0x200;
            internal const int WM_LBUTTONDOWN = 0x201;
            internal const int WM_LBUTTONUP = 0x202;
            internal const int WM_RBUTTONDOWN = 0x204;
            internal const int WM_RBUTTONUP = 0x205;
            internal const int WM_WHEEL_CLK_DOWN = 0x207;
            internal const int WM_WHEEL_CLK_UP = 0x208;
            internal const int WM_MOUSEWHEEL = 0x20a;
            internal const int WM_MOUSESIDEDOWN = 0x20b; //鼠标侧键按下事件（猜测）。
            internal const int WM_MOUSESIDEUP = 0x20c; //鼠标侧键抬起事件（猜测）。
            internal const int WM_MOUSEHWHEEL = 0x20e; //水平方向的滚动，一般鼠标没有这个事件。
        }

        internal static readonly Dictionary<ushort, Type> MouseKeys = new Dictionary<ushort, Type>
        {
            {TypeNumber.MOUSE_LEFT_BTN,                 new Type(TypeNumber.MOUSE_LEFT_BTN,         "鼠标左键") },
            {TypeNumber.MOUSE_RIGHT_BTN,                new Type(TypeNumber.MOUSE_RIGHT_BTN,        "鼠标右键")},
            {TypeNumber.MOUSE_SIDE_FORWARD,             new Type(TypeNumber.MOUSE_SIDE_FORWARD,     "鼠标前侧键")},
            {TypeNumber.MOUSE_SIDE_BACKWARD,            new Type(TypeNumber.MOUSE_SIDE_BACKWARD,    "鼠标后侧键")},
            {TypeNumber.MOUSE_WHEEL_FORWARD,            new Type(TypeNumber.MOUSE_WHEEL_FORWARD,    "鼠标滑轮前滑")},
            {TypeNumber.MOUSE_WHEEL_BACKWARD,           new Type(TypeNumber.MOUSE_WHEEL_BACKWARD,   "鼠标滑轮后滑")},
            {TypeNumber.MOUSE_WHEEL_CLICK,              new Type(TypeNumber.MOUSE_WHEEL_CLICK,      "鼠标滑轮点击")},
        };

        internal static class MouseDataHighOrder
        {
            internal const int SIDE_BACKWARD = 0x1; //鼠标侧键后退标志。high-order in DWORD
            internal const int SIDE_FORWARD = 0x2; //鼠标侧键前标志。high-order in DWORD
        }

        internal static class Statistic
        {
            internal static Type KbTotal = new Type(TypeNumber.KEYBOARD_TOTAL, "键盘事件");
            internal static Type KbComboTotal = new Type(TypeNumber.KEYBOARD_COMBOL_TOTAL, "键盘组合键事件");
            internal static Type KbSkTop1 = new Type(TypeNumber.KB_SK_TOP1, "Single key top1");
            internal static Type KbSkTop2 = new Type(TypeNumber.KB_SK_TOP2, "Single key top2");
            internal static Type KbSkTop3 = new Type(TypeNumber.KB_SK_TOP3, "Single key top3");
            internal static Type KbSkTop4 = new Type(TypeNumber.KB_SK_TOP3, "Single key top4");
            internal static Type KbSkTop5 = new Type(TypeNumber.KB_SK_TOP3, "Single key top5");
            internal static Type KbTotalToday = new Type(TypeNumber.KEYBOARD_TOTAL, "键盘事件");
            internal static Type MsTotalToday = new Type(TypeNumber.MOUSE_TOTAL, "鼠标事件");
            internal static Type KbLetterTop1Today = new Type(TypeNumber.KB_SK_TOP1, "今日键入最多字母");
            internal static Type MostOpHourToday = new Type(TypeNumber.MOST_OPERATION_HOUR, "今日操作最多的时段");
        }
    }
}
