#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGroupTargetNullificationEntries
{
    internal void CompleteGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window) => _resolve().CompleteGroupTargetNullification(window);
    internal global::CardGame.Core.CardKind GroupNullificationCardKind(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().GroupNullificationCardKind(group);
    internal bool IsGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window) => _resolve().IsGroupTargetNullification(window);
    internal string NullificationEffectDescription(global::CardGame.Core.NullificationWindowFrame window) => _resolve().NullificationEffectDescription(window);
    internal bool TryBeginGroupTargetNullification(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().TryBeginGroupTargetNullification(group);
}
