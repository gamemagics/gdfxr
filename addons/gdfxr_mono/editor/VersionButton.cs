using Godot;
using System;

public class VersionButton : LinkButton {

    [Export]
    private string website;

    private EditorPlugin plugin;

    public void OnVersionButtonPressed() {
        if (website != null && !website.Empty()) {
            OS.ShellOpen(website);
        }
    }

    public EditorPlugin Plugin {
        set {
            plugin = value;

            Script script = GetScript() as Script;
            if (script != null) {
                var path = script.ResourcePath.GetBaseDir().PlusFile("../plugin.cfg");
                var cfg = new ConfigFile();
                var err = cfg.Load(path);

                if (err == Error.Ok) {
                    this.Text = String.Format("{0} v{1}",
                        cfg.GetValue("plugin", "name", "plugin"),
                        cfg.GetValue("plugin", "version", "1.0"));
                }
                else {
                    GD.PrintErr("Can't load configuration file ../plugin.cfg");
                }
            }
        }
        get {
            return plugin;
        }
    }
}
