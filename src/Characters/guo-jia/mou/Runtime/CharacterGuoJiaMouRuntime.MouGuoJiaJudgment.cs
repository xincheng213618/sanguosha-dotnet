#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMouRuntime
{
    internal bool MouGjJudgmentRequest(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => MouGuoJia.Valid(f) && f.MouGuoJia is { Stage: MouGuoJiaStage.Judging, Yin: true, Operation: SkillProgramEffectOp.ResolveMouGuoJiaXianmou } r && r.Actual >= r.X && r.X > 0 && target == f.OwnerSeat && reason == JudgmentReasons.Lightning && bind == "mou-xianmou-lightning" && visibility == SkillProgramCardSetVisibility.Public;
    internal void BeginMouGjJudgment(ProgramSkillFrame f)
    {
        if (!MouGjJudgmentRequest(f, f.OwnerSeat, JudgmentReasons.Lightning, "mou-xianmou-lightning", SkillProgramCardSetVisibility.Public) || f.MouGuoJia!.JudgmentId != 0)
            throw new InvalidOperationException("Xianmou can issue only one native Lightning judgment after its real X-card discard.");
        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MouGuoJia = f.MouGuoJia with { JudgmentId = id } });
        _ = _coreNativeJudgmentOperations.BeginJudgment(null, f.OwnerSeat, JudgmentReasons.Lightning, f.Id, CardKind.Lightning, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "mou-xianmou-lightning", programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    internal bool IsMouGjJudgment(JudgmentFrame j, ProgramSkillFrame f) => MouGjJudgmentRequest(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private) && f.MouGuoJia!.JudgmentId == j.Id && j.ParentFrameId == f.Id && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill;
    internal bool IsMouGjLightningJudgment(JudgmentFrame j) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is { } f && IsMouGjJudgment(j, f);
    internal bool ReturnMouGjJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoJia: { Stage: MouGuoJiaStage.Judging } } f || j.ParentFrameId != f.Id)
            return false;
        if (!IsMouGjJudgment(j, f) || _resolutionStackOperations._resolutionStack.Any(x => x.Id == j.Id))
            throw new InvalidOperationException("Xianmou received a different or still-active native judgment return.");
        var result = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JudgmentResolvedEvent>().Single(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.TargetSeat == f.OwnerSeat && e.Reason == JudgmentReasons.Lightning);
        CardMovementRecord? handoff = null;
        if (result.CardId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
        {
            handoff = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id);
            if (handoff is null || handoff.From != CardLocation.Judgment(f.OwnerSeat) || handoff.To != CardLocation.Processing || handoff.Reason.Value != "skill-program.judgment.result")
                throw new InvalidOperationException("Xianmou cannot retire a claimed or unrelated judgment entity.");
        }

        MouGuoJia.JudgmentReturned(f, result, handoff?.Sequence);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (handoff is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(handoff.CardId), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish, beforeFact: m =>
            {
                var own = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, own with { MouGuoJia = own.MouGuoJia!with { After = m.Sequence } });
            });
        MouGuoJia.Continue(f.Id);
        return true;
    }
}
