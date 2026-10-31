#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtReceiptsEntries
{
    internal bool ValidHalfHandDraw(ProgramSkillFrame f) => _resolve().ValidHalfHandDraw(f);
    internal bool ValidHalfHandGift(ProgramSkillFrame f) => _resolve().ValidHalfHandGift(f);
    internal bool ValidPhaseHandDebtPayment(ProgramSkillFrame f) => _resolve().ValidPhaseHandDebtPayment(f);
    internal bool ValidPhaseHandExchange(ProgramSkillFrame f) => _resolve().ValidPhaseHandExchange(f);
}
