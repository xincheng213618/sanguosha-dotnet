namespace CardGame.Core;

// Resolution identities have one owner shared by native callers and runtime
// frame services. This scalar is rebuilt by the existing command replay.
internal sealed class ResolutionIdentitySequence
{
    internal long Value { get; set; }
    internal long Next() => ++Value;
}
