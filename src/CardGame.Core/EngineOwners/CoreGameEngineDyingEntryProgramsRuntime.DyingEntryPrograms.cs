#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDyingEntryProgramsRuntime
{
    internal bool TryBeginDyingEntryProgramWindow(DyingFrame dying)
    {
        const string once = "window:dying-entering";
        if (dying.AttemptedSelfDyingBindings.Contains(once) || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0)
            return false;
        var participants = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).ToArray();
        var facts = participants.ToDictionary(player => player.Seat, player => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(player)with { EventTargetHp = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp, EventTargetRuleWounded = _coreTreatedWoundedOperations.UsesTreatedWounded ? _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat]) : null, EventTargetMaxHp = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].MaxHp, EventTargetHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat]).Count });
        var candidates = participants.SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, SkillProgramTriggerWindow.DyingEntering, facts[player.Seat]).Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Subject != SkillProgramTriggerSubject.Owner || player.Seat == dying.VictimSeat)).OrderBy(candidate => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatAccess._currentSeat, candidate.OwnerSeat)).ThenByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0)
            return false;
        _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { AttemptedSelfDyingBindings = [..current.AttemptedSelfDyingBindings, once] });
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, dying.VictimSeat, SkillProgramTriggerWindow.DyingEntering, candidates, ProgramLifecycleContinuation.ResumeDyingEntry, facts[dying.VictimSeat]) { ParticipantFacts = facts, ResumeDyingFrameId = dying.FrameId });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal void CompleteDyingEntryProgramWindow(ProgramLifecycleTriggerWindowFrame frame)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Dying entry lost its resolution.");
        if (dying.FrameId != frame.ResumeDyingFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != dying.FrameId)
            throw new InvalidOperationException("Dying entry lost its parent continuation.");
        if (_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0)
            _coreNativeDyingOperations.CompleteDying(dying, survived: true);
        else
        {
            _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.AwaitingResponse);
            _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
        }
    }
}
