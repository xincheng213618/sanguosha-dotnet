#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralNativeCardsEntries
{
    internal void CaptureGuanduDamagingCards(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureGuanduDamagingCards(payload);
    internal void ContinueGuanduChoulue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduChoulue(f);
    internal void ContinueGuanduMoushi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduMoushi(f);
    internal void ContinueGuanduYuanlve(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduYuanlve(f);
    internal bool FinishGuanduYuanlveUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishGuanduYuanlveUse(f);
    internal global::CardGame.Core.CardKind? GuanduChoulueFrozenBasicKind(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GuanduChoulueFrozenBasicKind(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuanduGiftChoices(global::CardGame.Core.ProgramSkillFrame f, bool nonEquipment) => _resolve().GuanduGiftChoices(f, nonEquipment);
    internal global::CardGame.Core.CardKind? GuanduLastDamageCard(int seat) => _resolve().GuanduLastDamageCard(seat);
    internal global::CardGame.Core.GuanduMoushiGiftedEvent? GuanduMoushiOccurrence(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().GuanduMoushiOccurrence(owner, instance, c);
    internal bool GuanduMoushiQualifies(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().GuanduMoushiQualifies(owner, instance, c);
    internal bool IsGuanduChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().IsGuanduChoiceResult(f, binding);
    internal bool IsGuanduChoulueVirtualOrdinaryProgram(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().IsGuanduChoulueVirtualOrdinaryProgram(f, effect);
    internal bool ValidGuanduYuanlveOffer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGuanduYuanlveOffer(f);
}
