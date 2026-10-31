#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralQualificationEntries
{
    internal bool AllowsGuanduGeneralActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsGuanduGeneralActivation(gate);
    internal bool AllowsGuanduGeneralTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsGuanduGeneralTrigger(gate);
    internal bool GuanduCardUseForbidden(ProgramCardUseQuery query) => _resolve().GuanduCardUseForbidden(query);
    internal bool IsGuanduCardActionForbidden(int actorSeat, CardKind kind, CardActionType type) => _resolve().IsGuanduCardActionForbidden(actorSeat, kind, type);
    internal void IssueGuanduAllCardBan(ProgramSkillFrame f, int target) => _resolve().IssueGuanduAllCardBan(f, target);
    internal bool TryGetGuanduIncomingHpReplacement(GameEngine.IDamageAttempt attack, scoped out CardUseEffectSource source) => _resolve().TryGetGuanduIncomingHpReplacement(attack, out source);
}
