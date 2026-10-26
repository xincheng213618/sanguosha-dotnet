using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class CustomGeneralEditorChecks
{
    public static void VisualRulesSaveReopenAndErrorsKeepDraft(string output)
    {
        var registry = StandardContentRegistry.CreateWithClassicGenerals();
        var store = new MemoryCustomGeneralLibraryStore(); var callbacks = 0;
        var model = new CustomGeneralEditorViewModel(registry, store, _ => callbacks++);
        var window = new CustomGeneralEditorWindow(model); window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            Program.Render(root, 1360, 820, null);
            Click((Button)window.FindName("NewGeneralButton")); var generalId = model.SelectedGeneral!.Id;
            Program.Render(root, 1360, 820, null);
            var name = (TextBox)window.FindName("GeneralNameText"); SetText(name, "自捏将领");
            Program.Assert(model.SelectedGeneral.Name == "自捏将领" && generalId.StartsWith("custom:general-", StringComparison.Ordinal),
                "The real name binding edits a stable custom identity rather than generating a new general ID on rename.");
            SetText((TextBox)window.FindName("ExistingSkillSearchText"), "classic:paoxiao");
            Program.Render(root, 1360, 820, null);
            var existing = Program.Find<CheckBox>(root).Single(c => Equals(c.Tag, "classic:paoxiao"));
            existing.SetCurrentValue(CheckBox.IsCheckedProperty, true); existing.GetBindingExpression(CheckBox.IsCheckedProperty)!.UpdateSource();
            Program.Assert(model.SelectedGeneral.ExistingSkillIds.SequenceEqual(["classic:paoxiao"]),
                "Searching and checking a real existing skill retains its original ID without cloning its definition.");
            Click((Button)window.FindName("AddCustomSkillButton")); Program.Render(root, 1360, 820, null);
            SetText((TextBox)window.FindName("CustomSkillNameText"), "回响");
            var trigger = (ComboBox)window.FindName("CustomTriggerCombo");
            Select(trigger, CustomSkillTrigger.AfterDamage);
            Select((ComboBox)window.FindName("CustomTargetCombo"), CustomSkillTarget.Other);
            Select((ComboBox)window.FindName("CustomConditionCombo"), CustomSkillCondition.Wounded);
            model.AddEffectCommand.Execute(null);
            var rule = model.SelectedCustomSkill!; var damage = rule.Effects[1];
            damage.Kind = CustomSkillEffectKind.Damage; damage.Amount = 2; damage.DamageNature = DamageNature.Fire;
            model.MoveEffectUpCommand.Execute(damage);
            Program.Render(root, 1360, 820, Path.Combine(output, "custom-general-visual-rules.png"));
            var selectedText = Program.Find<TextBlock>(trigger).FirstOrDefault(t => t.Text == "受到伤害后");
            Program.Assert(rule.Trigger == CustomSkillTrigger.AfterDamage && rule.Target == CustomSkillTarget.Other && rule.Condition == CustomSkillCondition.Wounded &&
                rule.Effects.Select(e => e.Kind).SequenceEqual([CustomSkillEffectKind.Damage, CustomSkillEffectKind.Draw]) &&
                selectedText?.Foreground is SolidColorBrush brush && brush.Color.R < 160 && brush.Color.G < 160 && brush.Color.B < 160 &&
                ((TextBox)window.FindName("CustomRulePreviewText")).Text == model.Preview && model.Preview.Contains("回响", StringComparison.Ordinal) && !model.HasErrors,
                "Real enum selectors, ordered effects and a readable selected label feed the generated rule preview without a script editor.");
            var customId = rule.Id;
            Click((Button)window.FindName("SaveCustomLibraryButton"));
            var saved = store.Read().Generals.Single();
            Program.Assert(callbacks == 1 && !model.HasUnsavedChanges && !model.HasErrors && saved.Id == generalId &&
                saved.SkillIds.SequenceEqual(["classic:paoxiao"]) && saved.CustomSkills is [var frozen] && frozen.Id == customId &&
                frozen.Effects is [{ Kind: CustomSkillEffectKind.Damage, Amount: 2, DamageNature: DamageNature.Fire }, { Kind: CustomSkillEffectKind.Draw, Amount: 1 }] &&
                CustomGeneralContent.CompileSkill(frozen).Program is not null,
                "Saving validates an executable skill program and persists the exact existing ID, custom identity and ordered numeric effects.");
            var reopened = new CustomGeneralEditorViewModel(registry, store);
            Program.Assert(reopened.Generals.Count == 1 && reopened.SelectedGeneral!.Id == generalId && reopened.SelectedCustomSkill!.Id == customId &&
                reopened.SelectedCustomSkill.Effects[0].DamageNature == DamageNature.Fire,
                "Reopening the library restores the same editable identities and real effect values.");
            model.DuplicateCommand.Execute(null);
            Program.Assert(model.Generals.Count == 2 && model.SelectedGeneral!.Id != generalId && model.SelectedCustomSkill!.Id != customId &&
                model.SelectedGeneral.ExistingSkillIds.SequenceEqual(["classic:paoxiao"]),
                "Copying creates new custom identities while sharing selected existing skills.");
            model.DeleteCommand.Execute(null); Program.Assert(model.Generals.Count == 1 && model.SelectedGeneral!.Id == generalId,
                "Deleting a staged copy keeps the saved source general available.");
            Program.Render(root, 1120, 760, null);
            SetText(name, string.Empty); Click((Button)window.FindName("SaveCustomLibraryButton"));
            Program.Render(root, 1120, 760, Path.Combine(output, "custom-general-validation-errors.png"));
            Program.Assert(model.HasErrors && model.HasUnsavedChanges && model.SelectedGeneral!.Name.Length == 0 &&
                ((FrameworkElement)window.FindName("CustomEditorErrors")).Visibility == Visibility.Visible &&
                ReferenceEquals(window.DataContext, model) && store.Read().Generals.Single().Name == "自捏将领" && callbacks == 1,
                "An invalid draft remains in the same editor with visible errors; it does not overwrite the saved library or invoke the new-game callback.");
            var failing = new CustomGeneralEditorViewModel(registry, new FailingStore(store));
            failing.SelectedGeneral!.Name = "写入失败仍保留"; failing.SaveCommand.Execute(null);
            Program.Assert(failing.HasErrors && failing.Errors.Any(e => e.Contains("测试写入失败", StringComparison.Ordinal)) &&
                failing.HasUnsavedChanges && failing.SelectedGeneral.Name == "写入失败仍保留" && store.Read().Generals.Single().Name == "自捏将领",
                "A storage failure exposes the real error and preserves both the editable draft and previously saved library.");
        }
        finally { window.Content = null; window.Close(); }
    }

    private static void SetText(TextBox text, string value)
    { text.SetCurrentValue(TextBox.TextProperty, value); text.GetBindingExpression(TextBox.TextProperty)!.UpdateSource(); }
    private static void Select(ComboBox combo, object value)
    { combo.SetCurrentValue(ComboBox.SelectedValueProperty, value); combo.GetBindingExpression(ComboBox.SelectedValueProperty)!.UpdateSource(); }
    private static void Click(Button button)
    { Program.Assert(button.Command?.CanExecute(button.CommandParameter) == true, "The real editor button must have an enabled bound command."); button.Command!.Execute(button.CommandParameter); }
    private sealed class FailingStore(ICustomGeneralLibraryStore inner) : ICustomGeneralLibraryStore
    {
        public CustomGeneralLibrary Read() => inner.Read();
        public void Write(CustomGeneralLibrary library) => throw new IOException("测试写入失败");
    }
}
