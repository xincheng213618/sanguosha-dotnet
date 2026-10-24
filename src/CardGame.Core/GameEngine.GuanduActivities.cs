namespace CardGame.Core;

public sealed partial class GameEngine
{
    private GuanduBattleConfiguredEvent? _guanduInitialConfiguration;
    private long _guanduQualificationStamp;
    private (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies;
    private long CaptureGuanduQualificationStamp(long underlying)
    {
        if (!IsGuanduActivity) return underlying;
        var living = 0; var factions = new HashCode();
        foreach (var p in _players)
        {
            if (p.IsAlive) living |= 1 << p.Seat;
            factions.Add(p.General.Id, StringComparer.Ordinal);
            factions.Add(p.General.FactionId, StringComparer.Ordinal);
            factions.Add(p.ChosenFactionId, StringComparer.Ordinal);
            factions.Add(p.NativeEffectiveFactionOverrideId, StringComparer.Ordinal);
        }
        var dependencies = (underlying, living, factions.ToHashCode());
        if (_guanduQualificationStamp == 0 || dependencies != _guanduQualificationDependencies)
        { _guanduQualificationDependencies = dependencies; _guanduQualificationStamp++; }
        return long.MinValue / 4 + _guanduQualificationStamp;
    }
    private bool IsGuanduActivity => _modeDefinition.GuanduActivity is not null;
    private GuanduBattleConfiguredEvent? GuanduConfiguration => IsGuanduActivity
        ? ProgramEventHistory<GuanduBattleConfiguredEvent>().SingleOrDefault() ?? _guanduInitialConfiguration : null;
    private void RecordGuanduSetupFact() => (GuanduRules as IMatchModeLifecycle)?.OnMatchStart(ModeMatchStartPhase.AfterRunning);
    private bool IsGuanduCommander(int seat) => GuanduRules?.IsCommander(seat) ?? false;
    private bool GuanduAllies(int first, int second) => GuanduRules?.Allies(first, second) ??
        (IsValidPlayerSeat(first) && IsValidPlayerSeat(second) && _players[first].TeamId is { } team && team == _players[second].TeamId);
    private bool IsGuanduCommanderPrintedLordGrant(CharacterState player, SkillGrant grant) => IsGuanduCommander(player.Seat) &&
        grant.SourceId == CharacterState.PrimarySkillSource && grant.LordProjection is null && grant.GeneralLibraryProjection is null &&
        player.General.Skills.Any(skill => skill.ContentId == grant.SkillId && skill.Tags.HasFlag(SkillTag.Lord));
    private GuanduActivitySnapshot? GetGuanduSnapshot() => GuanduRules?.Snapshot;
    private IEnumerable<RuleQueryContribution> GuanduActivityRuleContributions(CharacterState player, SkillRuleQuery query) =>
        GuanduRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
    private void RecordGuanduDeckExhaustion() => GuanduRules?.RecordDeckExhaustion();
    private void ApplyGuanduDeathReward(CharacterState victim, CharacterState? killer) =>
        (GuanduRules as IMatchModeLifecycle)?.ApplyDeathReward(victim.Seat, killer?.Seat);
    private bool TryCompleteGuanduBattle() => GuanduRules is IMatchModeLifecycle lifecycle &&
        lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterTurnEnded);
}
