using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private sealed record ShanheBattleLogStageBoundary(int BossSeat, int LastPreviousFormLogSequence,
        string PreviousGeneralId, string GeneralId);

    private string BattleLogGeneralNameAt(int sequence, int seat, string currentName)
    {
        // Historical facts do not authorize revealing a name hidden by the
        // current player view. Keep the existing human/observer visibility rule.
        var player = _snapshot.Players.FirstOrDefault(player => player.Seat == seat);
        if (player is null || string.IsNullOrWhiteSpace(player.GeneralName) ||
            !player.IsHuman && (player.IdentityConcealed || !player.IsGeneralPublic)) return currentName;
        var stage = _battleLogShanheStageHistory.FirstOrDefault(stage => stage.BossSeat == seat);
        if (stage is null || sequence > stage.LastPreviousFormLogSequence) return currentName;
        return _game.ContentRegistry.Generals.TryGetValue(stage.PreviousGeneralId, out var general) ? general.Name : currentName;
    }

    private static IReadOnlyList<ShanheBattleLogStageBoundary> BuildShanheBattleLogStageHistory(
        IReadOnlyList<GameLogEntry> logs, IReadOnlyList<EventEnvelope> events, IReadOnlyList<PlayerSnapshot> players)
    {
        var stages = events.Where(entry => entry.Payload is ShanheBossStageStartedEvent).OrderBy(entry => entry.Sequence).ToArray();
        if (stages.Length == 0) return [];
        // Shanhe's existing once-only transition has one public identity pair.
        // An unsupported repeated transition must not guess a historical name.
        if (stages.Select(entry => ((ShanheBossStageStartedEvent)entry.Payload).BossSeat).Distinct().Count() != stages.Length ||
            events.Select(entry => entry.Sequence).Distinct().Count() != events.Count ||
            logs.Select(entry => entry.Sequence).Distinct().Count() != logs.Count ||
            players.Select(player => player.Seat).Distinct().Count() != players.Count) return [];

        var result = new List<ShanheBattleLogStageBoundary>();
        var legacy = new List<EventEnvelope>();
        var maximumLogSequence = logs.Select(entry => entry.Sequence).DefaultIfEmpty(0).Max();
        foreach (var envelope in stages)
        {
            var stage = (ShanheBossStageStartedEvent)envelope.Payload;
            if (!players.Any(player => player.Seat == stage.BossSeat) || string.IsNullOrWhiteSpace(stage.PreviousGeneralId) ||
                string.IsNullOrWhiteSpace(stage.GeneralId) || stage.PreviousGeneralId == stage.GeneralId) continue;
            if (stage.LastPreviousFormLogSequence is { } cut)
            {
                // This output-only bridge is captured immediately before the
                // actual replacement. It also covers HP/max-HP loss, no draw
                // and startup transitions without inferring a causal log row.
                if (cut >= 0 && cut <= maximumLogSequence)
                    result.Add(new(stage.BossSeat, cut, stage.PreviousGeneralId, stage.GeneralId));
                // Present but invalid metadata is not permission to guess.
                continue;
            }
            legacy.Add(envelope);
        }
        if (legacy.Count == 0) return result.AsReadOnly();

        var damageLogs = logs.Where(entry => entry.Type == "Damage").OrderBy(entry => entry.Sequence).ToArray();
        var damages = events.Where(entry => entry.Payload is DamageAppliedEvent).OrderBy(entry => entry.Sequence).ToArray();
        if (damageLogs.Length != damages.Length) return result.AsReadOnly();
        var playerNames = players.ToDictionary(player => player.Seat, player => player.Name);
        var logByDamageSequence = new Dictionary<long, GameLogEntry>();
        for (var index = 0; index < damages.Length; index++)
        {
            var damage = (DamageAppliedEvent)damages[index].Payload;
            var log = damageLogs[index];
            // Both public streams have one native damage writer. Pair their
            // occurrences in order, validating the complete public result;
            // their independent sequence counters are never compared.
            if (log.ActorSeat != (damage.SourceLess ? (int?)null : damage.SourceSeat) || log.TargetSeat != damage.TargetSeat ||
                !playerNames.TryGetValue(damage.TargetSeat, out var target) ||
                !TryDamageMessage(damage, target, playerNames, out var message) || log.Message != message) return result.AsReadOnly();
            logByDamageSequence.Add(damages[index].Sequence, log);
        }

        foreach (var envelope in legacy)
        {
            var stage = (ShanheBossStageStartedEvent)envelope.Payload;
            // A prior lethal hit from another command or a later HP/max-HP
            // replacement cannot establish a damage-triggered stage boundary.
            // Those loss paths do not have a guaranteed pre-transition result
            // log. Stage initial-deal draws also omit CardsDrawnEvent, so neither
            // loss facts nor a later draw/announcement can supply a guessed cut.
            var lastHealth = events.Where(entry => entry.Sequence < envelope.Sequence && entry.Revision == envelope.Revision &&
                AffectsHealth(entry.Payload, stage.BossSeat)).MaxBy(entry => entry.Sequence);
            if (lastHealth?.Payload is not DamageAppliedEvent { Amount: > 0, RemainingHp: <= 0 } ||
                !logByDamageSequence.TryGetValue(lastHealth.Sequence, out var lethal) || lethal.TurnNumber != stage.TurnNumber) continue;
            result.Add(new(stage.BossSeat, lethal.Sequence, stage.PreviousGeneralId, stage.GeneralId));
        }
        return result.AsReadOnly();

        static bool AffectsHealth(IGameEvent payload, int seat) => payload switch
        {
            DamageAppliedEvent damage => damage.TargetSeat == seat,
            RecoveryAppliedEvent recovery => recovery.TargetSeat == seat,
            ProgramSkillHpLostEvent loss => loss.TargetSeat == seat,
            MaximumHpChangedEvent maximum => maximum.PlayerSeat == seat,
            DamageReplacedWithHpLossEvent replacement => replacement.TargetSeat == seat,
            CardDamageReplacedWithMaximumHpLossEvent replacement => replacement.Receipt.TargetSeat == seat,
            _ => false
        };

        static bool TryDamageMessage(DamageAppliedEvent damage, string target, IReadOnlyDictionary<int, string> names,
            out string? message)
        {
            var nature = damage.Nature switch
            {
                DamageNature.Normal => string.Empty,
                DamageNature.Fire => "火焰",
                DamageNature.Thunder => "雷电",
                DamageNature.Cold => "冰",
                _ => null
            };
            message = null;
            if (nature is null) return false;
            if (damage.SourceLess)
                message = $"{target} 受到 {damage.Amount} 点无来源{nature}伤害，剩余 {damage.RemainingHp} 点体力。";
            else if (names.TryGetValue(damage.SourceSeat, out var source))
                message = $"{target} 受到 {source} 造成的 {damage.Amount} 点{nature}伤害，剩余 {damage.RemainingHp} 点体力。";
            return message is not null;
        }
    }
}
