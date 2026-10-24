using CardGame.Core;

namespace CardGame.Modes.Standard;

/// <summary>Standard mode policies, replaceable independently of opening and draft algorithms.</summary>
public sealed class StandardModeProfileFactory : IGameModeProfileFeatureFactory
{
    public string Id => "mode:standard-profile";
    public string ModeId => ModeFeatureTargets.AllModes;
    public IGameModeFeature Create(ModeFeatureCreationContext context) =>
        new StandardModeProfileAdapter(StandardModeSetupFactory.CreateImplementation(context));
}
