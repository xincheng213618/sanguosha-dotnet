#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess : IProgramRuntimeOperations
{
    ref readonly List<OrderedPrintedSkillLossLease> _orderedPrintedSkillLossLeases { get; }

    ref bool _orderedPrintedSkillLossStateStarted { get; }
}
