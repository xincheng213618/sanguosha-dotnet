#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenEntries
{
    internal bool AdvanceSourceMouChildren() => _resolve().AdvanceSourceMouChildren();
    internal bool SourceMouDying() => _resolve().SourceMouDying();
    internal bool SourceMouEdge(int index) => _resolve().SourceMouEdge(index);
    internal bool SourceMouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SourceMouFirstChild(f, child);
    internal bool SourceMouRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().SourceMouRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? SourceMouRoot() => _resolve().SourceMouRoot();
    internal bool SourceMouSuffix(int index) => _resolve().SourceMouSuffix(index);
}
