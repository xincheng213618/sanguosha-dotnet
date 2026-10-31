#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRedDiscardRecoveryRuntime
{
    internal bool UsesRedDiscardRecoveryCounter => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferRedDiscardRecoveryChoice);
    internal int CurrentTurnRedDiscardCount => _coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber ? _coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardCount : 0;

    internal void CaptureTurnRedDiscardCount(int turn, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!UsesRedDiscardRecoveryCounter || turn <= 0)
            return;
        if (turn != _coreActualTurnProgressTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("A discard batch lost its actual turn.");
        if (_coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            _coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardTurn = _coreActualTurnProgressTurnNumberAccess._turnNumber;
            _coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardCount = 0;
        }

        foreach (var move in movements.Where(m => m.To == CardLocation.DiscardPile && m.From != CardLocation.DiscardPile))
        {
            var suit = _cardZonesOperations._cardZones.CardsAt(move.To).Single(c => c.Id == move.CardId).Suit;
            if (suit is Suit.Heart or Suit.Diamond)
                _coreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess._turnRedDiscardCount++;
        }
    }

    internal bool RedDiscardRecoverySourceValid(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId);
    internal Dictionary<string, string> RedDiscardRecoveryParameters(ProgramSkillFrame f, string mode) => new()
    {
        ["program-action"] = "red-discard-recovery",
        ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
        ["mode"] = mode
    };
    internal SkillProgramStepOutcome BeginRedDiscardRecovery(ProgramSkillFrame f)
    {
        if (!_characterMengHuoBoundaryMobileOperations.ValidMobileOrOlRedDiscardRecoveryWindow(f))
            throw new InvalidOperationException("Red discard recovery requires its actual own rule boundary.");
        var budget = CurrentTurnRedDiscardCount;
        if (budget <= 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var draft = new ProgramRedDiscardRecoveryDraft(budget, []);
        var published = f with
        {
            RedDiscardRecoveryDraft = draft
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(published);
        _coreDiscardBudgetAndFactionGiftsOperations.PublishBudgetGiftPrompt(published, f.OwnerSeat, $"本回合置入弃牌堆的红色牌有 {(budget)} 张，请选择至多 {(budget)} 名角色。", RedDiscardRecoverySelectionChoices(published));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> RedDiscardRecoverySelectionChoices(ProgramSkillFrame f)
    {
        var d = f.RedDiscardRecoveryDraft!;
        var choices = new List<PromptChoice>();
        void Add(string key, string label, IReadOnlyList<int> targets) => choices.Add(new(new($"red-discard-recovery.{(f.Id)}.{(key)}"), label, [], targets, RedDiscardRecoveryParameters(f, key)));
        if (d.Selected.Count < d.Budget)
        {
            foreach (var other in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _characterMengHuoBoundaryMobileOperations.CanMobileOrOlRedDiscardRecoverySelect(f, p.Seat) && !d.Selected.Contains(p.Seat)))
                Add($"seat:{(other.Seat)}", $"选择 {(other.Name)}", [other.Seat]);
        }

        if (d.Selected.Count > 0)
            Add("finish", $"确认，依次结算（已选 {(d.Selected.Count)}/{(d.Budget)} 名）", d.Selected);
        Add("skip", "不发动", []);
        return choices;
    }

    internal IReadOnlyList<PromptChoice> RedDiscardRecoveryQuestionChoices(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var choices = new List<PromptChoice>
        {
            new(new($"red-discard-recovery.{(f.Id)}.draw"), "摸一张牌", [], [], RedDiscardRecoveryParameters(f, "draw"))
        };
        if (owner.Hp < owner.MaxHp)
            choices.Add(new(new($"red-discard-recovery.{(f.Id)}.recovery"), $"令 {(owner.Name)} 回复1点体力", [], [], RedDiscardRecoveryParameters(f, "recovery")));
        return choices;
    }

    [ProgramAnswer("red-discard-recovery")]
    internal void ResolveRedDiscardRecoveryChoice(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(long.Parse(selected.Parameters["frame-id"]));
        var d = f.RedDiscardRecoveryDraft ?? throw new InvalidOperationException("The red discard recovery lost its draft.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!RedDiscardRecoverySourceValid(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var mode = selected.Parameters["mode"];
        if (d.AskingSeat is { } askedSeat)
        {
            var legal = RedDiscardRecoveryQuestionChoices(f).Single(choice => choice.Id == selected.Id);
            if (legal.Parameters["mode"] != mode)
                throw new InvalidOperationException("The red discard recovery answer lost its legal prompt.");
            if (!_coreMatchParticipantsPlayersAccess._players[askedSeat].IsAlive)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), false);
                return;
            }

            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RedDiscardRecoveryDraft = active.RedDiscardRecoveryDraft!with { AskingSeat = null } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRedDiscardRecoveryChosenEvent(f.Id, f.OwnerSeat, askedSeat, mode));
            if (mode == "recovery")
            {
                var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                var recovered = Math.Min(1, owner.MaxHp - owner.Hp);
                if (recovered > 0)
                {
                    if (_coreRecoveryAmountBonusesOperations.TracksRecoveryAmountBonuses && _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(active.Id, active.OwnerSeat, active.OwnerSeat, recovered, new(RecoveryAttemptProducer.Program, active.InstructionIndex)))
                    {
                        if (!_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(active.Id, PostEventContinuation.Program))
                            throw new InvalidOperationException("The paid red-discard recovery lost its exact queued recovery child.");
                        return;
                    }

                    var recoveryId = _coreNativeRecoveryFramesOperations.BeginRecovery(active.Id, active.OwnerSeat, active.OwnerSeat, recovered);
                    owner.Hp += recovered;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(active.OwnerSeat, active.OwnerSeat, recovered, owner.Hp));
                    _coreRuntimeOperations.PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
                }

                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
                return;
            }

            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, askedSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.red-discard-recovery.draw"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (mode == "skip")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { RedDiscardRecoveryDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (mode == "finish")
        {
            var committed = d with
            {
                Committed = true,
                Cursor = 0
            };
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RedDiscardRecoveryDraft = committed });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRedDiscardRecoveryCommittedEvent(f.Id, f.OwnerSeat, committed.Budget, committed.Selected));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (!d.Committed && d.Selected.Count < d.Budget && int.TryParse(mode["seat:".Length..], out var seat) && _characterMengHuoBoundaryMobileOperations.CanMobileOrOlRedDiscardRecoverySelect(f, seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !d.Selected.Contains(seat))
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var grown = active.RedDiscardRecoveryDraft!with
            {
                Selected = Array.AsReadOnly(active.RedDiscardRecoveryDraft!.Selected.Append(seat).ToArray())
            };
            var published = active with
            {
                RedDiscardRecoveryDraft = grown
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(published);
            _coreDiscardBudgetAndFactionGiftsOperations.PublishBudgetGiftPrompt(published, f.OwnerSeat, $"请选择至多 {(grown.Budget)} 名角色。已选 {(grown.Selected.Count)} 名。", RedDiscardRecoverySelectionChoices(published));
            return;
        }

        throw new InvalidOperationException("The red discard recovery selection does not match its suspended instruction.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 980)]
    internal bool ResumeRedDiscardRecovery(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.RedDiscardRecoveryDraft is not { Committed: true, AskingSeat: null } d)
            return false;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { RedDiscardRecoveryDraft = null });
            return false;
        }

        var cursor = d.Cursor;
        while (cursor < d.Selected.Count && !_coreMatchParticipantsPlayersAccess._players[d.Selected[cursor]].IsAlive)
            cursor++;
        if (cursor >= d.Selected.Count || !RedDiscardRecoverySourceValid(f))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { RedDiscardRecoveryDraft = null });
            return false;
        }

        var seat = d.Selected[cursor];
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var asking = active.RedDiscardRecoveryDraft!with
        {
            Cursor = cursor + 1,
            AskingSeat = seat
        };
        var published = active with
        {
            RedDiscardRecoveryDraft = asking
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(published);
        _coreDiscardBudgetAndFactionGiftsOperations.PublishBudgetGiftPrompt(published, seat, $"请选择【再起】的效果。", RedDiscardRecoveryQuestionChoices(published));
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferRedDiscardRecoveryChoice)]
    internal PromptChoice SelectAiRedDiscardRecovery(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (!decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("mode")is "draw" or "recovery"))
            return decision.Choices[0];
        // Public HP relationship alone separates the branches; private hands do not enter.
        var owner = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat).Players.Single(player => player.Seat == frame.OwnerSeat);
        var heal = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("mode") == "recovery");
        if (heal is not null && owner.MaxHp - owner.Hp >= 2)
            return heal;
        return decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("mode") == "draw");
    }

    SkillProgramStepOutcome global::CardGame.Core.IRedDiscardRecoveryHost.OfferRedDiscardRecoveryChoice(ProgramSkillFrame f) => BeginRedDiscardRecovery(f);
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 400)]
    internal void CaptureMovementBatchFactsCaptureTurnRedDiscardCount(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureTurnRedDiscardCount(batch.TurnNumber, movements);
}
