#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownEntityTurnPoliciesEntries
{
    internal void AssertShownEntityPolicyOrigin(global::CardGame.Core.ShownEntityTurnPolicy p) => _resolve().AssertShownEntityPolicyOrigin(p);
    internal void AssertShownEntityTurnPrograms() => _resolve().AssertShownEntityTurnPrograms();
    internal void AssertShownEntityUseBenefits(global::CardGame.Core.CardUseFrame use) => _resolve().AssertShownEntityUseBenefits(use);
    internal bool CanOfferShownEntityTurnPolicy(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShownEntityTurnPolicy(candidate, trigger, context);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetShownEntityUseDamageBenefits(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetShownEntityUseDamageBenefits(attack);
    internal bool HasShownEntityPlayStartParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasShownEntityPlayStartParent(frame, context);
    internal void IssueShownEntityTurnPolicy(global::CardGame.Core.ProgramSkillFrame frame, string bind, string stateId) => _resolve().IssueShownEntityTurnPolicy(frame, bind, stateId);
    internal void IssueShownEntityUseBenefits(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueShownEntityUseBenefits(useId, action);
    internal bool ShownEntityPoliciesMatch(global::CardGame.Core.ShownEntityTurnPolicy a, global::CardGame.Core.ShownEntityTurnPolicy b) => _resolve().ShownEntityPoliciesMatch(a, b);
    internal bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider) => _resolve().ShownEntityUseActorMatches(use, originalActor, provider);
    internal bool TracksShownEntityTurnPolicies { get => _resolve().TracksShownEntityTurnPolicies; }
}
