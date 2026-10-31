#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceQuhuRuntime
{
    internal const string ShanheSourceQuhuSkill = "shanhe:source-quhu-14190";
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 62050, SkillProgramEffectOp.DamageShanheQuhuOpponentMaximumHp)]
    internal bool AllowsShanheSourceQuhu(ProgramActivationLegalityContext gate) => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;
    SkillProgramStepOutcome IShanheSourceQuhuHost.DamageOpponentMaximumHp(ProgramSkillFrame frame, int targetSeat) => DamageShanheQuhuOpponentMaximumHp(frame, targetSeat);
    internal SkillProgramStepOutcome DamageShanheQuhuOpponentMaximumHp(ProgramSkillFrame supplied, int targetSeat)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || frame.SkillId != ShanheSourceQuhuSkill || frame.ActivationId != "contest" || frame.TriggerId is not null || frame.InstructionIndex != 3 || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat || frame.PindianResultBindings.SingleOrDefault(binding => binding.Name == "__active-pindian-result")is not { SourceWon: true, NativePindianFrameId: { } nativeId } result || result.SourceSeat != frame.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Count(fact => fact.FrameId == nativeId && fact.SkillId == frame.SkillId && fact.Result.SourceSeat == result.SourceSeat && fact.Result.OpponentSeat == result.OpponentSeat && fact.Result.SourceRank == result.SourceRank && fact.Result.OpponentRank == result.OpponentRank && fact.Result.SourceWon) != 1)
            throw new InvalidOperationException("Source Quhu lost its exact original winning Pindian and chosen target.");
        var source = new ProgramParticipantReference(ProgramParticipantRef.ResultOpponent, "__active-pindian-result");
        var opponent = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, source);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(opponent) || !_coreMatchParticipantsPlayersAccess._players[opponent].IsAlive || opponent == targetSeat || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(opponent, targetSeat))
            throw new InvalidOperationException("Source Quhu requires its original living opponent's actual attack range.");
        // Native program damage owns the already-advanced cursor, source and child return.
        return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, targetSeat, Math.Min(5, _coreMatchParticipantsPlayersAccess._players[opponent].MaxHp), source);
    }
}
