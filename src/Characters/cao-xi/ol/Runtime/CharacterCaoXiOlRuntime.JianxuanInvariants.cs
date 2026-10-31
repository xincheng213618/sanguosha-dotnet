#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal bool ValidCaoXiJianxuan(ProgramSkillFrame f)
    {
        if (f.CaoXiJianxuan is not { } r || f.SkillId != "ol:jianxuan" || f.InstructionIndex != 1 || r.InstructionIndex != 1 || r.Source != CaoXiSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c || c.ParentFrameId != r.ParentWindowId || c.DamageFrameId != r.DamageFrameId || c.TargetSeat != f.OwnerSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveJianxuanRepeatedDraw || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiJianxuanStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ParentWindowId == r.ParentWindowId && e.DamageFrameId == r.DamageFrameId && e.ActualTurn == r.ActualTurn) != 1)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2 || _resolutionStackOperations._resolutionStack[i - 1] is not DamageTriggerWindowFrame w || w.Id != r.ParentWindowId || w.ParentFrameId != r.DamageFrameId || w.TargetSeat != f.OwnerSeat || w.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack[i - 2] is not DamageFrame damage || damage.Id != r.DamageFrameId || damage.TargetSeat != f.OwnerSeat || damage.Amount != c.Amount || damage.Amount <= 0)
            return false;
        var recipients = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiJianxuanRecipientEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiJianxuanDrawPaidEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Cursor).ToArray();
        var compared = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CaoXiJianxuanComparedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Cursor).ToArray();
        if (r.Stage == CaoXiJianxuanStage.ChoosingRecipient)
            return r.RecipientSeat is null && r.Cursor == 0 && r.Before == 0 && r.After == 0 && r.ActualDrawCount == 0 && recipients.Length == 0 && paid.Length == 0 && compared.Length == 0;
        if (r.RecipientSeat is not { } recipient || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipients is not [{ RecipientSeat: var chosen }] || chosen != recipient || r.Cursor < 1 || paid.Length != r.Cursor || compared.Length != r.Cursor - 1 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualDrawCount is < 0 or > 1)
            return false;
        for (var n = 0; n < paid.Length; n++)
        {
            var p = paid[n];
            if (p.Cursor != n + 1 || p.RecipientSeat != recipient || p.ActualDrawCount is < 0 or > 1 || p.Before < 0 || p.After < p.Before || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason.Value == CaoXiDrawReason && m.Sequence > p.Before && m.Sequence <= p.After) != p.ActualDrawCount)
                return false;
            if (n < compared.Length && (compared[n].Cursor != n + 1 || compared[n].RecipientSeat != recipient || !compared[n].Repeat || !compared[n].Numbers.Contains(compared[n].HandCount) || p.ActualDrawCount != 1))
                return false;
        }

        var last = paid[^1];
        return last.Before == r.Before && last.After == r.After && last.ActualDrawCount == r.ActualDrawCount;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26810)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26810)]
    internal bool ContinuePaidCaoXi(ProgramSkillFrame f) => f.CaoXiJianxuan is { Stage: CaoXiJianxuanStage.DrawChildren, Cursor: > 0 } && ValidCaoXiJianxuan(f);
}
