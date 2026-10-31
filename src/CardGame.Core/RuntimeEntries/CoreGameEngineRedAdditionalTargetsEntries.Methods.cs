#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedAdditionalTargetsEntries
{
    internal void AddRedAdditionalTargetActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => _resolve().AddRedAdditionalTargetActions(actions, actor);
    internal int GetAdditionalTargetAdjustmentLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().GetAdditionalTargetAdjustmentLimit(actor, action);
    internal void GrantRedAdditionalTargets(global::CardGame.Core.ProgramSkillFrame frame, int maximum, bool? persistAfterSkillLoss = default) => _resolve().GrantRedAdditionalTargets(frame, maximum, persistAfterSkillLoss);
    internal bool HasNextCardTargetAdjustment(global::CardGame.Core.CharacterState actor) => _resolve().HasNextCardTargetAdjustment(actor);
    internal bool HasRedAdditionalTargets(global::CardGame.Core.CharacterState actor) => _resolve().HasRedAdditionalTargets(actor);
    internal bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().IsTargetAdjustmentAction(actor, action);
}
