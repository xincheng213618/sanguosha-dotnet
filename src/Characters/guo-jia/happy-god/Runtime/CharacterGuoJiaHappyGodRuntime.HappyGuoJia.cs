#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaHappyGodRuntime
{
    internal const string HappyGjHuishi = "happy:huishi", HappyGjTianyi = "happy:tianyi", HappyGjGift = "happy:huishi-gift", HappyGjZuoxing = "happy:zuoxing";
    SkillProgramStepOutcome IHappyGuoJiaHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyGuoJia(f, op);
    internal CardConversionSource HappyGjSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 40900, SkillProgramEffectOp.ResolveHappyGuoJiaHuishi)]
    internal bool CanHappyGjHuishi(CharacterState owner) => owner.HasHpValue && owner.MaxHp < 10;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 40901, SkillProgramEffectOp.ResolveHappyGuoJiaHuishiGrant)]
    internal bool CanHappyGjGift(CharacterState owner) => owner.HasHpValue && _coreMatchParticipantsPlayersAccess._players.Any(p => HappyGjGiftTarget(owner, p.Seat));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 40902, SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing)]
    internal bool CanHappyGjZuoxing(CharacterState owner) => HappyGjBenefactors(owner, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, HappyGjZuoxing)).Any() && HappyGjPlans(owner.Seat).Count > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 40900, SkillProgramEffectOp.ResolveHappyGuoJiaTianyi)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40900, SkillProgramEffectOp.ResolveHappyGuoJiaTianyi)]
    internal bool CanAwakenHappyGj(ProgramTriggerLegalityContext g) => _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(g.OwnerSeat, g.SkillId, g.Candidate.BindingId) || _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Any(d => d.TargetSeat == p.Seat && d.Amount > 0));
    internal bool HappyGjGiftTarget(CharacterState owner, int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && (_characterGuoJiaMobileGodOperations.ShenGuoJiaUnawakened(seat).Count > 0 ? owner.MaxHp >= _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) : owner.MaxHp > 2);
    internal IEnumerable<int> HappyGjBenefactors(CharacterState owner, string instance) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyGuoJiaBenefactorIssuedEvent>().Where(e => e.Recipient == owner.Seat && e.GrantedInstance == instance && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Source.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat] is { IsAlive: true, HasHpValue: true, MaxHp: > 1 } && owner.SkillGrants.Grants.Any(g => g.IsEnabled && g.SkillId == HappyGjZuoxing && g.SkillInstanceId == instance && g.SourceId == "acquired:" + e.GrantSource)).Select(e => e.Source.OwnerSeat).Distinct();
    internal IReadOnlyList<ShenGuoJiaPlan> HappyGjPlans(int owner)
    {
        var starts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaStartedEvent>().Where(e => e.Source is { SkillId: HappyGjZuoxing } && e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat).Select(e => e.FrameId).ToHashSet();
        var used = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaNativeIssuedEvent>().Where(e => starts.Contains(e.FrameId)).Select(e => e.Kind).ToHashSet();
        return Array.AsReadOnly(_characterGuoJiaMobileGodOperations.ShenGuoJiaPlans(owner).Where(p => !used.Contains(p.Kind)).ToArray());
    }

    internal SkillProgramStepOutcome BeginHappyGuoJia(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.ResolveHappyGuoJiaTianyi)
        {
            if (f.SkillId != HappyGjTianyi || f.TriggerId != "all-living-damaged" || f.SelectedTargetSeats is not [var seat] || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                throw new InvalidOperationException("Happy Tianyi lost its real awakening recipient.");
            var grant = $"happy-guo-jia:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}";
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[seat], grant, [HappyGjZuoxing]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuoJiaBenefactorIssuedEvent(f.Id, HappyGjSource(f), seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[seat], HappyGjZuoxing), grant));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillAwakenedEvent(f.OwnerSeat, f.SkillId, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, Array.AsReadOnly(new[] { HappyGjZuoxing })));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.HappyGuoJia is not null || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.InstructionIndex != (op == SkillProgramEffectOp.ResolveHappyGuoJiaHuishi ? 2 : 1))
            throw new InvalidOperationException("Happy Guo Jia requires its original unpaid zero-input activation.");
        var stage = op switch
        {
            SkillProgramEffectOp.ResolveHappyGuoJiaHuishi when f.SkillId == HappyGjHuishi => HappyGuoJiaStage.GrowChildren,
            SkillProgramEffectOp.ResolveHappyGuoJiaHuishiGrant when f.SkillId == HappyGjGift => HappyGuoJiaStage.Target,
            SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing when f.SkillId == HappyGjZuoxing => HappyGuoJiaStage.UseChoice,
            _ => throw new InvalidOperationException("Happy Guo Jia lost its exact operation source.")};
        var r = new HappyGuoJiaReceipt(HappyGjSource(f), f.GameplayHash, op, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage);
        if (op == SkillProgramEffectOp.ResolveHappyGuoJiaHuishi)
        {
            var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, "huishi-five").CardIds;
            r = r with
            {
                Cards = cards,
                SuitCount = cards.Select(id => _coreCardAttackStateOperations.GetAttackCard(id).Suit).Where(s => s != Suit.None).Distinct().Count()
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaStartedEvent(f.Id, r.Source, f.GameplayHash, op, r.Turn, r.TurnOwner, 0, -1));
        if (op == SkillProgramEffectOp.ResolveHappyGuoJiaHuishi)
        {
            f = HappyGjChildren(f, HappyGuoJiaStage.GrowChildren);
            PayHappyGjMaximum(f, f.OwnerSeat, Math.Max(0, Math.Min(r.SuitCount, 10 - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)), "happy-huishi-grow");
            ContinueHappyGuoJia(f.Id);
        }
        else
            PublishHappyGj(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice HappyGjChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? targets = null, string? value = null) => new(new($"happy-guo-jia.{(f.Id)}.{(f.HappyGuoJia!.Stage)}.{(branch)}.{(value)}"), label, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "happy-guo-jia", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value ?? "" });
    internal IReadOnlyList<PromptChoice> HappyGjChoices(ProgramSkillFrame f)
    {
        var r = f.HappyGuoJia!;
        var choices = new List<PromptChoice>();
        if (r.Stage == HappyGuoJiaStage.Recipient)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                choices.Add(HappyGjChoice(f, "recipient", $"将全部展示牌交给{(p.Name)}", [p.Seat], p.Seat.ToString(CultureInfo.InvariantCulture)));
            choices.Add(HappyGjChoice(f, "decline", "不交给角色，将展示牌置入弃牌堆"));
        }

        if (r.Stage == HappyGuoJiaStage.Target)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => HappyGjGiftTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p.Seat)))
                choices.Add(HappyGjChoice(f, "target", $"对{(p.Name)}发动辉逝", [p.Seat], p.Seat.ToString(CultureInfo.InvariantCulture)));
        if (r.Stage == HappyGuoJiaStage.Awakening)
            foreach (var s in _characterGuoJiaMobileGodOperations.ShenGuoJiaUnawakened(r.Target))
                choices.Add(HappyGjChoice(f, "awakening", $"令{(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)}满足【{(_contentRegistryOperations._contentRegistry.GetSkill(s.SkillId).Name)}】觉醒条件", [r.Target], s.SkillId));
        if (r.Stage == HappyGuoJiaStage.UseChoice)
        {
            var plans = HappyGjPlans(f.OwnerSeat);
            foreach (var benefactor in HappyGjBenefactors(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillInstanceId))
                for (var i = 0; i < plans.Count; i++)
                    choices.Add(HappyGjChoice(f, "use", $"令{(_coreMatchParticipantsPlayersAccess._players[benefactor].Name)}减1点体力上限：{(CardCatalog.Get(plans[i].Kind).DisplayName)}（{(string.Join('、', plans[i].Targets.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)))}）", plans[i].Targets, $"{(benefactor)}:{(i)}"));
            choices.Add(HappyGjChoice(f, "cancel", "取消发动"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishHappyGj(ProgramSkillFrame f)
    {
        var choices = HappyGjChoices(f);
        if (choices.Count == 0)
        {
            FinishHappyGj(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, $"【{(_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name)}】请选择。", f.OwnerSeat);
    }

    [ProgramAnswer("happy-guo-jia")]
    internal void AnswerHappyGuoJia(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuoJia: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ValidHappyGj(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], HappyGjChoices(f).Where(x => x.Id == c.Id).ToArray()))
            throw new InvalidOperationException("Happy Guo Jia requires its exact current owner menu.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = c.Parameters["branch"];
        if (r.Stage == HappyGuoJiaStage.Recipient)
        {
            MoveHappyGjRevealed(f, branch == "decline" ? -1 : c.Targets.Single());
            return;
        }

        if (r.Stage == HappyGuoJiaStage.Target)
        {
            var target = c.Targets.Single();
            if (!HappyGjGiftTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], target))
                throw new InvalidOperationException("Happy Hui Shi lost its actual branch eligibility.");
            var awaken = _characterGuoJiaMobileGodOperations.ShenGuoJiaUnawakened(target).Count > 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = r with { Target = target, Stage = awaken ? HappyGuoJiaStage.Awakening : HappyGuoJiaStage.Drawing } });
            if (awaken)
                PublishHappyGj(f);
            else
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 4, new("skill-program.happy:huishi-gift.draw"));
            return;
        }

        if (r.Stage == HappyGuoJiaStage.Awakening)
        {
            var skill = _characterGuoJiaMobileGodOperations.ShenGuoJiaUnawakened(r.Target).Single(s => s.SkillId == c.Parameters["value"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaAwakeningOverrideEvent(f.Id, r.Source, r.Target, skill.SkillId, skill.SkillInstanceId, skill.Program.GameplayHash));
            f = HappyGjChildren(f, HappyGuoJiaStage.CostChildren);
            PayHappyGjMaximum(f, f.OwnerSeat, -2, "happy-huishi-gift");
            ContinueHappyGuoJia(f.Id);
            return;
        }

        if (r.Stage != HappyGuoJiaStage.UseChoice)
            throw new InvalidOperationException("Happy Guo Jia's choice was already paid.");
        if (branch == "cancel")
        {
            FinishHappyGj(f);
            return;
        }

        var parts = c.Parameters["value"].Split(':');
        var benefactor = int.Parse(parts[0], CultureInfo.InvariantCulture);
        var plan = HappyGjPlans(f.OwnerSeat)[int.Parse(parts[1], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = r with { Plan = plan, Benefactor = benefactor } });
        f = HappyGjChildren(f, HappyGuoJiaStage.CostChildren);
        PayHappyGjMaximum(f, benefactor, -1, "happy-zuoxing");
        ContinueHappyGuoJia(f.Id);
    }

    internal ProgramSkillFrame HappyGjChildren(ProgramSkillFrame f, HappyGuoJiaStage stage)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = f.HappyGuoJia!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal int HappyGjMaximumAfterPayment(int seat, int before, int amount) => _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, 2020) ? 7 : Math.Max(0, checked(before + amount));
    internal void PayHappyGjMaximum(ProgramSkillFrame f, int seat, int amount, string purpose)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = p.MaxHp;
        var hp = p.Hp;
        var after = HappyGjMaximumAfterPayment(seat, before, amount);
        p.MaxHp = after;
        p.Hp = Math.Min(hp, after);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuoJia = f.HappyGuoJia!with { MaximumBefore = before, MaximumAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, after - before, after, f.SkillId) { HpBeforeClamp = hp });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaMaximumPaidEvent(f.Id, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Count(e => e.FrameId == f.Id), seat, before, after, purpose));
        if (after == 0 && p.IsAlive)
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, p, null, null, null, causingProgramSkillFrameId: f.Id);
    }

    internal void MoveHappyGjRevealed(ProgramSkillFrame f, int target)
    {
        var ids = f.HappyGuoJia!.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = f.HappyGuoJia with { Target = target } });
        f = HappyGjChildren(f, target < 0 ? HappyGuoJiaStage.CleanupChildren : HappyGuoJiaStage.GainChildren);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, target < 0 ? CardLocation.DiscardPile : CardLocation.Hand(target), new(target < 0 ? "skill-program.happy:huishi.cleanup" : "skill-program.happy:huishi.give"), target < 0 ? null : _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, target, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, _) =>
        {
        }));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuoJia = f.HappyGuoJia!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueHappyGuoJia(f.Id);
    }

    internal void ContinueHappyGuoJia(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuoJia: { } r } f || f.Id != id)
            return;
        if (r.Stage is HappyGuoJiaStage.Drawing or HappyGuoJiaStage.IssuingUse or HappyGuoJiaStage.NativeUse or HappyGuoJiaStage.ReturnedUse)
            return;
        if (_characterGuoJiaMobileGodOperations.DrainShenGuoJia(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.HappyGuoJia!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == HappyGuoJiaStage.GrowChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !r.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c) == CardLocation.Processing))
            {
                MoveHappyGjRevealed(f, -1);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = r with { Stage = HappyGuoJiaStage.Recipient } });
            PublishHappyGj(f);
            return;
        }

        if (r.Stage == HappyGuoJiaStage.GainChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Count == _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Max(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count))
        {
            f = HappyGjChildren(f, HappyGuoJiaStage.CostChildren);
            PayHappyGjMaximum(f, f.OwnerSeat, -1, "happy-huishi-most-hand");
            ContinueHappyGuoJia(id);
            return;
        }

        if (r.Stage == HappyGuoJiaStage.DrawChildren)
        {
            f = HappyGjChildren(f, HappyGuoJiaStage.CostChildren);
            PayHappyGjMaximum(f, f.OwnerSeat, -2, "happy-huishi-gift");
            ContinueHappyGuoJia(id);
            return;
        }

        if (r.Stage == HappyGuoJiaStage.CostChildren && r.Operation == SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            IssueHappyGuoJiaUse(f);
            return;
        }

        if (r.Stage is HappyGuoJiaStage.GainChildren or HappyGuoJiaStage.CleanupChildren or HappyGuoJiaStage.CostChildren)
        {
            FinishHappyGj(f);
            return;
        }

        PublishHappyGj(f);
    }

    internal void FinishHappyGj(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40900)]
    internal bool ResumeHappyGuoJia(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuoJia: not null } f || f.Id != id)
            return false;
        ContinueHappyGuoJia(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40900)]
    internal bool ReturnHappyGjMovement(ProgramSkillFrame f) => f.HappyGuoJia is not null && ResumeHappyGuoJia(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40900)]
    internal bool ContinuePaidHappyGj(ProgramSkillFrame f) => f.HappyGuoJia is not null && ValidHappyGj(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyGuoJiaHuishi, SkillProgramEffectOp.ResolveHappyGuoJiaHuishiGrant, SkillProgramEffectOp.ResolveHappyGuoJiaZuoxing)]
    internal PromptChoice HappyGjAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch")is "recipient" or "target" && c.Targets.Contains(f.OwnerSeat)) ?? p.Choices[0];
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyGuoJiaHuishiGrant)]
    internal void ReturnHappyGjDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.HappyGuoJia!;
        if (!ValidHappyGj(f) || r.Stage != HappyGuoJiaStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != r.Target || outcome.RequestedCount != 4 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Happy Hui Shi lost its exact native four-card return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuoJia = r with { Stage = HappyGuoJiaStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueHappyGuoJia(f.Id);
    }
}
