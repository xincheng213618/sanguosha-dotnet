#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileShiEntries
{
    internal bool AdvanceShiCaoZChildren() => _resolve().AdvanceShiCaoZChildren();
    internal void AnswerShiCaoZ(PromptChoice selected) => _resolve().AnswerShiCaoZ(selected);
    internal SkillProgramStepOutcome BeginShiCaoZ(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginShiCaoZ(supplied, operation);
    internal void BeginShiCaoZEndMove(ProgramSkillFrame supplied, bool obtain) => _resolve().BeginShiCaoZEndMove(supplied, obtain);
    internal void CollectShiCaoZIssuedUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectShiCaoZIssuedUses(action, window, result);
    internal void ContinueShiCaoZ(long id) => _resolve().ContinueShiCaoZ(id);
    internal IReadOnlyList<int> DiscardShiCaoZHolderDeathPiles(DeathFrame death) => _resolve().DiscardShiCaoZHolderDeathPiles(death);
    internal void FinishShiCaoZ(ProgramSkillFrame f) => _resolve().FinishShiCaoZ(f);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeShiCaoZAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeShiCaoZAfterTurnEndedItems(parent, ordinary);
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeShiCaoZPileViews(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeShiCaoZPileViews(ordinary, holder, viewer, revealAll);
    internal bool IsIssuedShiCaoZCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZCandidate(c, context);
    internal bool IsIssuedShiCaoZEndCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZEndCandidate(c, context);
    internal bool IsIssuedShiCaoZUseCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZUseCandidate(c, context);
    internal bool IsShiCaoZPrivateProcessing(int id) => _resolve().IsShiCaoZPrivateProcessing(id);
    internal bool KeepShiCaoZIssued(ProgramSkillFrame f) => _resolve().KeepShiCaoZIssued(f);
    internal ShiCaoZMaterial[] LiveShiCaoZMaterials() => _resolve().LiveShiCaoZMaterials();
    internal void MoveShiCaoZMaterial(ProgramSkillFrame f, ShiCaoZMaterial material, CardLocation destination, string reason) => _resolve().MoveShiCaoZMaterial(f, material, destination, reason);
    internal bool ObservesShiCaoZResponseUse(CardActionContext action) => _resolve().ObservesShiCaoZResponseUse(action);
    internal void PublishShiCaoZ(ProgramSkillFrame f) => _resolve().PublishShiCaoZ(f);
    internal bool QualifyShiCaoZ(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyShiCaoZ(c, trigger, context);
    internal bool ResumeShiCaoZ(long id) => _resolve().ResumeShiCaoZ(id);
    internal bool ReturnShiCaoZMovement(ProgramSkillFrame f) => _resolve().ReturnShiCaoZMovement(f);
    internal void SetShiCaoZAfter(long id) => _resolve().SetShiCaoZAfter(id);
    internal PromptChoice ShiCaoZAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ShiCaoZAi(decision, f);
    internal bool ShiCaoZChildEdge(int i) => _resolve().ShiCaoZChildEdge(i);
    internal bool ShiCaoZChildSuffix(int i) => _resolve().ShiCaoZChildSuffix(i);
    internal IReadOnlyList<PromptChoice> ShiCaoZChoices(ProgramSkillFrame f) => _resolve().ShiCaoZChoices(f);
    internal int ShiCaoZChooser(ProgramSkillFrame f) => _resolve().ShiCaoZChooser(f);
    internal bool ShiCaoZDyingObserver() => _resolve().ShiCaoZDyingObserver();
    internal bool ShiCaoZFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiCaoZFirstChild(f, child);
    internal bool ShiCaoZMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().ShiCaoZMovementProof(f, e, p);
    internal ProgramSkillFrame? ShiCaoZObserverRoot() => _resolve().ShiCaoZObserverRoot();
    internal bool ShiCaoZOwnMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().ShiCaoZOwnMovement(f, m);
    internal IReadOnlyList<CardSnapshot> ShiCaoZPrivateCards(int viewer) => _resolve().ShiCaoZPrivateCards(viewer);
    internal bool ShiCaoZRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiCaoZRecoveryParent(f, recovery);
    internal bool ShiCaoZSameSource(ShiCaoZMaterial m, int owner, string instance, string hash) => _resolve().ShiCaoZSameSource(m, owner, instance, hash);
    internal bool TryShiCaoZCompletedUse(ProgramSkillWindowContext context, scoped out CardActionContext action, scoped out long origin) => _resolve().TryShiCaoZCompletedUse(context, out action, out origin);
    internal bool ValidShiCaoZ(ProgramSkillFrame f) => _resolve().ValidShiCaoZ(f);
    internal bool ValidShiCaoZMaterial(ShiCaoZMaterial m) => _resolve().ValidShiCaoZMaterial(m);
    internal IReadOnlyList<Card> WithoutShiCaoZPrivateProcessing(IReadOnlyList<Card> cards) => _resolve().WithoutShiCaoZPrivateProcessing(cards);
    internal bool HasShiCaoZ { get => _resolve().HasShiCaoZ; }
}
