#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SequentialDeckKindGrowthCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialDeckKindGrowthEntries> getCoreGameEngineSequentialDeckKindGrowthRuntime) : ISequentialDeckKindGrowthHost
{
    public SkillProgramStepOutcome AcquireDeckKindSequentiallyAndGrowOnShortfall(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSequentialDeckKindGrowthRuntime().AcquireDeckKindSequentiallyAndGrowOnShortfall(frame, effect);
}
