#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess : IProgramRuntimeOperations
{
    ref readonly List<GameEngine.ProgramBooleanStateLease> _programBooleanStateLeases { get; }
}
