#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ConversionPolarityProgramCapability(Func<RuntimeEntries.CoreGameEngineProgramConversionPolarityEntries> getCoreGameEngineProgramConversionPolarityRuntime) : IConversionPolarityProgramHost
{
    public void CommitConversionPolarity(ProgramSkillFrame frame) => getCoreGameEngineProgramConversionPolarityRuntime().CommitProgramConversionPolarity(frame);
}
