#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess : IProgramRuntimeOperations
{
    ref readonly List<OrderedPrintedSkillLossLease> _orderedPrintedSkillLossLeases { get; }
}
