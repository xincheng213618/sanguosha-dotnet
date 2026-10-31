#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengDaOlRuntime
{
    internal const string GoudeDrawReason = "skill-program.meng-da.goude.draw", GoudeDiscardReason = global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.GoudeDiscardReason;
    SkillProgramStepOutcome global::CardGame.Core.IMengDaHost.Resolve(ProgramSkillFrame f) => BeginGoude(f);
    internal SkillProgramStepOutcome BeginGoude(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority);
        if (f.Goude is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !QualifyGoude(c, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Goude requires a true native completed-turn opportunity with an unexecuted legal option.");
        var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
        var seats = GoudeFactionSeats(f.OwnerSeat);
        var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])!;
        var r = new GoudeReceipt(MengDaSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, context.ParentFrameId, cutoff, faction, GoudeExecutedMask(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, seats, cutoff), GoudeStage.Option)
        {
            FactionSeats = seats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.WindowId, cutoff, faction, r.ExecutedMask));
        for (var i = 0; i < seats.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeFactionSeatFrozenEvent(f.Id, i, seats[i]));
        PublishGoude(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> GoudeChoices(ProgramSkillFrame f)
    {
        var r = f.Goude!;
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "meng-da-goude",
            ["branch"] = branch
        };
        PromptChoice Seat(int seat) => new(new($"goude.{(f.Id)}.{(r.Stage)}.{(seat)}"), _coreMatchParticipantsPlayersAccess._players[seat].Name, [], [seat], Args("target"));
        if (r.Stage == GoudeStage.Option)
            return Array.AsReadOnly(GoudeAvailableOptions(f.OwnerSeat, r.ExecutedMask).Select(o => new PromptChoice(new($"goude.{(f.Id)}.option.{(o)}"), o switch
            {
                GoudeOption.Draw => "摸一张牌",
                GoudeOption.Discard => "弃置一名角色一张手牌",
                GoudeOption.Slash => "视为使用一张【杀】",
                _ => "变更势力"
            }, [], [], new Dictionary<string, string>(Args("option")) { ["option"] = o.ToString() })).ToArray());
        if (r.Stage == GoudeStage.DiscardTarget)
            return Array.AsReadOnly(GoudeDiscardTargets(f.OwnerSeat).Select(Seat).ToArray());
        if (r.Stage == GoudeStage.SlashTarget)
            return Array.AsReadOnly(GoudeSlashTargets(f.OwnerSeat).Select(Seat).ToArray());
        if (r.Stage == GoudeStage.Faction)
            return Array.AsReadOnly(GoudeFactionChoices(f.OwnerSeat).Select(id => new PromptChoice(new($"goude.{(f.Id)}.faction.{(id)}"), global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(id), [], [], new Dictionary<string, string>(Args("faction")) { ["faction"] = id })).ToArray());
        if (r.Stage == GoudeStage.DiscardCard)
            return Array.AsReadOnly(r.Materials.Select(m => new PromptChoice(new($"goude.{(f.Id)}.hand.{(m.Slot)}"), m.HolderSeat == f.OwnerSeat ? $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】" : $"弃置第{(m.Slot + 1)}张手牌", m.HolderSeat == f.OwnerSeat ? [m.CardId] : [], [], new Dictionary<string, string>(Args("card")) { ["slot-index"] = m.Slot.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray());
        return[];
    }

    internal void PublishGoude(ProgramSkillFrame f)
    {
        var choices = GoudeChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishGoude(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "苟得：执行本回合当前同势力角色尚未执行的一项。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = GoudePrivateChoice(f),
            TargetSeat = f.Goude!.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal static bool GoudePrivateChoice(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_meng_da_ol_MengDa.GoudePrivateChoice(f);
    [ProgramAnswer("meng-da-goude")]
    internal void AnswerGoude(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Goude: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !GoudeChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Goude requires its exact published option, target, faction or opaque Hand slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishGoude(f);
            return;
        }

        if (r.Stage == GoudeStage.Option)
        {
            var option = Enum.Parse<GoudeOption>(choice.Parameters["option"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeOptionChosenEvent(f.Id, option));
            var stage = option switch
            {
                GoudeOption.Draw => GoudeStage.MovementChildren,
                GoudeOption.Discard => GoudeStage.DiscardTarget,
                GoudeOption.Slash => GoudeStage.SlashTarget,
                _ => GoudeStage.Faction
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r with { Option = option, Stage = stage } });
            if (option != GoudeOption.Draw)
            {
                PublishGoude(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(GoudeDrawReason)).Count;
            RecordGoudePayment(f.Id, before, actual, null, null);
            ContinueMengDa(f.Id);
            return;
        }

        if (r.Stage == GoudeStage.Faction)
        {
            var faction = choice.Parameters["faction"];
            var previous = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r with { NewFaction = faction, Stage = GoudeStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeFactionChosenEvent(f.Id, previous, faction));
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreNativeFactionChangesOperations.ChangeNativeEffectiveFaction(f, faction);
            RecordGoudePayment(f.Id, before, 0, null, null);
            ContinueMengDa(f.Id);
            return;
        }

        if (r.Stage is GoudeStage.DiscardTarget or GoudeStage.SlashTarget)
        {
            var target = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeTargetChosenEvent(f.Id, r.Option!.Value, target));
            if (r.Stage == GoudeStage.SlashTarget)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r with { TargetSeat = target, Stage = GoudeStage.NativeSlash } });
                if (_coreTriggeredCardProgramsOperations.UseTriggeredProgramVirtualSlash(f, target) != SkillProgramStepOutcome.AwaitChild)
                    FinishGoude(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }

            var cards = GoudeDiscardMaterials(f.OwnerSeat, target);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r with { TargetSeat = target, Stage = GoudeStage.DiscardCard, Materials = cards } });
            for (var i = 0; i < cards.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeHandMaterialFrozenEvent(f.Id, i, cards[i]));
            PublishGoude(f);
            return;
        }

        if (r.Stage != GoudeStage.DiscardCard || r.TargetSeat is not { } holder)
            throw new InvalidOperationException("Goude cannot pay an unissued branch.");
        var selected = r.Materials.Single(m => m.Slot.ToString(System.Globalization.CultureInfo.InvariantCulture) == choice.Parameters["slot-index"]);
        if (_characterLuoXianOlOperations.CurrentHandEntry(holder, selected.CardId) != selected.HandEntry || !GoudeDiscardMaterials(f.OwnerSeat, holder).Contains(selected))
            throw new InvalidOperationException("Goude cannot substitute its original selected hand entity.");
        var paymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = r with { PaidMaterial = selected, Stage = GoudeStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [selected.CardId], new CardMoveReason(GoudeDiscardReason), f.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([selected.CardId], CardLocation.DiscardPile, new(GoudeDiscardReason));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > paymentBefore && m.CardId == selected.CardId && m.From == CardLocation.Hand(holder) && m.To == CardLocation.DiscardPile && m.Reason.Value == GoudeDiscardReason);
        RecordGoudePayment(f.Id, paymentBefore, 1, selected, movement.Sequence);
        ContinueMengDa(f.Id);
    }

    internal void RecordGoudePayment(long id, long before, int count, GoudeHandMaterial? material, long? sequence)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.Goude!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Goude = r with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeMovementPaidEvent(id, r.Option!.Value, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count, material, sequence));
    }

    internal void ContinueMengDa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Goude: { } r } f || f.Id != id)
            return;
        if (r.Stage is not (GoudeStage.MovementChildren or GoudeStage.NativeChildren))
        {
            if (r.Stage != GoudeStage.NativeSlash && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishGoude(f);
            return;
        }

        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishGoude(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishGoude(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Goude = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27240)]
    internal bool ResumeMengDa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Goude: not null } f || f.Id != id)
            return false;
        ContinueMengDa(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27240)]
    internal bool ReturnMengDaMovement(ProgramSkillFrame f)
    {
        if (f.Goude is null)
            return false;
        ContinueMengDa(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveGoudeActualTurnEnd)]
    internal PromptChoice ChooseGoudeAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.Goude!;
        if (r.Stage == GoudeStage.Option)
            return p.Choices.OrderBy(c => c.Parameters.GetValueOrDefault("option") == "Draw" ? 0 : c.Parameters.GetValueOrDefault("option") == "Discard" ? 1 : c.Parameters.GetValueOrDefault("option") == "Slash" ? 2 : 3).First();
        if (r.Stage == GoudeStage.Faction)
            return p.Choices.OrderByDescending(c => _coreMatchParticipantsPlayersAccess._players.Count(v => v.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(v) == c.Parameters["faction"])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        if (r.Stage is GoudeStage.DiscardTarget or GoudeStage.SlashTarget)
            return p.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets[0], new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false))).First();
        return p.Choices.OrderBy(c => c.Cards.Count > 0 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 2).First();
    }
}
