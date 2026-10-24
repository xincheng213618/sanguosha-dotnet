namespace CardGame.Core;

public sealed partial class GameEngine
{
    private MatchModeRuntime CreateMatchModeRuntime()
    {
        var context = new ModeFeatureCreationContext(_options, _modeDefinition, _contentRegistry, CreateModeHostServices());
        var runtime = new MatchModeRuntime(ModeFeatureComposition.Create(context).ToArray());
        runtime.ValidateNativePorts();
        return runtime;
    }
}
