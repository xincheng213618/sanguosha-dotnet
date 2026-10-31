#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNextSlashProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.NamedNextSlashReserve> _namedNextSlashReserves = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.NamedNextSlashReserve> ReferenceToNamedNextSlashReserves => ref _namedNextSlashReserves;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.NamedNextSlashReserve> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves => ref _namedNextSlashReserves;
}
