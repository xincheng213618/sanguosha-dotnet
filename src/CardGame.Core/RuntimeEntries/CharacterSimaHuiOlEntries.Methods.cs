#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaHuiOlEntries
{
    internal bool AllowsDistributeTopCardsAfterElementalChainOriginTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsDistributeTopCardsAfterElementalChainOriginTrigger(gate);
    internal void AssertSimaHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSimaHui(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSimaHuiDistribution(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginSimaHuiDistribution(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSimaHuiOperation(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginSimaHuiOperation(supplied, op);
    internal bool CanActivateSimaHui(int owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanActivateSimaHui(owner, activation);
    internal bool CanContinueSimaHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueSimaHui(f);
    internal bool CanOfferSimaHuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferSimaHuiDamage(owner, trigger, attack);
    internal bool CanOfferSimaHuiLifecycle(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSimaHuiLifecycle(candidate, trigger, context);
    internal bool CanPreventSimaHuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanPreventSimaHuiDamage(owner, trigger, context);
    internal bool ExactSimaHuiParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactSimaHuiParent(f);
    internal void FinishSimaHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSimaHui(f);
    internal bool HasSimaHuiBeforeDamageObserver(long id) => _resolve().HasSimaHuiBeforeDamageObserver(id);
    internal bool HasSimaHuiDamageObserver(long id) => _resolve().HasSimaHuiDamageObserver(id);
    internal bool IsSimaHuiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsSimaHuiMovement(f, effect, pending);
    internal bool IsSimaHuiYeyanSelectedPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsSimaHuiYeyanSelectedPayment(f);
    internal void IssueSimaHuiGift(global::CardGame.Core.ProgramSkillFrame f, int recipient) => _resolve().IssueSimaHuiGift(f, recipient);
    internal global::CardGame.Core.SimaHuiReceipt NewSimaHuiReceipt(global::CardGame.Core.ProgramSkillFrame f, string stage) => _resolve().NewSimaHuiReceipt(f, stage);
    internal global::CardGame.Core.SkillProgramStepOutcome PaySimaHuiYeyanSeals(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaySimaHuiYeyanSeals(f);
    internal void PublishSimaHuiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSimaHuiChoice(f);
    internal void RefreshSimaHuiCombinedGrants() => _resolve().RefreshSimaHuiCombinedGrants();
    internal void ResolveSimaHuiChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSimaHuiChoice(selected);
    internal bool ResumeSimaHui(long id) => _resolve().ResumeSimaHui(id);
    internal bool ReturnSimaHuiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSimaHuiMovement(f);
    internal bool ReturnSimaHuiUnassignedCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSimaHuiUnassignedCards(f);
    internal void SetSimaHuiSeal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardConversionSource issuer, global::CardGame.Core.PlayerMarkerKind marker, int? from, int? to) => _resolve().SetSimaHuiSeal(f, issuer, marker, from, to);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SimaHuiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimaHuiChoices(f);
    internal bool SimaHuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SimaHuiFirstChild(f, child);
    internal int? SimaHuiHolder(global::CardGame.Core.CardConversionSource issuer, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().SimaHuiHolder(issuer, marker);
    internal global::CardGame.Core.CardConversionSource SimaHuiIssuer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimaHuiIssuer(f);
    internal global::CardGame.Core.ProgramSkillFrame? SimaHuiObserverRoot() => _resolve().SimaHuiObserverRoot();
    internal int SimaHuiOwnTurns(int owner) => _resolve().SimaHuiOwnTurns(owner);
    internal global::CardGame.Core.SkillProgramEffect? SimaHuiPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimaHuiPausedEffect(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SimaHuiPrivatelyViewedCards(int viewer) => _resolve().SimaHuiPrivatelyViewedCards(viewer);
    internal global::CardGame.Core.CardConversionSource SimaHuiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimaHuiSource(f);
    internal bool SimaHuiYeyanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SimaHuiYeyanFirstChild(f, child);
    internal bool TryAdvanceSimaHuiSubtree() => _resolve().TryAdvanceSimaHuiSubtree();
    internal bool TrySimaHuiChainOrigin(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.SimaHuiChainOriginProof proof) => _resolve().TrySimaHuiChainOrigin(owner, trigger, context, out proof);
    internal bool TrySimaHuiOriginalChainMembers(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out int[] members) => _resolve().TrySimaHuiOriginalChainMembers(attack, out members);
    internal bool ValidSimaHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSimaHui(f);
}
