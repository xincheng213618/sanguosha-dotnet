#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class WushuangShopRandom(int seed) : IWushuangShopRandom
    {
        private readonly DeterministicRandom _random = new(seed);
        public void Shuffle<T>(IList<T> values) => _random.Shuffle(values);
    }
