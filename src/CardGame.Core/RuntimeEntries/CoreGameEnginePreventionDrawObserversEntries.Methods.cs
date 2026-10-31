#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePreventionDrawObserversEntries
{
    internal bool HasPreventionDrawDying(long beforeDamageId) => _resolve().HasPreventionDrawDying(beforeDamageId);
    internal bool IsPreventionDrawProgramDying() => _resolve().IsPreventionDrawProgramDying();
    internal bool IsValidPreventionDrawReceipt(ProgramSkillFrame root, BeforeDamageProgramWindowFrame parent) => _resolve().IsValidPreventionDrawReceipt(root, parent);
    internal ProgramSkillFrame? PreventionDrawObserverRoot(long beforeDamageId) => _resolve().PreventionDrawObserverRoot(beforeDamageId);
}
