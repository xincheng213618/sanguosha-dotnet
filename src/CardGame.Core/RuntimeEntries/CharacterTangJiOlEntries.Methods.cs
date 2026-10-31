#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangJiOlEntries
{
    internal int? GetKanggeMarkedSeat(int ownerSeat) => _resolve().GetKanggeMarkedSeat(ownerSeat);
    internal int GiftJielieSuitCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.JielieGiftState state, int amount) => _resolve().GiftJielieSuitCards(frame, state, amount);
    internal bool HasJielieGiftDamageObserver(long beforeDamageId) => _resolve().HasJielieGiftDamageObserver(beforeDamageId);
    internal bool HasJielieGiftDying(long beforeDamageId) => _resolve().HasJielieGiftDying(beforeDamageId);
    internal bool IsJielieGiftProgramDying() => _resolve().IsJielieGiftProgramDying();
    internal bool IsKanggeMarkedSeat(int seat, int ownerSeat) => _resolve().IsKanggeMarkedSeat(seat, ownerSeat);
    internal global::CardGame.Core.ProgramSkillFrame? JieliePaidObserverRoot(long beforeDamageId) => _resolve().JieliePaidObserverRoot(beforeDamageId);
    internal global::CardGame.Core.SkillProgramStepOutcome JielieProgramPreventAndGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().JielieProgramPreventAndGift(frame, effect);
    internal bool KanggeHealUsedThisRound(int ownerSeat) => _resolve().KanggeHealUsedThisRound(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome KanggeProgramChooseTarget(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().KanggeProgramChooseTarget(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome KanggeProgramDeathPrice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().KanggeProgramDeathPrice(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome KanggeProgramGainDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().KanggeProgramGainDraw(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome KanggeProgramHealVictim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().KanggeProgramHealVictim(frame, effect);
    internal bool? ProveKanggeMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.CharacterState owner) => _resolve().ProveKanggeMovement(trigger, context, owner);
    internal void ResolveJielieSuitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveJielieSuitChoice(selected);
    internal void ResolveKanggeChooseChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveKanggeChooseChoice(selected);
    internal void ResolveKanggeHealChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveKanggeHealChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiJielieSuitChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiJielieSuitChoice(decision, frame);
    internal global::CardGame.Core.PromptChoice SelectAiKanggeChooseChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiKanggeChooseChoice(decision);
}
