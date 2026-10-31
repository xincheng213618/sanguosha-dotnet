#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGaoLanOlRuntime
{
    internal void XizhenProgramResponseBenefit(ProgramSkillFrame frame)
    {
        var holders = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Markers.ContainsKey(PlayerMarkerKind.XiZhen)).ToArray();
        if (holders.Length != 1)
            return;
        var holder = holders[0];
        if (!holder.HasHpValue)
            return;
        var requested = Math.Min(1, holder.MaxHp - holder.Hp);
        if (requested > 0)
        {
            if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(frame.Id, frame.OwnerSeat, holder.Seat, requested, new(RecoveryAttemptProducer.Program, frame.InstructionIndex)))
                return;
            var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(frame.Id, frame.OwnerSeat, holder.Seat, requested);
            holder.Hp += requested;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(frame.OwnerSeat, holder.Seat, requested, holder.Hp));
            _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
        }

        var draw = _coreTreatedWoundedOperations.IsRuleWounded(holder) ? 1 : 2;
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, draw, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(frame.SkillId)}.xizhen"));
    }

    void global::CardGame.Core.IGaoLanProgramHost.XizhenResponseBenefit(ProgramSkillFrame frame) => XizhenProgramResponseBenefit(frame);
}
