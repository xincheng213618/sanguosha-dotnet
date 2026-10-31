#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal void CompleteZhanyuan(ProgramSkillFrame f, int? male)
    {
        var r = f.HuaMan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = HuaManStage.GrantChildren, OtherSeat = male }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            if (male is { } seat)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreMatchParticipantsPlayersAccess._players[seat].Gender != GeneralGender.Male || seat == f.OwnerSeat)
                    throw new InvalidOperationException("Zhanyuan lost the actual chosen male.");
                var grantSource = $"huaman-zhanyuan:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}";
                _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], grantSource, ["ol:xili"]);
                _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[seat], grantSource, ["ol:xili"]);
            }

            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == "ol:mansi").ToArray())
                _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CharacterSkillsLostEvent(f.OwnerSeat, f.OwnerSeat, Array.AsReadOnly(new[] { "ol:mansi" })));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhanyuanCompletedEvent(f.Id, f.OwnerSeat, male, true));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
