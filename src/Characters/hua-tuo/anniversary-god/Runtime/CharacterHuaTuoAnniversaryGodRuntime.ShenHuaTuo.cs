#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoAnniversaryGodRuntime
{
    internal static CardConversionSource ShenHtSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal Card ShenHtCard(int id) => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
    SkillProgramStepOutcome IShenHuaTuoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenHuaTuo(f, op);
    internal SkillProgramStepOutcome BeginShenHuaTuo(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenHuaTuo is not null || f.TriggerId is not null || f.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("Shen Hua Tuo requires its accepted original active occurrence.");
        var target = f.SelectedTargetSeats.Single();
        var r = new ShenHuaTuoReceipt(ShenHtSource(f), f.GameplayHash, f.InstructionIndex, target, op == SkillProgramEffectOp.ResolveShenHuaTuoLuxin ? ShenHuaTuoStage.LuxinChoice : ShenHuaTuoStage.RestoreChildren, Count: Math.Min(5, Math.Max(0, _coreActualRoundProgressRoundNumberAccess._roundNumber)));
        if (op == SkillProgramEffectOp.ResolveShenHuaTuoLuxin)
        {
            if (f.SelectedCardIds is not [var card] || f.SelectedCardPaymentResult is not { Completed: true, Operation: SkillProgramEffectOp.GiveSelected } || f.SelectedCardPayment is not { MovementCommitted: true, CardIds: [var paidCard] } || paidCard != card)
                throw new InvalidOperationException("Luxin requires its already-completed real GiveSelected payment.");
            r = r with
            {
                GiftId = card,
                GiftKind = CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(ShenHtCard(card).Kind)
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r });
        if (op == SkillProgramEffectOp.ResolveShenHuaTuoHuandao && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, target, false);
            _coreProgramLifecycleOperations.SetProgramChainedState(f, false, target);
        }

        ContinueShenHuaTuo(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShenHtChoices(ProgramSkillFrame f)
    {
        var r = f.ShenHuaTuo!;
        PromptChoice Choice(string key, string label) => new(new("shen-hua-tuo." + f.Id + "." + key), label, [], [r.TargetSeat], new Dictionary<string, string> { ["program-action"] = "shen-hua-tuo", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = key });
        if (r.Stage == ShenHuaTuoStage.LuxinChoice)
            return[Choice("draw", "令其摸" + r.Count + "张牌"), Choice("discard", "令其随机弃置" + r.Count + "张手牌")];
        if (r.Stage == ShenHuaTuoStage.AcceptSkill)
            return ShenHtGeneralCandidates(r.TargetSeat).Length == 0 ? [] : [Choice("accept", "获得随机一名同名武将的随机一个技能，然后失去一个其他技能"), Choice("decline", "不获得技能")];
        if (r.Stage == ShenHuaTuoStage.LoseSkill)
            return ShenHtLossSkillIds(r).Select(s => Choice("lose:" + s, "失去【" + _contentRegistryOperations._contentRegistry.GetSkill(s).Name + "】")).ToArray();
        return[];
    }

    internal void PublishShenHt(ProgramSkillFrame f)
    {
        var r = f.ShenHuaTuo!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = ShenHtChoices(f);
        var chooser = r.Stage == ShenHuaTuoStage.LuxinChoice ? f.OwnerSeat : r.TargetSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：请选择。", [], [r.TargetSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shen-hua-tuo")]
    internal void AnswerShenHt(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenHuaTuo: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == ShenHuaTuoStage.LuxinChoice ? f.OwnerSeat : r.TargetSeat) || !ShenHtChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shen Hua Tuo requires its exact original chooser and current option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var key = choice.Parameters["option"];
        if (r.Stage == ShenHuaTuoStage.LuxinChoice)
        {
            if (key == "draw")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.Drawing, HpDelta = 1 } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.TargetSeat, r.Count, new("skill.shen-hua-tuo.luxin.draw"));
                return;
            }

            var pool = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).ToList();
            var chosen = new List<int>();
            while (pool.Count > 0 && chosen.Count < r.Count)
            {
                var i = _randomOperations._random.Next(pool.Count);
                chosen.Add(pool[i].Id);
                pool.RemoveAt(i);
            }

            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.DiscardChildren, HpDelta = -1, DiscardedIds = chosen, SequenceBefore = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (chosen.Count > 0)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, chosen, new("skill.shen-hua-tuo.luxin.random-discard"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.From == CardLocation.Hand(r.TargetSeat) && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(r.TargetSeat)).Select(m => m.CardId).Distinct().ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = f.ShenHuaTuo!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DiscardedIds = actual } });
            ArmShenHtLuxin(f, actual);
            ContinueShenHuaTuo(f.Id);
            return;
        }

        if (r.Stage == ShenHuaTuoStage.AcceptSkill)
        {
            if (key == "decline")
            {
                FinishShenHt(f);
                return;
            }

            AcquireShenHtRandomSkill(f);
            return;
        }

        if (r.Stage == ShenHuaTuoStage.LoseSkill)
        {
            RevokeShenHtSkill(f, key[5..]);
            return;
        }

        throw new InvalidOperationException("Shen Hua Tuo cannot repay an issued child.");
    }

    internal void ArmShenHtLuxin(ProgramSkillFrame f, IReadOnlyList<int> ids)
    {
        var r = f.ShenHuaTuo!;
        if (ids.Any(id => CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(ShenHtCard(id).Kind) == r.GiftKind))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoLuxinArmedEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.HpDelta));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShenHuaTuoLuxin)]
    internal void ReturnShenHtDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenHuaTuo!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.DrawChildren, DrawOutcome = outcome, SequenceBefore = outcome.SequenceBefore, SequenceAfter = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ArmShenHtLuxin(f, outcome.ActualDrawIds);
        ContinueShenHuaTuo(f.Id);
    }

    internal string[] ShenHtGeneralCandidates(int seat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        var families = new[]
        {
            target.General.Id,
            target.SecondaryGeneral?.Id
        }.OfType<string>().Select(id => ShenHtFamily(_contentRegistryOperations._contentRegistry.Generals[id])).ToHashSet(StringComparer.Ordinal);
        return _contentRegistryOperations._contentRegistry.Generals.Values.Where(g => !g.Id.StartsWith("standard:", StringComparison.Ordinal) && families.Contains(ShenHtFamily(g)) && g.SkillIds.Count > 0 && g.SkillIds.All(s => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(s))).Select(g => g.Id).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    }

    internal static string ShenHtFamily(ContentGeneralDefinition g)
    {
        var key = g.Id[(g.Id.IndexOf(':') + 1)..];
        if (g.FactionId == "god" && key.StartsWith("shen-", StringComparison.Ordinal))
            key = key[5..];
        return (g.CharacterId ?? "character:" + key) switch
        {
            "character:gao-da-yi-hao" => "character:zhao-yun",
            "character:fu-huanghou" => "character:fu-huang-hou",
            "character:wolong-zhuge-liang" => "character:zhuge-liang",
            var family => family
        };
    }

    internal void AcquireShenHtRandomSkill(ProgramSkillFrame f)
    {
        var r = f.ShenHuaTuo!;
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var owned = _coreRandomDamageTargetSkillsOperations.OwnedAndRestorableRandomSkillIds(target);
        var generals = ShenHtGeneralCandidates(r.TargetSeat);
        if (generals.Length == 0)
        {
            FinishShenHt(f);
            return;
        }

        var before = _randomOperations._random.State;
        var general = generals[_randomOperations._random.Next(generals.Length)];
        var afterGeneral = _randomOperations._random.State;
        var skills = _contentRegistryOperations._contentRegistry.Generals[general].SkillIds.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        var selected = skills[_randomOperations._random.Next(skills.Length)];
        var afterSkill = _randomOperations._random.State;
        var sourceId = "acquired:shen-hua-tuo.huandao:" + f.Id + ":" + target.Seat;
        var added = !owned.Contains(selected);
        var grantId = added ? sourceId + ":" + selected : null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.SkillChildren, GeneralCandidates = generals, SkillCandidates = skills, SelectedGeneralId = general, SelectedSkillId = selected, RandomBefore = before, RandomAfterGeneral = afterGeneral, RandomAfterSkill = afterSkill, GrantedGrantId = grantId, GrantAdded = added } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoRandomGeneralEvent(f.Id, target.Seat, general));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoRandomSkillEvent(f.Id, target.Seat, general, selected));
        if (!added)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoSkillUnchangedEvent(f.Id, target.Seat, general, selected));
            ContinueShenHuaTuo(f.Id);
            return;
        }

        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        if (!target.SkillGrants.Grant(new(grantId!, selected, grantId!, sourceId)))
            throw new InvalidOperationException("Huandao cannot grant twice.");
        _coreConversionStatesOperations.RegisterTaggedConversionSkill(target, selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoSkillGrantedEvent(f.Id, target.Seat, general, selected, grantId!));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(target.Seat, f.SkillId, Array.AsReadOnly(new[] { selected })));
        ContinueShenHuaTuo(f.Id);
    }

    internal IEnumerable<SkillGrant> ShenHtRevocableGrants(int seat, string id) => _coreDamageTargetSkillRoundDrawOperations.OwnedAndRestorableRoundSkillGrants(_coreMatchParticipantsPlayersAccess._players[seat], id).Where(g => !g.IsModeRule && !g.SourceId.StartsWith("equipment:", StringComparison.Ordinal)).DistinctBy(g => (g.GrantId, g.SkillInstanceId, g.SourceId));
    internal string[] ShenHtLossSkillIds(ShenHuaTuoReceipt r) => _coreRandomDamageTargetSkillsOperations.OwnedAndRestorableRandomSkillIds(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Where(s => s != r.SelectedSkillId && ShenHtRevocableGrants(r.TargetSeat, s).Any()).Order(StringComparer.Ordinal).ToArray();
    internal void RevokeShenHtSkill(ProgramSkillFrame f, string skill)
    {
        var r = f.ShenHuaTuo!;
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        var grants = ShenHtRevocableGrants(target.Seat, skill).ToArray();
        if (grants.Length == 0 || skill == r.SelectedSkillId)
            throw new InvalidOperationException("Huandao lost its chosen other skill.");
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        foreach (var g in grants)
        {
            if (target.SkillGrants.Grants.Any(live => live.GrantId == g.GrantId))
                target.SkillGrants.RemoveGrant(g.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenHuaTuoSkillRevokedEvent(f.Id, target.Seat, skill, g.GrantId, g.SkillInstanceId, g.SourceId));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.SkillChildren } });
        ContinueShenHuaTuo(f.Id);
    }

    internal void ContinueShenHuaTuo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenHuaTuo: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == ShenHuaTuoStage.Drawing)
            return;
        if (r.Stage is ShenHuaTuoStage.DrawChildren or ShenHuaTuoStage.DiscardChildren or ShenHuaTuoStage.RestoreChildren or ShenHuaTuoStage.SkillChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenHuaTuo!;
            if (r.Stage == ShenHuaTuoStage.RestoreChildren)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.AcceptSkill } });
            else if (r.Stage == ShenHuaTuoStage.SkillChildren && r.GrantAdded && r.SelectedSkillId is not null && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShenHuaTuoSkillRevokedEvent>().Any(e => e.FrameId == id))
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenHuaTuo = r with { Stage = ShenHuaTuoStage.LoseSkill } });
            else
            {
                FinishShenHt(f);
                return;
            }
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.ShenHuaTuo!.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || ShenHtChoices(f).Count == 0)
        {
            FinishShenHt(f);
            return;
        }

        PublishShenHt(f);
    }

    internal void FinishShenHt(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenHuaTuo = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38700)]
    internal bool ResumeShenHt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenHuaTuo: not null } f || f.Id != id)
            return false;
        ContinueShenHuaTuo(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38700)]
    internal bool ReturnShenHtMovement(ProgramSkillFrame f)
    {
        if (f.ShenHuaTuo is null)
            return false;
        ContinueShenHuaTuo(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38700)]
    internal bool ContinueIssuedShenHt(ProgramSkillFrame f) => f.ShenHuaTuo is not null;

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenHuaTuoLuxin)]
    internal PromptChoice ChooseShenHtLuxin(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenHuaTuoHuandao)]
    internal PromptChoice ChooseShenHtHuandao(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
