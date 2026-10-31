#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientScopedDamageEntries
{
    internal void AdjustExactRecipientDamage(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSignedDamagePaymentReceipt receipt, int after) => _resolve().AdjustExactRecipientDamage(f, receipt, after);
    internal void AssertRecipientScopedDamageStates() => _resolve().AssertRecipientScopedDamageStates();
    internal void CaptureRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().CaptureRecipientScopedDamageBase(attack, targets);
    internal bool HasFixedTargetRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasFixedTargetRecipientDamageScope(attack);
    internal bool HasPublicEnemyRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasPublicEnemyRecipientDamageScope(attack);
    internal void RestoreRecipientScopedDamageBase(global::CardGame.Core.GameEngine.IDamageAttempt attack, int fromSeat) => _resolve().RestoreRecipientScopedDamageBase(attack, fromSeat);
    internal bool SupportsIssuedRecipientDamageScope(global::CardGame.Core.RecipientScopedDamageState scope) => _resolve().SupportsIssuedRecipientDamageScope(scope);
    internal void UpdateRecipientScopedDamageRedirect(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().UpdateRecipientScopedDamageRedirect(attack);
    internal bool HasShenwuRecipientDamageModifiers { get => _resolve().HasShenwuRecipientDamageModifiers; }
    internal bool NeedsRecipientScopedDamageBase { get => _resolve().NeedsRecipientScopedDamageBase; }
}
