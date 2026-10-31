#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamagePreventionProgramsRuntime
{
    internal bool HasRangePreventionPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheSourceKeshou) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveSourceExecutionerYishi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheBuwu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryCaoXiuQingxi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheZhanhunDao) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnqiFeiren) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheS5Yimie) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShenSunCeFenghe) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventCurrentDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReduceCurrentDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryFuRongFuQianXuewei) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventShenwuNitaiOwnTurnDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventHulaoArmoredLargeDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventYinjuDamageAndRecover) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PayXiliActualOtherOwnerDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.DamageNatureIs) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SpareFirstLethalTarget) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventOwnPlayOutsideTargetRangeDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventCurrentDamageAndDrawMultiple) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventParticipantDamageDrawActualSource) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventDamageAndConsumeSourceFaction) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuXunXiongmuFirstDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuaXiongYaowu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuBuWushuangBonus) || _characterDongChengMouOperations.HasMouDongCheng;

    internal static bool UsesDamageNatureCondition(SkillProgramTriggerCondition condition) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamagePreventionPrograms.UsesDamageNatureCondition(condition);
    internal bool TryVisitRangePreventionChainTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt))
            throw new InvalidOperationException("Chain range prevention lost its exact live attack owner.");
        if (attack is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
        {
            var state = _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId);
            if (state.RangePreventionVisitedTargets?.Contains(attack.TargetSeat) == true)
                return false;
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, current => current with { RangePreventionVisitedTargets = Array.AsReadOnly((current.RangePreventionVisitedTargets ?? []).Append(attack.TargetSeat).ToArray()) });
        }
        else
        {
            var state = _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId);
            if (state.RangePreventionVisitedTargets?.Contains(attack.TargetSeat) == true)
                return false;
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, current => current!with { RangePreventionVisitedTargets = Array.AsReadOnly((current!.RangePreventionVisitedTargets ?? []).Append(attack.TargetSeat).ToArray()) });
        }

        return true;
    }

    internal bool TryBeginBeforeDamageProgramWindowForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, DamageNature nature)
    {
        if (amount <= 0)
            return false;
        var rangeChainOnly = attack.BeforeDamageProgramsResolved;
        if (rangeChainOnly && (!attack.IsChainPropagation || !(HasRangePreventionPrograms || _coreProgramJudgmentOutcomesOperations.HasRecipientJudgmentOutcomePrevention(attack.TargetSeat) || _coreDamageCardColorHealthOperations.HasRecipientDamageCardColorHealth(attack.TargetSeat) || !attack.IsSourceLess && (_characterCaoSongOlOperations.HasDamageAmountSupport(attack.SourceSeat, attack.TargetSeat) || _characterZhangLiaoHappyGodOperations.HasHappyZlSourceRestoration(attack.SourceSeat, attack.TargetSeat)))))
            return false;
        // The first chain recipient with a binding may still use the full window.
        // Mark that visit too, before its continuation switches to chain-only mode.
        if (attack.IsChainPropagation && !TryVisitRangePreventionChainTarget(attack))
            return false;
        return TryBeginBeforeDamageProgramWindow(attack.SourceSeat, attack.TargetSeat, amount, nature, BeforeDamageProgramContinuation.Attack, rangeChainOnly);
    }

    internal bool TryBeginBeforeDamageProgramWindow(int sourceSeat, int targetSeat, int amount, DamageNature nature, BeforeDamageProgramContinuation continuation, bool rangeChainOnly = false)
    {
        if (amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            return false;
        var mouDcOccurrence = _characterDongChengMouOperations.ObserveMouDcCardDamage();
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(owner => owner.IsAlive).SelectMany(owner =>
        {
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner)with
            {
                EventTargetHp = _coreHealthQueriesOperations.UnambiguousHp(target) ?? 0,
                EventTargetHpUndefined = _coreHealthQueriesOperations.UnambiguousHp(target)is null,
                EventTargetRuleWounded = _coreTreatedWoundedOperations.UsesTreatedWounded ? _coreTreatedWoundedOperations.IsRuleWounded(target) : null,
                DamageNature = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.DamageNatureIs) ? nature : null,
                EventTargetMarkerCounts = _coreAttributedMarkerEventsOperations.HasAttributedEventOperations() ? new Dictionary<PlayerMarkerKind, int>(target.Markers) : null,
                OtherDamageSourceAlive = sourceSeat != owner.Seat && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess != true && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive,
                BlockedDamageSourceSkills = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(instance => _coreProgramPrivateDamageOfferOperationsOperations.IsDamageOfferPairBlocked(owner.Seat, instance.SkillId, sourceSeat)).Select(instance => instance.SkillId).Distinct().ToArray(),
                DamageCardIsSlash = _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { EffectiveCardKind: { } beforeDamageKind, IsChainPropagation: false, IsSourceLess: false } && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(beforeDamageKind),
                DirectCardUseDamage = _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false } direct && (direct is global::CardGame.Core.NativeOwners.CardAttackHandle native ? _coreCardUseLifecycleOperations.LifecycleCardUse(native.ResolutionId)is { Action.Type: CardActionType.Use } : direct.Card is not null),
                DamageSourceGender = _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsDelayedJudgmentDamage != true && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess != true && _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.DamageSourceGenderIs) ? _coreMatchParticipantsPlayersAccess._players[sourceSeat].Gender : null
            };
            return _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.BeforeDamageApplied).Where(candidate => !ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ConsumeZengdaoForActualSourceDamage) && !ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.PreventYinjuDamageAndRecover)).Where(candidate => !rangeChainOnly || ResolveDamageCardColorHealthDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || _coreProgramJudgmentOutcomesOperations.IsJudgmentOutcomePrevention(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op is SkillProgramEffectOp.PreventHulaoArmoredLargeDamage or SkillProgramEffectOp.PreventShenwuNitaiOwnTurnDamage) || UsesDamageNatureCondition(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Condition) || _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op is SkillProgramEffectOp.DiscardHuxinMirrorAndPreventDamage or SkillProgramEffectOp.ResolveShanheSourceKeshou or SkillProgramEffectOp.ResolveSourceExecutionerYishi or SkillProgramEffectOp.ResolveShanheBuwu or SkillProgramEffectOp.ResolveBoundaryCaoXiuQingxi or SkillProgramEffectOp.ResolveShanheZhanhunDao or SkillProgramEffectOp.ResolveAnqiFeiren or SkillProgramEffectOp.ResolveShanheS5Yimie or SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles or SkillProgramEffectOp.PreventYinjuDamageAndRecover or SkillProgramEffectOp.PayXiliActualOtherOwnerDamage or SkillProgramEffectOp.SpareFirstLethalTarget or SkillProgramEffectOp.PreventOwnPlayOutsideTargetRangeDamage or SkillProgramEffectOp.PreventCurrentDamageAndDrawMultiple or SkillProgramEffectOp.PreventParticipantDamageDrawActualSource or SkillProgramEffectOp.PreventDamageAndConsumeSourceFaction or SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage or SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction or SkillProgramEffectOp.ResolveWuLuXunXiongmuFirstDamage or SkillProgramEffectOp.ResolveMouHuaXiongYaowu or SkillProgramEffectOp.ResolveMouLuBuWushuangBonus or SkillProgramEffectOp.ResolveMouDongChengBaojia or SkillProgramEffectOp.PreventCurrentDamage or SkillProgramEffectOp.ReduceCurrentDamage or SkillProgramEffectOp.ResolveHappyZhangLiaoDuoruiDamage or SkillProgramEffectOp.ConsumeHappyDianWeiShenwei)).Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Subject switch
            {
                SkillProgramTriggerSubject.DamageTarget => owner.Seat == targetSeat && (_coreProgramAttackAttemptOperations.CurrentDamageAttempt?.DamageRedirected != true || !ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.RedirectCurrentDamage)),
                SkillProgramTriggerSubject.DamageSource => _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess != true && owner.Seat == sourceSeat && (owner.Seat != targetSeat || ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.IncreaseActualSourceDamageByYuqing)),
                SkillProgramTriggerSubject.Owner => owner.Seat != targetSeat,
                SkillProgramTriggerSubject.Any => true,
                _ => false
            }).Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId)).Select(candidate => new BeforeDamageProgramCandidate(candidate, facts));
        }).Concat(_characterLvQianOlOperations.CollectZengdaoDamageCandidates(sourceSeat, targetSeat, rangeChainOnly)).Concat(_characterFuRongFuQianAnniversaryOperations.CollectAnniversaryFuRongFuQianXueweiCandidates(sourceSeat, targetSeat, rangeChainOnly)).Concat(_characterXinPiOlOperations.CollectIssuedYinjuCandidates2(sourceSeat, targetSeat, rangeChainOnly)).Where(item => !_characterChenGongMouMobileOperations.IsMouChenGongZhichiBeforeCandidate(item.Candidate)).Concat(_characterChenGongMouMobileOperations.CollectIssuedMouChenGongZhichiCandidates(sourceSeat, targetSeat, rangeChainOnly)).OrderByDescending(item => item.Candidate.Priority).ThenBy(item => _coreTableOrderOperations.RelativeTableOrder(targetSeat, item.Candidate.OwnerSeat)).ThenBy(item => item.Candidate.SkillId, StringComparer.Ordinal).ThenBy(item => item.Candidate.SkillInstanceId, StringComparer.Ordinal).ThenBy(item => item.Candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0 && (_coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess == true || !_characterCaoSongOlOperations.HasDamageAmountSupport(sourceSeat, targetSeat)))
            return false;
        var parentFrameId = _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? (continuation == BeforeDamageProgramContinuation.Attack ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId : null) ?? throw new InvalidOperationException("Before-damage programs require an attack continuation.");
        var frame = new BeforeDamageProgramWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentFrameId, sourceSeat, targetSeat, amount, nature, continuation, Array.AsReadOnly(candidates))
        {
            ContinuationAttackResolutionId = rangeChainOnly ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt!.ResolutionId : null,
            MouDongChengOccurrence = mouDcOccurrence
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<BeforeDamageProgramWindowFrame>();
        return true;
    }

    internal ProgramSkillWindowContext CreateBeforeDamageProgramContext(BeforeDamageProgramWindowFrame frame, BeforeDamageProgramCandidate item) => _characterChenGongMouMobileOperations.AttachMouChenGongProtection(item, _characterLvQianOlOperations.CreateLvQianBeforeDamageContext(item, new ProgramSkillWindowContext(SkillProgramTriggerWindow.BeforeDamageApplied, frame.Id, item.Candidate.OwnerSeat, SourceSeat: BeforeDamageHasNoSource(frame) ? null : frame.SourceSeat, TargetSeat: frame.TargetSeat, Amount: frame.Amount, OccurrenceIndex: item.Candidate.OccurrenceIndex, Facts: item.Facts)));
    internal bool BeforeDamageHasNoSource(BeforeDamageProgramWindowFrame frame) => frame.Continuation == BeforeDamageProgramContinuation.Attack && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess == true;
    [NativeFrameRoute(typeof(BeforeDamageProgramWindowFrame), 410)]
    internal void ContinueBeforeDamageProgramWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is BeforeDamageProgramWindowFrame frame)
        {
            if (frame.AnniversaryJiedaoSource is null && frame.CaiYangJiaofeng is null && _coreProgramRuntimeHooksOperations.RuntimeHooks.TryAdvance(ProgramRuntimePhase.BeforeDamageApply))
                return;
            frame = (BeforeDamageProgramWindowFrame)_resolutionStackOperations._resolutionStack.Last();
            if (frame.Prevented || frame.RedirectedTargetSeat is not null || frame.CandidateIndex >= frame.Candidates.Count)
            {
                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.BeforeDamageProgramWindow);
                ResumeAfterBeforeDamageProgramWindow(frame);
                return;
            }

            var item = frame.Candidates[frame.CandidateIndex];
            var context = CreateBeforeDamageProgramContext(frame, item);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(item.Candidate, context))
            {
                AdvanceBeforeDamageProgramCandidate(frame, activated: false, completed: false);
                continue;
            }

            var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(item.Candidate);
            if (trigger.Optional)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(item.Candidate, context);
                return;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(item.Candidate, context);
            return;
        }
    }

    internal void AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => _programWindowCursorOperations.Get().AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
    internal void PreventProgramCurrentDamage(ProgramSkillFrame program)
    {
        if (program.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, ParentFrameId: var parentFrameId } || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame frame || frame.Id != parentFrameId || frame.Prevented)
            throw new InvalidOperationException("Damage prevention lost its before-damage parent window.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[^2].Id, frame with { Prevented = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamagePreventedEvent(frame.Id, program.SkillId, program.TriggerId!, program.OwnerSeat, BeforeDamageHasNoSource(frame) ? null : frame.SourceSeat, frame.TargetSeat, frame.Amount));
        _coreMatchLogOperations.AddLog("DamagePrevented", $"{(_coreMatchParticipantsPlayersAccess._players[program.OwnerSeat].Name)} 防止了 {(_coreMatchParticipantsPlayersAccess._players[frame.TargetSeat].Name)} 即将受到的 {(frame.Amount)} 点伤害。", program.OwnerSeat, frame.TargetSeat);
    }

    internal void ResumeAfterBeforeDamageProgramWindow(BeforeDamageProgramWindowFrame frame)
    {
        switch (frame.Continuation)
        {
            case BeforeDamageProgramContinuation.Attack:
            {
                var attack = _coreProgramAttackAttemptOperations.CurrentDamageAttempt ?? throw new InvalidOperationException("The before-damage attack continuation is unavailable.");
                if (frame.ContinuationAttackResolutionId is { } exactAttackId && attack.ResolutionId != exactAttackId || attack.SourceSeat != frame.SourceSeat || attack.TargetSeat != (frame.RedirectedTargetSeat ?? frame.TargetSeat))
                    throw new InvalidOperationException("The before-damage attack participants changed.");
                if (_characterMangYaChangAnniversaryOperations.ResumeAnniversaryJiedaoSourceWindow(frame, attack))
                    return;
                if (_characterCaiYangAnniversaryOperations.ResumeAnniversaryCaiYangJiaofengSourceWindow(frame, attack))
                    return;
                if (frame.RedirectedTargetSeat is null)
                    attack.MarkBeforeDamageProgramsResolved();
                if (frame.Prevented)
                {
                    _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
                    return;
                }

                if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                    _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
                return;
            }

            default:
                throw new InvalidOperationException("Unsupported before-damage program continuation.");
        }
    }
}
