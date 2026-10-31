#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramConversionPolarityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> _resolve;
    private CoreGameEngineProgramConversionPolarityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramConversionPolarityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> resolve) => new(resolve);
}
