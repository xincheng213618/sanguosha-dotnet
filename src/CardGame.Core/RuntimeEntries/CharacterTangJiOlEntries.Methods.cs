#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangJiOlEntries
{
    internal int? GetKanggeMarkedSeat(int ownerSeat) => _resolve().GetKanggeMarkedSeat(ownerSeat);
    internal int GiftJielieSuitCards(ProgramSkillFrame frame, JielieGiftState state, int amount) => _resolve().GiftJielieSuitCards(frame, state, amount);
    internal bool HasJielieGiftDying(long beforeDamageId) => _resolve().HasJielieGiftDying(beforeDamageId);
    internal bool IsJielieGiftProgramDying() => _resolve().IsJielieGiftProgramDying();
    internal bool IsKanggeMarkedSeat(int seat, int ownerSeat) => _resolve().IsKanggeMarkedSeat(seat, ownerSeat);
    internal ProgramSkillFrame? JieliePaidObserverRoot(long beforeDamageId) => _resolve().JieliePaidObserverRoot(beforeDamageId);
    internal SkillProgramStepOutcome JielieProgramPreventAndGift(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().JielieProgramPreventAndGift(frame, effect);
    internal bool KanggeHealUsedThisRound(int ownerSeat) => _resolve().KanggeHealUsedThisRound(ownerSeat);
    internal SkillProgramStepOutcome KanggeProgramChooseTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().KanggeProgramChooseTarget(frame, effect);
    internal SkillProgramStepOutcome KanggeProgramDeathPrice(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().KanggeProgramDeathPrice(frame, effect);
    internal SkillProgramStepOutcome KanggeProgramGainDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().KanggeProgramGainDraw(frame, effect);
    internal SkillProgramStepOutcome KanggeProgramHealVictim(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().KanggeProgramHealVictim(frame, effect);
    internal bool? ProveKanggeMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CharacterState owner) => _resolve().ProveKanggeMovement(trigger, context, owner);
    internal void ResolveJielieSuitChoice(PromptChoice selected) => _resolve().ResolveJielieSuitChoice(selected);
    internal void ResolveKanggeChooseChoice(PromptChoice selected) => _resolve().ResolveKanggeChooseChoice(selected);
    internal void ResolveKanggeHealChoice(PromptChoice selected) => _resolve().ResolveKanggeHealChoice(selected);
    internal PromptChoice SelectAiJielieSuitChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiJielieSuitChoice(decision, frame);
    internal PromptChoice SelectAiKanggeChooseChoice(PendingDecision decision) => _resolve().SelectAiKanggeChooseChoice(decision);
}
