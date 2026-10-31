#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelayedNullificationEntries
{
    internal void BeginDelayedNullificationWindow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().BeginDelayedNullificationWindow(owner, card, kind);
    internal void CompleteDelayedNullification(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed) => _resolve().CompleteDelayedNullification(window, delayed);
    internal bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => _resolve().HasNullificationEffectOwner(window);
    internal long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => _resolve().NullificationParentActionId(window);
}
