#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiMouEntries
{
    internal void BeginMouSmPlayedCompleted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouSmPlayedCompleted(f);
    internal void BeginMouSmSecretUse(long useId, long eventStart, long logStart) => _resolve().BeginMouSmSecretUse(useId, eventStart, logStart);
    internal global::CardGame.Core.DeclaredCardPayment CaptureMouSmDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => _resolve().CaptureMouSmDeclaration(payment, parent);
    internal void CaptureMouSmSecretFact(global::CardGame.Core.IGameEvent payload, long sequence) => _resolve().CaptureMouSmSecretFact(payload, sequence);
    internal void CaptureMouSmSecretLog(global::CardGame.Core.GameLogEntry log) => _resolve().CaptureMouSmSecretLog(log);
    internal void DiscloseMouSmTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DiscloseMouSmTargets(f);
    internal void FinishMouSmPlayedAccepted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouSmPlayedAccepted(f);
    internal void FinishMouSmPlayedCompleted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouSmPlayedCompleted(f);
    internal bool HoldMouSmAiAttack(int source, int target) => _resolve().HoldMouSmAiAttack(source, target);
    internal bool IsIssuedMouSmPrivateCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedMouSmPrivateCandidate(candidate, context);
    internal bool IsMouSmPlayedActor(global::CardGame.Core.CharacterState actor) => _resolve().IsMouSmPlayedActor(actor);
    internal bool IsMouSmPlayedWindow(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().IsMouSmPlayedWindow(action, window);
    internal bool IsMouSmShopEquipmentGivePayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouSmReceipt receipt, global::CardGame.Core.MouSmEntity material, global::CardGame.Core.CardMovementRecord movement) => _resolve().IsMouSmShopEquipmentGivePayment(f, receipt, material, movement);
    internal void IssueMouSmPlayed(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouSmPlayed(f);
    internal void IssueMouSmPlayedDeclaration(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouSmPlayedDeclaration(f);
    internal bool MatchesMouSmPlayedDeclaration(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardDeclarationFrame d) => _resolve().MatchesMouSmPlayedDeclaration(f, d);
    internal bool MouSmAdvanceChildren() => _resolve().MouSmAdvanceChildren();
    internal bool MouSmBeforeDamage(long id) => _resolve().MouSmBeforeDamage(id);
    internal bool MouSmDamageWindow(long id) => _resolve().MouSmDamageWindow(id);
    internal bool MouSmDeclarationEdge(int i) => _resolve().MouSmDeclarationEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouSmDeclarationTargets(global::CardGame.Core.CardDeclarationFrame frame, int viewer, bool revealAll) => _resolve().MouSmDeclarationTargets(frame, viewer, revealAll);
    internal bool MouSmDying() => _resolve().MouSmDying();
    internal bool MouSmEdge(int i) => _resolve().MouSmEdge(i);
    internal bool MouSmFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouSmFirstChild(f, child);
    internal bool MouSmMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouSmMovement(f, effect, p);
    internal bool MouSmPlanIsRed(global::CardGame.Core.CharacterState actor, global::CardGame.Core.MouSmPlayedPlan plan) => _resolve().MouSmPlanIsRed(actor, plan);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouSmPlayedPlan> MouSmPlayedPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouSmPlayedPlans(f);
    internal global::CardGame.Core.MouSmPlayedReturn? MouSmPlayedWindowReturn(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().MouSmPlayedWindowReturn(action, window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EventEnvelope> MouSmPublicEvents() => _resolve().MouSmPublicEvents();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameLogEntry> MouSmPublicLog() => _resolve().MouSmPublicLog();
    internal global::CardGame.Core.NativePublicPileCardRequest? MouSmPublicPileRequest(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => _resolve().MouSmPublicPileRequest(actor, kind);
    internal bool MouSmRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouSmRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouSmRoot() => _resolve().MouSmRoot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> MouSmSlashContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().MouSmSlashContributions(player, query);
    internal bool MouSmSuffix(int n) => _resolve().MouSmSuffix(n);
    internal global::CardGame.Core.GameEngine.MouSmPublicProjection? PrepareMouSmPublicProjection() => _resolve().PrepareMouSmPublicProjection();
    internal bool ReturnMouSmPlayedDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded) => _resolve().ReturnMouSmPlayedDeclaration(d, succeeded);
    internal bool TryBeginMouSmPrivateWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.MouSmPrivateCall call) => _resolve().TryBeginMouSmPrivateWindow(attack, call);
    internal bool TryReturnMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryReturnMouSmPlayedWindow(window);
    internal bool TryReturnMouSmPrivateWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame p) => _resolve().TryReturnMouSmPrivateWindow(p);
    internal bool ValidMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ValidMouSmPlayedWindow(window);
    internal bool ValidMouSmPrivateWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame p) => _resolve().ValidMouSmPrivateWindow(p);
    internal global::CardGame.Core.MouSimaYiRuntime MouSimaYi { get => _resolve().MouSimaYi; }
}
