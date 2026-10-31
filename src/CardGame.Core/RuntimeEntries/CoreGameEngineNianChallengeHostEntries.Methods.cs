#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNianChallengeHostEntries
{
    internal INianChallengeFeature? NianFeature { get => _resolve().NianFeature; }
}
