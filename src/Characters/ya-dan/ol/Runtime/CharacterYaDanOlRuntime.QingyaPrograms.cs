#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{
    internal void NextQingyaHand(ProgramSkillFrame f)
    {
        while (f.YaDan!.Cursor < f.YaDan.Targets.Count && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            var r = f.YaDan;
            var seat = r.Targets[r.Cursor];
            var materials = QingyaHandMaterials(f.OwnerSeat, seat);
            if (materials.Count == 0)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyaParticipantSkippedEvent(f.Id, r.Cursor, seat));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YaDan = r with { Cursor = r.Cursor + 1, Materials = [] } });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YaDan = r with { Stage = QingyaStage.HandChoice, Materials = materials } });
            for (var i = 0; i < materials.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyaHandCandidateEvent(f.Id, r.Cursor, i, materials[i]));
            PublishYaDan(f);
            return;
        }

        FinishYaDan(f);
    }
}
