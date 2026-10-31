#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelayedNullificationEntries
{
    internal void BeginDelayedNullificationWindow(CharacterState owner, Card card, CardKind kind) => _resolve().BeginDelayedNullificationWindow(owner, card, kind);
    internal void CompleteDelayedNullification(NullificationWindowFrame window, DelayedEffectContinuation delayed) => _resolve().CompleteDelayedNullification(window, delayed);
    internal bool HasNullificationEffectOwner(NullificationWindowFrame window) => _resolve().HasNullificationEffectOwner(window);
    internal long? NullificationParentActionId(NullificationWindowFrame window) => _resolve().NullificationParentActionId(window);
}
