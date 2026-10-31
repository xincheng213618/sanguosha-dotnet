#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiMouEntries
{
    internal void BeginMouSmPlayedCompleted(ProgramSkillFrame f) => _resolve().BeginMouSmPlayedCompleted(f);
    internal void BeginMouSmSecretUse(long useId, long eventStart, long logStart) => _resolve().BeginMouSmSecretUse(useId, eventStart, logStart);
    internal DeclaredCardPayment CaptureMouSmDeclaration(DeclaredCardPayment payment, CardDeclarationParentReturn parent) => _resolve().CaptureMouSmDeclaration(payment, parent);
    internal void CaptureMouSmSecretFact(IGameEvent payload, long sequence) => _resolve().CaptureMouSmSecretFact(payload, sequence);
    internal void CaptureMouSmSecretLog(GameLogEntry log) => _resolve().CaptureMouSmSecretLog(log);
    internal void DiscloseMouSmTargets(ProgramSkillFrame f) => _resolve().DiscloseMouSmTargets(f);
    internal void FinishMouSmPlayedAccepted(ProgramSkillFrame f) => _resolve().FinishMouSmPlayedAccepted(f);
    internal void FinishMouSmPlayedCompleted(ProgramSkillFrame f) => _resolve().FinishMouSmPlayedCompleted(f);
    internal bool HoldMouSmAiAttack(int source, int target) => _resolve().HoldMouSmAiAttack(source, target);
    internal bool IsIssuedMouSmPrivateCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedMouSmPrivateCandidate(candidate, context);
    internal bool IsMouSmPlayedActor(CharacterState actor) => _resolve().IsMouSmPlayedActor(actor);
    internal bool IsMouSmPlayedWindow(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().IsMouSmPlayedWindow(action, window);
    internal bool IsMouSmShopEquipmentGivePayment(ProgramSkillFrame f, MouSmReceipt receipt, MouSmEntity material, CardMovementRecord movement) => _resolve().IsMouSmShopEquipmentGivePayment(f, receipt, material, movement);
    internal void IssueMouSmPlayed(ProgramSkillFrame f) => _resolve().IssueMouSmPlayed(f);
    internal void IssueMouSmPlayedDeclaration(ProgramSkillFrame f) => _resolve().IssueMouSmPlayedDeclaration(f);
    internal bool MatchesMouSmPlayedDeclaration(ProgramSkillFrame f, CardDeclarationFrame d) => _resolve().MatchesMouSmPlayedDeclaration(f, d);
    internal bool MouSmAdvanceChildren() => _resolve().MouSmAdvanceChildren();
    internal bool MouSmDeclarationEdge(int i) => _resolve().MouSmDeclarationEdge(i);
    internal IReadOnlyList<int> MouSmDeclarationTargets(CardDeclarationFrame frame, int viewer, bool revealAll) => _resolve().MouSmDeclarationTargets(frame, viewer, revealAll);
    internal bool MouSmDying() => _resolve().MouSmDying();
    internal bool MouSmEdge(int i) => _resolve().MouSmEdge(i);
    internal bool MouSmFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouSmFirstChild(f, child);
    internal bool MouSmMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouSmMovement(f, effect, p);
    internal bool MouSmPlanIsRed(CharacterState actor, MouSmPlayedPlan plan) => _resolve().MouSmPlanIsRed(actor, plan);
    internal IReadOnlyList<MouSmPlayedPlan> MouSmPlayedPlans(ProgramSkillFrame f) => _resolve().MouSmPlayedPlans(f);
    internal MouSmPlayedReturn? MouSmPlayedWindowReturn(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().MouSmPlayedWindowReturn(action, window);
    internal IReadOnlyList<EventEnvelope> MouSmPublicEvents() => _resolve().MouSmPublicEvents();
    internal IReadOnlyList<GameLogEntry> MouSmPublicLog() => _resolve().MouSmPublicLog();
    internal NativePublicPileCardRequest? MouSmPublicPileRequest(CharacterState actor, CardKind kind) => _resolve().MouSmPublicPileRequest(actor, kind);
    internal bool MouSmRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouSmRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouSmRoot() => _resolve().MouSmRoot();
    internal IEnumerable<RuleQueryContribution> MouSmSlashContributions(CharacterState player, SkillRuleQuery query) => _resolve().MouSmSlashContributions(player, query);
    internal bool MouSmSuffix(int n) => _resolve().MouSmSuffix(n);
    internal GameEngine.MouSmPublicProjection? PrepareMouSmPublicProjection() => _resolve().PrepareMouSmPublicProjection();
    internal bool ReturnMouSmPlayedDeclaration(CardDeclarationFrame d, bool succeeded) => _resolve().ReturnMouSmPlayedDeclaration(d, succeeded);
    internal bool TryBeginMouSmPrivateWindow(NativeOwners.CardAttackHandle attack, MouSmPrivateCall call) => _resolve().TryBeginMouSmPrivateWindow(attack, call);
    internal bool TryReturnMouSmPlayedWindow(ProgramCardTriggerWindowFrame window) => _resolve().TryReturnMouSmPlayedWindow(window);
    internal bool TryReturnMouSmPrivateWindow(ProgramCardTriggerWindowFrame p) => _resolve().TryReturnMouSmPrivateWindow(p);
    internal bool ValidMouSmPlayedWindow(ProgramCardTriggerWindowFrame window) => _resolve().ValidMouSmPlayedWindow(window);
    internal bool ValidMouSmPrivateWindow(ProgramCardTriggerWindowFrame p) => _resolve().ValidMouSmPrivateWindow(p);
    internal MouSimaYiRuntime MouSimaYi { get => _resolve().MouSimaYi; }
}
