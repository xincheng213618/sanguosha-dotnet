#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreCommandSessionQueriesRuntime))]
internal sealed class CharacterLiuZhangOlCoreCommandSessionQueriesAdapter(Func<CoreCommandSessionQueriesRuntime> targetFactory) : ICharacterLiuZhangOlCoreCommandSessionQueriesOperations
{
    private readonly Func<CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
