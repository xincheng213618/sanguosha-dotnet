#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiHappyGodRuntime
{
    internal int HappyGuardCount(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.HappyGuard);
    internal bool HappyGuardOnField() => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && HappyGuardCount(p.Seat) > 0);
    internal CardConversionSource HappyGuardSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? HappyGuardAttack(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id),
        CardUseFrame { CardAttack: not null } or ProgramSkillFrame { CardAttack: not null } or JudgmentFrame { CardAttack: not null } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        _ => null
    };
    internal bool HappyGuardDamageOffer(int owner, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target, Amount: > 0 } || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || HappyGuardCount(target) != 1 || _coreMatchParticipantsPlayersAccess._players[target].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.HappyGuard, owner)) != 1 || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Prevented: false, RedirectedTargetSeat: null } window || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.SourceSeat != window.SourceSeat || attack.TargetSeat != target || attack.DamageAmount != window.Amount || attack.DamageRedirected && target != owner)
            return false;
        return context.Amount == window.Amount && context.TargetSeat == window.TargetSeat;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41980)]
    internal bool CanOfferHappyDianWei(ProgramTriggerLegalityContext g) => g.SkillId switch
    {
        HappyDianWeiComposition.Shenwei when g.Trigger.Id == "grant-guards" => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && HappyGuardCount(p.Seat) == 0),
        HappyDianWeiComposition.Shenwei when g.Trigger.Id == "guard-damage" => HappyGuardDamageOffer(g.OwnerSeat, g.WindowContext),
        HappyDianWeiComposition.Elai => g.WindowContext is { } context && IsHappyDianWeiElaiCandidate(g.Candidate, context),
        _ => true
    };
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 41980)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyDianWeiKuangxi(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || attack.IsChainPropagation || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !HappyGuardOnField())
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]).ActiveGrants.Where(s => s.SkillId == HappyDianWeiComposition.Kuangxi).Take(1).Select(s => (new CardUseEffectSource(s.SkillId, "field-guard-damage", attack.SourceSeat, s.SkillInstanceId), 1));
    }

    internal void SetHappyGuard(ProgramSkillFrame f, int target, int count)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[target];
        var key = (PlayerMarkerKind.HappyGuard, f.OwnerSeat);
        var before = p.MarkerSourceCounts.GetValueOrDefault(key);
        if (before == count)
            return;
        if (count is < 0 or > 1 || count == 1 && HappyGuardCount(target) != 0)
            throw new InvalidOperationException("Every character can carry at most one real guard.");
        if (count == 0)
            p.MarkerSourceCounts.Remove(key);
        else
            p.MarkerSourceCounts[key] = count;
        var total = HappyGuardCount(target) + count - before;
        if (total == 0)
            p.Markers.Remove(PlayerMarkerKind.HappyGuard);
        else
            p.Markers[PlayerMarkerKind.HappyGuard] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, target, PlayerMarkerKind.HappyGuard, count - before, total, f.OwnerSeat, "skill-program.happy:shenwei.guard"));
    }

    SkillProgramStepOutcome global::CardGame.Core.IHappyDianWeiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => ResolveHappyDianWei(f, op);
    internal SkillProgramStepOutcome ResolveHappyDianWei(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyGuard is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || !HappyGuardOriginalCandidate(f))
            throw new InvalidOperationException("Happy Dian Wei requires its unique original native trigger candidate.");
        HappyGuardStage stage;
        var target = -1;
        long attackId = 0;
        var damageSource = -1;
        var amount = 0;
        var nature = DamageNature.Normal;
        var limit = 0;
        if (op == SkillProgramEffectOp.GrantHappyDianWeiShenwei && context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
        {
            stage = HappyGuardStage.GrantChoice;
            limit = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp == 1 ? 2 : 1;
        }
        else if (op == SkillProgramEffectOp.ConsumeHappyDianWeiShenwei && HappyGuardDamageOffer(f.OwnerSeat, context) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack)
        {
            stage = HappyGuardStage.GuardChoice;
            target = context.TargetSeat!.Value;
            attackId = attack.ResolutionId;
            damageSource = attack.SourceSeat;
            amount = attack.DamageAmount;
            nature = _coreNativeCardClassificationsOperations.GetDamageNature(attack);
        }
        else if (op == SkillProgramEffectOp.ResolveHappyDianWeiElai && IsHappyDianWeiElaiContext(f))
            stage = HappyGuardStage.ElaiChoice;
        else
            throw new InvalidOperationException("Happy Dian Wei operation lost its exact real turn, damage or removed-guard boundary.");
        var r = new HappyGuardReceipt(HappyGuardSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage, target, attackId, damageSource, amount, nature, limit);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuard = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuardStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, stage, target, attackId, amount, limit));
        PublishHappyGuardChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HappyGuardChoices(ProgramSkillFrame f)
    {
        var r = f.HappyGuard!;
        var choices = new List<PromptChoice>();
        PromptChoice Make(string mode, string text, IReadOnlyList<int>? targets = null) => new(new ChoiceId($"happy-guard.{(f.Id)}.{(mode)}.{(string.Join('-', targets ?? []))}"), text, [], targets ?? [], new Dictionary<string, string> { { "program-action", "happy-dian-guard" }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) }, { "mode", mode } });
        if (r.Stage == HappyGuardStage.GrantChoice)
        {
            var seats = _coreTableOrderOperations.CurrentTableOrder().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && HappyGuardCount(s) == 0).ToArray();
            if (r.GrantLimit == 2)
                choices.Add(Make("grant", "不授予卫。"));
            for (var i = 0; i < seats.Length; i++)
            {
                choices.Add(Make("grant", $"令{(_coreMatchParticipantsPlayersAccess._players[seats[i]].Name)}获得卫。", [seats[i]]));
                if (r.GrantLimit == 2)
                    for (var j = i + 1; j < seats.Length; j++)
                        choices.Add(Make("grant", $"令{(_coreMatchParticipantsPlayersAccess._players[seats[i]].Name)}、{(_coreMatchParticipantsPlayersAccess._players[seats[j]].Name)}各获得卫。", [seats[i], seats[j]]));
            }
        }
        else if (r.Stage == HappyGuardStage.GuardChoice)
        {
            choices.Add(Make("decline", "保留卫，自行承受本次伤害。"));
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.GuardedSeat].IsAlive && HappyGuardCount(r.GuardedSeat) == 1)
                choices.Add(Make("protect", $"移除卫，由{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}承受本次伤害。"));
        }
        else if (r.Stage == HappyGuardStage.ElaiChoice)
        {
            choices.Add(Make("none", "不选择额外效果。"));
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                if (_coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                    choices.Add(Make("recover", "回复1点体力。"));
                foreach (var seat in _coreTableOrderOperations.CurrentTableOrder().Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, s)))
                    choices.Add(Make("damage", $"对{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}造成1点伤害。", [seat]));
            }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishHappyGuardChoice(ProgramSkillFrame f)
    {
        var choices = HappyGuardChoices(f);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuard = f.HappyGuard!with { Stage = HappyGuardStage.Declined } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var chooser = f.HappyGuard!.Stage == HappyGuardStage.GuardChoice ? f.HappyGuard.GuardedSeat : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("happy-dian-guard")]
    internal void AnswerHappyGuard(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuard: { } r } f || !ValidHappyGuardFrame(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == HappyGuardStage.GuardChoice ? r.GuardedSeat : f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HappyGuardChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Guard choice lost its exact current chooser, native occurrence or unpaid source.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var mode = choice.Parameters["mode"];
        if (r.Stage == HappyGuardStage.GrantChoice)
        {
            foreach (var target in choice.Targets)
                SetHappyGuard(f, target, 1);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuard = r with { Stage = HappyGuardStage.GrantCommitted } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == HappyGuardStage.GuardChoice)
        {
            if (mode == "decline")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuard = r with { Stage = HappyGuardStage.Declined } });
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuard = r with { Stage = HappyGuardStage.GuardPaid } });
            SetHappyGuard(f, r.GuardedSeat, 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuardPaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.AttackId, r.GuardedSeat, r.DamageSource, r.Amount, r.Nature));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuard = f.HappyGuard!with { Stage = HappyGuardStage.RemovedChildren } });
            var ret = new HappyGuardRemovalReturn(f.Id, r.GuardedSeat, f.OwnerSeat, f.Id, false);
            if (!BeginHappyGuardRemovedWindow(ret))
                FinishHappyGuardTransfer(f.Id);
            return;
        }

        if (r.Stage != HappyGuardStage.ElaiChoice)
            throw new InvalidOperationException("Unknown original guard menu stage.");
        var targetSeat = choice.Targets.Count == 1 ? choice.Targets[0] : -1;
        var removal = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(w => w.Id == r.ParentId).HappyGuardRemoval!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuard = r with { Stage = HappyGuardStage.ElaiCommitted, ElaiMode = mode, ElaiTarget = targetSeat }, SelectedTargetSeats = Array.AsReadOnly(choice.Targets.ToArray()), ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("elai-mode", mode, f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuardElaiChoiceEvent(f.Id, removal.RemovalId, f.OwnerSeat, mode, targetSeat));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.GrantHappyDianWeiShenwei, SkillProgramEffectOp.ConsumeHappyDianWeiShenwei, SkillProgramEffectOp.ResolveHappyDianWeiElai)]
    internal PromptChoice HappyGuardAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.HappyGuard?.Stage == HappyGuardStage.GuardChoice)
            return p.Choices.FirstOrDefault(c => c.Parameters["mode"] == "protect" && (f.OwnerSeat == f.HappyGuard.GuardedSeat || _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].TeamId is { } team && team == _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].TeamId)) ?? p.Choices[0];
        return p.Choices.FirstOrDefault(c => c.Parameters["mode"] == "recover") ?? p.Choices.FirstOrDefault(c => c.Parameters["mode"] == "damage" && c.Targets is [var target] && _coreTeamActivityProgramsOperations.PublicTeamEnemy(f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[target])) ?? p.Choices.FirstOrDefault(c => c.Parameters["mode"] == "grant" && c.Targets.Contains(f.OwnerSeat)) ?? p.Choices[0];
    }
}
