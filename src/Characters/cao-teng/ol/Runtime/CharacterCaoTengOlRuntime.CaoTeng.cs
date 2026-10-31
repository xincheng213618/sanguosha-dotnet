#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoTengOlRuntime
{
    internal const string CaoTengYongzuDrawReason = "skill-program.cao-teng.yongzu.draw";
    internal const string CaoTengJianxiongDrawReason = "skill-program.cao-teng.jianxiong.draw";
    internal const string CaoTengJianxiongClaimReason = "skill-program.cao-teng.jianxiong.claim";
    internal bool HasCaoTengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingliuFactionChoice) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYongzuDistinctBenefits) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveCaoTengJianxiongDrawAndClaim);

    internal static CardConversionSource CaoTengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cao_teng_ol_CaoTeng.CaoTengSource(f);
    SkillProgramStepOutcome ICaoTengHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCaoTeng(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27300)]
    internal bool CanOfferCaoTeng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveQingliuFactionChoice) || context.Window == SkillProgramTriggerWindow.GameStarting || IsCaoTengQingliuDyingExitContext(c, context);
    internal bool CaoTengParentMatches(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat)
            return false;
        if (c.Window == SkillProgramTriggerWindow.AfterDamageApplied)
            return c.Amount > 0 && c.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } w && w.ParentFrameId == c.DamageFrameId && w.TargetSeat == f.OwnerSeat && w.TriggerWindow == c.Window && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate());
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } p || p.Window != c.Window || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]))
            return false;
        return c.Window switch
        {
            SkillProgramTriggerWindow.GameStarting => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.NotStarted && p.Continuation == ProgramLifecycleContinuation.CompleteGameStarting && c.TargetSeat == p.OwnerSeat && c.SourceSeat == p.OwnerSeat,
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => p.OwnerSeat == f.OwnerSeat && c.TargetSeat == f.OwnerSeat && c.SourceSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == f.OwnerSeat,
            SkillProgramTriggerWindow.DyingExited => IsCaoTengQingliuDyingExitContext(p.Candidates[p.CandidateIndex], c),
            _ => false
        };
    }

    internal SkillProgramStepOutcome BeginCaoTeng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var q = op == SkillProgramEffectOp.ResolveQingliuFactionChoice;
        var y = op == SkillProgramEffectOp.ResolveYongzuDistinctBenefits;
        if (f.CaoTeng is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CaoTengParentMatches(f) || f.SkillId != (q ? "ol:qingliu" : y ? "ol:yongzu" : "ol:caoteng-jianxiong") || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Cao Teng requires his exact living unpaid native binding.");
        // Prime only missing native ownership observations before any benefit can
        // grant a skill. Preserve known revisions and their unobserved changes.
        if (y)
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        var r = new CaoTengReceipt(CaoTengSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext!.ParentFrameId, q ? CaoTengStage.FactionChoice : y ? CaoTengStage.YongzuTarget : CaoTengStage.JianxiongDrawChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (!q && !y)
            r = r with
            {
                Materials = _coreProgramLifecycleOperations.GetClaimableProgramDamageCards(f).Select(c => new CaoTengDamageMaterial(c.Id, c.Kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == c.Id).Sequence)).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.ParentId));
        if (q && f.WindowContext.Window == SkillProgramTriggerWindow.DyingExited)
        {
            var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) == "wei" ? "qun" : "wei";
            ApplyCaoTengFaction(f, faction, false);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!q && !y)
        {
            for (var i = 0; i < r.Materials.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengDamageMaterialFrozenEvent(f.Id, i, r.Materials[i]));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengJianxiongDrawIssuedEvent(f.Id, r.Before));
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(CaoTengJianxiongDrawReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoTeng = f.CaoTeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueCaoTeng(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (y && !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat))
        {
            FinishCaoTeng(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishCaoTeng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int CaoTengChooser(ProgramSkillFrame f) => f.CaoTeng is { Stage: CaoTengStage.YongzuChoice or CaoTengStage.YongzuChildren, Cursor: 1, TargetSeat: { } target } ? target : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> CaoTengChoices(ProgramSkillFrame f)
    {
        var r = f.CaoTeng!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string label, int? target = null) => choices.Add(new(new($"cao-teng.{(f.Id)}.{(branch)}.{(target)}"), label, [], target is { } s ? [s] : [], new Dictionary<string, string> { { "program-action", "cao-teng" }, { "branch", branch } }));
        if (r.Stage == CaoTengStage.FactionChoice)
        {
            Add("qun", "加入群势力");
            Add("wei", "加入魏势力");
        }
        else if (r.Stage == CaoTengStage.YongzuTarget)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                Add("target", $"选择{(p.Name)}", p.Seat);
        }
        else if (r.Stage == CaoTengStage.YongzuChoice)
            foreach (var item in new[]
            {
                ("draw", "摸两张牌"),
                ("recover", "回复1点体力"),
                ("restore", "复原武将牌"),
                ("limit", "手牌上限+1"),
                ("skill", "获得本势力技能至自己下回合开始")
            }

            )
                if (item.Item1 != r.FirstOption && (r.SameFaction || item.Item1 is not ("limit" or "skill")) && (item.Item1 != "recover" || _coreMatchParticipantsPlayersAccess._players[CaoTengChooser(f)].HasHpValue && _coreMatchParticipantsPlayersAccess._players[CaoTengChooser(f)].Hp < _coreMatchParticipantsPlayersAccess._players[CaoTengChooser(f)].MaxHp))
                    Add(item.Item1, item.Item2);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishCaoTeng(ProgramSkillFrame f)
    {
        var chooser = CaoTengChooser(f);
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, s.Name, [], CaoTengChoices(f).SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = CaoTengChoices(f),
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("cao-teng")]
    internal void AnswerCaoTeng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoTeng: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != CaoTengChooser(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], CaoTengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Cao Teng lost the actual private chooser or original distinct choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == CaoTengStage.FactionChoice)
        {
            ApplyCaoTengFaction(f, branch, true);
            return;
        }

        if (r.Stage == CaoTengStage.YongzuTarget)
        {
            var target = choice.Targets.Single();
            var same = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[target]) == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = r with { TargetSeat = target, SameFaction = same, Stage = CaoTengStage.YongzuChoice } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengRecipientChosenEvent(f.Id, target, same));
            PublishCaoTeng(f);
            return;
        }

        var actor = CaoTengChooser(f);
        var recovery = branch == "recover" && _coreMatchParticipantsPlayersAccess._players[actor].HasHpValue ? Math.Min(1, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[actor].MaxHp - _coreMatchParticipantsPlayersAccess._players[actor].Hp)) : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, CaoTeng = r = r with { Option = branch, RecoveryRequested = recovery, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Stage = CaoTengStage.YongzuChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengBenefitIssuedEvent(f.Id, r.Cursor, actor, branch, recovery, r.Before));
        if (_coreMatchParticipantsPlayersAccess._players[actor].IsAlive)
        {
            switch (branch)
            {
                case "draw":
                    _coreProgramLifecycleOperations.DrawProgramCards(f.Id, actor, 2, null, null, SkillProgramCardSetVisibility.Private, new(CaoTengYongzuDrawReason));
                    break;
                case "recover":
                    if (recovery > 0)
                        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, actor, recovery, null, null);
                    break;
                case "restore":
                    _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, actor, false);
                    _coreProgramLifecycleOperations.SetProgramChainedState(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), false, actor);
                    break;
                case "limit":
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengHandLimitGrantedEvent(f.Id, r.Cursor, actor));
                    break;
                case "skill":
                    GrantCaoTengSkill(f, actor);
                    break;
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoTeng = f.CaoTeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengBenefitAppliedEvent(f.Id, r.Cursor, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueCaoTeng(f.Id);
    }

    internal bool DrainCaoTeng(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueCaoTeng(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { CaoTeng: { } r } f && f.Id == id)
        {
            if (r.Stage is CaoTengStage.YongzuChildren or CaoTengStage.JianxiongDrawChildren or CaoTengStage.JianxiongClaimChildren)
            {
                if (DrainCaoTeng(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.CaoTeng!;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishCaoTeng(f);
                return;
            }

            if (r.Stage == CaoTengStage.YongzuChildren)
            {
                if (r.Cursor == 1 || r.TargetSeat is not { } target || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                {
                    FinishCaoTeng(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = r with { Cursor = 1, FirstOption = r.Option, Option = null, RecoveryRequested = 0, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Stage = CaoTengStage.YongzuChoice } });
                PublishCaoTeng(f);
                return;
            }

            if (r.Stage == CaoTengStage.JianxiongDrawChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = r = r with { Stage = CaoTengStage.JianxiongClaimChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengJianxiongClaimIssuedEvent(f.Id, r.Before));
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    var cards = _coreProgramLifecycleOperations.GetClaimableProgramDamageCards(f).Where(c => r.Materials.Any(m => m.CardId == c.Id && m.Kind == c.Kind && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(x => x.CardId == c.Id).Sequence == m.EntrySequence)).ToArray();
                    if (cards.Length > 0)
                    {
                        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == r.ParentId);
                        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(damage);
                        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Processing, CardLocation.Hand(f.OwnerSeat), new(CaoTengJianxiongClaimReason));
                        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group)
                        {
                            var claimed = group.DamageClaimedPhysicalCardIds.ToHashSet();
                            foreach (var card in cards)
                                RuntimeSkills.RuntimeRules_Core_CommonRules.RecordClaimedGroupPhysicalCard(claimed, group.ResolutionId, group.PhysicalCards, attack.ResolutionId, card.Id);
                            group.SetClaimedPhysicalCards(claimed.ToArray());
                        }

                        foreach (var card in cards)
                            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengJianxiongClaimedEvent(f.Id, card.Id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > r.Before && m.CardId == card.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == CaoTengJianxiongClaimReason).Sequence));
                        _coreExtraEffectMaterialClaimsOperations.CaptureExtraEffectMaterialClaims(f, attack, cards, r.Before, new(CaoTengJianxiongClaimReason));
                    }
                }

                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoTeng = f.CaoTeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                continue;
            }

            if (r.Stage == CaoTengStage.JianxiongClaimChildren)
            {
                FinishCaoTeng(f);
                return;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCaoTeng(f);
            return;
        }
    }

    internal void FinishCaoTeng(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27300)]
    internal bool ResumeCaoTeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoTeng: not null } f || f.Id != id)
            return false;
        ContinueCaoTeng(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27300)]
    internal bool ContinuePaidCaoTeng(ProgramSkillFrame f) => f.CaoTeng is { } r && (r.Op == SkillProgramEffectOp.ResolveCaoTengJianxiongDrawAndClaim || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoTengBenefitIssuedEvent>().Any(e => e.FrameId == f.Id && e.Index == 0)) && ValidCaoTeng(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveQingliuFactionChoice, SkillProgramEffectOp.ResolveYongzuDistinctBenefits, SkillProgramEffectOp.ResolveCaoTengJianxiongDrawAndClaim)]
    internal PromptChoice ChooseCaoTengAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.CaoTeng!.Stage == CaoTengStage.FactionChoice)
            return p.Choices.First(c => c.Parameters["branch"] == "wei");
        if (f.CaoTeng.Stage == CaoTengStage.YongzuTarget)
            return p.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets[0], new(1, 0, 0, 0, 0, 0, false, false))).ThenBy(c => c.Targets[0]).First();
        var actor = _coreMatchParticipantsPlayersAccess._players[CaoTengChooser(f)];
        var best = actor.IsFaceDown ? "restore" : actor.Hp < actor.MaxHp ? "recover" : "draw";
        return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == best) ?? p.Choices.First();
    }
}
