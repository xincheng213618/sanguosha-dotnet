#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGroupTargetNullificationEntries
{
    internal void CompleteGroupTargetNullification(NullificationWindowFrame window) => _resolve().CompleteGroupTargetNullification(window);
    internal CardKind GroupNullificationCardKind(NativeOwners.GroupCardHandle group) => _resolve().GroupNullificationCardKind(group);
    internal bool IsGroupTargetNullification(NullificationWindowFrame window) => _resolve().IsGroupTargetNullification(window);
    internal string NullificationEffectDescription(NullificationWindowFrame window) => _resolve().NullificationEffectDescription(window);
    internal bool TryBeginGroupTargetNullification(NativeOwners.GroupCardHandle group) => _resolve().TryBeginGroupTargetNullification(group);
}
