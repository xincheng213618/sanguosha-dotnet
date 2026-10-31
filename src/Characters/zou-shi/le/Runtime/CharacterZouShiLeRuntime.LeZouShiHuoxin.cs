#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZouShiLeRuntime
{
    internal const string HuoxinObtainReason = "skill-program.le-zou-shi.huoxin.obtain";
    internal bool TryGetHuoxinUse(int owner, ProgramSkillWindowContext context, out ProgramCardTriggerWindowFrame window)
    {
        window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>()
            .SingleOrDefault(frame => frame.Id == context.ParentFrameId)!;
        return window is not null && window.Action.ActorSeat == owner &&
            CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(window.Action.EffectiveKind) != SkillProgramCardCategory.Equipment &&
            (window.Action.Type != CardActionType.Response || window.CompletedResponseReturn is { IsCommitted: true }) &&
            (window.Action.Type != CardActionType.Use || window.CompletedResponseReturn is null);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28951)]
    internal bool CanOfferHuoxin(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse) ||
        TryGetHuoxinUse(candidate.OwnerSeat, context, out _) && _coreMatchParticipantsPlayersAccess._players.Any(player =>
            player.IsAlive && player.Seat != candidate.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0);

    internal SkillProgramStepOutcome BeginHuoxin(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var window = (ProgramCardTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(value => value.Id == frame.WindowContext!.ParentFrameId);
        var receipt = new HuoxinReceipt(window.Action.EffectiveSuit ?? frame.WindowContext!.CardUse!.PublicSuit ?? Suit.None)
        { Stage = HuoxinStage.TargetChoice };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Huoxin = receipt });
        PublishHuoxin(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HuoxinChoices(ProgramSkillFrame f)
    {
        var r = f.Huoxin!;
        var result = new List<PromptChoice>();
        PromptChoice C(string branch, string label, int? target = null, int? slot = null, int? card = null) => new(new($"huoxin.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target ?? -1)}.{(slot ?? -1)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "le-zou-shi", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["slot"] = (slot ?? -1).ToString(CultureInfo.InvariantCulture) });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        if (r.Stage == HuoxinStage.TargetChoice)
            result.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => C("target", $"展示 {(p.Name)} 的一张手牌", p.Seat)));
        else if (r.Stage == HuoxinStage.CardChoice && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            result.AddRange(r.Candidates.Select((id, index) => (id, index)).Where(x => _cardZonesOperations._cardZones.GetLocation(x.id) == CardLocation.Hand(r.TargetSeat)).Select(x => C("card", $"展示第 {(x.index + 1)} 张手牌", slot: x.index)));
        else if (r.Stage == HuoxinStage.ObtainChoice && r.MayObtain && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && r.CardId is { } shown && _cardZonesOperations._cardZones.GetLocation(shown) == CardLocation.Hand(r.TargetSeat))
        {
            result.Add(C("obtain", $"获得展示的【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(shown).DisplayName)}】", card: shown));
            result.Add(C("decline", "不获得此牌"));
        }

        if (!r.Paid && r.Stage is HuoxinStage.TargetChoice or HuoxinStage.CardChoice)
            result.Add(C("cancel", "取消惑心"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishHuoxin(ProgramSkillFrame f)
    {
        var choices = HuoxinChoices(f);
        if (choices.Count == 0)
        {
            FinishHuoxin(f);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, f.Huoxin!.Stage == HuoxinStage.ObtainChoice ? "惑心：是否获得展示的牌？" : "惑心：选择一名其他角色，再展示其一张手牌。", f.Huoxin.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("le-zou-shi")]
    internal void AnswerHuoxin(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huoxin: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HuoxinChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Huoxin cannot answer an unpublished target, hidden slot or stale material choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (choice.Parameters["branch"])
        {
            case "target":
                var target = choice.Targets.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huoxin = r with { Stage = HuoxinStage.CardChoice, TargetSeat = target, Candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray() } });
                PublishHuoxin(f);
                return;
            case "card":
                var slot = int.Parse(choice.Parameters["slot"], CultureInfo.InvariantCulture);
                var id = r.Candidates[slot];
                var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Single(c => c.Id == id);
                var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card);
                var marked = HasYunzhengEntityMark(id);
                var may = marked || r.EffectiveSuit != Suit.None && r.EffectiveSuit == suit;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huoxin = r with { Stage = HuoxinStage.ObtainChoice, Paid = true, CardId = id, MayObtain = may } });
                _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "huoxin-shown", [id], SkillProgramCardSetVisibility.Private, [CardLocation.Hand(r.TargetSeat)]);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(f, "huoxin-shown");
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _cardEntityMarkOperations.Mark(f.Id, "zheng", [id]);
                if (may)
                    PublishHuoxin(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                else
                    FinishHuoxin(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case "obtain":
                var selected = r.CardId!.Value;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with
                { Huoxin = r with { Stage = HuoxinStage.ObtainChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([selected], CardLocation.Hand(f.OwnerSeat), new(HuoxinObtainReason));
                ContinueHuoxin(f.Id);
                return;
            case "decline":
            case "cancel":
                FinishHuoxin(f);
                return;
            default:
                throw new InvalidOperationException("Huoxin cannot invent a branch.");
        }
    }

    internal void ContinueHuoxin(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huoxin: { } r } f || f.Id != id)
            return;
        if (r.Stage != HuoxinStage.ObtainChildren)
        {
            PublishHuoxin(f);
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        FinishHuoxin(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishHuoxin(ProgramSkillFrame f)
    {
        var r = f.Huoxin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huoxin = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28951)]
    internal bool ResumeHuoxin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huoxin: not null } f || f.Id != id)
            return false;
        ContinueHuoxin(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28951)]
    internal bool ReturnHuoxinMovement(ProgramSkillFrame f)
    {
        if (f.Huoxin is null || f.PendingMovementContinuation is null)
            return false;
        ContinueHuoxin(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28951)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28951)]
    internal bool PermitPaidHuoxin(ProgramSkillFrame f) => f.Huoxin is { Paid: true };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse)]
    internal PromptChoice HuoxinAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.Huoxin?.Stage == HuoxinStage.TargetChoice)
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
            return p.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "target").OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets.Single(), new(0, 0, 0, 0, 0, 1, false, false))).ThenBy(c => c.Targets.Single()).FirstOrDefault() ?? p.Choices.Last();
        }

        return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "obtain") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") != "cancel") ?? p.Choices.First();
    }
}
