#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal const string SayingObtainReason = "skill-program.sun-yuan.saying.obtain";
    internal bool HasSayingRoundRange(int actor, int target) => HasSunYuanCapability && actor != target && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<SayingRangeGrantedEvent>().Any(e => e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && (e.OwnerSeat == actor && e.Holder == target || e.Holder == actor && e.OwnerSeat == target));
    internal bool TryGetSayingSlashRequest(ProgramSkillFrame f, out int actorSeat, out IReadOnlyList<int> targetSeats)
    {
        actorSeat = -1;
        targetSeats = [];
        if (f.SunYuan is not { Paid: true } r || r.Op != SkillProgramEffectOp.ResolveSayingPlayStartObtainAndReply || r.Stage is not (SunYuanStage.Request or SunYuanStage.SlashIssuing or SunYuanStage.NativeUse or SunYuanStage.NativeChildren) || r.Source != SunYuanSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<SayingRequestIssuedEvent>().Count(e => e.FrameId == f.Id && e.ActorSeat == r.Holder && e.TargetSeat == f.OwnerSeat) != 1)
            return false;
        actorSeat = r.Holder;
        targetSeats = Array.AsReadOnly(new[] { f.OwnerSeat });
        return true;
    }

    internal bool IsSayingSlashRequest(ProgramSkillFrame f) => f.SunYuan?.Stage == SunYuanStage.Request && TryGetSayingSlashRequest(f, out _, out _);
    internal bool IsSayingSlashQuery(CharacterState actor)
    {
        if (!actor.IsAlive)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame top)
            return false;
        if (top.SunYuan?.Stage is SunYuanStage.Request or SunYuanStage.SlashIssuing && TryGetSayingSlashRequest(top, out var seat, out _) && seat == actor.Seat && _coreMatchParticipantsPlayersAccess._players[top.OwnerSeat].IsAlive)
            return true;
        return top.EquipmentSlotNamedUse is { OriginalParentId: { } parent } && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == parent)is { } owner && top.OwnerSeat == actor.Seat && _coreMatchParticipantsPlayersAccess._players[owner.OwnerSeat].IsAlive && _coreSayingTieredSuppliesOperations.SayingEquipmentSlotPaymentRidesOn(owner, top);
    }

    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 10)]
    internal bool DistanceSpecificSlashExemption10HasSayingRoundRange(ProgramSlashDistanceQuery q) => HasSayingRoundRange(q.Actor.Seat, q.Target.Seat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 40)]
    internal bool SayingSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => IsSayingSlashQuery(query.Actor);
}
