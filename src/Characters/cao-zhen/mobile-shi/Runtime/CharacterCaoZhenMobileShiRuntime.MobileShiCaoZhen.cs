#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenMobileShiRuntime
{
    SkillProgramStepOutcome IShiCaoZHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginShiCaoZ(f, operation);
    internal SkillProgramStepOutcome BeginShiCaoZ(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShiCaoZSifeng || f.ShiCaoZhen is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Sifeng requires its original issued native trigger.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace && (f.TriggerId != "place-forecast" || context.Window != SkillProgramTriggerWindow.TurnEnding || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive) || operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengUse && !IsIssuedShiCaoZUseCandidate(candidate, context) || operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengEnd && !IsIssuedShiCaoZEndCandidate(candidate, context))
            throw new InvalidOperationException("Sifeng lost its own ending, completed native Use or source-independent issued end.");
        var stage = operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace ? ShiCaoZStage.TopChildren : operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengUse ? ShiCaoZStage.UseRemovalChildren : ShiCaoZStage.EndChoice;
        var r = new ShiCaoZReceipt(ShiCaoZSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, context.ParentFrameId, operation, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Paid = operation != SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId, operation));
        if (stage == ShiCaoZStage.TopChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            for (var i = 0; i < 3 && _corePhysicalDrawFacadesOperations.EnsureDrawPile(); i++)
            {
                var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new(ShiCaoZTopReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZTopTakenEvent(f.Id, i, card.Id, m.Sequence)), disclosure: CardMovementDisclosure.FaceDown);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = f.ShiCaoZhen!with { Pool = f.ShiCaoZhen.Pool.Append(card.Id).ToArray(), After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            }
        }
        else if (stage == ShiCaoZStage.UseRemovalChildren)
        {
            if (!TryShiCaoZCompletedUse(context, out var action, out var origin))
                throw new InvalidOperationException("Sifeng cannot replace its completed native use.");
            var first = LiveShiCaoZMaterials().First(m => m.Holder == action.ActorSeat && ShiCaoZSameSource(m, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash));
            var different = ShiCaoZColor(first.Suit) != ShiCaoZColor(action.EffectiveSuit ?? Suit.None);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Paid = true, Holder = action.ActorSeat, UseId = origin, ActionId = action.ActionId, UsedSuit = action.EffectiveSuit ?? Suit.None, Materials = [first] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZUseIssuedEvent(f.Id, r.Source, origin, action.ActionId, action.ActorSeat, first.PlacementId, action.EffectiveSuit ?? Suit.None, different));
            MoveShiCaoZMaterial(f, first, CardLocation.DiscardPile, ShiCaoZRemoveReason);
        }
        else
        {
            var holder = context.SourceSeat!.Value;
            var materials = LiveShiCaoZMaterials().Where(m => m.Holder == holder && m.Source.OwnerSeat == f.OwnerSeat).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Holder = holder, Materials = materials } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZEndIssuedEvent(f.Id, r.Source, r.Turn, holder, materials.Length));
        }

        ContinueShiCaoZ(f.Id);
        return _corePendingDecisionPolicyPipelineOperations._pendingDecision is null ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    internal static int ShiCaoZColor(Suit suit) => suit is Suit.Heart or Suit.Diamond ? 1 : suit is Suit.Spade or Suit.Club ? 2 : 0;
    internal int ShiCaoZChooser(ProgramSkillFrame f) => f.ShiCaoZhen!.Stage == ShiCaoZStage.Discard ? f.ShiCaoZhen.Holder : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> ShiCaoZChoices(ProgramSkillFrame f)
    {
        var r = f.ShiCaoZhen!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string token, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => choices.Add(new(new($"shi-cao-zhen.{(f.Id)}.{(branch)}.{(token)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-cao-zhen", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) }));
        if (r.Stage == ShiCaoZStage.Assign)
        {
            foreach (var id in r.Pool.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                    Add("assign", $"{(id)}.{(target.Seat)}", $"将【{(CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)))}】放在{(target.Name)}的伺锋队尾", [id], [target.Seat]);
            return _coreCardDistributionPresentationOperations.PresentCardDistribution(f, choices, "shi-cao-zhen-forecast", r.Pool.Count, label: c => "扣置于 " + _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()].Name);
        }

        if (r.Stage == ShiCaoZStage.Discard)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Holder, r.Holder, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, destination: SkillProgramCardDestination.DiscardPile).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-shi-cao-zhen", ["branch"] = "discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) } }).ToArray());
        if (r.Stage == ShiCaoZStage.EndChoice && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
        {
            Add("obtain", "all", $"获得{(_coreMatchParticipantsPlayersAccess._players[r.Holder].Name)}剩余的{(r.Materials.Count)}张伺锋");
            Add("damage", "all", $"移去{(_coreMatchParticipantsPlayersAccess._players[r.Holder].Name)}剩余的伺锋并对其造成1点伤害");
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiCaoZ(ProgramSkillFrame f)
    {
        var choices = ShiCaoZChoices(f);
        if (choices.Count == 0)
        {
            ContinueShiCaoZ(f.Id);
            return;
        }

        var chooser = ShiCaoZChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, f.ShiCaoZhen!.Stage switch
        {
            ShiCaoZStage.Assign => "伺锋：依序选择真实顶牌和其他角色；同一角色按放置顺序移去首张。",
            ShiCaoZStage.Discard => "伺锋颜色不同：弃置一张手牌。",
            _ => "伺锋：选择获得剩余牌，或移去后造成1点伤害。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = chooser,
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-cao-zhen")]
    internal void AnswerShiCaoZ(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiCaoZhen: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != ShiCaoZChooser(f) || !ShiCaoZChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Sifeng lost its exact private order, recipient, hand-only payment or end choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (r.Stage == ShiCaoZStage.Assign)
        {
            var id = selected.Cards.Single();
            var holder = selected.Targets.Single();
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var placement = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
            var location = new CardLocation(CardZoneKind.PrivatePersistentPile, holder, $"shi-cao-zhen:{(placement)}");
            _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.AssignmentChildren, Pool = r.Pool.Where(c => c != id).ToArray(), Assigned = r.Assigned + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, location, new(ShiCaoZPlaceReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZPlacedEvent(new(placement, f.Id, r.Assigned, r.Source, r.Hash, holder, id, card.Kind, card.Suit, location, m.Sequence))), disclosure: CardMovementDisclosure.FaceDown);
            SetShiCaoZAfter(f.Id);
            ContinueShiCaoZ(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (r.Stage == ShiCaoZStage.Discard)
        {
            var id = selected.Cards.Single();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.DiscardChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Holder, [id], new(ShiCaoZDiscardReason));
            SetShiCaoZAfter(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZDiscardPaidEvent(f.Id, r.Holder, id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueShiCaoZ(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        BeginShiCaoZEndMove(f, branch == "obtain");
        ContinueShiCaoZ(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void SetShiCaoZAfter(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiCaoZhen = f.ShiCaoZhen!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void MoveShiCaoZMaterial(ProgramSkillFrame f, ShiCaoZMaterial material, CardLocation destination, string reason)
    {
        if (_cardZonesOperations._cardZones.GetLocation(material.CardId) != material.Location)
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.Location, destination, new(reason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZRemovedEvent(material.PlacementId, f.Id, material.CardId, m.Sequence, reason)), disclosure: destination.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        SetShiCaoZAfter(f.Id);
    }

    internal void BeginShiCaoZEndMove(ProgramSkillFrame supplied, bool obtain)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShiCaoZhen!;
        var alive = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive;
        obtain &= alive;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.EndChildren, Paid = true, Damage = !obtain && alive && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZEndChoiceEvent(f.Id, obtain, alive, r.Materials.Count));
        foreach (var material in r.Materials)
            MoveShiCaoZMaterial(f, material, obtain ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile, obtain ? ShiCaoZObtainReason : ShiCaoZRemoveReason);
    }

    internal void ContinueShiCaoZ(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiCaoZhen: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (f.PendingMovementContinuation is not null)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShiCaoZhen!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        }

        if (r.Stage is ShiCaoZStage.TopChildren or ShiCaoZStage.AssignmentChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.Assign } });
            r = f.ShiCaoZhen!;
        }

        if (r.Stage == ShiCaoZStage.Assign)
        {
            if (r.Pool.Count == 0)
            {
                FinishShiCaoZ(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.CleanupChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                foreach (var card in r.Pool.Where(card => _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.Processing))
                    _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), CardLocation.Processing, CardLocation.DiscardPile, new(ShiCaoZRemoveReason), disclosure: CardMovementDisclosure.FaceUp);
                SetShiCaoZAfter(id);
                ContinueShiCaoZ(id);
                return;
            }

            PublishShiCaoZ(f);
            return;
        }

        if (r.Stage == ShiCaoZStage.UseRemovalChildren)
        {
            var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZUseIssuedEvent>().Single(e => e.FrameId == id);
            if (issued.DifferentColor && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.Discard } });
                r = f.ShiCaoZhen!;
            }
            else
            {
                FinishShiCaoZ(f);
                return;
            }
        }

        if (r.Stage == ShiCaoZStage.Discard)
        {
            if (ShiCaoZChoices(f).Count == 0)
                FinishShiCaoZ(f);
            else
                PublishShiCaoZ(f);
            return;
        }

        if (r.Stage == ShiCaoZStage.EndChoice)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
            {
                BeginShiCaoZEndMove(f, false);
                ContinueShiCaoZ(id);
                return;
            }

            PublishShiCaoZ(f);
            return;
        }

        if (r.Stage == ShiCaoZStage.EndChildren && r.Damage && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = r with { Stage = ShiCaoZStage.DamageChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZDamageIssuedEvent(f.Id, f.OwnerSeat, r.Holder));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Damage(f, r.Holder, 1);
            return;
        }

        FinishShiCaoZ(f);
    }

    internal void FinishShiCaoZ(ProgramSkillFrame f)
    {
        if (f.ShiCaoZhen!.Stage == ShiCaoZStage.Complete)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiCaoZhen = f.ShiCaoZhen with { Stage = ShiCaoZStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZCompletedEvent(f.Id, f.ShiCaoZhen.Operation));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49900)]
    internal bool ResumeShiCaoZ(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiCaoZhen: not null } f || f.Id != id)
            return false;
        ContinueShiCaoZ(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49900)]
    internal bool ReturnShiCaoZMovement(ProgramSkillFrame f)
    {
        if (f.ShiCaoZhen is null)
            return false;
        ContinueShiCaoZ(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace, SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengUse, SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengEnd)]
    internal PromptChoice ShiCaoZAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "damage" && !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.ShiCaoZhen!.Holder])) ?? decision.Choices[0];
}
