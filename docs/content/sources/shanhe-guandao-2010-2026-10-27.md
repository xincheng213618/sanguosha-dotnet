# 关刀之脊2010

固定[战法表](https://s.xiaochao.org/data/tactics.json)的2010为“关刀之脊”，原文“方片【杀】无距离限制”，普通1、200铜币、伤害流4，seasonIds包含5且未弃用。所用固定快照SHA-256为`209ce819d57fc8985308376ecc24fe12d2c3ca94dec75b775578f34543774c0e`。[S5战法树](https://s.xiaochao.org/data/tactic-tree.json)没有2010的固定节点，沿用无固定节点进入默认货池的本地约定。

独立注册`shanhe:current-guandao-2010`，复用已有按实际花色豁免杀距离的卡策略；不把其他花色或非杀牌扩大到相同范围，也不增加出杀次数。完整杀类别声明沿既有匹配规则覆盖普通、火、雷、冰杀；转化杀使用原生转化实体的实际花色。

已有[缓存原图](https://s.xiaochao.org/data/images/4e71f4c31ff7d60fefeacc04.png)可直接复用，2010.png的SHA-256为`4e71f4c31ff7d60fefeacc0470cd8a6b57efba1b8144028da9ce8a4fc9a4f356`。行为检查通过真实常驻集市付款后对距离2目标使用实体与转化杀，保留非方片与顺手牵羊的正常距离及原生次数限制；不运行存档恢复。
