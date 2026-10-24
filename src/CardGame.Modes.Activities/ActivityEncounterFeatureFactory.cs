using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Composes only the encounter flow selected by the current immutable mode definition.</summary>
public sealed class ActivityEncounterFeatureFactory : IGameModeFeatureFactory
{
    public string Id => "activity:encounter-flow";
    public string ModeId => ModeFeatureTargets.AllModes;

    public IGameModeFeature Create(ModeFeatureCreationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var mode = context.ModeDefinition ??
            throw new InvalidOperationException("Encounter flow requires the current immutable mode definition.");
        if (mode.BossActivity is null || mode.HellActivity is not null) return new EmptyEncounterFeature();
        var host = context.GetHost<IModeEncounterHost>();
        return mode.HulaoActivity is not null ? new HulaoEncounterFeature(mode, host) : new BossEncounterFeature(mode, host);
    }

    private sealed class EmptyEncounterFeature : IGameModeFeature { }
}
