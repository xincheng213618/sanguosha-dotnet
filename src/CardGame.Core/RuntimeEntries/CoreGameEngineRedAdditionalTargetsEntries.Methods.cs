#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedAdditionalTargetsEntries
{
    internal void AddRedAdditionalTargetActions(List<LegalAction> actions, CharacterState actor) => _resolve().AddRedAdditionalTargetActions(actions, actor);
    internal int GetAdditionalTargetAdjustmentLimit(CharacterState actor, LegalAction action) => _resolve().GetAdditionalTargetAdjustmentLimit(actor, action);
    internal void GrantRedAdditionalTargets(ProgramSkillFrame frame, int maximum, bool? persistAfterSkillLoss = default) => _resolve().GrantRedAdditionalTargets(frame, maximum, persistAfterSkillLoss);
    internal bool HasNextCardTargetAdjustment(CharacterState actor) => _resolve().HasNextCardTargetAdjustment(actor);
    internal bool HasRedAdditionalTargets(CharacterState actor) => _resolve().HasRedAdditionalTargets(actor);
    internal bool IsTargetAdjustmentAction(CharacterState actor, LegalAction action) => _resolve().IsTargetAdjustmentAction(actor, action);
}
