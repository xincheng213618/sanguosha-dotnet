#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientScopedDamageEntries
{
    internal void AdjustExactRecipientDamage(ProgramSkillFrame f, ProgramSignedDamagePaymentReceipt receipt, int after) => _resolve().AdjustExactRecipientDamage(f, receipt, after);
    internal void CaptureRecipientScopedDamageBase(GameEngine.IDamageAttempt attack, IReadOnlyList<int> targets) => _resolve().CaptureRecipientScopedDamageBase(attack, targets);
    internal bool HasFixedTargetRecipientDamageScope(GameEngine.IDamageAttempt attack) => _resolve().HasFixedTargetRecipientDamageScope(attack);
    internal bool HasPublicEnemyRecipientDamageScope(GameEngine.IDamageAttempt attack) => _resolve().HasPublicEnemyRecipientDamageScope(attack);
    internal void RestoreRecipientScopedDamageBase(GameEngine.IDamageAttempt attack, int fromSeat) => _resolve().RestoreRecipientScopedDamageBase(attack, fromSeat);
    internal bool SupportsIssuedRecipientDamageScope(RecipientScopedDamageState scope) => _resolve().SupportsIssuedRecipientDamageScope(scope);
    internal void UpdateRecipientScopedDamageRedirect(GameEngine.IDamageAttempt attack) => _resolve().UpdateRecipientScopedDamageRedirect(attack);
    internal bool HasShenwuRecipientDamageModifiers { get => _resolve().HasShenwuRecipientDamageModifiers; }
    internal bool NeedsRecipientScopedDamageBase { get => _resolve().NeedsRecipientScopedDamageBase; }
}
