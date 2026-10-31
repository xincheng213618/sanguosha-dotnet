#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuHongOlEntries
{
    internal void AbolishYujueSlot(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.EquipmentSlot slot) => _resolve().AbolishYujueSlot(frame, slot);
    internal void ArmProgramGameDamageBonus(global::CardGame.Core.ProgramSkillFrame frame, string skillId, int ownerSeat, int amount) => _resolve().ArmProgramGameDamageBonus(frame, skillId, ownerSeat, amount);
    internal void AssertYujuePending(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertYujuePending(frame, paused);
    internal bool CanRunYujueResolve(int ownerSeat) => _resolve().CanRunYujueResolve(ownerSeat);
    internal void CompleteYujueInvocation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.YujuePendingState pending, bool given, int? targetSeat, int? givenCardId) => _resolve().CompleteYujueInvocation(frame, pending, given, targetSeat, givenCardId);
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetArmedGameDamageBonuses(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetArmedGameDamageBonuses(attack);
    internal bool IsTuxingDamageArmPending(int ownerSeat) => _resolve().IsTuxingDamageArmPending(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentYujueGivePrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.YujuePendingState pending) => _resolve().PresentYujueGivePrompt(frame, pending);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentYujueSlotPrompt(global::CardGame.Core.ProgramSkillFrame active) => _resolve().PresentYujueSlotPrompt(active);
    internal void PresentYujueTargetPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.YujuePendingState pending) => _resolve().PresentYujueTargetPrompt(frame, pending);
    internal void ResolveTuxingAfterSlotAbolish(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount) => _resolve().ResolveTuxingAfterSlotAbolish(frame, recipientSeat, abolishedSlotCount);
    internal void ResolveYujueGiveChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYujueGiveChoice(selected);
    internal void ResolveYujueProgramChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYujueProgramChoice(selected);
    internal void ResolveYujueSlotChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYujueSlotChoice(selected);
    internal void ResolveYujueTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYujueTargetChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiYujueChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiYujueChoice(decision);
    internal global::CardGame.Core.PromptChoice SelectAiYujueGiveChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiYujueGiveChoice(decision);
    internal global::CardGame.Core.PromptChoice SelectAiYujueSlotChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiYujueSlotChoice(decision);
    internal global::CardGame.Core.PromptChoice SelectAiYujueTargetChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiYujueTargetChoice(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome TuxingProgramArmGameDamage(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TuxingProgramArmGameDamage(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome YujueProgramResolve(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YujueProgramResolve(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhihuProgramExpire(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhihuProgramExpire(frame, effect);
}
