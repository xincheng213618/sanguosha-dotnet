#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativePlayerDeathRuntime
{
    internal void FinalizePlayerDeath(DyingFrame dying)
    {
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var killer = dying.KillerSeat is { } killerSeat ? _coreMatchParticipantsPlayersAccess._players[killerSeat] : null;
        BeginPlayerDeath(dying.FrameId, victim, killer, _coreNativeDamageHandlesOperations.GetDyingAttack(dying), dying);
    }

    internal void BeginPlayerDeath(long parentFrameId, CharacterState victim, CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, DyingFrame? dying, long? causingProgramSkillFrameId = null, long? causingCardDamageReplacementId = null)
    {
        if (!victim.IsAlive)
        {
            if (causingProgramSkillFrameId is { } programFrameId)
            {
                _coreRuntimeOperations.AdvanceRuntimeProgram(programFrameId);
            }

            return;
        }

        var returnKind = (dying, causingProgramSkillFrameId, causingCardDamageReplacementId) switch
        {
            (not null, null, null)when dying.FrameId == parentFrameId => DeathReturnKind.Dying,
            (null, not null, null)when causingProgramSkillFrameId == parentFrameId => DeathReturnKind.ProgramSkill,
            (null, null, not null)when causingCardDamageReplacementId == parentFrameId && _coreCardAttackStateOperations.GetCardAttackState(parentFrameId).MaximumHpReplacement is { MaximumHpAfter: 0, TargetSeat: var seat } && seat == victim.Seat => DeathReturnKind.CardDamageReplacement,
            _ => throw new InvalidOperationException("A death resolution needs exactly one matching parent continuation.")};
        var deathFrameId = _coreNativeDeathFramesOperations.BeginDeath(parentFrameId, victim.Seat, killer?.Seat, returnKind);
        _characterHeJinOlOperations.CaptureHeJinYanhuoDeath(deathFrameId, victim);
        try
        {
            if (!_characterDaQiaoXiaoQiaoHappyGodOperations.TryCompleteHappyQiaoWholeDeath(deathFrameId, victim))
                victim.Hp = 0;
            var caoYuDeathMinimum = _characterCaoYuOlOperations.CaptureCaoYuDeathMinimumSources(victim.Seat);
            victim.IsAlive = false;
            if (causingCardDamageReplacementId is { } replacementId)
                _coreCardDamageMaximumHpReplacementOperations.PublishMaximumHpDamageReplacement(replacementId);
            _characterCaoYuOlOperations.RecordCaoYuDeathMinimumTransitions(deathFrameId, victim.Seat, caoYuDeathMinimum);
            _coreCardUseModulesOperations.ExpireDeadOwnerDamageModifiers(victim.Seat);
            _characterDongZhaoOlOperations.ApplyZaowangSuccession(deathFrameId, victim);
            if (victim.IsChained)
            {
                victim.IsChained = false;
                var chainResolutionId = attack?.ResolutionId ?? parentFrameId;
                var chainSourceSeat = attack?.SourceSeat ?? victim.Seat;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(chainResolutionId, chainSourceSeat, victim.Seat, IsChained: false));
            }

            victim.RoleRevealed = true;
            if (_coreNativeDamageHandlesOperations.IsTeamMode)
            {
                victim.TeamRevealed = true;
            }

            if (_coreNativeDamageHandlesOperations.IsNationalWarMode)
            {
                _coreNationalGeneralRevealOperations.RevealNationalInformation(victim);
            }
            else
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoleRevealedEvent(victim.Seat, victim.Role));
            }

            _modeRuntimeOperations._modeRuntime.ApplyDeathReward(victim.Seat, killer?.Seat);
            if (_characterYuanYinAnniversaryOperations.TryBeginAnniversaryYuanYinDeathBeforeCleanup(deathFrameId))
                return;
            _coreDeathProgramsOperations.ContinuePlayerDeathCleanup(deathFrameId);
        }
        catch
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is DeathFrame { Id: var currentId } && currentId == deathFrameId)
                _coreRuntimeOperations.PopResolutionFrame(deathFrameId, ResolutionFrameKind.Death);
            throw;
        }
    }
}
