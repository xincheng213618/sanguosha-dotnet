#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralQualificationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> _resolve;
    private CoreGameEngineGuanduGeneralQualificationEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralQualificationEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> resolve) => new(resolve);
}
