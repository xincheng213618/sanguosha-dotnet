#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileShiEntries
{
    internal bool AdvanceShiCaoZChildren() => _resolve().AdvanceShiCaoZChildren();
    internal void AnswerShiCaoZ(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiCaoZ(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiCaoZ(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginShiCaoZ(supplied, operation);
    internal void BeginShiCaoZEndMove(global::CardGame.Core.ProgramSkillFrame supplied, bool obtain) => _resolve().BeginShiCaoZEndMove(supplied, obtain);
    internal void CollectShiCaoZIssuedUses(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectShiCaoZIssuedUses(action, window, result);
    internal void ContinueShiCaoZ(long id) => _resolve().ContinueShiCaoZ(id);
    internal global::System.Collections.Generic.IReadOnlyList<int> DiscardShiCaoZHolderDeathPiles(global::CardGame.Core.DeathFrame death) => _resolve().DiscardShiCaoZHolderDeathPiles(death);
    internal void FinishShiCaoZ(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiCaoZ(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeShiCaoZAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeShiCaoZAfterTurnEndedItems(parent, ordinary);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? IncludeShiCaoZPileViews(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeShiCaoZPileViews(ordinary, holder, viewer, revealAll);
    internal bool IsIssuedShiCaoZCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZCandidate(c, context);
    internal bool IsIssuedShiCaoZEndCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZEndCandidate(c, context);
    internal bool IsIssuedShiCaoZUseCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCaoZUseCandidate(c, context);
    internal bool IsShiCaoZPrivateProcessing(int id) => _resolve().IsShiCaoZPrivateProcessing(id);
    internal bool KeepShiCaoZIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepShiCaoZIssued(f);
    internal global::CardGame.Core.ShiCaoZMaterial[] LiveShiCaoZMaterials() => _resolve().LiveShiCaoZMaterials();
    internal void MoveShiCaoZMaterial(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiCaoZMaterial material, global::CardGame.Core.CardLocation destination, string reason) => _resolve().MoveShiCaoZMaterial(f, material, destination, reason);
    internal bool ObservesShiCaoZResponseUse(global::CardGame.Core.CardActionContext action) => _resolve().ObservesShiCaoZResponseUse(action);
    internal void PublishShiCaoZ(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCaoZ(f);
    internal bool QualifyShiCaoZ(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyShiCaoZ(c, trigger, context);
    internal bool ResumeShiCaoZ(long id) => _resolve().ResumeShiCaoZ(id);
    internal bool ReturnShiCaoZMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiCaoZMovement(f);
    internal void SetShiCaoZAfter(long id) => _resolve().SetShiCaoZAfter(id);
    internal global::CardGame.Core.PromptChoice ShiCaoZAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCaoZAi(decision, f);
    internal bool ShiCaoZChildEdge(int i) => _resolve().ShiCaoZChildEdge(i);
    internal bool ShiCaoZChildSuffix(int i) => _resolve().ShiCaoZChildSuffix(i);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCaoZChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCaoZChoices(f);
    internal int ShiCaoZChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCaoZChooser(f);
    internal bool ShiCaoZDyingObserver() => _resolve().ShiCaoZDyingObserver();
    internal bool ShiCaoZFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiCaoZFirstChild(f, child);
    internal bool ShiCaoZMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiCaoZMovementProof(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiCaoZObserverRoot() => _resolve().ShiCaoZObserverRoot();
    internal bool ShiCaoZOwnMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().ShiCaoZOwnMovement(f, m);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ShiCaoZPrivateCards(int viewer) => _resolve().ShiCaoZPrivateCards(viewer);
    internal bool ShiCaoZRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiCaoZRecoveryParent(f, recovery);
    internal bool ShiCaoZSameSource(global::CardGame.Core.ShiCaoZMaterial m, int owner, string instance, string hash) => _resolve().ShiCaoZSameSource(m, owner, instance, hash);
    internal bool TryShiCaoZCompletedUse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardActionContext action, scoped out long origin) => _resolve().TryShiCaoZCompletedUse(context, out action, out origin);
    internal bool ValidShiCaoZ(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShiCaoZ(f);
    internal bool ValidShiCaoZMaterial(global::CardGame.Core.ShiCaoZMaterial m) => _resolve().ValidShiCaoZMaterial(m);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> WithoutShiCaoZPrivateProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().WithoutShiCaoZPrivateProcessing(cards);
    internal bool HasShiCaoZ { get => _resolve().HasShiCaoZ; }
}
