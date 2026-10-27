using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Activities;

/// <summary>Initial entrants arrive before any actual turn or game-starting program has begun.</summary>
internal sealed class ShanheInitialDongdangFeature(ShanheBattleSetup setup,
    IModeEncounterHost encounter, IStandardBattleHost battle) : IMatchModeLifecycle
{
    public int Order => 11;

    public void OnMatchStart(ModeMatchStartPhase phase)
    {
        if (phase != ModeMatchStartPhase.AfterOpeningFacts) return;
        var facts = encounter.CaptureFacts();
        if (!facts.SetupComplete || facts.HasWinner || facts.TurnNumber != 0) return;
        // Initial cards are already dealt. No running turn or paid program is
        // retired here; later form arrivals retain the separate stage feature.
        for (var index = 0; index < setup.EnemyRoster.Count; index++)
        {
            var seat = setup.AllyRoster.Count + 1 + index;
            var player = battle.Player(seat);
            if (!player.IsAlive || player.GeneralId != setup.EnemyRoster[index].GeneralId ||
                battle.ActiveShanheDongdangSource(seat) is not { } active) continue;
            var source = active with { BindingId = ShanheSourceDongdangRules.InitialBindingId };
            if (encounter.History<ShanheSourceDongdangInitialEntryEvent>().Any(fact => fact.Source == source))
                throw new InvalidOperationException("Initial Dongdang cannot consume its original entry twice.");
            var previous = encounter.CaptureFacts().CurrentSeat;
            encounter.BeginNaturalTurnAt(seat);
            encounter.PublishFact(new ShanheSourceDongdangInitialEntryEvent(seat, source, player.GeneralId, previous));
        }
    }
}
