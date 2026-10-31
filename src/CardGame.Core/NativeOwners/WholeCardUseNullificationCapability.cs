#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WholeCardUseNullificationCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime>> getCoreGameEngineProgramCardUseNullificationRuntime) : IWholeCardUseNullificationHost
{
    public void NullifyCurrentCardUse(ProgramSkillFrame frame) => getCoreGameEngineProgramCardUseNullificationRuntime().Value.NullifyCurrentProgramCardUse(frame);
}
