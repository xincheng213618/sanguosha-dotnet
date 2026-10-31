#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal bool CanBeginNingwuJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.ZhouQun is { Op: SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment, Stage: ZhouQunStage.Judgment } r && target == r.Peer && reason == "ningwu" && bind == "ningwu-result" && visibility == SkillProgramCardSetVisibility.Public;
    internal bool IsNingwuJudgment(JudgmentFrame j, ProgramSkillFrame f) => f.ZhouQun is { Op: SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment, Stage: ZhouQunStage.Judgment } r && j.Id == r.JudgmentId && j.ParentFrameId == f.Id && j.TargetSeat == r.Peer && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill && j.Reason == "ningwu" && j.ProgramResultBind == "ningwu-result" && j.ProgramResultVisibility == SkillProgramCardSetVisibility.Public;
    internal bool TryReturnNingwuJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is not { ZhouQun: { } r } f)
            return false;
        if (!IsNingwuJudgment(j, f))
            throw new InvalidOperationException("Ningwu received a different native judgment return.");
        var result = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Single(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.TargetSeat == r.Peer && e.Reason == "ningwu");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Stage = ZhouQunStage.JudgmentChildren, JudgmentCardId = result.CardId, JudgmentRank = result.Rank } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NingwuJudgmentReturnedEvent(f.Id, j.Id, result.CardId, result.Rank));
        BeginZhouQunMovement(ref f, ZhouQunStage.JudgmentChildren);
        if (result.CardId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id);
            if (entry.From != CardLocation.Judgment(r.Peer) || entry.To != CardLocation.Processing || entry.Reason.Value != "skill-program.judgment.result")
                throw new InvalidOperationException("Ningwu cannot clean a foreign claimed judgment entity.");
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        }

        CompleteZhouQunMovement(f.Id);
        ContinueZhouQun(f.Id);
        return true;
    }

    internal void ApplyNingwuJudgment(ProgramSkillFrame f)
    {
        var r = f.ZhouQun!;
        if (r.SlashRank is not { } slashRank || r.JudgmentRank is not { } rank || rank <= slashRank || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } action } use || action.ActionId != r.ActionId)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<NingwuUseNullifiedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Ningwu may finish its own native judgment once.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NingwuUseNullifiedEvent(f.Id, use.Id, action.ActionId, r.JudgmentId, r.Peer, r.Targets.Single(), slashRank, rank));
        foreach (var target in action.EffectiveDesignatedTargetSeats)
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, action.ActorSeat, target, action.EffectiveKind, CardEffectSkipReason.SkillNullified));
        }
    }

    internal bool IsNingwuUseIneffective(CardUseFrame use)
    {
        if (!HasZhouQunCapability)
            return false;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<NingwuUseNullifiedEvent>().Where(e => e.UseId == use.Id))
        {
            if (use.Action?.ActionId != e.ActionId || e.JudgmentRank <= e.SlashRank || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<NingwuJudgmentIssuedEvent>().Count(i => i.FrameId == e.FrameId && i.JudgmentId == e.JudgmentId && i.UseId == use.Id && i.ActionId == e.ActionId && i.ActorSeat == e.ActorSeat && i.TargetSeat == e.TargetSeat && i.SlashRank == e.SlashRank) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<NingwuJudgmentReturnedEvent>().Count(i => i.FrameId == e.FrameId && i.JudgmentId == e.JudgmentId && i.Rank == e.JudgmentRank) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunStartedEvent>().Count(i => i.FrameId == e.FrameId && i.Op == SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment && i.Source.SkillId == "ol:ningwu") != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Count(i => i.ResolutionId == e.JudgmentId && i.ParentResolutionId == e.FrameId && i.TargetSeat == e.ActorSeat && i.Rank == e.JudgmentRank && i.Reason == "ningwu") != 1)
                throw new InvalidOperationException("Ningwu whole-use nullification lost its exact issued native judgment proof.");
            return true;
        }

        return false;
    }
}
