#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownEntityTurnPoliciesEntries
{
    internal void AssertShownEntityPolicyOrigin(ShownEntityTurnPolicy p) => _resolve().AssertShownEntityPolicyOrigin(p);
    internal void AssertShownEntityUseBenefits(CardUseFrame use) => _resolve().AssertShownEntityUseBenefits(use);
    internal bool CanOfferShownEntityTurnPolicy(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShownEntityTurnPolicy(candidate, trigger, context);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> GetShownEntityUseDamageBenefits(GameEngine.IDamageAttempt attack) => _resolve().GetShownEntityUseDamageBenefits(attack);
    internal bool HasShownEntityPlayStartParent(ProgramSkillFrame frame, ProgramSkillWindowContext context) => _resolve().HasShownEntityPlayStartParent(frame, context);
    internal void IssueShownEntityTurnPolicy(ProgramSkillFrame frame, string bind, string stateId) => _resolve().IssueShownEntityTurnPolicy(frame, bind, stateId);
    internal void IssueShownEntityUseBenefits(long useId, CardActionContext? action) => _resolve().IssueShownEntityUseBenefits(useId, action);
    internal bool ShownEntityPoliciesMatch(ShownEntityTurnPolicy a, ShownEntityTurnPolicy b) => _resolve().ShownEntityPoliciesMatch(a, b);
    internal bool ShownEntityUseActorMatches(CardUseFrame use, int originalActor, int provider) => _resolve().ShownEntityUseActorMatches(use, originalActor, provider);
    internal bool TracksShownEntityTurnPolicies { get => _resolve().TracksShownEntityTurnPolicies; }
}
