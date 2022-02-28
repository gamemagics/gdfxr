#if TOOLS

using Godot;
using System;

[Tool]
public class Plugin : EditorPlugin {
    public override void _EnterTree() {
        base._EnterTree();
    }

    public override void _ExitTree() {
        base._ExitTree();
    }
}

#endif // TOOLS