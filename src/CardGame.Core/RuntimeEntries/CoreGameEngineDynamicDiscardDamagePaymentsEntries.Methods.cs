#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDynamicDiscardDamagePaymentsEntries
{
    internal SkillProgramStepOutcome BeginDynamicDiscardDamage(ProgramSkillFrame f) => _resolve().BeginDynamicDiscardDamage(f);
    internal bool CanSelectDynamicDiscardDamageTarget(CharacterState owner, string skill, int target) => _resolve().CanSelectDynamicDiscardDamageTarget(owner, skill, target);
    internal bool CanStartDynamicDiscardDamage(CharacterState owner, string skill) => _resolve().CanStartDynamicDiscardDamage(owner, skill);
    internal ProgramSkillFrame CaptureDynamicDiscardDamageCompleted(ProgramSkillFrame f) => _resolve().CaptureDynamicDiscardDamageCompleted(f);
    internal void CaptureDynamicDiscardDamageDying(long dyingId, long damageId, int victim) => _resolve().CaptureDynamicDiscardDamageDying(dyingId, damageId, victim);
    internal void CaptureDynamicDiscardDamageFrame(long damageId, long ownerId, int target) => _resolve().CaptureDynamicDiscardDamageFrame(damageId, ownerId, target);
    internal void CaptureDynamicDiscardDamageSurvival(GameEngine.DyingCompletionReceipt completion, bool survived) => _resolve().CaptureDynamicDiscardDamageSurvival(completion, survived);
    internal Card[] DynamicDiscardDamageCards(int owner, string skill, string instance) => _resolve().DynamicDiscardDamageCards(owner, skill, instance);
    internal IReadOnlyList<PromptChoice> DynamicDiscardDamageChoices(ProgramSkillFrame f) => _resolve().DynamicDiscardDamageChoices(f);
    internal bool DynamicDiscardDamageSourceValid(ProgramSkillFrame f) => _resolve().DynamicDiscardDamageSourceValid(f);
    internal bool IsDynamicDiscardDamageMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => _resolve().IsDynamicDiscardDamageMovement(f, effect, movement);
    internal void PublishDynamicDiscardDamage(ProgramSkillFrame f) => _resolve().PublishDynamicDiscardDamage(f);
    internal void ResolveDynamicDiscardDamageChoice(PromptChoice c) => _resolve().ResolveDynamicDiscardDamageChoice(c);
    internal bool ResumeDynamicDiscardDamage(long id) => _resolve().ResumeDynamicDiscardDamage(id);
    internal bool ReturnDynamicDiscardDamageMovement(ProgramSkillFrame f) => _resolve().ReturnDynamicDiscardDamageMovement(f);
    internal bool TryDrainDynamicDiscardDamageCost(ProgramSkillFrame f) => _resolve().TryDrainDynamicDiscardDamageCost(f);
    internal bool ValidDynamicDiscardDamagePayments(ProgramSkillFrame f) => _resolve().ValidDynamicDiscardDamagePayments(f);
}
