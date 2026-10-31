#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Owner, string Skill, string Instance, string State), ProgramEndingHandCountHistory> _endingHandCountHistories { get; }
}
