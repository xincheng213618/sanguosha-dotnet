#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRiceJudgmentRuntime
{
    internal IReadOnlyList<Card> RiceJudgmentReplacementCards(CharacterState owner, SkillProgramTrigger trigger)
    {
        var effect = trigger.Effects.Single();
        return _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, SkillProgramTriggerWindow.JudgmentReplacing).Where(binding => ReferenceEquals(binding.Trigger, trigger) && binding.Trigger.Effects is [{ Op: SkillProgramEffectOp.ReplaceJudgmentFromPublicRice }]).SelectMany(binding => _coreRicePileOperations.RiceSources(owner.Seat, effect, binding.SkillInstanceId)).SelectMany(_corePublicPileSourcesOperations.PublicPileCards).DistinctBy(card => card.Id).OrderBy(card => card.Id).ToArray();
    }

    internal SkillProgramStepOutcome BeginRiceJudgmentReplacement(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreRicePileOperations.ExactRiceParent(f, effect.Op) || f.RicePile is not null || f.WindowContext?.JudgmentReplacement is not { } context || _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is not { } judgment || judgment.Id != context.JudgmentFrameId || _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(judgment)?.Id != context.OldCardId)
            throw new InvalidOperationException("Rice judgment must retain its real selected replacement and owning judgment candidate.");
        var pile = _coreRicePileOperations.RiceSources(f.OwnerSeat, effect, f.SkillInstanceId).Single(p => _corePublicPileSourcesOperations.PublicPileCards(p).Any(c => c.Id == context.ReplacementCardId));
        var replacement = _corePublicPileSourcesOperations.PublicPileCards(pile).Single(c => c.Id == context.ReplacementCardId);
        var old = _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(judgment)!;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[judgment.TargetSeat], replacement);
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        var r = new RicePileReceipt
        {
            Operation = effect.Op,
            Source = _coreRicePileOperations.RiceIssuer(f),
            GameplayHash = f.GameplayHash,
            ParentId = context.JudgmentFrameId,
            ActualTurn = _coreActualTurnProgressTurnNumberAccess._turnNumber,
            Pile = pile,
            BeneficiarySeat = judgment.TargetSeat,
            Stage = RicePileStage.MovementChildren,
            CardIds = [replacement.Id],
            From = [pile.Location],
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r, PendingMovementContinuation = new(judgment.TargetSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePileStartedEvent(f.Id, r));
        _coreJudgmentReplacementProgramsOperations.CommitProgramJudgmentReplacement(judgment, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], old, replacement, suit, SkillProgramOldJudgmentCardDestination.DiscardPile, pile.Location);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { RicePile = r with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence }, WindowContext = f.WindowContext!with { JudgmentReplacement = context with { ReplacementSuit = suit, ReplacementRank = replacement.Rank } } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePilePaymentIssuedEvent(f.Id, f.RicePile!));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementResolvedEvent(judgment.Id, judgment.Id, judgment.TargetSeat, f.OwnerSeat, judgment.Reason, true, old.Id, replacement.Id, replacement.Kind, suit, replacement.Rank));
        _coreMatchLogOperations.AddLog("JudgmentReplaced", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 用米替换了判定牌。", f.OwnerSeat, judgment.TargetSeat);
        if (!_coreRicePileOperations.DrainRiceChildren(f))
            _coreRicePileOperations.ReturnRiceMovement(f);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
