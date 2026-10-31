#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenEntries
{
    internal bool AdvanceSourceMouChildren() => _resolve().AdvanceSourceMouChildren();
    internal bool SourceMouDying() => _resolve().SourceMouDying();
    internal bool SourceMouEdge(int index) => _resolve().SourceMouEdge(index);
    internal bool SourceMouFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SourceMouFirstChild(f, child);
    internal bool SourceMouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().SourceMouRecoveryParent(f, recovery);
    internal ProgramSkillFrame? SourceMouRoot() => _resolve().SourceMouRoot();
    internal bool SourceMouSuffix(int index) => _resolve().SourceMouSuffix(index);
}
