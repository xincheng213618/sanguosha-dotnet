#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnEntries
{
    internal bool TryGetCurrentEnhancementVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primaryTarget) => _resolve().TryGetCurrentEnhancementVirtualPrimaryReturn(use, parent, out primaryTarget);
}
