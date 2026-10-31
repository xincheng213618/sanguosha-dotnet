#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDelegatedJudgmentChoicesRuntime
{
    internal bool IsDelegatedJudgment(SkillProgramTrigger trigger) => trigger.Effects.FirstOrDefault()?.Op == SkillProgramEffectOp.DelegateJudgmentReplacement;
    internal PromptChoice DelegatedJudgmentChoice(JudgmentFrame j, string action, string label, int? card = null) => new(new($"delegated-judgment.{(j.Id)}.{(action)}.{(card)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["action"] = action, ["judgment-id"] = j.Id.ToString(CultureInfo.InvariantCulture), ["card-id"] = card?.ToString(CultureInfo.InvariantCulture) ?? string.Empty });
    internal IReadOnlyList<PromptChoice> DelegatedJudgmentChoices(JudgmentFrame j)
    {
        var d = j.DelegatedReplacement ?? throw new InvalidOperationException("Delegated judgment has no owning draft.");
        return d.Stage == DelegatedJudgmentStage.Offered ? Array.AsReadOnly(new[] { DelegatedJudgmentChoice(j, "delegated-judgment-accept", "发动缓释，让被判定者观看手牌并选择一张牌。"), DelegatedJudgmentChoice(j, "program-judgment-replace-skip", "保留当前判定牌，不发动缓释。") }) : d.Stage == DelegatedJudgmentStage.Choosing ? Array.AsReadOnly(d.Material.Select(c => DelegatedJudgmentChoice(j, "delegated-judgment-select", $"选择 {(_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].Name)} 的【{(c.DisplayName)}】作为自己的判定牌。", c.Id)).ToArray()) : Array.Empty<PromptChoice>();
    }

    internal bool ValidDelegatedJudgmentDraft(JudgmentFrame j, DelegatedJudgmentDraft d)
    {
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(j);
        var identity = d.JudgmentFrameId == j.Id && d.SubjectSeat == j.TargetSeat && candidate is not null && d.Source.OwnerSeat == candidate.OwnerSeat && d.Source.SkillId == candidate.ProgramId && d.Source.SkillInstanceId == candidate.SkillInstanceId && d.GameplayHash == candidate.GameplayHash && d.TriggerId == candidate.ProgramTriggerId && d.Source.BindingId == d.TriggerId && d.Material.Count > 0 && d.Material.Select(c => c.Id).Distinct().Count() == d.Material.Count && (d.Stage == DelegatedJudgmentStage.Paid || _coreNativeDamageHandlesOperations.GetJudgmentCard(j)?.Id == d.OldCardId);
        if (!identity || !Enum.IsDefined(d.Stage))
            return false;
        if (d.Stage is DelegatedJudgmentStage.Chosen or DelegatedJudgmentStage.Paid)
            return d.SelectedCardId is { } selected && d.Material.Any(c => c.Id == selected);
        var owner = _coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat];
        var(_, trigger) = _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate!);
        return d.SelectedCardId is null && owner.IsAlive && _coreMatchParticipantsPlayersAccess._players[d.SubjectSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, d.Source.SkillId, d.Source.SkillInstanceId) && d.Material.SequenceEqual(_coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacementCards(owner, trigger, j.TargetSeat, j.Reason).Select(_coreSnapshotValueProjectionOperations.ToSnapshot)) && (d.Stage == DelegatedJudgmentStage.Offered ? d.HandView.Count == 0 : d.HandView.SequenceEqual(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Select(_coreSnapshotValueProjectionOperations.ToSnapshot)));
    }

    internal void BeginDelegatedJudgmentChoice(JudgmentFrame j, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger, IReadOnlyList<Card> cards)
    {
        var d = new DelegatedJudgmentDraft(j.Id, new(program.Id, trigger.Id, candidate.OwnerSeat, candidate.SkillInstanceId!), program.GameplayHash, trigger.Id, j.TargetSeat, _coreNativeDamageHandlesOperations.GetJudgmentCard(j)!.Id, DelegatedJudgmentStage.Offered, cards.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray(), []);
        _coreNativeJudgmentOperations.SetJudgmentFrameStep(j.Id, ResolutionFrameStep.AwaitingResponse);
        _coreNativeJudgmentOperations.ReplaceJudgmentFrame(j = _coreNativeJudgmentOperations.GetJudgmentFrame(j.Id)with { DelegatedReplacement = d });
        var current = _coreNativeDamageHandlesOperations.GetJudgmentCard(j)!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementRequestedEvent(j.Id, j.Id, j.TargetSeat, candidate.OwnerSeat, j.Reason, current.Id, current.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[j.TargetSeat], current)));
        PublishDelegatedJudgmentChoice(j);
    }

    internal void PublishDelegatedJudgmentChoice(JudgmentFrame j)
    {
        var d = j.DelegatedReplacement!;
        if (!ValidDelegatedJudgmentDraft(j, d))
            throw new InvalidOperationException("Delegated judgment lost its exact source and parent.");
        var chooser = d.Stage == DelegatedJudgmentStage.Offered ? d.Source.OwnerSeat : d.SubjectSeat;
        var choices = DelegatedJudgmentChoices(j);
        var presentation = _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramJudgmentReplacement, chooser, d.Stage == DelegatedJudgmentStage.Offered ? "缓释：是否让被判定者观看并选择你的牌？" : "缓释：观看诸葛瑾的手牌，从其可选牌中选择一张替换自己的判定牌。", d.Stage == DelegatedJudgmentStage.Choosing ? d.Material.Select(c => c.Id).ToArray() : [], [], d.Source.OwnerSeat, IncomingCard: _coreNativeDamageHandlesOperations.GetJudgmentCard(j)!.Kind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = d.SubjectSeat,
            Choices = choices,
            SkillPrompt = new(d.Source.SkillId, presentation.Name, "缓释 · 判定替换", presentation.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool TryResolveDelegatedJudgmentChoice(PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("action")is not ("delegated-judgment-accept" or "delegated-judgment-select"))
            return false;
        var j = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("Delegated judgment lost its actual judgment.");
        var d = j.DelegatedReplacement ?? throw new InvalidOperationException("Delegated judgment has no owning draft.");
        if (!ValidDelegatedJudgmentDraft(j, d) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != (d.Stage == DelegatedJudgmentStage.Offered ? d.Source.OwnerSeat : d.SubjectSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], DelegatedJudgmentChoices(j).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The delegated choice no longer matches its private chooser or material.");
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(j)!;
        var(_, trigger) = _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate);
        var owner = _coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat];
        if (!owner.IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.SubjectSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, d.Source.SkillId, d.Source.SkillInstanceId!))
            throw new InvalidOperationException("An unpaid delegated replacement lost its living exact source.");
        var current = _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacementCards(owner, trigger, j.TargetSeat, j.Reason).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray();
        if (!current.SequenceEqual(d.Material))
            throw new InvalidOperationException("Delegated replacement material changed before payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (d.Stage == DelegatedJudgmentStage.Offered)
        {
            _coreNativeJudgmentOperations.ReplaceJudgmentFrame(j = j with { DelegatedReplacement = d with { Stage = DelegatedJudgmentStage.Choosing, HandView = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DelegatedJudgmentViewedEvent(j.Id, d.Source, d.SubjectSeat, _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count));
            PublishDelegatedJudgmentChoice(j);
            return true;
        }

        var id = selected.Cards.Single();
        _coreNativeJudgmentOperations.ReplaceJudgmentFrame(j with { DelegatedReplacement = d with { Stage = DelegatedJudgmentStage.Chosen, SelectedCardId = id } });
        _coreJudgmentReplacementProgramsOperations.ResolveProgramJudgmentReplacementChoice(new(new($"delegated-judgment.{(j.Id)}.paid-{(id)}"), selected.Label, [id], [], new Dictionary<string, string> { ["action"] = "program-judgment-replace", ["card-id"] = id.ToString(CultureInfo.InvariantCulture) }));
        return true;
    }

    internal bool IsDelegatedJudgmentPromptValid(JudgmentFrame j, PendingDecision? p)
    {
        if (j.DelegatedReplacement is not { Stage: DelegatedJudgmentStage.Offered or DelegatedJudgmentStage.Choosing } d || !ValidDelegatedJudgmentDraft(j, d) || p is not { IsPrivate: true, Kind: DecisionKind.ProgramJudgmentReplacement } || p.PlayerSeat != (d.Stage == DelegatedJudgmentStage.Offered ? d.Source.OwnerSeat : d.SubjectSeat) || p.SourceSeat != d.Source.OwnerSeat || p.TargetSeat != d.SubjectSeat)
            return false;
        return p.ValidCardIds.SequenceEqual(d.Stage == DelegatedJudgmentStage.Choosing ? d.Material.Select(c => c.Id) : []) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, DelegatedJudgmentChoices(j));
    }

    internal IEnumerable<CardSnapshot> DelegatedJudgmentPrivateCards(int viewer) => _coreJudgmentCursorRuntimeOperations.ActiveJudgment is { DelegatedReplacement: { Stage: DelegatedJudgmentStage.Choosing } d } j && d.SubjectSeat == viewer && IsDelegatedJudgmentPromptValid(j, _corePendingDecisionPolicyPipelineOperations._pendingDecision) ? d.HandView : Array.Empty<CardSnapshot>();
    internal bool ResolveAiDelegatedJudgmentChoice()
    {
        if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is not { DelegatedReplacement: { Stage: DelegatedJudgmentStage.Offered or DelegatedJudgmentStage.Choosing } d } j)
            return false;
        var p = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI delegate has no private prompt.");
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
        var target = view.Players.Single(c => c.Seat == d.SubjectSeat);
        PromptChoice choice;
        if (d.Stage == DelegatedJudgmentStage.Offered)
        {
            var self = view.Players.Single(c => c.Seat == p.PlayerSeat);
            var helps = self.Seat == target.Seat || self.TeamId is not null && self.TeamId == target.TeamId || (self.Role is Role.Lord or Role.Loyalist) && (target.Role is Role.Lord or Role.Loyalist) || self.Role == Role.Rebel && target.Role == Role.Rebel;
            choice = p.Choices.Single(c => c.Parameters["action"] == (helps ? "delegated-judgment-accept" : "program-judgment-replace-skip"));
        }
        else
        {
            var cards = view.PrivateRevealedCards ?? [];
            var owner = view.Players.Single(c => c.Seat == d.Source.OwnerSeat);
            var available = cards.Concat(owner.Equipment).DistinctBy(c => c.Id).Where(c => p.ValidCardIds.Contains(c.Id));
            var reason = j.Reason;
            bool favorable(CardSnapshot c) => reason switch
            {
                JudgmentReasons.Lightning => !(c.Suit == Suit.Spade && c.Rank is >= 2 and <= 9),
                JudgmentReasons.Indulgence => c.Suit == Suit.Heart,
                JudgmentReasons.SupplyShortage => c.Suit == Suit.Club,
                _ => c.Suit is Suit.Heart or Suit.Diamond
            };
            var id = available.OrderByDescending(favorable).ThenBy(c => CardCatalog.Get(c.Kind).HandKeepValue).ThenBy(c => c.Id).First().Id;
            choice = p.Choices.Single(c => c.Cards.SequenceEqual([id]));
        }

        _coreJudgmentReplacementProgramsOperations.ResolveProgramJudgmentReplacementChoice(choice);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
