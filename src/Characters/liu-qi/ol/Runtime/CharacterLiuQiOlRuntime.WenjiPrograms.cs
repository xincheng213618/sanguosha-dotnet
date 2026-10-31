#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuQiOlRuntime
{
    internal IReadOnlyList<Card> WenjiPayableCards(int seat) => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(seat)).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Equipment(seat))).Where(c => !c.IsGeneralWeapon).ToArray();
    internal void PublishWenjiChoice(ProgramSkillFrame f)
    {
        var r = f.LiuQi!;
        var seat = r.Stage == LiuQiStage.ChooseRecipient ? f.OwnerSeat : r.RecipientSeat!.Value;
        var choices = LiuQiChoices(f);
        if (choices.Count == 0)
        {
            FinishLiuQi(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "请选择问计交牌。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == LiuQiStage.ChooseGift,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wenji")]
    internal void ResolveWenjiChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireLiuQi(f);
        var r = f.LiuQi!;
        var seat = r.Stage == LiuQiStage.ChooseRecipient ? f.OwnerSeat : r.RecipientSeat!.Value;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LiuQiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Wenji lost its exact independent recipient authority.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
        {
            FinishLiuQi(f);
            return;
        }

        if (r.Stage == LiuQiStage.ChooseRecipient)
        {
            if (!_coreDragonBoatGeneralsOperations.WenjiEligibleGiftRecipients(f.OwnerSeat, f.SkillId).Any(p => p.Seat == choice.Targets.Single()))
                throw new InvalidOperationException("Wenji lost its legal gift recipient.");
            r = r with
            {
                RecipientSeat = choice.Targets.Single(),
                Stage = LiuQiStage.ChooseGift
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuQi = r });
            PublishWenjiChoice(f);
            return;
        }

        var id = choice.Cards.Single();
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        r = r with
        {
            CardId = id,
            From = _cardZonesOperations._cardZones.GetLocation(id),
            Name = card.Kind,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Stage = LiuQiStage.GiftChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuQi = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(f.OwnerSeat), new(WenjiGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.RecipientSeat!.Value, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var paid = r with
            {
                After = records.Single().Sequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { LiuQi = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenjiGiftPaidEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, paid.RecipientSeat!.Value, id, paid.From!.Value, paid.Name!.Value, paid.Before, paid.After));
            var grant = new WenjiTurnNameGrant(paid.Source, paid.GameplayHash, paid.ActualTurn, f.Id, paid.RecipientSeat.Value, id, paid.Name.Value, paid.After);
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].WenjiTurnNameGrants = (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].WenjiTurnNameGrants ?? []).Append(grant).ToArray();
            _characterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi = true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenjiNameArmedEvent(grant));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
