#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNextSlashProgramsState : Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess
{
    private List<GameEngine.NamedNextSlashReserve> _namedNextSlashReserves = [];
    internal ref readonly List<GameEngine.NamedNextSlashReserve> ReferenceToNamedNextSlashReserves => ref _namedNextSlashReserves;

    ref readonly List<GameEngine.NamedNextSlashReserve> Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves => ref _namedNextSlashReserves;
}
