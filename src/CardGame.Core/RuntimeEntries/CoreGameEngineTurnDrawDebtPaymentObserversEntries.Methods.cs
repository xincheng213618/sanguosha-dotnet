#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtPaymentObserversEntries
{
    internal bool IsTurnDrawDebtPaymentProgramDying() => _resolve().IsTurnDrawDebtPaymentProgramDying();
    internal ProgramSkillFrame? TurnDrawDebtPaymentObserverRoot() => _resolve().TurnDrawDebtPaymentObserverRoot();
}
