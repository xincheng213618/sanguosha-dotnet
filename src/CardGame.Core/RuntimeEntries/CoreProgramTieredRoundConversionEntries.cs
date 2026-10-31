#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramTieredRoundConversionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime> _resolve;
    private CoreProgramTieredRoundConversionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramTieredRoundConversionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime> resolve) => new(resolve);
}
