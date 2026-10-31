#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramBooleanStateExpirationState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramBooleanStateLease> _programBooleanStateLeases = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramBooleanStateLease> ReferenceToProgramBooleanStateLeases => ref _programBooleanStateLeases;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramBooleanStateLease> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases => ref _programBooleanStateLeases;
}
