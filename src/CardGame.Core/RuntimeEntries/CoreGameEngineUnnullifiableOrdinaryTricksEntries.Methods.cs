#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksEntries
{
    internal void AssertOrdinaryTrickCannotNullify(global::CardGame.Core.CardUseFrame use) => _resolve().AssertOrdinaryTrickCannotNullify(use);
    internal bool IsOriginalOrdinaryTrickCannotNullify(long useId) => _resolve().IsOriginalOrdinaryTrickCannotNullify(useId);
    internal void ObserveUnnullifiableOrdinaryTrick(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveUnnullifiableOrdinaryTrick(payload);
    internal bool HasOrdinaryTrickCannotNullifyCapability { get => _resolve().HasOrdinaryTrickCannotNullifyCapability; }
}
