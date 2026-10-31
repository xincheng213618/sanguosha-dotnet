#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDamageTransferRuntime
{
    internal void RedirectProgramCurrentDamage(ProgramSkillFrame frame, string sourceBind, bool drawLostHpAfterDamage)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, ParentFrameId: var parentId } || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != parentId || window.Prevented || window.RedirectedTargetSeat is not null || frame.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("Damage redirection lost its selected recipient or parent window.");
        var attack = _coreProgramAttackAttemptOperations.CurrentDamageAttempt ?? throw new InvalidOperationException("Damage redirection has no pending damage occurrence.");
        var recipientSeat = frame.SelectedTargetSeats[0];
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipientSeat) || !_coreMatchParticipantsPlayersAccess._players[recipientSeat].IsAlive || recipientSeat == window.TargetSeat || attack.SourceSeat != window.SourceSeat || attack.TargetSeat != window.TargetSeat || attack.DamageRedirected)
            throw new InvalidOperationException("The damage recipient or frozen participants changed.");
        var cost = _coreProgramLifecycleOperations.GetProgramCardSet(active, sourceBind);
        if (cost.CardIds.Count != 1)
            throw new InvalidOperationException("Damage redirection requires exactly one bound cost card.");
        attack.RedirectFinalizedDamageTarget(recipientSeat, new global::CardGame.Core.GameEngine.ProgramDamageTransferFollowup(active.SkillId, active.OwnerSeat, recipientSeat, drawLostHpAfterDamage));
        _coreRecipientScopedDamageOperations.UpdateRecipientScopedDamageRedirect(attack);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[^2].Id, window with { RedirectedTargetSeat = recipientSeat });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageTransferredEvent(attack.ResolutionId, active.SkillId, active.OwnerSeat, attack.SourceSeat, recipientSeat, cost.CardIds[0], window.Amount, window.Nature));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 转移 {(window.Amount)} 点伤害给 {(_coreMatchParticipantsPlayersAccess._players[recipientSeat].Name)}。", active.OwnerSeat, recipientSeat);
    }
}
