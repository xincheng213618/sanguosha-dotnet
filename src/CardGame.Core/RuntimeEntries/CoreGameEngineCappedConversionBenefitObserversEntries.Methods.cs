#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? CappedConversionObserverRoot(long? damageWindowId = default) => _resolve().CappedConversionObserverRoot(damageWindowId);
    internal bool HasCappedConversionBenefitObserver(long damageWindowId) => _resolve().HasCappedConversionBenefitObserver(damageWindowId);
    internal bool IsCappedConversionBenefitProgramDying() => _resolve().IsCappedConversionBenefitProgramDying();
}
