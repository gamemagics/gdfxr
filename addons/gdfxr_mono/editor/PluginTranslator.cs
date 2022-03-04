using Godot;
using System;
using System.Collections.Generic;

public class PluginTranslator : Node {
    private EditorPlugin plugin;

    private Translation translation;

    public EditorPlugin Plugin {
        set {
            if (plugin == value) {
                return;
            }

            plugin = value;
            if (value == null) {
                translation = null;
                return;
            }

            string locale = plugin.GetEditorInterface()
                .GetEditorSettings().
                Get("interface/editor/editor_language") as string;

            Script script = GetScript() as Script;
            string path = script.ResourcePath.GetBaseDir().PlusFile(String.Format("translations/{0}.po", locale));

            if (ResourceLoader.Exists(path)) {
                translation = ResourceLoader.Load<Translation>(path);
            }

            if (translation != null) {
                TranslateNode(GetParent());
            }
        }
        get {
            return plugin;
        }
    }

    private string tr(string messgae) {
        if (translation != null) {
            string translated = translation.GetMessage(messgae);
            if (!translated.Empty()) {
                return translated;
            }
        }

        return messgae;
    }

    private void TranslateNode(Node node) {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(node);

        while (queue.Count > 0) {
            Node current = queue.Dequeue();
            if (current is Control) {
                Control control = current as Control;
                control.HintTooltip = tr(control.HintTooltip);
            }

            // TODO:
            // if (current is HBoxContainer && current.HasMethod("")) {
            //     HBoxContainer container = current as HBoxContainer;
                
            // }

            if (current is Button && !(current is OptionButton)) {
                Button button = current as Button;
                button.Text = tr(button.Text);
            }

            if (current is Label) {
                Label label = current as Label;
                label.Text = tr(label.Text);
            }

            foreach (var child in current.GetChildren()) {
                queue.Enqueue(child as Node);
            }
        }
    }
}
