#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCommandSessionCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandSessionCommandSessionOperations
    {
        public ref readonly global::CardGame.Core.CommandSession _commandSession => ref engine._commandSession;
    }

    [ProgramCapability]
    private sealed class CoreCommandSessionKernelCoreGameEngineCommandSessionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandSessionKernelCoreGameEngineCommandSessionOperations
    {
        public global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }

    [ProgramCapability]
    private sealed class CoreCommandSessionCommandEventJournalAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCommandSessionCommandEventJournalOperations
    {
        public ref global::CardGame.Core.CommandEventJournal? _commandEventJournal => ref engine._commandEventJournal;
    }
}
