#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SourceCurseSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries> getCoreGameEngineSourceCursesRuntime) : ISourceCurseSelectionQueries
{
    public bool HasCurse(int targetSeat) => getCoreGameEngineSourceCursesRuntime().SourceCurseAt(targetSeat)is not null;
}
