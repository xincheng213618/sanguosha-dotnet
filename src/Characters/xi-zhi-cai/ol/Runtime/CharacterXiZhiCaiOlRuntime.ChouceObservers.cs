#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{
    internal GameEngine.IDamageAttempt? ChouceOriginalAttack(ChoucePointReceipt r)
    {
        var owner = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.OriginalAttackId);
        return owner switch
        {
            ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(r.OriginalAttackId),
            CardUseFrame { CardAttack.Active: true } or ProgramSkillFrame { CardAttack.Active: true } or JudgmentFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(r.OriginalAttackId),
            _ => null
        };
    }

    internal bool ValidChouce(ProgramSkillFrame f)
    {
        if (f.Chouce is not { } p)
            return false;
        var r = p.Receipt;
        if (r.Source != XiZhiCaiSource(f) || r.GameplayHash != f.GameplayHash || r.DamageTargetSeat != f.OwnerSeat || r.PointIndex < 0 || r.PointIndex >= r.OriginalAmount || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } c || c.ParentFrameId != r.DamageWindowId || c.OccurrenceIndex != r.PointIndex || f.InstructionIndex < r.StartInstructionIndex || f.InstructionIndex > r.StartInstructionIndex + 1 || !Enum.IsDefined(p.Stage) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChouceJudgmentIssuedEvent>().Count(e => e.FrameId == f.Id && e.Receipt == r) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame w || w.Id != r.DamageWindowId || w.ParentFrameId != r.DamageFrameId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()))
            return false;
        var original = ChouceOriginalAttack(r);
        if (original is null || original.ResolutionId != r.OriginalAttackId || original.TargetSeat != r.DamageTargetSeat || original.DamageAmount != r.OriginalAmount || !original.DamageWasApplied || (original.IsSourceLess || original.IsDelayedJudgmentDamage ? null : (int? )original.SourceSeat) != r.DamageSourceSeat)
            return false;
        if (p.FrozenSuit is { } suit && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChouceJudgmentFrozenEvent>().Count(e => e.FrameId == f.Id && e.JudgmentFrameId == r.JudgmentFrameId && e.EffectiveSuit == suit) != 1)
            return false;
        if (p.Stage == ChouceStage.Judgment)
            return _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op is SkillProgramEffectOp.StartChouceDamagePointJudgment or SkillProgramEffectOp.ResolveChouceFrozenJudgment;
        if (p.FrozenSuit is null || f.InstructionIndex != r.StartInstructionIndex + 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveChouceFrozenJudgment)
            return false;
        if (p.Stage is ChouceStage.ChooseTarget or ChouceStage.ChooseCard)
            return f.PendingMovementContinuation is null;
        if (p.TargetSeat is not { } target || p.Before < 0 || p.After < p.Before || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null, SubjectSeat: var actor } || actor != target)
            return false;
        var red = ChouceIsRed(f);
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChouceOutcomeIssuedEvent>().Count(e => e.FrameId == f.Id && e.JudgmentFrameId == r.JudgmentFrameId && e.TargetSeat == target && e.Red == red && e.Before == p.Before && e.After == p.After && e.Count == p.Count && e.CardId == p.CardId) != 1)
            return false;
        return red ? p.Count is >= 0 and <= 2 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(target) && m.Reason.Value == "program.chouce.red") == p.Count : p.Count == 1 && p.CardId is { } id && p.CardLocation is { } location && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.CardId == id && m.From == location && m.Reason.Value == "program.chouce.black" && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(id).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)))) == 1;
    }
}
