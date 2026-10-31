#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsRuntime
{

    internal OlMoRumoEnteredEvent? OlMoRumoEntry(int ownerSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat))
            return null;
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoEnteredEvent>().Where(item => item.OwnerSeat == ownerSeat).ToArray();
        if (entries.Length > 1 || entries.Any(item => item.EntryId <= 0 || item.Origin.OwnerSeat != ownerSeat || item.ActualTurn < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(item.TurnOwner) || !(item.Origin.SkillId == "ol:mo-qiangang" && item.EntryRound >= 1 && _characterSunQuanOlMagicOperations.IsPaidQiangangRumoOrigin(item.EntryId, item.Origin, item.OriginGameplayHash) || _characterLuBuOlMagicOperations.IsMoLuBuRumoOrigin(item) || _characterDiaoChanOlMagicOperations.IsMoDcQingshiRumoOrigin(item) || _programPipelineHooksOperations._programPipelineHooks.RumoOrigin(item.EntryId, item.Origin, item.OriginGameplayHash))))
            throw new InvalidOperationException("OL Rumo must retain one exact permanent paid entry per owner.");
        return entries.SingleOrDefault();
    }

    internal static ProgramTriggerCandidate OlMoRumoCandidate(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules__shared_OlMoRumo_Proofs.OlMoRumoCandidate(frame);
    internal bool OlMoRumoCandidateSource(ProgramTriggerCandidate candidate) => candidate.SkillId == "ol:mo-rumo" && candidate.BindingId == "no-damage-round-ending" && candidate.Priority == 0 && candidate.OccurrenceIndex == 0 && OlMoRumoEntry(candidate.OwnerSeat)is { } entry && entry.EntryRound <= _coreActualRoundProgressRoundNumberAccess._roundNumber && candidate.SkillInstanceId == CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumoInstance(entry.EntryId) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault("ol:mo-rumo")?.Program is { } program && program.GameplayHash == candidate.GameplayHash && program.Triggers is [{ Id: "no-damage-round-ending", Window: SkillProgramTriggerWindow.RoundEnding, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ResolveOlMoRumoRoundPenalty }] }];
}
