#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCommandSessionCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CommandSession _commandSession { get; }
}

internal interface ICoreCommandSessionKernelCoreGameEngineCommandSessionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ICommandSessionRuntime CommandRuntime { get; }
}

internal interface ICoreCommandSessionCommandEventJournalOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CommandEventJournal? _commandEventJournal { get; }
}

internal interface ICoreCommandSessionCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot State { get; }
}

internal interface ICoreCommandSessionCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FlushNotifications(global::CardGame.Core.GameSnapshot preparedSnapshot);
}
