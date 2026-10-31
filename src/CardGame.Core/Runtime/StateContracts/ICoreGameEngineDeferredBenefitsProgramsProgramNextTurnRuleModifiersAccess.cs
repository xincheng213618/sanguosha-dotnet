#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess : IProgramRuntimeOperations
{
    ref readonly List<ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers { get; }
}
