#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramBooleanStateExpirationState : Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess
{
    private List<GameEngine.ProgramBooleanStateLease> _programBooleanStateLeases = [];
    internal ref readonly List<GameEngine.ProgramBooleanStateLease> ReferenceToProgramBooleanStateLeases => ref _programBooleanStateLeases;

    ref readonly List<GameEngine.ProgramBooleanStateLease> Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases => ref _programBooleanStateLeases;
}
