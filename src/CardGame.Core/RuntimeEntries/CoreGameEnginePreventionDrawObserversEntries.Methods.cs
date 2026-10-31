#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePreventionDrawObserversEntries
{
    internal bool HasPreventionDrawDying(long beforeDamageId) => _resolve().HasPreventionDrawDying(beforeDamageId);
    internal bool IsPreventionDrawInsideDamageProgramDying() => _resolve().IsPreventionDrawInsideDamageProgramDying();
    internal bool IsPreventionDrawProgramDying() => _resolve().IsPreventionDrawProgramDying();
    internal bool IsValidPreventionDrawReceipt(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.BeforeDamageProgramWindowFrame parent) => _resolve().IsValidPreventionDrawReceipt(root, parent);
    internal global::CardGame.Core.ProgramSkillFrame? PreventionDrawObserverRoot(long beforeDamageId) => _resolve().PreventionDrawObserverRoot(beforeDamageId);
}
