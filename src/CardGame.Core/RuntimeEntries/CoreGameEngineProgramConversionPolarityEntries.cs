#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramConversionPolarityRuntime))]
internal sealed partial class CoreGameEngineProgramConversionPolarityEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> _resolve;
    private CoreGameEngineProgramConversionPolarityEntries(Func<EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramConversionPolarityEntries Create(Func<EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> resolve) => new(resolve);
}
