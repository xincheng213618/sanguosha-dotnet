#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramCardLegalityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _resolve;
    private CoreProgramCardLegalityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramCardLegalityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> resolve) => new(resolve);
}
