using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Installs only the campaign services selected by this match's frozen mode.</summary>
public sealed class HellNianFeatureFactory : IGameModeFeatureFactory
{
    public string Id => "activity:hell-nian-flow";
    public string ModeId => ModeFeatureTargets.AllModes;

    public IGameModeFeature Create(ModeFeatureCreationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var mode = context.ModeDefinition ??
            throw new InvalidOperationException("Campaign flow requires the current immutable mode definition.");
        var features = new List<IGameModeFeature>();
        if (mode.HellActivity is not null)
            features.Add(new HellEncounterFeature(mode, context.GetHost<IHellEncounterHost>()));
        if (mode.NianActivity is not null)
            features.Add(new NianChallengeFeature(mode, context.GetHost<INianChallengeHost>(), context.GetHost<IModeSetupRandom>()));
        return new ModeFeatureBundle(features);
    }
}
