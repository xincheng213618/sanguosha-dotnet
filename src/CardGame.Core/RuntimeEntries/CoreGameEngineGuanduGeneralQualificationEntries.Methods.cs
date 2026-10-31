#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralQualificationEntries
{
    internal bool AllowsGuanduGeneralActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsGuanduGeneralActivation(gate);
    internal bool AllowsGuanduGeneralTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsGuanduGeneralTrigger(gate);
    internal bool GuanduCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().GuanduCardUseForbidden(query);
    internal bool IsGuanduCardActionForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsGuanduCardActionForbidden(actorSeat, kind, type);
    internal void IssueGuanduAllCardBan(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().IssueGuanduAllCardBan(f, target);
    internal bool TryGetGuanduIncomingHpReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.CardUseEffectSource source) => _resolve().TryGetGuanduIncomingHpReplacement(attack, out source);
}
