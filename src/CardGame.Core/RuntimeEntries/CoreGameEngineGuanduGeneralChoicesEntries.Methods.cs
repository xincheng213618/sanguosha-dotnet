#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralChoicesEntries
{
    internal global::CardGame.Core.PromptChoice GuanduChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().GuanduChoice(f, option, text, cards, targets);
    internal void GuanduDiscardOne(global::CardGame.Core.ProgramSkillFrame f, int id, int owner) => _resolve().GuanduDiscardOne(f, id, owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduForeignGainChoices(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().GuanduForeignGainChoices(f, seat);
    internal void GuanduGiveOne(global::CardGame.Core.ProgramSkillFrame f, int id, int owner, int target) => _resolve().GuanduGiveOne(f, id, owner, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduOwnCardChoices(global::CardGame.Core.ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false) => _resolve().GuanduOwnCardChoices(f, seat, option, discard, allowSkip, nonEquipment, handOnly);
    internal void PublishGuanduGeneralChoice(global::CardGame.Core.ProgramSkillFrame f, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishGuanduGeneralChoice(f, chooser, choices);
    internal void ResolveGuanduGeneralChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveGuanduGeneralChoice(choice);
}
