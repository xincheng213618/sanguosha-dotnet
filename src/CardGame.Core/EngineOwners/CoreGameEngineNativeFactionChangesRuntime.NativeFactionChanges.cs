#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeFactionChangesRuntime
{
    internal bool TracksNativeEffectiveFactionChanges => _characterSunShangxiangMouMobileOperations.HasMouSunShangxiangMobile || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGoudeActualTurnEnd) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingliuFactionChoice) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiZhHuiYunan);

    internal IReadOnlyList<string> AvailableEffectiveFactionIds() => _coreModeAllegianceOperations.ModeAllegiancePolicy.NativeFactionChoices;
    internal bool HasNativeEffectiveFactionDeclaration(CharacterState player) => TracksNativeEffectiveFactionChanges && player.NativeEffectiveFactionWasDeclared;
    internal global::CardGame.Core.GameEngine.NativeFactionAvatar? CaptureNativeFactionAvatar(CharacterState player) => player.GeneralSelected && _coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsAvatarFactionChanges && _corePrivateGeneralLibrariesOperations.EffectiveGeneralLibrary(player)is { } library ? new(library.Source, library.RevealedGeneralId!, library.DeclaredSkillId, library.SelectedFactionId) : null;
    internal string? CaptureNativeAvatarFactionBeforeDeclaration(CharacterState player) => TracksNativeEffectiveFactionChanges ? _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) : null;
    internal void CompleteNativeAvatarFactionDeclaration(ProgramSkillFrame frame, CharacterState player, string? previous)
    {
        if (!TracksNativeEffectiveFactionChanges || !_coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsAvatarFactionChanges || frame.GeneralLibraryDraft?.Source != _corePrivateGeneralLibrariesOperations.EffectiveGeneralLibrary(player)?.Source)
            return;
        // A real declaration replaces an earlier explicit change even when the
        // same avatar/skill is selected. Acquiring an unused avatar does not.
        player.NativeEffectiveFactionOverrideId = null;
        var current = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player);
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations[player.Seat] = new(current, CaptureNativeFactionAvatar(player));
        if (string.Equals(previous, current, StringComparison.Ordinal) || current is null)
            return;
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision++;
        if (!_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber < 1)
            return;
        player.NativeEffectiveFactionWasDeclared = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, player.Seat, previous, current, frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), "avatar-state"));
    }

    internal void ChangeNativeEffectiveFaction(ProgramSkillFrame frame, string faction)
    {
        if (!TracksNativeEffectiveFactionChanges || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ResolveGoudeActualTurnEnd || frame.WindowContext?.Window != SkillProgramTriggerWindow.AfterTurnEnded || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(frame.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !AvailableEffectiveFactionIds().Contains(faction, StringComparer.Ordinal))
            throw new InvalidOperationException("A faction change requires its actual living native program owner and supported faction.");
        SynchronizeNativeEffectiveFactions();
        var player = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var previous = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player);
        if (string.Equals(previous, faction, StringComparison.Ordinal))
            throw new InvalidOperationException("A faction change cannot select the current effective faction.");
        var previousAvatar = CaptureNativeFactionAvatar(player);
        player.NativeEffectiveFactionOverrideId = faction;
        player.NativeEffectiveFactionWasDeclared = true;
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations[player.Seat] = new(faction, previousAvatar);
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, player.Seat, previous, faction, frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), "program"));
        _coreMatchLogOperations.AddLog("FactionChanged", $"{(player.Name)} 将势力变更为{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(faction))}。", player.Seat);
    }

    internal void ApplyCaoTengFactionChange(ProgramSkillFrame frame, string faction)
    {
        if (!TracksNativeEffectiveFactionChanges || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !_characterCaoTengOlOperations.IsCaoTengFactionChange(frame, faction) || faction is not ("qun" or "wei") || !AvailableEffectiveFactionIds().Contains(faction, StringComparer.Ordinal))
            throw new InvalidOperationException("Qingliu requires its exact living setup or first rescued-Dying choice.");
        SynchronizeNativeEffectiveFactions();
        var player = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var previous = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player);
        player.NativeEffectiveFactionOverrideId = faction;
        player.NativeEffectiveFactionWasDeclared = true;
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations[player.Seat] = new(faction, CaptureNativeFactionAvatar(player));
        if (string.Equals(previous, faction, StringComparison.Ordinal))
            return;
        _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision++;
        // A setup declaration is visible, but is not an actual-turn mutation.
        if (_coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, player.Seat, previous, faction, frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), "qingliu"));
        _coreMatchLogOperations.AddLog("FactionChanged", $"{(player.Name)} 将势力变更为{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(faction))}。", player.Seat);
    }

    // This observes completed native mutations. The cache contains no pending
    // resolution or entitlement; every actual change is retained as a scalar fact.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 35)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 15)]
    internal void SynchronizeNativeEffectiveFactions()
    {
        if (!TracksNativeEffectiveFactionChanges || _coreGameEngineNativeFactionChangesFieldsAccess._synchronizingNativeFactions)
            return;
        _coreGameEngineNativeFactionChangesFieldsAccess._synchronizingNativeFactions = true;
        try
        {
            var changes = new List<(CharacterState Player, string? Previous, string Current, string Cause)>();
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            {
                var avatar = CaptureNativeFactionAvatar(player);
                var known = _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations.TryGetValue(player.Seat, out var previous);
                if (known && previous!.Avatar != avatar)
                    player.NativeEffectiveFactionOverrideId = null;
                var current = player.GeneralSelected ? _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) : null;
                _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations[player.Seat] = new(current, avatar);
                if (!known || string.Equals(previous!.FactionId, current, StringComparison.Ordinal) || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber < 1 || current is null)
                    continue;
                player.NativeEffectiveFactionWasDeclared = true;
                _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision++;
                changes.Add((player, previous.FactionId, current, previous.Avatar != avatar ? "avatar-state" : "native-state"));
            }

            // Refresh every observation before recursive rule delivery.
            foreach (var change in changes)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, change.Player.Seat, change.Previous, change.Current, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id, null, change.Cause));
        }
        finally
        {
            _coreGameEngineNativeFactionChangesFieldsAccess._synchronizingNativeFactions = false;
        }
    }

    internal long CaptureNativeFactionQualificationStamp(long underlying)
    {
        if (!TracksNativeEffectiveFactionChanges)
            return underlying;
        var current = (underlying, _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision);
        if (_coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationStamp == 0 || current != _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationDependencies)
        {
            _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationDependencies = current;
            _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationStamp++;
        }

        return long.MinValue / 2 + _coreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationStamp;
    }
}
