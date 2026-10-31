#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaHuiOlEntries
{
    internal bool AllowsDistributeTopCardsAfterElementalChainOriginTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsDistributeTopCardsAfterElementalChainOriginTrigger(gate);
    internal void AssertSimaHui(ProgramSkillFrame f) => _resolve().AssertSimaHui(f);
    internal SkillProgramStepOutcome BeginSimaHuiDistribution(ProgramSkillFrame f) => _resolve().BeginSimaHuiDistribution(f);
    internal SkillProgramStepOutcome BeginSimaHuiOperation(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginSimaHuiOperation(supplied, op);
    internal bool CanActivateSimaHui(int owner, SkillProgramActivation activation) => _resolve().CanActivateSimaHui(owner, activation);
    internal bool CanContinueSimaHui(ProgramSkillFrame f) => _resolve().CanContinueSimaHui(f);
    internal bool CanOfferSimaHuiDamage(int owner, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => _resolve().CanOfferSimaHuiDamage(owner, trigger, attack);
    internal bool CanOfferSimaHuiLifecycle(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSimaHuiLifecycle(candidate, trigger, context);
    internal bool CanPreventSimaHuiDamage(int owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanPreventSimaHuiDamage(owner, trigger, context);
    internal bool ExactSimaHuiParent(ProgramSkillFrame f) => _resolve().ExactSimaHuiParent(f);
    internal void FinishSimaHui(ProgramSkillFrame f) => _resolve().FinishSimaHui(f);
    internal bool IsSimaHuiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsSimaHuiMovement(f, effect, pending);
    internal bool IsSimaHuiYeyanSelectedPayment(ProgramSkillFrame f) => _resolve().IsSimaHuiYeyanSelectedPayment(f);
    internal void IssueSimaHuiGift(ProgramSkillFrame f, int recipient) => _resolve().IssueSimaHuiGift(f, recipient);
    internal SimaHuiReceipt NewSimaHuiReceipt(ProgramSkillFrame f, string stage) => _resolve().NewSimaHuiReceipt(f, stage);
    internal SkillProgramStepOutcome PaySimaHuiYeyanSeals(ProgramSkillFrame f) => _resolve().PaySimaHuiYeyanSeals(f);
    internal void PublishSimaHuiChoice(ProgramSkillFrame f) => _resolve().PublishSimaHuiChoice(f);
    internal void RefreshSimaHuiCombinedGrants() => _resolve().RefreshSimaHuiCombinedGrants();
    internal void ResolveSimaHuiChoice(PromptChoice selected) => _resolve().ResolveSimaHuiChoice(selected);
    internal bool ResumeSimaHui(long id) => _resolve().ResumeSimaHui(id);
    internal bool ReturnSimaHuiMovement(ProgramSkillFrame f) => _resolve().ReturnSimaHuiMovement(f);
    internal bool ReturnSimaHuiUnassignedCards(ProgramSkillFrame f) => _resolve().ReturnSimaHuiUnassignedCards(f);
    internal void SetSimaHuiSeal(ProgramSkillFrame f, CardConversionSource issuer, PlayerMarkerKind marker, int? from, int? to) => _resolve().SetSimaHuiSeal(f, issuer, marker, from, to);
    internal IReadOnlyList<PromptChoice> SimaHuiChoices(ProgramSkillFrame f) => _resolve().SimaHuiChoices(f);
    internal bool SimaHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SimaHuiFirstChild(f, child);
    internal int? SimaHuiHolder(CardConversionSource issuer, PlayerMarkerKind marker) => _resolve().SimaHuiHolder(issuer, marker);
    internal CardConversionSource SimaHuiIssuer(ProgramSkillFrame f) => _resolve().SimaHuiIssuer(f);
    internal ProgramSkillFrame? SimaHuiObserverRoot() => _resolve().SimaHuiObserverRoot();
    internal int SimaHuiOwnTurns(int owner) => _resolve().SimaHuiOwnTurns(owner);
    internal SkillProgramEffect? SimaHuiPausedEffect(ProgramSkillFrame f) => _resolve().SimaHuiPausedEffect(f);
    internal IEnumerable<Card> SimaHuiPrivatelyViewedCards(int viewer) => _resolve().SimaHuiPrivatelyViewedCards(viewer);
    internal CardConversionSource SimaHuiSource(ProgramSkillFrame f) => _resolve().SimaHuiSource(f);
    internal bool SimaHuiYeyanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SimaHuiYeyanFirstChild(f, child);
    internal bool TryAdvanceSimaHuiSubtree() => _resolve().TryAdvanceSimaHuiSubtree();
    internal bool TrySimaHuiChainOrigin(int owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context, scoped out SimaHuiChainOriginProof proof) => _resolve().TrySimaHuiChainOrigin(owner, trigger, context, out proof);
    internal bool TrySimaHuiOriginalChainMembers(GameEngine.IDamageAttempt attack, scoped out int[] members) => _resolve().TrySimaHuiOriginalChainMembers(attack, out members);
    internal bool ValidSimaHui(ProgramSkillFrame f) => _resolve().ValidSimaHui(f);
}
