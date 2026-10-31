#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShenwuActivitiesRuntime
{
    internal IShenwuBattleFeature? ShenwuBattleRules => _modeRuntimeOperations._modeRuntime.Find<IShenwuBattleFeature>();
}
