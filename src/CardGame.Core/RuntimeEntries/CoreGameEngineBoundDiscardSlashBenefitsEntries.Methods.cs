#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundDiscardSlashBenefitsEntries
{
    internal SkillProgramStepOutcome BeginBoundDiscardSlashBenefits(long id, string bind) => _resolve().BeginBoundDiscardSlashBenefits(id, bind);
    internal IReadOnlyList<FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int owner) => _resolve().GetFirstRoundGameUsageRefundsSnapshot(owner);
    internal IReadOnlyList<TurnSlashSuitAllowance>? GetTurnSlashSuitAllowancesSnapshot(int owner) => _resolve().GetTurnSlashSuitAllowancesSnapshot(owner);
    internal bool HasTurnAlcoholSuitAllowance(CharacterState actor, Card? material = default, CardConversionSource? conversion = default) => _resolve().HasTurnAlcoholSuitAllowance(actor, material, conversion);
    internal bool HasTurnSlashSuitAllowance(int owner, Suit? suit) => _resolve().HasTurnSlashSuitAllowance(owner, suit);
    internal void ResolveFirstRoundGameUsageRefunds(int turn, int turnSeat) => _resolve().ResolveFirstRoundGameUsageRefunds(turn, turnSeat);
    internal bool ResumeBoundDiscardSlashBenefits(long id) => _resolve().ResumeBoundDiscardSlashBenefits(id);
    internal void ScheduleProgramFirstRoundRefund(ProgramSkillFrame frame) => _resolve().ScheduleProgramFirstRoundRefund(frame);
}
