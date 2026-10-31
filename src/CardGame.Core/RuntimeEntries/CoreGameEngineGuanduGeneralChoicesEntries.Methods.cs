#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralChoicesEntries
{
    internal PromptChoice GuanduChoice(ProgramSkillFrame f, string option, string text, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().GuanduChoice(f, option, text, cards, targets);
    internal void GuanduDiscardOne(ProgramSkillFrame f, int id, int owner) => _resolve().GuanduDiscardOne(f, id, owner);
    internal IReadOnlyList<PromptChoice> GuanduForeignGainChoices(ProgramSkillFrame f, int seat) => _resolve().GuanduForeignGainChoices(f, seat);
    internal void GuanduGiveOne(ProgramSkillFrame f, int id, int owner, int target) => _resolve().GuanduGiveOne(f, id, owner, target);
    internal IReadOnlyList<PromptChoice> GuanduOwnCardChoices(ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false) => _resolve().GuanduOwnCardChoices(f, seat, option, discard, allowSkip, nonEquipment, handOnly);
    internal void PublishGuanduGeneralChoice(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices) => _resolve().PublishGuanduGeneralChoice(f, chooser, choices);
    internal void ResolveGuanduGeneralChoice(PromptChoice choice) => _resolve().ResolveGuanduGeneralChoice(choice);
}
