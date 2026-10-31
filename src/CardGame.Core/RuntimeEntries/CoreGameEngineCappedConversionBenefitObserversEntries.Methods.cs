#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitObserversEntries
{
    internal ProgramSkillFrame? CappedConversionObserverRoot(long? damageWindowId = default) => _resolve().CappedConversionObserverRoot(damageWindowId);
    internal bool IsCappedConversionBenefitProgramDying() => _resolve().IsCappedConversionBenefitProgramDying();
}
