namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ProgramEventHistoryIndex? _programHistory;
    // Native owners exist before rule modules can be resolved.
    private ProgramEventHistoryIndex ProgramHistory => _programHistory ??= new(_events, _pendingEvents);
}
