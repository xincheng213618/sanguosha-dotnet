#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ModeSetupRandom(global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom) : IModeSetupRandom
    {
        public int Next(int exclusiveMax) => getDeterministicRandom().Next(exclusiveMax);
        public void Shuffle<T>(IList<T> values) => getDeterministicRandom().Shuffle(values);
    }
