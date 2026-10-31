#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed partial class CoreProgramCardLegalityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _resolve;
    private CoreProgramCardLegalityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramCardLegalityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> resolve) => new(resolve);
}
