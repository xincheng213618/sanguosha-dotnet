#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24850)]
    internal bool CanOfferCaoSongSourceTargets(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat)
    {
        var selection = trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.SelectTarget && e.TargetKind is SkillProgramTargetKind.PublicTeamEnemyHpGreaterThanOwner or SkillProgramTargetKind.DyingEventPublicTeamAlly);
        return selection is null || (selection.TargetKind != SkillProgramTargetKind.DyingEventPublicTeamAlly || _coreMatchParticipantsPlayersAccess._players[ownerSeat].MaxHp > 1) && CaoSongSourceTargetSeats(ownerSeat, selection.TargetKind!.Value, context)is { Count: > 0 };
    }

    internal IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, SkillProgramTargetKind kind, ProgramSkillWindowContext? context)
    {
        if (kind == SkillProgramTargetKind.PublicTeamEnemyHpGreaterThanOwner)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
            return owner.IsAlive && owner.HasHpValue ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue && p.Hp > owner.Hp && _coreTeamActivityProgramsOperations.PublicTeamEnemy(ownerSeat, p)).Select(p => p.Seat).Order().ToArray() : [];
        }

        if (kind != SkillProgramTargetKind.DyingEventPublicTeamAlly)
            return null;
        // The payment gate belongs to trigger qualification, not the bound
        // target: a real payment from two MaxHp to one must retain its victim.
        if (!_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].HasHpValue || context is not { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: { } victim } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive || !_coreMatchParticipantsPlayersAccess._players[victim].HasHpValue || _coreMatchParticipantsPlayersAccess._players[victim].Hp > 0 || !_coreTeamActivitiesOperations.SamePublicTeam(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[victim]) || _coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } active || active.VictimSeat != victim || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault()is not { } entry || entry.Id != context.ParentFrameId || entry.OwnerSeat != victim || entry.Window != SkillProgramTriggerWindow.DyingEntering || entry.Continuation != ProgramLifecycleContinuation.ResumeDyingEntry || entry.ResumeDyingFrameId != active.FrameId || !_resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(f => f.Id == active.FrameId && f.VictimSeat == victim))
            return[];
        return[victim];
    }
}
