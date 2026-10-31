#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtReceiptsEntries
{
    internal bool ValidHalfHandDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHalfHandDraw(f);
    internal bool ValidHalfHandGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHalfHandGift(f);
    internal bool ValidPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPhaseHandDebtPayment(f);
    internal bool ValidPhaseHandExchange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPhaseHandExchange(f);
}
