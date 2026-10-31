#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnEntries
{
    internal bool TryGetCurrentEnhancementVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget) => _resolve().TryGetCurrentEnhancementVirtualPrimaryReturn(use, parent, out primaryTarget);
}
