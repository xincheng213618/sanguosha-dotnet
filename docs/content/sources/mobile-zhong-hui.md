# 普通移动版钟会

核查日期：2026-10-26。`mobile:zhong-hui` 是魏、男、4体力，一将成名2012普通身份，共用 `character:zhong-hui`。按当前普通技能逐项核对后引用已有能力，不复制界钟会的人物定义或界权计的额外收益。

## 当前来源

- [移动官网详情122](https://www.sanguosha.cn/index.php/pc/hero-detail-122.html)：实际页面标题为钟会。当前武将列表将图片173映射至详情122；详情173是公孙瓒，不能用音频或图片数字直接充当官网页面ID。
- [人物33703](https://wiki.biligame.com/msgs/index.php?title=钟会&oldid=33703)：2025-10-21修订，普通魏男4、一将成名2012、画师雪君S。
- [权计21164](https://wiki.biligame.com/msgs/index.php?title=权计/钟会&oldid=21164)、[自立23274](https://wiki.biligame.com/msgs/index.php?title=自立/钟会&oldid=23274)、[排异21173](https://wiki.biligame.com/msgs/index.php?title=排异/钟会&oldid=21173)：本次实际读取 MediaWiki revisions API 的原始正文、修订号和时间。权计与排异为2023-01-23修订，自立为2023-06-16修订；三项当前普通正文与官网一致。

## 完整能力复用

权计引用 `classic:quanji`。真实伤害按每点建立可选触发；接受后先摸一张，再由本人选一张当前Hand原实体置入公开Authority区。不是每个伤害事件只发动一次，也不允许从装备区或旧权中取替代费用。手牌上限按当前权数量动态增加，没有额外“本人出牌阶段结束时手牌大于体力”的触发。

自立引用 `boundary:zili-mobile` 的既有实现，是由于其完整行为与当前普通自立相同：准备阶段当前权不少于三张，强制觉醒并选择回复一点或摸两张；满体力时不能选择无收益回复。收益完成后减少一点体力上限，经通用技能变更层授予既有 `boundary:paiyi-mobile`。该准备窗口在 `GameEngine` 已建立真实Preparation阶段后进入，翻面跳过的回合不会凭空触发；觉醒的两个选择共用原生游戏次数组，不是各觉醒一次。

派生排异的这项实现也与当前普通正文逐项一致：每个真实Play阶段一次，移去一张真实权，令一名存活角色摸两张；其当前手牌数严格多于本人时，才由钟会造成一点伤害。可以选择本人补牌，本人手牌不可能严格多于自身，因此不自伤。费用、正常摸牌和伤害均沿原生节点及原程序帧的子返回；没有使用界人物身份作规则依据。

旧 `classic:paiyi` 的 `usesPerTurn:1` 会把同回合额外出牌阶段也封住，故没有整项引用旧经典钟会来承诺当前阶段次数。普通自立及排异已有完整等价实现，也不为换命名空间复制同一JSON、人物定义快照或行为夹具。已有 `Mobile boundary Zhong Hui:` 检查承担准备觉醒、派生排异真实权支付及子返回cold的机制回归；其中界权计的额外阶段收益不是普通人物获得的能力。

## 素材与注册

`portraitKey=mobile-zhong-hui`、gallery `fame-2`（一将2012），人物及当前将池86999。普通形象为[官网17300.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/17300.jpg)，官网122的普通经典图；不取首张界版51200或势版图片。没有新技能注册、运行时、规则格式或包版本改动。
