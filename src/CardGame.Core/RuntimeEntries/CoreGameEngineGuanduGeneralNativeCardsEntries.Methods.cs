#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralNativeCardsEntries
{
    internal void CaptureGuanduDamagingCards(IGameEvent payload) => _resolve().CaptureGuanduDamagingCards(payload);
    internal void ContinueGuanduChoulue(ProgramSkillFrame f) => _resolve().ContinueGuanduChoulue(f);
    internal void ContinueGuanduMoushi(ProgramSkillFrame f) => _resolve().ContinueGuanduMoushi(f);
    internal void ContinueGuanduYuanlve(ProgramSkillFrame f) => _resolve().ContinueGuanduYuanlve(f);
    internal bool FinishGuanduYuanlveUse(ProgramSkillFrame f) => _resolve().FinishGuanduYuanlveUse(f);
    internal CardKind? GuanduChoulueFrozenBasicKind(ProgramSkillFrame f) => _resolve().GuanduChoulueFrozenBasicKind(f);
    internal IReadOnlyList<PromptChoice> GuanduGiftChoices(ProgramSkillFrame f, bool nonEquipment) => _resolve().GuanduGiftChoices(f, nonEquipment);
    internal CardKind? GuanduLastDamageCard(int seat) => _resolve().GuanduLastDamageCard(seat);
    internal GuanduMoushiGiftedEvent? GuanduMoushiOccurrence(int owner, string instance, ProgramSkillWindowContext c) => _resolve().GuanduMoushiOccurrence(owner, instance, c);
    internal bool GuanduMoushiQualifies(int owner, string instance, ProgramSkillWindowContext c) => _resolve().GuanduMoushiQualifies(owner, instance, c);
    internal bool IsGuanduChoulueVirtualOrdinaryProgram(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().IsGuanduChoulueVirtualOrdinaryProgram(f, effect);
    internal bool ValidGuanduYuanlveOffer(ProgramSkillFrame f) => _resolve().ValidGuanduYuanlveOffer(f);
}
