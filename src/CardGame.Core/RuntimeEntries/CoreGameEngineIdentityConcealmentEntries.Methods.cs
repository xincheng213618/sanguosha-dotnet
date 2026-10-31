#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIdentityConcealmentEntries
{
    internal void AdvanceIdentityAppearance(global::CardGame.Core.IdentityAppearanceFrame frame) => _resolve().AdvanceIdentityAppearance(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmGaolingRecovery(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ArmGaolingRecovery(input);
    internal void BeginIdentityAppearance(global::CardGame.Core.CharacterState player, global::CardGame.Core.IdentityAppearanceEntry entry, long? parent, int deduction, int? damageSource, global::CardGame.Core.CardKind? cardKind, global::CardGame.Core.GameEngine.IDamageAttempt? attack = default) => _resolve().BeginIdentityAppearance(player, entry, parent, deduction, damageSource, cardKind, attack);
    internal bool CanRunIdentityAppearanceCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIdentityAppearanceCandidate(c, context);
    internal void CompleteIdentityAppearance(global::CardGame.Core.IdentityAppearanceFrame frame) => _resolve().CompleteIdentityAppearance(frame);
    internal void ContinueTurnAfterIdentityAppearance(global::CardGame.Core.CharacterState player) => _resolve().ContinueTurnAfterIdentityAppearance(player);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateIdentityAppearanceContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateIdentityAppearanceContext(window, candidate);
    internal bool GeneralHasIdentityConcealment(global::CardGame.Core.GeneralDefinition general) => _resolve().GeneralHasIdentityConcealment(general);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? IdentityAppearanceAttack(long id) => _resolve().IdentityAppearanceAttack(id);
    internal bool IdentityAppearanceAttackMatches(global::CardGame.Core.IdentityAppearanceReceipt r, global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().IdentityAppearanceAttackMatches(r, a);
    internal void InitializeSelectedIdentityConcealment(global::CardGame.Core.CharacterState player) => _resolve().InitializeSelectedIdentityConcealment(player);
    internal void RequireIdentityAppearance(global::CardGame.Core.IdentityAppearanceFrame frame) => _resolve().RequireIdentityAppearance(frame);
    internal void ResumeIdentityAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().ResumeIdentityAppearanceWindow(window);
    internal bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().TryBeginIdentityAppearanceForDamage(attack, amount);
    internal bool TryBeginIdentityAppearanceForProgramHpLoss(long frameId, string skillId, int targetSeat, int amount) => _resolve().TryBeginIdentityAppearanceForProgramHpLoss(frameId, skillId, targetSeat, amount);
    internal bool TryBeginIdentityAppearanceForTurn(global::CardGame.Core.CharacterState player) => _resolve().TryBeginIdentityAppearanceForTurn(player);
    internal bool ValidGaolingRecovery(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGaolingRecovery(f);
}
