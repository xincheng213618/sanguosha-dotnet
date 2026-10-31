#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksEntries
{
    internal void AssertOrdinaryTrickCannotNullify(CardUseFrame use) => _resolve().AssertOrdinaryTrickCannotNullify(use);
    internal bool IsOriginalOrdinaryTrickCannotNullify(long useId) => _resolve().IsOriginalOrdinaryTrickCannotNullify(useId);
    internal void ObserveUnnullifiableOrdinaryTrick(IGameEvent payload) => _resolve().ObserveUnnullifiableOrdinaryTrick(payload);
    internal bool HasOrdinaryTrickCannotNullifyCapability { get => _resolve().HasOrdinaryTrickCannotNullifyCapability; }
}
