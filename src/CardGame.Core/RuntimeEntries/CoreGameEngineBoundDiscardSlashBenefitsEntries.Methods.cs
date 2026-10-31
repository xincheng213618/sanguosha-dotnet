#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundDiscardSlashBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundDiscardSlashBenefits(long id, string bind) => _resolve().BeginBoundDiscardSlashBenefits(id, bind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int owner) => _resolve().GetFirstRoundGameUsageRefundsSnapshot(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnSlashSuitAllowance>? GetTurnSlashSuitAllowancesSnapshot(int owner) => _resolve().GetTurnSlashSuitAllowancesSnapshot(owner);
    internal bool HasTurnAlcoholSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default) => _resolve().HasTurnAlcoholSuitAllowance(actor, material, conversion);
    internal bool HasTurnSlashSuitAllowance(int owner, global::CardGame.Core.Suit? suit) => _resolve().HasTurnSlashSuitAllowance(owner, suit);
    internal void ResolveFirstRoundGameUsageRefunds(int turn, int turnSeat) => _resolve().ResolveFirstRoundGameUsageRefunds(turn, turnSeat);
    internal bool ResumeBoundDiscardSlashBenefits(long id) => _resolve().ResumeBoundDiscardSlashBenefits(id);
    internal void ScheduleProgramFirstRoundRefund(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ScheduleProgramFirstRoundRefund(frame);
}
