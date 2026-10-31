#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIdentityConcealmentEntries
{
    internal void AdvanceIdentityAppearance(IdentityAppearanceFrame frame) => _resolve().AdvanceIdentityAppearance(frame);
    internal SkillProgramStepOutcome ArmGaolingRecovery(ProgramSkillFrame input) => _resolve().ArmGaolingRecovery(input);
    internal void BeginIdentityAppearance(CharacterState player, IdentityAppearanceEntry entry, long? parent, int deduction, int? damageSource, CardKind? cardKind, GameEngine.IDamageAttempt? attack = default) => _resolve().BeginIdentityAppearance(player, entry, parent, deduction, damageSource, cardKind, attack);
    internal bool CanRunIdentityAppearanceCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunIdentityAppearanceCandidate(c, context);
    internal void CompleteIdentityAppearance(IdentityAppearanceFrame frame) => _resolve().CompleteIdentityAppearance(frame);
    internal void ContinueTurnAfterIdentityAppearance(CharacterState player) => _resolve().ContinueTurnAfterIdentityAppearance(player);
    internal ProgramSkillWindowContext CreateIdentityAppearanceContext(ProgramLifecycleTriggerWindowFrame window, ProgramTriggerCandidate candidate) => _resolve().CreateIdentityAppearanceContext(window, candidate);
    internal bool GeneralHasIdentityConcealment(GeneralDefinition general) => _resolve().GeneralHasIdentityConcealment(general);
    internal GameEngine.IDamageAttempt? IdentityAppearanceAttack(long id) => _resolve().IdentityAppearanceAttack(id);
    internal bool IdentityAppearanceAttackMatches(IdentityAppearanceReceipt r, GameEngine.IDamageAttempt a) => _resolve().IdentityAppearanceAttackMatches(r, a);
    internal void InitializeSelectedIdentityConcealment(CharacterState player) => _resolve().InitializeSelectedIdentityConcealment(player);
    internal void RequireIdentityAppearance(IdentityAppearanceFrame frame) => _resolve().RequireIdentityAppearance(frame);
    internal void ResumeIdentityAppearanceWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().ResumeIdentityAppearanceWindow(window);
    internal bool TryBeginIdentityAppearanceForDamage(GameEngine.IDamageAttempt attack, int amount) => _resolve().TryBeginIdentityAppearanceForDamage(attack, amount);
    internal bool TryBeginIdentityAppearanceForProgramHpLoss(long frameId, string skillId, int targetSeat, int amount) => _resolve().TryBeginIdentityAppearanceForProgramHpLoss(frameId, skillId, targetSeat, amount);
    internal bool TryBeginIdentityAppearanceForTurn(CharacterState player) => _resolve().TryBeginIdentityAppearanceForTurn(player);
    internal bool ValidGaolingRecovery(ProgramSkillFrame f) => _resolve().ValidGaolingRecovery(f);
}
