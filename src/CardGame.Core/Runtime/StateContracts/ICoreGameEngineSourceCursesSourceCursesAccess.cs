#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineSourceCursesSourceCursesAccess : IProgramRuntimeOperations
{
    ref readonly List<SourceCurseDeposit> _sourceCurses { get; }
}
