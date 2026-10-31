#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionEntries
{
    internal bool IsExtraForeignPublicAnswer(TargetCardSelectionFrame pending, PromptChoice selected) => _resolve().IsExtraForeignPublicAnswer(pending, selected);
    internal bool TryBeginExtraForeignSelection(NullificationWindowFrame pending, CharacterState target) => _resolve().TryBeginExtraForeignSelection(pending, target);
    internal bool TryResolveExtraForeignSelection(TargetCardSelectionFrame pending, int slot) => _resolve().TryResolveExtraForeignSelection(pending, slot);
}
