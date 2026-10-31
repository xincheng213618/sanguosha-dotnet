#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduActivitiesRuntime
{
    internal long CaptureGuanduQualificationStamp(long underlying)
    {
        if (!IsGuanduActivity)
            return underlying;
        var living = 0;
        var factions = new HashCode();
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
        {
            if (p.IsAlive)
                living |= 1 << p.Seat;
            factions.Add(p.General.Id, StringComparer.Ordinal);
            factions.Add(p.General.FactionId, StringComparer.Ordinal);
            factions.Add(p.ChosenFactionId, StringComparer.Ordinal);
            factions.Add(p.NativeEffectiveFactionOverrideId, StringComparer.Ordinal);
        }

        var dependencies = (underlying, living, factions.ToHashCode());
        if (_coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationStamp == 0 || dependencies != _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationDependencies)
        {
            _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationDependencies = dependencies;
            _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationStamp++;
        }

        return long.MinValue / 4 + _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationStamp;
    }

    internal bool IsGuanduActivity => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.GuanduActivity is not null;
    internal GuanduBattleConfiguredEvent? GuanduConfiguration => IsGuanduActivity ? _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduBattleConfiguredEvent>().SingleOrDefault() ?? _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduInitialConfiguration : null;

    internal void RecordGuanduSetupFact() => (_coreActivityBattleRulesOperations.GuanduRules as IMatchModeLifecycle)?.OnMatchStart(ModeMatchStartPhase.AfterRunning);
    internal bool IsGuanduCommander(int seat) => _coreActivityBattleRulesOperations.GuanduRules?.IsCommander(seat) ?? false;
    internal bool GuanduAllies(int first, int second) => _coreActivityBattleRulesOperations.GuanduRules?.Allies(first, second) ?? (_coreTableSeatQueriesOperations.IsValidPlayerSeat(first) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(second) && _coreMatchParticipantsPlayersAccess._players[first].TeamId is { } team && team == _coreMatchParticipantsPlayersAccess._players[second].TeamId);
    internal bool IsGuanduCommanderPrintedLordGrant(CharacterState player, SkillGrant grant) => IsGuanduCommander(player.Seat) && grant.SourceId == CharacterState.PrimarySkillSource && grant.LordProjection is null && grant.GeneralLibraryProjection is null && player.General.Skills.Any(skill => skill.ContentId == grant.SkillId && skill.Tags.HasFlag(SkillTag.Lord));
    internal GuanduActivitySnapshot? GetGuanduSnapshot() => _coreActivityBattleRulesOperations.GuanduRules?.Snapshot;
    internal IEnumerable<RuleQueryContribution> GuanduActivityRuleContributions(CharacterState player, SkillRuleQuery query) => _coreActivityBattleRulesOperations.GuanduRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
    internal void RecordGuanduDeckExhaustion() => _coreActivityBattleRulesOperations.GuanduRules?.RecordDeckExhaustion();
    internal void ApplyGuanduDeathReward(CharacterState victim, CharacterState? killer) => (_coreActivityBattleRulesOperations.GuanduRules as IMatchModeLifecycle)?.ApplyDeathReward(victim.Seat, killer?.Seat);
    internal bool TryCompleteGuanduBattle() => _coreActivityBattleRulesOperations.GuanduRules is IMatchModeLifecycle lifecycle && lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterTurnEnded);
}
