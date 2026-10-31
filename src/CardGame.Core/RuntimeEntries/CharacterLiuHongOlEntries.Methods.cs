#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuHongOlEntries
{
    internal void AbolishYujueSlot(ProgramSkillFrame frame, EquipmentSlot slot) => _resolve().AbolishYujueSlot(frame, slot);
    internal void ArmProgramGameDamageBonus(ProgramSkillFrame frame, string skillId, int ownerSeat, int amount) => _resolve().ArmProgramGameDamageBonus(frame, skillId, ownerSeat, amount);
    internal bool CanRunYujueResolve(int ownerSeat) => _resolve().CanRunYujueResolve(ownerSeat);
    internal void CompleteYujueInvocation(ProgramSkillFrame frame, YujuePendingState pending, bool given, int? targetSeat, int? givenCardId) => _resolve().CompleteYujueInvocation(frame, pending, given, targetSeat, givenCardId);
    internal IReadOnlyList<(CardUseEffectSource Source, int Amount)> GetArmedGameDamageBonuses(GameEngine.IDamageAttempt attack) => _resolve().GetArmedGameDamageBonuses(attack);
    internal bool IsTuxingDamageArmPending(int ownerSeat) => _resolve().IsTuxingDamageArmPending(ownerSeat);
    internal SkillProgramStepOutcome PresentYujueGivePrompt(ProgramSkillFrame frame, YujuePendingState pending) => _resolve().PresentYujueGivePrompt(frame, pending);
    internal SkillProgramStepOutcome PresentYujueSlotPrompt(ProgramSkillFrame active) => _resolve().PresentYujueSlotPrompt(active);
    internal void PresentYujueTargetPrompt(ProgramSkillFrame frame, YujuePendingState pending) => _resolve().PresentYujueTargetPrompt(frame, pending);
    internal void ResolveTuxingAfterSlotAbolish(ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount) => _resolve().ResolveTuxingAfterSlotAbolish(frame, recipientSeat, abolishedSlotCount);
    internal void ResolveYujueGiveChoice(PromptChoice selected) => _resolve().ResolveYujueGiveChoice(selected);
    internal void ResolveYujueProgramChoice(PromptChoice selected) => _resolve().ResolveYujueProgramChoice(selected);
    internal void ResolveYujueSlotChoice(PromptChoice selected) => _resolve().ResolveYujueSlotChoice(selected);
    internal void ResolveYujueTargetChoice(PromptChoice selected) => _resolve().ResolveYujueTargetChoice(selected);
    internal PromptChoice SelectAiYujueChoice(PendingDecision decision) => _resolve().SelectAiYujueChoice(decision);
    internal PromptChoice SelectAiYujueGiveChoice(PendingDecision decision) => _resolve().SelectAiYujueGiveChoice(decision);
    internal PromptChoice SelectAiYujueSlotChoice(PendingDecision decision) => _resolve().SelectAiYujueSlotChoice(decision);
    internal PromptChoice SelectAiYujueTargetChoice(PendingDecision decision) => _resolve().SelectAiYujueTargetChoice(decision);
    internal SkillProgramStepOutcome TuxingProgramArmGameDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TuxingProgramArmGameDamage(frame, effect);
    internal SkillProgramStepOutcome YujueProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YujueProgramResolve(frame, effect);
    internal SkillProgramStepOutcome ZhihuProgramExpire(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhihuProgramExpire(frame, effect);
}
