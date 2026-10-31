#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCoreInvariantsEntries
{
    internal void AssertCoreInvariants() => _resolve().AssertCoreInvariants();
    internal bool IsActiveAttackCardConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsActiveAttackCardConsistent(attack, processing);
}
