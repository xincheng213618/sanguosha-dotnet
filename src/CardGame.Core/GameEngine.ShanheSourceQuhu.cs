using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourceQuhuSkill = "shanhe:source-quhu-14190";

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 62050,
        SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp)]
    private bool AllowsShanheSourceQuhu(ProgramActivationLegalityContext gate) =>
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;

    [ProgramCapability]
    private sealed class ShanheSourceQuhuCapability(GameEngine engine) : IShanheSourceQuhuHost
    {
        public SkillProgramStepOutcome DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat) =>
            engine.DamageShanheQuhuOpponentMaximumHp(frame, targetSeat);
    }

    private SkillProgramStepOutcome DamageShanheQuhuOpponentMaximumHp(ProgramSkillFrame supplied, int targetSeat)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (_modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is null ||
            frame.SkillId != ShanheSourceQuhuSkill || frame.ActivationId != "contest" || frame.TriggerId is not null ||
            frame.InstructionIndex != 3 || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat ||
            frame.PindianResultBindings.SingleOrDefault(binding => binding.Name == "__active-pindian-result") is not
                { SourceWon: true, NativePindianFrameId: { } nativeId } result || result.SourceSeat != frame.OwnerSeat ||
            ProgramEventHistory<PindianResultDeterminedEvent>().Count(fact => fact.FrameId == nativeId &&
                fact.SkillId == frame.SkillId && fact.Result.SourceSeat == result.SourceSeat &&
                fact.Result.OpponentSeat == result.OpponentSeat && fact.Result.SourceRank == result.SourceRank &&
                fact.Result.OpponentRank == result.OpponentRank && fact.Result.SourceWon) != 1)
            throw new InvalidOperationException("Source Quhu lost its exact original winning Pindian and chosen target.");
        var source = new ProgramParticipantReference(ProgramParticipantRef.ResultOpponent, "__active-pindian-result");
        var opponent = ResolveProgramParticipant(frame, source);
        if (!IsValidPlayerSeat(opponent) || !_players[opponent].IsAlive || opponent == targetSeat ||
            !IsWithinAttackRange(opponent, targetSeat))
            throw new InvalidOperationException("Source Quhu requires its original living opponent's actual attack range.");
        // Native program damage owns the already-advanced cursor, source and child return.
        return BeginProgramSkillDamage(frame, targetSeat, Math.Min(5, _players[opponent].MaxHp), source);
    }
}
