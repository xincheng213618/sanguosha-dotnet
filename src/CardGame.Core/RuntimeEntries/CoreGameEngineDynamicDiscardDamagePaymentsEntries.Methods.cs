#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDynamicDiscardDamagePaymentsEntries
{
    internal void AssertDynamicDiscardDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDynamicDiscardDamage(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDynamicDiscardDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginDynamicDiscardDamage(f);
    internal bool CanSelectDynamicDiscardDamageTarget(global::CardGame.Core.CharacterState owner, string skill, int target) => _resolve().CanSelectDynamicDiscardDamageTarget(owner, skill, target);
    internal bool CanStartDynamicDiscardDamage(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanStartDynamicDiscardDamage(owner, skill);
    internal global::CardGame.Core.ProgramSkillFrame CaptureDynamicDiscardDamageCompleted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaptureDynamicDiscardDamageCompleted(f);
    internal void CaptureDynamicDiscardDamageDying(long dyingId, long damageId, int victim) => _resolve().CaptureDynamicDiscardDamageDying(dyingId, damageId, victim);
    internal void CaptureDynamicDiscardDamageFrame(long damageId, long ownerId, int target) => _resolve().CaptureDynamicDiscardDamageFrame(damageId, ownerId, target);
    internal void CaptureDynamicDiscardDamageSurvival(global::CardGame.Core.GameEngine.DyingCompletionReceipt completion, bool survived) => _resolve().CaptureDynamicDiscardDamageSurvival(completion, survived);
    internal global::CardGame.Core.Card[] DynamicDiscardDamageCards(int owner, string skill, string instance) => _resolve().DynamicDiscardDamageCards(owner, skill, instance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DynamicDiscardDamageChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DynamicDiscardDamageChoices(f);
    internal bool DynamicDiscardDamageSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DynamicDiscardDamageSourceValid(f);
    internal bool IsDynamicDiscardDamageMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsDynamicDiscardDamageMovement(f, effect, movement);
    internal void PublishDynamicDiscardDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDynamicDiscardDamage(f);
    internal void ResolveDynamicDiscardDamageChoice(global::CardGame.Core.PromptChoice c) => _resolve().ResolveDynamicDiscardDamageChoice(c);
    internal bool ResumeDynamicDiscardDamage(long id) => _resolve().ResumeDynamicDiscardDamage(id);
    internal bool ReturnDynamicDiscardDamageMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDynamicDiscardDamageMovement(f);
    internal bool TryDrainDynamicDiscardDamageCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TryDrainDynamicDiscardDamageCost(f);
    internal bool ValidDynamicDiscardDamagePayments(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDynamicDiscardDamagePayments(f);
}
